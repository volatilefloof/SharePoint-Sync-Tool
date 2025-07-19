namespace MaysSharePointSync
{
    partial class WebViewForm
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2Control;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.webView2Control = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webView2Control
            // 
            this.webView2Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2Control.Location = new System.Drawing.Point(0, 0);
            this.webView2Control.Name = "webView2Control";
            this.webView2Control.Size = new System.Drawing.Size(1280, 670);
            this.webView2Control.TabIndex = 0;
            this.webView2Control.NavigationStarting += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs>(this.OnNavigationStarting);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.Location = new System.Drawing.Point(1180, 676);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // WebViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 705);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.webView2Control);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "WebViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web View";
            this.Load += new System.EventHandler(this.WebViewForm_Load);
            this.ResumeLayout(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WebViewForm_Load(object sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size((int)(screen.Width * 0.8), (int)(screen.Height * 0.8));
            this.Location = new Point((screen.Width - this.Width) / 2, (screen.Height - this.Height) / 2);
        }
    }
}