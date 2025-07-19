namespace MaysSharePointSync
{
    partial class FolderMappingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Panel sidebarPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Label lblFolders;
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnOpenSharePointFolder;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblMultiSelect;
        private System.Windows.Forms.TextBox txtSearch;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderMappingForm));
            mainLayoutPanel = new TableLayoutPanel();
            sidebarPanel = new Panel();
            btnReturn = new Button();
            btnOpenSharePointFolder = new Button();
            btnSelectAll = new Button();
            btnSync = new Button();
            cmbDepartment = new ComboBox();
            lblDepartment = new Label();
            contentPanel = new Panel();
            progressBar = new ProgressBar();
            lblStatus = new Label();
            lstFolders = new ListBox();
            lblMultiSelect = new Label();
            txtSearch = new TextBox();
            lblFolders = new Label();
            mainLayoutPanel.SuspendLayout();
            sidebarPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            mainLayoutPanel.ColumnCount = 2;
            mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 219F));
            mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayoutPanel.Controls.Add(sidebarPanel, 0, 0);
            mainLayoutPanel.Controls.Add(contentPanel, 1, 0);
            mainLayoutPanel.Dock = DockStyle.Fill;
            mainLayoutPanel.Location = new Point(0, 0);
            mainLayoutPanel.Margin = new Padding(3, 2, 3, 2);
            mainLayoutPanel.Name = "mainLayoutPanel";
            mainLayoutPanel.RowCount = 1;
            mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayoutPanel.Size = new Size(788, 450);
            mainLayoutPanel.TabIndex = 0;
            // 
            // sidebarPanel
            // 
            sidebarPanel.BackColor = Color.FromArgb(240, 240, 240);
            sidebarPanel.Controls.Add(btnReturn);
            sidebarPanel.Controls.Add(btnOpenSharePointFolder);
            sidebarPanel.Controls.Add(btnSelectAll);
            sidebarPanel.Controls.Add(btnSync);
            sidebarPanel.Controls.Add(cmbDepartment);
            sidebarPanel.Controls.Add(lblDepartment);
            sidebarPanel.Dock = DockStyle.Fill;
            sidebarPanel.Location = new Point(3, 2);
            sidebarPanel.Margin = new Padding(3, 2, 3, 2);
            sidebarPanel.Name = "sidebarPanel";
            sidebarPanel.Size = new Size(213, 446);
            sidebarPanel.TabIndex = 0;
            // 
            // btnReturn
            // 
            btnReturn.Font = new Font("Segoe UI", 9F);
            btnReturn.Location = new Point(18, 214);
            btnReturn.Margin = new Padding(3, 2, 3, 2);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(175, 26);
            btnReturn.TabIndex = 5;
            btnReturn.Text = "Return to Main Homepage";
            btnReturn.UseVisualStyleBackColor = true;
            btnReturn.Click += btnReturn_Click;
            // 
            // btnOpenSharePointFolder
            // 
            btnOpenSharePointFolder.Font = new Font("Segoe UI", 9F);
            btnOpenSharePointFolder.Location = new Point(18, 180);
            btnOpenSharePointFolder.Margin = new Padding(3, 2, 3, 2);
            btnOpenSharePointFolder.Name = "btnOpenSharePointFolder";
            btnOpenSharePointFolder.Size = new Size(175, 26);
            btnOpenSharePointFolder.TabIndex = 4;
            btnOpenSharePointFolder.Text = "Open SharePoint Folder";
            btnOpenSharePointFolder.UseVisualStyleBackColor = true;
            btnOpenSharePointFolder.Click += btnOpenSharePointFolder_Click;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Font = new Font("Segoe UI", 9F);
            btnSelectAll.Location = new Point(18, 146);
            btnSelectAll.Margin = new Padding(3, 2, 3, 2);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(175, 26);
            btnSelectAll.TabIndex = 3;
            btnSelectAll.Text = "Select All";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // btnSync
            // 
            btnSync.Font = new Font("Segoe UI", 9F);
            btnSync.Location = new Point(18, 112);
            btnSync.Margin = new Padding(3, 2, 3, 2);
            btnSync.Name = "btnSync";
            btnSync.Size = new Size(175, 26);
            btnSync.TabIndex = 2;
            btnSync.Text = "Add Folder(s)";
            btnSync.UseVisualStyleBackColor = true;
            btnSync.Click += btnSync_Click;
            // 
            // cmbDepartment
            // 
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDepartment.Font = new Font("Segoe UI", 9F);
            cmbDepartment.Location = new Point(18, 45);
            cmbDepartment.Margin = new Padding(3, 2, 3, 2);
            cmbDepartment.Name = "cmbDepartment";
            cmbDepartment.Size = new Size(176, 23);
            cmbDepartment.TabIndex = 1;
            cmbDepartment.SelectedIndexChanged += cmbDepartment_SelectedIndexChanged;
            // 
            // lblDepartment
            // 
            lblDepartment.AutoSize = true;
            lblDepartment.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDepartment.Location = new Point(18, 22);
            lblDepartment.Name = "lblDepartment";
            lblDepartment.Size = new Size(93, 19);
            lblDepartment.TabIndex = 0;
            lblDepartment.Text = "Department:";
            lblDepartment.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(progressBar);
            contentPanel.Controls.Add(lblStatus);
            contentPanel.Controls.Add(lstFolders);
            contentPanel.Controls.Add(lblMultiSelect);
            contentPanel.Controls.Add(txtSearch);
            contentPanel.Controls.Add(lblFolders);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(222, 2);
            contentPanel.Margin = new Padding(3, 2, 3, 2);
            contentPanel.Name = "contentPanel";
            contentPanel.Size = new Size(563, 446);
            contentPanel.TabIndex = 1;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(18, 371);
            progressBar.Margin = new Padding(3, 2, 3, 2);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(528, 17);
            progressBar.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.Location = new Point(18, 349);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Status:";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lstFolders
            // 
            lstFolders.Enabled = false;
            lstFolders.Font = new Font("Segoe UI", 9F);
            lstFolders.ItemHeight = 15;
            lstFolders.Location = new Point(18, 112);
            lstFolders.Margin = new Padding(3, 2, 3, 2);
            lstFolders.Name = "lstFolders";
            lstFolders.ScrollAlwaysVisible = true;
            lstFolders.SelectionMode = SelectionMode.MultiExtended;
            lstFolders.Size = new Size(529, 214);
            lstFolders.TabIndex = 4;
            lstFolders.SelectedIndexChanged += lstFolders_SelectedIndexChanged;
            // 
            // lblMultiSelect
            // 
            lblMultiSelect.AutoSize = true;
            lblMultiSelect.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblMultiSelect.Location = new Point(18, 90);
            lblMultiSelect.Name = "lblMultiSelect";
            lblMultiSelect.Size = new Size(206, 13);
            lblMultiSelect.TabIndex = 3;
            lblMultiSelect.Text = "Hold Ctrl or Shift to select multiple folders";
            lblMultiSelect.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Location = new Point(18, 60);
            txtSearch.Margin = new Padding(3, 2, 3, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search folders...";
            txtSearch.Size = new Size(529, 23);
            txtSearch.TabIndex = 2;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblFolders
            // 
            lblFolders.AutoSize = true;
            lblFolders.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblFolders.Location = new Point(18, 38);
            lblFolders.Name = "lblFolders";
            lblFolders.Size = new Size(62, 19);
            lblFolders.TabIndex = 0;
            lblFolders.Text = "Folders:";
            lblFolders.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FolderMappingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(788, 450);
            ControlBox = false;
            Controls.Add(mainLayoutPanel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "FolderMappingForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add SharePoint Folder(s)";
            Load += FolderMappingForm_Load;
            mainLayoutPanel.ResumeLayout(false);
            sidebarPanel.ResumeLayout(false);
            sidebarPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            contentPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}