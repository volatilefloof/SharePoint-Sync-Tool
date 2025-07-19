using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaysSharePointSync
{
    public partial class SignInForm : Form
    {
        private readonly IPublicClientApplication _pca;
        public AuthenticationResult AuthenticationResult { get; private set; }
        public IAccount AuthenticatedAccount { get; private set; }
        public string AuthenticatedEmail { get; private set; }

        public SignInForm(IPublicClientApplication pca)
        {
            _pca = pca ?? throw new ArgumentNullException(nameof(pca));
            InitializeComponent();
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            try
            {
                btnSignIn.Enabled = false;
                lblStatus.Text = "Signing in...";

                // Explicitly define the scopes matching the SharePoint requirement, consistent with the standardized scopes in MainForm.
                var scopes = new[] { "https://tamucs.sharepoint.com/.default" };
                var accounts = await _pca.GetAccountsAsync();
                AuthenticationResult authResult;

                if (accounts.Any())
                {
                    // Explicitly try silent authentication first, replicating the EntraGroupsApp's preference for silent acquisition when possible, to avoid unnecessary UI interaction.
                    authResult = await _pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                }
                else
                {
                    // Explicitly fallback to interactive authentication using the embedded WebView2 configured in the PCA builder, without additional options, as the embedded support handles the UI, replicating the exact embedded browser sign-in experience from EntraGroupsApp.
                    // Added .WithPrompt(Prompt.SelectAccount) to allow account selection, matching the provided code's enhancement for user choice.
                    // Added .WithParentActivityOrWindow(this.Handle) to tie the authentication window to the sign-in form, improving UX by making it modal-like.
                    authResult = await _pca.AcquireTokenInteractive(scopes)
                        .WithPrompt(Prompt.SelectAccount)
                        .WithParentActivityOrWindow(this.Handle)
                        .ExecuteAsync();
                }

                AuthenticationResult = authResult;
                AuthenticatedAccount = authResult.Account;
                AuthenticatedEmail = authResult.Account.Username;
                lblStatus.Text = "Signed in successfully.";
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Explicitly handle exceptions in the sign-in form, showing errors and re-enabling the button, but propagating to MainForm for further handling if needed.
                MessageBox.Show($"Sign-in failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Sign-in failed.";
                btnSignIn.Enabled = true;
            }
        }
    }
}