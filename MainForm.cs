using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Desktop;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaysSharePointSync
{
    public partial class MainForm : Form
    {
        private IPublicClientApplication _pca;
        private AuthenticationResult _authResult;
        private IAccount _account;
        private string _userEmail;
        private bool _isSigningIn;
        private bool _cleanedUp;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Explicitly create a custom user data folder for WebView2 to ensure isolated storage for the embedded browser sessions during authentication, replicating the exact setup from the EntraGroupsApp code where a dedicated folder is created to store WebView2 data, preventing conflicts with other applications or instances and ensuring reliable authentication sessions.
                // The folder path is explicitly based on the application's name ("MaysSharePointSync") within the user's ApplicationData special folder, matching the isolation approach in EntraGroupsApp for secure and consistent browser data handling.
                string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MaysSharePointSync", "WebView2");
                try
                {
                    Directory.CreateDirectory(userDataFolder);
                    // Explicitly set the WEBVIEW2_USER_DATA_FOLDER environment variable to direct the WebView2 runtime to use this custom folder, ensuring that the embedded browser used for interactive sign-in stores its data (such as cookies and cache) in an isolated location, exactly replicating the EntraGroupsApp's configuration for enhanced reliability and security during authentication.
                    Environment.SetEnvironmentVariable("WEBVIEW2_USER_DATA_FOLDER", userDataFolder);
                }
                catch (Exception ex)
                {
                    // If the folder creation explicitly fails, display a detailed error message to the user and halt initialization, as this could explicitly prevent proper embedded browser functionality during the sign-in process, maintaining the error handling consistency from EntraGroupsApp.
                    MessageBox.Show($"Failed to create WebView2 user data folder at {userDataFolder}:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Explicitly build the IPublicClientApplication instance replicating the EntraGroupsApp configuration for the embedded browser sign-in experience.
                // Use the provided client ID ("59eb6041-746d-49f7-9269-dfe67c78e958") from the updated code.
                // Set the authority to the specific tenant ("https://login.microsoftonline.com/68f381e3-46da-47b9-ba57-6f322b8f0da1") as in the updated code.
                // Specify the redirect URI as "http://localhost" to allow MSAL to dynamically select a free port, addressing potential port conflicts (e.g., "loopback_redirect_port_in_use") for improved reliability while maintaining loopback callback handling.
                // Explicitly enable Windows embedded browser support via .WithWindowsEmbeddedBrowserSupport() to use WebView2 for the interactive sign-in UI, replicating the modern, embedded browser experience from EntraGroupsApp for an exact match in sign-in UI, instead of the system browser, providing a seamless and integrated authentication dialog within the application.
                _pca = PublicClientApplicationBuilder.Create("59eb6041-746d-49f7-9269-dfe67c78e958")
                    .WithAuthority("https://login.microsoftonline.com/68f381e3-46da-47b9-ba57-6f322b8f0da1")
                    .WithRedirectUri("http://localhost")
                    .WithWindowsEmbeddedBrowserSupport()
                    .Build();

                // Explicitly define the scopes matching the original MaysSharePointSync requirement for SharePoint access, standardizing on "https://tamucs.sharepoint.com/.default" to ensure consistency between silent and interactive authentication, fixing the inconsistency in the provided code where silent used Graph scopes, preventing MsalUiRequiredException due to scope mismatch.
                var scopes = new[] { "https://tamucs.sharepoint.com/.default" };

                // Explicitly attempt silent authentication if any accounts are present, replicating the EntraGroupsApp's silent sign-in logic to acquire tokens without user interaction if possible, updating UI and properties on success for a smooth user experience.
                var accounts = await _pca.GetAccountsAsync();
                if (accounts.Any())
                {
                    try
                    {
                        _authResult = await _pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                        _account = _authResult.Account;
                        _userEmail = _authResult.Account.Username;
                        SetStatusText($"Signed in as: {_userEmail}");
                        btnSignIn.Visible = false;
                        flowLayoutPanelButtons.Visible = true;
                    }
                    catch (MsalUiRequiredException)
                    {
                        SetStatusText("Please sign in to continue");
                    }
                }
                else
                {
                    SetStatusText("Please sign in to continue");
                }

                // Explicitly wire up event handlers for buttons as in the provided code, replicating EntraGroupsApp's button click assignments while including the additional buttons for mapping, connecting, and opening folders.
                btnSignIn.Click += btnSignIn_Click;
                btnSignOut.Click += btnSignOut_Click;
                btnMapFolder.Click += btnMapFolder_Click;
                btnMaysSharePointConnect.Click += btnMaysSharePointConnect_Click;
                btnOpenSyncFolder.Click += btnOpenSyncFolder_Click;
                Resize += MainForm_Resize;
                FormClosing += MainForm_FormClosing;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize application: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStatusText(string text)
        {
            lblStatus.Text = text;
            CenterControls();
            this.Invalidate();
            this.Update();
        }

        private void CenterControls()
        {
            lblStatus.Location = new Point((ClientSize.Width - lblStatus.Width) / 2, lblStatus.Location.Y);
            flowLayoutPanelButtons.PerformLayout();
            int panelWidth = flowLayoutPanelButtons.Controls.Cast<Control>().Sum(c => c.Width + c.Margin.Left + c.Margin.Right) + flowLayoutPanelButtons.Padding.Left + flowLayoutPanelButtons.Padding.Right;
            flowLayoutPanelButtons.Width = panelWidth;
            flowLayoutPanelButtons.Location = new Point((ClientSize.Width - flowLayoutPanelButtons.Width) / 2, flowLayoutPanelButtons.Location.Y);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterControls();
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            if (_isSigningIn)
                return;

            _isSigningIn = true;
            btnSignIn.Enabled = false;
            SetStatusText("Signing in...");  // Explicitly update status on MainForm to indicate progress, replicating feedback without a separate form.

            try
            {
                if (!string.IsNullOrEmpty(_userEmail))
                {
                    SetStatusText($"Signed in as: {_userEmail}");
                    return;
                }

                // Explicitly define the scopes matching the SharePoint requirement, consistent across silent and interactive.
                var scopes = new[] { "https://tamucs.sharepoint.com/.default" };
                var accounts = await _pca.GetAccountsAsync();
                AuthenticationResult authResult;

                if (accounts.Any())
                {
                    // Explicitly try silent authentication first, replicating the EntraGroupsApp's preference for silent acquisition when possible, to avoid unnecessary UI interaction.
                    authResult = await _pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
                }
                else
                {
                    // Explicitly perform interactive authentication directly here, using the embedded WebView2 configured in the PCA builder, without a separate form.
                    // This triggers the sign-in popup (embedded browser) modally from MainForm, replicating the exact EntraGroupsApp UI experience.
                    // Added .WithPrompt(Prompt.SelectAccount) to allow account selection, and .WithParentActivityOrWindow(this.Handle) to tie the popup to MainForm for better modal behavior.
                    authResult = await _pca.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.SelectAccount)
                        .WithParentActivityOrWindow(this.Handle)
                        .ExecuteAsync();
                }

                // Explicitly store results and update UI on success, without needing a dialog result.
                _authResult = authResult;
                _account = authResult.Account;
                _userEmail = authResult.Account.Username;
                SetStatusText($"Signed in as: {_userEmail}");
                btnSignIn.Visible = false;
                flowLayoutPanelButtons.Visible = true;
                CenterControls();
                this.Invalidate();
                this.Update();
            }
            catch (MsalClientException ex) when (ex.ErrorCode == "loopback_redirect_port_in_use")
            {
                // Explicitly handle port conflict exception, displaying a user-friendly message suggesting resolution steps.
                MessageBox.Show("The localhost port is in use. Close conflicting apps or restart your computer and try again.", "Sign-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatusText("Sign-in failed: Port conflict.");
            }
            catch (MsalServiceException ex) when (ex.StatusCode == 503)
            {
                // Explicitly handle service unavailable exception, advising the user to check internet or retry later.
                MessageBox.Show("Service unavailable during authentication. Check your internet or try later.", "Sign-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatusText("Sign-in failed: Service unavailable.");
            }
            catch (MsalUiRequiredException ex)
            {
                // Explicitly handle UI required exception, showing a message to retry.
                MessageBox.Show($"UI required for sign-in, but failed: {ex.Message}. Please try again.", "Sign-in Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatusText("Sign-in failed.");
            }
            catch (Exception ex)
            {
                // Explicitly handle general exceptions with a message reminding to ensure redirect URI registration in Azure AD.
                MessageBox.Show($"Sign-in failed: {ex.Message}. Ensure the redirect URI is registered in Azure AD.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatusText("Sign-in failed.");
            }
            finally
            {
                _isSigningIn = false;
                btnSignIn.Enabled = true;
            }
        }

        private async void btnSignOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_userEmail))
                    return;

                // Explicitly clear all accounts from the token cache to perform sign-out, replicating the EntraGroupsApp's sign-out logic for complete session termination.
                var accounts = await _pca.GetAccountsAsync();
                foreach (var account in accounts)
                {
                    await _pca.RemoveAsync(account);
                }
                _userEmail = null;
                _account = null;
                _authResult = null;
                btnSignIn.Visible = true;
                flowLayoutPanelButtons.Visible = false;
                SetStatusText("Not signed in");
                CenterControls();
                this.Invalidate();
                this.Update();
                // Explicitly show the SignOutDialog as in the provided code, assuming it's a simple confirmation dialog; if more complex, it can be adjusted accordingly.
                using (var signOutDialog = new SignOutDialog())
                {
                    signOutDialog.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign-out failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMapFolder_Click(object sender, EventArgs e)
        {
            // Explicitly open the FolderMappingForm with authentication details, as in the provided code, for mapping SharePoint folders.
            using (var folderMappingForm = new FolderMappingForm(_pca, _authResult, _account, _userEmail))
            {
                folderMappingForm.ShowDialog();
            }
        }

        private void btnMaysSharePointConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Explicitly open the specified SharePoint site in the default browser, as in the provided code.
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://tamucs.sharepoint.com/sites/MaysSharePointConnect",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Mays SharePoint Connect: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenSyncFolder_Click(object sender, EventArgs e)
        {
            // Explicitly open the sync folder in Windows Explorer if it exists, as in the provided code, with error handling if not found.
            string syncFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Texas A&M University";
            if (Directory.Exists(syncFolder))
            {
                Process.Start("explorer.exe", syncFolder);
            }
            else
            {
                MessageBox.Show("The sync folder 'Texas A&M University' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cleanedUp)
                return;

            // Explicitly clean up event handlers to prevent memory leaks, as in the provided code.
            btnSignIn.Click -= btnSignIn_Click;
            btnSignOut.Click -= btnSignOut_Click;
            btnMapFolder.Click -= btnMapFolder_Click;
            btnMaysSharePointConnect.Click -= btnMaysSharePointConnect_Click;
            btnOpenSyncFolder.Click -= btnOpenSyncFolder_Click;
            Resize -= MainForm_Resize;
            FormClosing -= MainForm_FormClosing;
            _cleanedUp = true;
        }
    }
}