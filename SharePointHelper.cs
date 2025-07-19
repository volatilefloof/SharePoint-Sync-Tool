using Microsoft.SharePoint.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace MaysSharePointSync
{
    public static class SharePointHelper
    {
        public static async Task<(Guid siteId, Guid webId, string webUrl, string webTitle)> GetSiteMetadataAsync(ClientContext context)
        {
            try
            {
                var web = context.Web;
                context.Load(web, w => w.Id, w => w.Url, w => w.Title);
                var site = context.Site;
                context.Load(site, s => s.Id);
                await context.ExecuteQueryAsync();
                return (site.Id, web.Id, web.Url, web.Title);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to get site metadata: {ex.Message}");
                throw;
            }
        }

        public static async Task<List<Microsoft.SharePoint.Client.List>> GetDocumentLibrariesAsync(ClientContext context)
        {
            try
            {
                var web = context.Web;
                var lists = web.Lists;
                context.Load(lists, ls => ls.Include(l => l.Title, l => l.Id, l => l.BaseType, l => l.Hidden));
                await context.ExecuteQueryAsync();
                return lists.Where(l => l.BaseType == BaseType.DocumentLibrary && !l.Hidden).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to get document libraries: {ex.Message}");
                throw;
            }
        }

        public static string BuildOdopenUrl(Guid siteId, Guid webId, Guid listId, string webUrl, string userEmail, string listTitle, string webTitle)
        {
            string tenantName = Uri.EscapeDataString("Texas A&M University"); // Explicitly specify to avoid personal fallback
            return $"odopen://sync/?siteId={Uri.EscapeDataString(siteId.ToString())}" +
                   $"&webId={Uri.EscapeDataString(webId.ToString())}" +
                   $"&listId={Uri.EscapeDataString(listId.ToString())}" +
                   $"&userEmail={Uri.EscapeDataString(userEmail)}" +
                   $"&webUrl={Uri.EscapeDataString(webUrl)}" +
                   $"&listTitle={Uri.EscapeDataString(listTitle)}" +
                   $"&webTitle={Uri.EscapeDataString(webTitle)}" +
                   $"&tenantName={tenantName}"; // Added to ensure business tenant association
        }

        public static bool IsLibrarySynced(string webTitle, string listTitle)
        {
            string oneDrivePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Texas A&M University";
            string expectedFolder = $@"{oneDrivePath}\{webTitle} - {listTitle}";
            try
            {
                bool exists = Directory.Exists(expectedFolder);
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Checking if library is synced: {expectedFolder} - Exists: {exists}");
                return exists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error checking if library is synced: {ex.Message}");
                return false;
            }
        }

        public static void StartSync(string odopenUrl)
        {
            try
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting sync with URL: {odopenUrl}");
                Process.Start(new ProcessStartInfo
                {
                    FileName = odopenUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to start sync: {ex.Message}");
                throw;
            }
        }
    }
}