using Microsoft.Web.WebView2.WinForms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaysSharePointSync
{
    public partial class WebViewForm : Form
    {
        private readonly TaskCompletionSource<Uri> _tcs;
        public WebView2 WebView2Control { get; private set; }

        public WebViewForm(string title)
        {
            _tcs = new TaskCompletionSource<Uri>();
            InitializeComponent();
            Text = title;
            WebView2Control = webView2Control;
        }

        public async Task<Uri> NavigateAsync(string url)
        {
            WebView2Control.Source = new Uri(url);
            return await _tcs.Task;
        }

        private void OnNavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.Uri.StartsWith("http://localhost:57672", StringComparison.OrdinalIgnoreCase))
            {
                _tcs.SetResult(new Uri(e.Uri));
                this.Close();
            }
        }
    }
}