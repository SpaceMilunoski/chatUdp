using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace p2pchat_sinf
{
    public partial class fLogin : Form
    {
        string userName = "";
        public string UserName {
            get { return userName; }
        }
        public fLogin()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(LoginForm_FormClosing);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            userName = tbUsuario.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Please select a user name up to 32 characters.");
                return;
            }

            this.FormClosing -= LoginForm_FormClosing;
            this.Close();
        }
        void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            userName = "";
        }
    }
}
