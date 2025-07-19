using Microsoft.Identity.Client;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaysSharePointSync
{
    public partial class FolderMappingForm : System.Windows.Forms.Form
    {
        private readonly IPublicClientApplication _pca;
        private AuthenticationResult _authResult;
        private readonly IAccount _account;
        private readonly string _userEmail;
        private readonly Dictionary<string, string> _departments = new Dictionary<string, string>
        {
            { "BUSP", "https://tamucs.sharepoint.com/teams/BUSPTeamSite" },
            { "UPO", "https://tamucs.sharepoint.com/teams/UPOTeamSite" },
            { "ACCT", "https://tamucs.sharepoint.com/teams/AccountingTeamSite" },
            { "MBA Programs", "https://tamucs.sharepoint.com/teams/BizGradTeamSite" },
            { "DEAN", "https://tamucs.sharepoint.com/teams/DeansOfficeDrive" },
            { "CED", "https://tamucs.sharepoint.com/teams/team-CEDTeamSite" },
            { "MKTG", "https://tamucs.sharepoint.com/teams/MKTGTeamSite" },
            { "INFO", "https://tamucs.sharepoint.com/teams/INFOTeamSite" },
            { "MEDIA", "https://tamucs.sharepoint.com/teams/MediaTeamSite" },
            { "CIBS", "https://tamucs.sharepoint.com/teams/CIBSTeamSite" },
            { "MGMT", "https://tamucs.sharepoint.com/teams/MGMTTeamSite" }
        };
        private CancellationTokenSource _cts;
        private List<FolderItem> _allFolders;
        private readonly List<string> _excludedLibraries = new List<string> { "Form Templates", "Styles", "Site Assets", "Site Pages", "Style Library", "Documents" };

        private class FolderItem
        {
            public string Title { get; set; }
            public Guid Id { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }

        public FolderMappingForm(IPublicClientApplication pca, AuthenticationResult authResult, IAccount account, string userEmail)
        {
            _pca = pca;
            _authResult = authResult;
            _account = account;
            _userEmail = userEmail;
            _cts = new CancellationTokenSource();
            _allFolders = new List<FolderItem>();
            InitializeComponent();
        }

        private async Task<string> GetValidAccessToken(CancellationToken cancellationToken)
        {
            if (_authResult != null && _authResult.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(5))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(_authResult.AccessToken);
                    var audience = jwtToken.Audiences.FirstOrDefault() ?? "Unknown";
                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using existing token with audience: {audience}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to parse JWT: {ex.Message}");
                }
                return _authResult.AccessToken;
            }

            try
            {
                var scopes = new[] { "https://graph.microsoft.com/.default" };
                _authResult = await _pca.AcquireTokenSilent(scopes, _account).ExecuteAsync(cancellationToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(_authResult.AccessToken);
                var audience = jwtToken.Audiences.FirstOrDefault() ?? "Unknown";
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] New token acquired with audience: {audience}");
                return _authResult.AccessToken;
            }
            catch (MsalUiRequiredException ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Token refresh failed: {ex.Message}");
                throw new Exception("Token refresh failed. Please sign in again.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Token acquisition failed: {ex.Message}");
                throw;
            }
        }

        private async void FolderMappingForm_Load(object sender, EventArgs e)
        {
            try
            {
                cmbDepartment.Items.AddRange(_departments.Keys.ToArray());
                cmbDepartment.SelectedIndex = -1;
                btnSync.Enabled = false;
                btnSelectAll.Enabled = false;
                lstFolders.Enabled = false;
                progressBar.Visible = false;
                lblStatus.Text = "Please select a department.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error initializing form.";
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to initialize form: {ex.Message}");
            }
        }

        private async void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDepartment.SelectedIndex == -1)
            {
                btnSync.Enabled = false;
                btnSelectAll.Enabled = false;
                lstFolders.Enabled = false;
                progressBar.Visible = false;
                txtSearch.Text = string.Empty;
                lstFolders.Items.Clear();
                lblStatus.Text = "Please select a department.";
                return;
            }
            btnSync.Enabled = false;
            btnSelectAll.Enabled = true;
            lstFolders.Enabled = false;
            progressBar.Visible = false;
            txtSearch.Text = string.Empty;
            await LoadFoldersAsync();
        }

        private async Task LoadFoldersAsync()
        {
            try
            {
                lblStatus.Text = "Loading folders...";
                using (var context = new ClientContext(_departments[cmbDepartment.SelectedItem.ToString()]))
                {
                    context.ExecutingWebRequest += (s, ev) =>
                    {
                        var token = GetValidAccessToken(_cts.Token).Result;
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using access token: {token.Substring(0, 20)}...");
                        ev.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + token;
                    };

                    var libraries = await SharePointHelper.GetDocumentLibrariesAsync(context);
                    _allFolders.Clear();
                    _allFolders.AddRange(libraries.Where(l => !_excludedLibraries.Any(excluded => l.Title.IndexOf(excluded, StringComparison.OrdinalIgnoreCase) >= 0)).Select(l => new FolderItem { Title = l.Title, Id = l.Id }));
                    UpdateFolderList(string.Empty);
                    lstFolders.Enabled = libraries.Any(l => !_excludedLibraries.Any(excluded => l.Title.IndexOf(excluded, StringComparison.OrdinalIgnoreCase) >= 0));
                    btnSync.Enabled = libraries.Any(l => !_excludedLibraries.Any(excluded => l.Title.IndexOf(excluded, StringComparison.OrdinalIgnoreCase) >= 0));
                    btnSelectAll.Enabled = libraries.Any(l => !_excludedLibraries.Any(excluded => l.Title.IndexOf(excluded, StringComparison.OrdinalIgnoreCase) >= 0));
                    lblStatus.Text = "Select folders to sync.";
                }
            }
            catch (ClientRequestException ex) when (ex.InnerException is WebException webEx && ((HttpWebResponse)webEx.Response)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Unauthorized access to department: {ex.Message}");
                await TryLoadAccessibleLibraries(_departments[cmbDepartment.SelectedItem.ToString()]);
            }
            catch (Exception ex) when (ex.Message.Contains("attempt to perform an unauthorized operation"))
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Unauthorized operation: {ex.Message}");
                await TryLoadAccessibleLibraries(_departments[cmbDepartment.SelectedItem.ToString()]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Load folders failed: {ex.Message}");
                MessageBox.Show($"Failed to load folders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading folders.";
            }
        }

        private async Task TryLoadAccessibleLibraries(string siteUrl)
        {
            try
            {
                using (var context = new ClientContext(siteUrl))
                {
                    context.ExecutingWebRequest += (s, ev) =>
                    {
                        var token = GetValidAccessToken(_cts.Token).Result;
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using access token for individual library check: {token.Substring(0, 20)}...");
                        ev.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + token;
                    };

                    var knownLibraries = new List<string> { "debbie" };
                    var accessibleLibraries = new List<FolderItem>();

                    foreach (var libraryName in knownLibraries)
                    {
                        try
                        {
                            var list = context.Web.Lists.GetByTitle(libraryName);
                            context.Load(list, l => l.Title, l => l.Id);
                            await context.ExecuteQueryAsync();
                            if (!_excludedLibraries.Any(excluded => list.Title.IndexOf(excluded, StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                accessibleLibraries.Add(new FolderItem { Title = list.Title, Id = list.Id });
                                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Successfully accessed library: {libraryName}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to access library {libraryName}: {ex.Message}");
                        }
                    }

                    _allFolders.Clear();
                    _allFolders.AddRange(accessibleLibraries);
                    UpdateFolderList(string.Empty);
                    lstFolders.Enabled = accessibleLibraries.Any();
                    btnSync.Enabled = accessibleLibraries.Any();
                    btnSelectAll.Enabled = accessibleLibraries.Any();
                    lblStatus.Text = accessibleLibraries.Any() ? "Select folders to sync." : "No access to this department.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to load accessible libraries: {ex.Message}");
                _allFolders.Clear();
                UpdateFolderList(string.Empty);
                lstFolders.Enabled = false;
                btnSync.Enabled = false;
                btnSelectAll.Enabled = false;
                lblStatus.Text = "No access to this department.";
            }
        }

        private void UpdateFolderList(string searchText)
        {
            lstFolders.Items.Clear();
            var filteredFolders = string.IsNullOrEmpty(searchText)
                ? _allFolders
                : _allFolders.Where(f => f.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            foreach (var library in filteredFolders)
            {
                lstFolders.Items.Add(library);
            }
            if (_allFolders.Count == 0 && lstFolders.Items.Count == 0)
            {
                lstFolders.Items.Add(new FolderItem { Title = "No access.", Id = Guid.Empty });
            }
            lstFolders.Enabled = filteredFolders.Any();
            btnSync.Enabled = lstFolders.Items.Count > 0 && lstFolders.SelectedItems.Count > 0 && filteredFolders.Any();
            btnSelectAll.Enabled = filteredFolders.Any();
            lstFolders.DrawMode = DrawMode.OwnerDrawFixed;
            lstFolders.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                e.DrawBackground();
                var item = (FolderItem)lstFolders.Items[e.Index];
                bool isNoAccess = item.Title.Contains("No access");
                var font = isNoAccess ? new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic) : new System.Drawing.Font("Segoe UI", 9F);
                e.Graphics.DrawString(item.Title, font, System.Drawing.Brushes.Black, e.Bounds);
                e.DrawFocusRectangle();
            };
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateFolderList(txtSearch.Text);
        }

        private void lstFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSync.Enabled = lstFolders.SelectedItems.Count > 0 && _allFolders.Any();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstFolders.Items.Count; i++)
            {
                if (!((FolderItem)lstFolders.Items[i]).Title.Contains("No access"))
                {
                    lstFolders.SetSelected(i, true);
                }
            }
            btnSync.Enabled = lstFolders.SelectedItems.Count > 0 && _allFolders.Any();
        }

        private void btnOpenSharePointFolder_Click(object sender, EventArgs e)
        {
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

        private async Task<bool> VerifyFolderExists(string webTitle, string listTitle, CancellationToken cancellationToken)
        {
            string oneDrivePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Texas A&M University";
            string expectedFolder = $@"{oneDrivePath}\{webTitle} - {listTitle}";
            int maxAttempts = 20;
            int delayMs = 3000;

            try
            {
                if (!Process.GetProcessesByName("OneDrive").Any())
                {
                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] OneDrive process not running");
                    return false;
                }

                string tempFile = Path.Combine(oneDrivePath, $"test_sync_{Guid.NewGuid()}.tmp");
                try
                {
                    System.IO.File.WriteAllText(tempFile, "test");
                    await Task.Delay(500, cancellationToken);
                    if (!System.IO.File.Exists(tempFile))
                    {
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] OneDrive sync test failed: Temporary file not found (possible dialog closure)");
                        return false;
                    }
                    System.IO.File.Delete(tempFile);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] OneDrive sync test error: {ex.Message} (possible dialog closure)");
                    return false;
                }

                int noProgressCount = 0;
                string lastStatus = null;
                for (int i = 0; i < maxAttempts; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested(); // Added for more robust cancellation

                    try
                    {
                        var directories = Directory.GetDirectories(oneDrivePath, $"{webTitle} - {listTitle}*", SearchOption.TopDirectoryOnly);
                        string currentStatus = directories.Any() ? directories.FirstOrDefault() : "Not found";
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Checking folder: {expectedFolder}, status: {currentStatus}, attempt {i + 1}/{maxAttempts}");
                        if (directories.Any(dir => dir.ToLower().Contains(expectedFolder.ToLower())))
                        {
                            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Folder verified: {expectedFolder}");
                            return true;
                        }
                        if (currentStatus == lastStatus)
                        {
                            noProgressCount++;
                            if (noProgressCount > 5)
                            {
                                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] No progress detected for {expectedFolder}, breaking after {i + 1}/{maxAttempts} attempts");
                                break;
                            }
                        }
                        else
                        {
                            noProgressCount = 0;
                        }
                        lastStatus = currentStatus;
                        lblStatus.Text = $"Verifying if '{listTitle}' has synced to OneDrive... (Progress: {((i + 1) * 5)}%)"; // Changed text for better robustness and user-friendliness
                        Application.DoEvents();
                        await Task.Delay(delayMs, cancellationToken);
                        delayMs = Math.Min(delayMs + 2000, 60000);
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Verification cancelled for '{listTitle}'");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Folder verification error: {ex.Message}, attempt {i + 1}/{maxAttempts}");
                        lblStatus.Text = $"Verification in progress for '{listTitle}'... (Error occurred, retrying)";
                        Application.DoEvents();
                        await Task.Delay(delayMs, cancellationToken);
                        delayMs = Math.Min(delayMs + 2000, 60000);
                    }
                }
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Folder verification failed after {maxAttempts} attempts: {expectedFolder}");
                return false;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] VerifyFolderExists cancelled");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error in VerifyFolderExists: {ex.Message}");
                return false;
            }
        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            try
            {
                btnSync.Enabled = false;
                btnReturn.Text = "Cancel";
                btnReturn.Enabled = true;
                lblStatus.Text = "Preparing to sync folders...";
                progressBar.Visible = true;
                progressBar.Value = 0;
                progressBar.Maximum = lstFolders.SelectedItems.Count;

                string oneDrivePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Texas A&M University";

                if (!Directory.Exists(oneDrivePath))
                {
                    var result = MessageBox.Show("The OneDrive business sync folder 'Texas A&M University' does not exist. Please sign in to OneDrive with your work/school account (e.g., your @tamu.edu email) and ensure it's set up for syncing SharePoint libraries. Do you want to continue anyway?", "OneDrive Setup Required", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        lblStatus.Text = "Sync cancelled: OneDrive business account not set up.";
                        return;
                    }
                }

                if (!Process.GetProcessesByName("OneDrive").Any())
                {
                    MessageBox.Show("OneDrive is not running. Please start OneDrive and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Sync failed: OneDrive not running.";
                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync failed: OneDrive not running");
                    return;
                }

                using (var context = new ClientContext(_departments[cmbDepartment.SelectedItem.ToString()]))
                {
                    context.ExecutingWebRequest += (s, ev) =>
                    {
                        var token = GetValidAccessToken(_cts.Token).Result;
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Using access token: {token.Substring(0, 20)}...");
                        ev.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + token;
                    };

                    var syncTasks = new List<(string ListTitle, string OdopenUrl)>();
                    var selectedLibraries = new List<FolderItem>();
                    foreach (var item in lstFolders.SelectedItems)
                    {
                        var folderItem = (FolderItem)item;
                        if (!folderItem.Title.Contains("No access"))
                        {
                            selectedLibraries.Add(folderItem);
                        }
                    }

                    (Guid siteId, Guid webId, string webUrl, string webTitle) = await SharePointHelper.GetSiteMetadataAsync(context);

                    foreach (var library in selectedLibraries)
                    {
                        string listTitle = library.Title;
                        Guid listId = library.Id;

                        if (SharePointHelper.IsLibrarySynced(webTitle, listTitle))
                        {
                            var result = MessageBox.Show($"The folder '{listTitle}' appears to be already synced. Do you want to proceed with a new sync?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.No)
                            {
                                lblStatus.Text = $"Sync skipped for '{listTitle}' ({progressBar.Value + 1}/{progressBar.Maximum}).";
                                progressBar.Value++;
                                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync skipped for '{listTitle}'");
                                continue;
                            }
                        }

                        string odopenUrl = SharePointHelper.BuildOdopenUrl(siteId, webId, listId, webUrl, _userEmail, listTitle, webTitle);
                        syncTasks.Add((listTitle, odopenUrl));
                    }

                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting sync for {syncTasks.Count} folders");
                    int syncedCount = 0;
                    int currentFolder = 0;

                    foreach (var (listTitle, odopenUrl) in syncTasks)
                    {
                        if (_cts.Token.IsCancellationRequested)
                        {
                            lblStatus.Text = "Sync process cancelled.";
                            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync cancelled by user");
                            break;
                        }

                        currentFolder++;
                        lblStatus.Text = $"Initiating sync for '{listTitle}' ({currentFolder}/{syncTasks.Count})...";
                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Processing folder {currentFolder}/{syncTasks.Count}: {listTitle}");
                        Application.DoEvents();

                        int maxRetries = 4;
                        bool syncSuccess = false;
                        try
                        {
                            for (int retry = 0; retry < maxRetries && !syncSuccess && !_cts.Token.IsCancellationRequested; retry++)
                            {
                                _cts.Token.ThrowIfCancellationRequested(); // Added for more robust cancellation

                                try
                                {
                                    lblStatus.Text = $"Attempting to start sync for '{listTitle}' (Retry {retry + 1} of {maxRetries}, Folder {currentFolder}/{syncTasks.Count})..."; // Changed text for clarity
                                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Attempt {retry + 1}/{maxRetries} for '{listTitle}' with URL: {odopenUrl}");
                                    SharePointHelper.StartSync(odopenUrl);
                                    lblStatus.Text = $"Verifying sync for '{listTitle}' (Folder {currentFolder}/{syncTasks.Count})...";
                                    bool folderExists = await VerifyFolderExists(webTitle, listTitle, _cts.Token);
                                    if (folderExists)
                                    {
                                        lblStatus.Text = $"Sync completed successfully for '{listTitle}' ({currentFolder}/{syncTasks.Count}).";
                                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync succeeded for '{listTitle}'");
                                        syncedCount++;
                                        syncSuccess = true;
                                        progressBar.Value++;
                                        await Task.Delay(4000, _cts.Token);
                                    }
                                    else
                                    {
                                        lblStatus.Text = $"Sync verification in progress for '{listTitle}' (Retry {retry + 1} of {maxRetries}, Folder {currentFolder}/{syncTasks.Count})...";
                                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync pending for '{listTitle}': Folder not found after verification, retry {retry + 1}/{maxRetries}");
                                        if (retry < maxRetries - 1)
                                        {
                                            await Task.Delay(4000 * (retry + 1), _cts.Token);
                                        }
                                    }
                                }
                                catch (OperationCanceledException)
                                {
                                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync attempt cancelled for '{listTitle}'");
                                    lblStatus.Text = $"Sync cancelled for '{listTitle}' ({currentFolder}/{syncTasks.Count}).";
                                    break;
                                }
                                catch (Exception ex) when (ex is System.ComponentModel.Win32Exception)
                                {
                                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync interrupted for '{listTitle}': {ex.Message} (possible OneDrive dialog closure)");
                                    lblStatus.Text = $"Sync cancelled for '{listTitle}' ({currentFolder}/{syncTasks.Count}) due to dialog closure.";
                                    _cts.Cancel();
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync attempt {retry + 1}/{maxRetries} failed for '{listTitle}': {ex.Message}");
                                    lblStatus.Text = $"Error during sync for '{listTitle}' (Retry {retry + 1} of {maxRetries}, Folder {currentFolder}/{syncTasks.Count}): {ex.Message}";
                                    if (retry < maxRetries - 1)
                                    {
                                        await Task.Delay(4000 * (retry + 1), _cts.Token);
                                    }
                                    else
                                    {
                                        lblStatus.Text = $"Sync failed for '{listTitle}' after all retries ({currentFolder}/{syncTasks.Count}).";
                                        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Sync failed after {maxRetries} retries for '{listTitle}': {ex.Message}");
                                    }
                                }
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            lblStatus.Text = $"Sync process cancelled during '{listTitle}' ({currentFolder}/{syncTasks.Count}).";
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Unexpected error during sync for '{listTitle}': {ex.Message}");
                            lblStatus.Text = $"Unexpected error during sync for '{listTitle}' ({currentFolder}/{syncTasks.Count}): {ex.Message}";
                        }
                        if (!syncSuccess && !_cts.Token.IsCancellationRequested)
                        {
                            lblStatus.Text = $"Sync failed for '{listTitle}' after all retries ({currentFolder}/{syncTasks.Count}).";
                            progressBar.Value++;
                        }
                    }

                    if (_cts.Token.IsCancellationRequested)
                    {
                        MessageBox.Show("The sync process was cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (syncedCount > 0)
                    {
                        MessageBox.Show($"Sync initiated successfully for {syncedCount} folder(s)!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No folders were synced.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("The sync process was cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatus.Text = "Sync cancelled.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sync failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Sync failed.";
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] General sync error: {ex.Message}");
            }
            finally
            {
                btnSync.Enabled = lstFolders.SelectedItems.Count > 0 && _allFolders.Any();
                btnSelectAll.Enabled = _allFolders.Any();
                btnReturn.Text = "Return to Main Homepage";
                btnReturn.Enabled = true;
                progressBar.Visible = false;
                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (btnReturn.Text == "Cancel" && btnReturn.Enabled)
            {
                _cts.Cancel();
                lblStatus.Text = "Cancelling sync process...";
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }
    }
}