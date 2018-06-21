using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bitfresh_WindowsForm
{
    public partial class PasswordInput : Form
    {
        internal string password;
        public PasswordInput()
        {
            InitializeComponent();
            password = string.Empty;
            this.AcceptButton = OKbtn;
        }

        private void OKbtn_Click(object sender, EventArgs e)
        {
            password = passTextBox.Text;
            passTextBox.Text = string.Empty;
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            passTextBox.Text = string.Empty;
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }
    }
}
