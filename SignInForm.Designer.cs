namespace MaysSharePointSync
{
    partial class SignInForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignInForm));
            btnSignIn = new Button();
            lblStatus = new Label();
            SuspendLayout();
            // 
            // btnSignIn
            // 
            btnSignIn.Location = new Point(58, 35);
            btnSignIn.Margin = new Padding(4, 3, 4, 3);
            btnSignIn.Name = "btnSignIn";
            btnSignIn.Size = new Size(117, 35);
            btnSignIn.TabIndex = 0;
            btnSignIn.Text = "Sign In";
            btnSignIn.UseVisualStyleBackColor = true;
            btnSignIn.Click += btnSignIn_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(14, 81);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Status:";
            // 
            // SignInForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(233, 115);
            Controls.Add(lblStatus);
            Controls.Add(btnSignIn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SignInForm";
            Text = "Mays SharePointSync - Sign In";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Label lblStatus;
    }
}