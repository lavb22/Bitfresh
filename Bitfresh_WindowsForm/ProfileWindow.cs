using Bitfresh_Core;
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
    public partial class ProfileWindow : Form
    {
        private Button cntc, conf, pbtn;
        private ProfileManager _manager;

        public ProfileWindow(Button conbutton, Button confbutton, Button ProfBtn,ProfileManager manager)
        {
            InitializeComponent();
            ApiBox.Enabled = false;
            SecretBox.Enabled = false;
            PassBox1.Enabled = false;
            PassBox2.Enabled = false;
            AddButton.Enabled = false;
            Deletebtn.Enabled = false;
            UsersListView.DataSource = manager.UserNames;
            UsersListView.SelectedIndex = -1;


            _manager = manager;
            cntc = conbutton;
            conf = confbutton;
            pbtn = ProfBtn;
        }
        private void UserBox_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(UserBox.Text))
            {
                ApiBox.Enabled = true;
                return;
            }

        }

        private void ApiBox_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ApiBox.Text))
            {
                SecretBox.Enabled = true;
                return;
            }
        }

        private void SecretBox_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ApiBox.Text))
            {
                PassBox1.Enabled = true;
                return;
            }
        }

        private void PassBox2_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ApiBox.Text))
            {
                AddButton.Enabled = true;
                return;
            }

        }

        private async void AddButton_Click(object sender, EventArgs e)
        {

            UserStatusLabel.Visible = false;

            if(String.IsNullOrWhiteSpace(UserBox.Text) || String.IsNullOrWhiteSpace(ApiBox.Text) || String.IsNullOrWhiteSpace(SecretBox.Text) || String.IsNullOrWhiteSpace(PassBox1.Text) || String.IsNullOrWhiteSpace(PassBox2.Text))
            {
                UserStatusLabel.Text = "Error: empty paramether";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;

            }
            if (!(PassBox1.Text==PassBox2.Text))
            {
                PassBox1.Text = String.Empty;
                PassBox2.Text = String.Empty;
                UserStatusLabel.Text = "Error: Passwords don't match";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;
            }

            if(PassBox1.Text.Length < 6)
            {
                UserStatusLabel.Text = "Error: Password must be at least 6 char long";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;
            }

            if(!System.Text.RegularExpressions.Regex.IsMatch(UserBox.Text, @"^[a-zA-Z0-9_]+$"))
            {
                UserStatusLabel.Text = "Error: Only {a-z,0-9,_} allowed in username";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;
            }

            if(ApiBox.Text.Length != 32)
            {
                UserStatusLabel.Text = "Error: Invalid ApiKey";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;
            }

            if (SecretBox.Text.Length != 32)
            {
                UserStatusLabel.Text = "Error: Invalid ApiSecret";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                return;
            }

            DisableAll();

            UserStatusLabel.Text = "Testing credentials...";
            UserStatusLabel.ForeColor = Color.Green;
            UserStatusLabel.Visible = true;

            bool response = await BittrexBridge.testConnection(ApiBox.Text,SecretBox.Text);

            if (!response)
            {
                UserStatusLabel.Text = "Error: Invalid Key and Secret Combination";
                UserStatusLabel.ForeColor = Color.Red;
                UserStatusLabel.Visible = true;
                AddButton.Enabled = Deletebtn.Enabled = true;
                EnableAll();
                return;
            }

            UserStatusLabel.Visible = false;

            try
            {
                _manager.AddNewUser(UserBox.Text, ApiBox.Text, SecretBox.Text, PassBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableAll();
                return;
            }

            EmptyStuff();
        }

        private void EmptyStuff()
        {
            UserBox.Focus();
            UserBox.Text = String.Empty;
            ApiBox.Text = String.Empty;
            SecretBox.Text = String.Empty;
            PassBox1.Text = String.Empty;
            PassBox2.Text = String.Empty;

            UserBox.Enabled = true;
            ApiBox.Enabled = false;
            SecretBox.Enabled = false;
            PassBox1.Enabled = false;
            PassBox2.Enabled = false;
            AddButton.Enabled = false;
        }

        private void DisableAll()
        {
            UserBox.Enabled = false;
            ApiBox.Enabled = false;
            SecretBox.Enabled = false;
            PassBox1.Enabled = false;
            PassBox2.Enabled = false;
            AddButton.Enabled = false;
        }

        private void EnableAll()
        {
            UserBox.Enabled = true;
            ApiBox.Enabled = true;
            SecretBox.Enabled = true;
            PassBox1.Enabled = true;
            PassBox2.Enabled = true;
            AddButton.Enabled = true;
        }

        private void ProfileWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserStatusLabel.Visible = false;
            AddButton.Enabled = false;
            EmptyStuff();
            cntc.Enabled = true;
            conf.Enabled = true;
            pbtn.Enabled = true;

            UsersListView.SelectedIndex = -1;

            this.Hide();
            e.Cancel = true;

        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            try
            {
                _manager.EraseUser((string)UsersListView.SelectedItem);
            }
            catch
            {
                throw;
            }

        }

        private void UsersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UsersListView.SelectedIndex == -1)
            {
                Deletebtn.Enabled = false;
                return;
            }

            Deletebtn.Enabled = true;
        }

        private void PassBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ApiBox.Text))
            {
                PassBox2.Enabled = true;
                return;
            }
        }
        private void enableBttons()
        {
            cntc.Enabled = true;
            conf.Enabled = true;
            pbtn.Enabled = true;
        }
    }
}
