namespace MaysSharePointSync
{
    partial class SignOutDialog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblMessage1;
        private System.Windows.Forms.Label lblMessage2;
        private System.Windows.Forms.TableLayoutPanel buttonPanel;
        private System.Windows.Forms.Button btnOK;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignOutDialog));
            tableLayoutPanel = new TableLayoutPanel();
            lblMessage1 = new Label();
            lblMessage2 = new Label();
            buttonPanel = new TableLayoutPanel();
            btnOK = new Button();
            tableLayoutPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Controls.Add(lblMessage1, 0, 0);
            tableLayoutPanel.Controls.Add(lblMessage2, 0, 1);
            tableLayoutPanel.Controls.Add(buttonPanel, 0, 2);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 3;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Reduced from 46F to 30F for closer text spacing
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));  // Reduced from 46F to 30F for closer text spacing
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // Reduced from 81F/75 to 50F to fit tighter layout
            tableLayoutPanel.Size = new Size(350, 110);  // Adjusted total size to match new row heights (30+30+50=110)
            tableLayoutPanel.TabIndex = 0;
            // 
            // lblMessage1
            // 
            lblMessage1.AutoSize = true;
            lblMessage1.Dock = DockStyle.Fill;
            lblMessage1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage1.Location = new Point(4, 0);
            lblMessage1.Margin = new Padding(4, 0, 4, 0);
            lblMessage1.Name = "lblMessage1";
            lblMessage1.Size = new Size(342, 30);
            lblMessage1.TabIndex = 0;
            lblMessage1.Text = "You have successfully signed out.";
            lblMessage1.TextAlign = ContentAlignment.MiddleCenter;  // Explicitly retained for horizontal and vertical centering
            // 
            // lblMessage2
            // 
            lblMessage2.AutoSize = true;
            lblMessage2.Dock = DockStyle.Fill;
            lblMessage2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage2.Location = new Point(4, 30);
            lblMessage2.Margin = new Padding(4, 0, 4, 0);
            lblMessage2.Name = "lblMessage2";
            lblMessage2.Size = new Size(342, 30);
            lblMessage2.TabIndex = 1;
            lblMessage2.Text = "You may now close this window.";
            lblMessage2.TextAlign = ContentAlignment.MiddleCenter;  // Explicitly retained for horizontal and vertical centering
            // 
            // buttonPanel
            // 
            buttonPanel.ColumnCount = 3;
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 117F));
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            buttonPanel.Controls.Add(btnOK, 1, 0);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(4, 63);
            buttonPanel.Margin = new Padding(4, 3, 4, 3);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.RowCount = 1;
            buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            buttonPanel.Size = new Size(342, 43);  // Adjusted to fit new row height (reduced proportionally)
            buttonPanel.TabIndex = 2;
            // 
            // btnOK
            // 
            btnOK.AutoSize = true;
            btnOK.Dock = DockStyle.Top;
            btnOK.Location = new Point(116, 3);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(109, 29);
            btnOK.TabIndex = 0;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // SignOutDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 110);  // Reduced height from 173 to 110 to match tighter row spacing, keeping width for centering
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SignOutDialog";
            StartPosition = FormStartPosition.CenterScreen;  // Explicitly retained to center the dialog on the screen
            Text = "Sign Out";
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            buttonPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}