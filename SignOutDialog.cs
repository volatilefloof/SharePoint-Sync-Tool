using System;
using System.Windows.Forms;

namespace MaysSharePointSync
{
    public partial class SignOutDialog : Form
    {
        public SignOutDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}