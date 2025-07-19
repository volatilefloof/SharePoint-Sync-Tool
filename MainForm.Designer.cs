namespace MaysSharePointSync
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnSignIn = new Button();
            flowLayoutPanelButtons = new FlowLayoutPanel();
            btnMapFolder = new Button();
            btnOpenSyncFolder = new Button();
            btnMaysSharePointConnect = new Button();
            btnSignOut = new Button();
            lblStatus = new Label();
            flowLayoutPanelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // btnSignIn
            // 
            btnSignIn.Font = new Font("Segoe UI", 10F);
            btnSignIn.Location = new Point(284, 172);
            btnSignIn.Margin = new Padding(3, 2, 3, 2);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(131, 30);
            btnSignIn.TabIndex = 0;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelButtons
            // 
            flowLayoutPanelButtons.AutoSize = true;
            flowLayoutPanelButtons.Controls.Add(btnMapFolder);
            flowLayoutPanelButtons.Controls.Add(btnOpenSyncFolder);
            flowLayoutPanelButtons.Controls.Add(btnMaysSharePointConnect);
            flowLayoutPanelButtons.Controls.Add(btnSignOut);
            flowLayoutPanelButtons.Location = new Point(0, 150);
            flowLayoutPanelButtons.Margin = new Padding(3, 2, 3, 2);
            flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            flowLayoutPanelButtons.Size = new Size(681, 34);
            flowLayoutPanelButtons.TabIndex = 1;
            flowLayoutPanelButtons.Visible = false;
            // 
            // btnMapFolder
            // 
            btnMapFolder.Font = new Font("Segoe UI", 9F);
            btnMapFolder.Location = new Point(4, 4);
            btnMapFolder.Margin = new Padding(4);
            btnMapFolder.Name = "btnMapFolder";
            btnMapFolder.Size = new Size(158, 26);
            btnMapFolder.TabIndex = 1;
            btnMapFolder.Text = "Add a SharePoint Folder";
            btnMapFolder.UseVisualStyleBackColor = true;
            // 
            // btnOpenSyncFolder
            // 
            btnOpenSyncFolder.Font = new Font("Segoe UI", 9F);
            btnOpenSyncFolder.Location = new Point(170, 4);
            btnOpenSyncFolder.Margin = new Padding(4);
            btnOpenSyncFolder.Name = "btnOpenSyncFolder";
            btnOpenSyncFolder.Size = new Size(158, 26);
            btnOpenSyncFolder.TabIndex = 2;
            btnOpenSyncFolder.Text = "Open My Folder(s)";
            btnOpenSyncFolder.UseVisualStyleBackColor = true;
            // 
            // btnMaysSharePointConnect
            // 
            btnMaysSharePointConnect.Font = new Font("Segoe UI", 9F);
            btnMaysSharePointConnect.Location = new Point(336, 4);
            btnMaysSharePointConnect.Margin = new Padding(4);
            btnMaysSharePointConnect.Name = "btnMaysSharePointConnect";
            btnMaysSharePointConnect.Size = new Size(175, 26);
            btnMaysSharePointConnect.TabIndex = 4;
            btnMaysSharePointConnect.Text = "Mays SharePoint Connect";
            btnMaysSharePointConnect.UseVisualStyleBackColor = true;
            // 
            // btnSignOut
            // 
            btnSignOut.Font = new Font("Segoe UI", 9F);
            btnSignOut.Location = new Point(519, 4);
            btnSignOut.Margin = new Padding(4);
            btnSignOut.Name = "btnSignOut";
            btnSignOut.Size = new Size(158, 26);
            btnSignOut.TabIndex = 3;
            btnSignOut.Text = "Sign Out";
            btnSignOut.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatus.Location = new Point(306, 60);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(113, 21);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Not signed in";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 300);
            Controls.Add(lblStatus);
            Controls.Add(btnSignIn);
            Controls.Add(flowLayoutPanelButtons);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mays SharePoint Sync Tool";
            Load += MainForm_Load;
            flowLayoutPanelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Button btnSignOut;
        private System.Windows.Forms.Button btnMapFolder;
        private System.Windows.Forms.Button btnMaysSharePointConnect;
        private System.Windows.Forms.Button btnOpenSyncFolder;
        private System.Windows.Forms.Label lblStatus;
    }
}