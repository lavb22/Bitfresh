using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bitfresh_Core;

namespace Bitfresh_WindowsForm
{
    public partial class MainWindow : Form
    {
        private System.Windows.Forms.NotifyIcon AppNotifyIcon;
        private System.Windows.Forms.ContextMenu NotifyMenu;
        private ProfileManager ProfileMngr;
        private OrderManager Manager;
        private Configure confWindow;
        private PasswordInput passWindow;
        private ProfileWindow PrfWindow;
        public Control refcontrol;

        private bool fOneShot = false;
        public bool fMonoFramework = Type.GetType("Mono.Runtime") != null; //Mono Checker
        public bool fInstClose = false;

        public BindingList<Order> OrderList { get; set; }
        public bool IsEnabled { get; private set; }

        public StatusClass ErrorStatus;

        private Task GUIupdater;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                ProfileMngr = new ProfileManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            ErrorStatus = new StatusClass();
            refcontrol = new Control();
            refcontrol.CreateControl();

            //Bindings

            StatusLabel.DataBindings.Add(new Binding("Text", ErrorStatus, "STATUS"));
            OrdersView.DataSource = OrderList;
            UsersComboBox.DataSource = ProfileMngr.UserNames;

            UsersComboBox.SelectedIndex = -1;

            //Initialize List for the ListView

            OrderList = new BindingList<Order>();
            OrdersView.DataSource = OrderList;

            if (!fMonoFramework)
            {
                //Creation of context menu of NotifyIcon

                NotifyMenu = new System.Windows.Forms.ContextMenu();
                NotifyMenu.MenuItems.Add("Restore", MenuItem_Restore);
                NotifyMenu.MenuItems.Add("Exit", MenuItem_Exit);

                //Initializing for the NotifyIcon

                AppNotifyIcon = new System.Windows.Forms.NotifyIcon();
                AppNotifyIcon.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.logo.GetHicon());
                AppNotifyIcon.Visible = true;
                AppNotifyIcon.DoubleClick += OnDoubleClickNotify;
                AppNotifyIcon.ContextMenu = NotifyMenu;
                AppNotifyIcon.BalloonTipText = "Bitfresh is minimized to system tray";
                AppNotifyIcon.Text = "Bitfresh";
            }

            //Add Icon to Window
            this.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.logo.GetHicon());

            //Create configuration object
            PrfWindow = new ProfileWindow(ConnectButton, confBtn, ProfManagerButton, ProfileMngr);
            confWindow = new Configure(ConnectButton, confBtn, ProfManagerButton);
            passWindow = new PasswordInput();
        }

        //Functions related with IconTray

        protected override void OnClosing(CancelEventArgs e)
        {
            if (fMonoFramework)
            {
                base.OnClosing(e);
                return;
            }

            if (!fInstClose)
            {
                e.Cancel = true;
                this.Hide();

                if (!fOneShot)
                {
                    AppNotifyIcon.ShowBalloonTip(3000);
                    fOneShot = true;
                }
            }
            else
            {
                NotifyMenu.Dispose();
                AppNotifyIcon.Dispose();
            }
        }

        protected void OnDoubleClickNotify(object sender, EventArgs e)
        {
            this.Show();
        }

        //Menu Functions

        protected void MenuItem_Restore(object sender, EventArgs e)
        {
            this.Show();
        }

        protected void MenuItem_Exit(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() => closingTask());
        }

        //Button Functions

        private void conf_Clik(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            confBtn.Enabled = false;
            ProfManagerButton.Enabled = false;
            confWindow.Show();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (this.ConnectButton.Text.ToString() == "Disconnect")
            {
                this.ConnectButton.Enabled = false;

                System.Threading.Tasks.Task.Run(() => disconnectingTask(ErrorStatus));

                return;
            }

            if (!ProfileMngr.UserNames.Any())
            {
                MessageBox.Show(this, "Please create at least one user to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            if (UsersComboBox.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Please select a user to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            UsersComboBox.Enabled = false;
            this.ConnectButton.Enabled = false;
            confBtn.Enabled = false;
            ProfManagerButton.Enabled = false;

            DialogResult result = passWindow.ShowDialog();

            if (result == DialogResult.Cancel || String.IsNullOrWhiteSpace(passWindow.password))
            {
                this.ConnectButton.Enabled = true;
                confBtn.Enabled = true;
                ProfManagerButton.Enabled = true;
                UsersComboBox.Enabled = true;
                passWindow.password = String.Empty;
                return;
            }

            Tuple<string, string> UserData;

            try
            {
                var CurrentUser = ProfileMngr.GetUserData((string)UsersComboBox.SelectedItem, passWindow.password, out UserData);
            }
            catch (InvalidPasswordException)
            {
                MessageBox.Show(this, "Invalid or Incorrect password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.ConnectButton.Enabled = true;
                UsersComboBox.Enabled = Enabled;
                ProfManagerButton.Enabled = Enabled;
                passWindow.password = string.Empty;
                confBtn.Enabled = true;
                return;
            }
            catch (NoPasswordException)
            {
                MessageBox.Show(this, "Error in user data, please recreate it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.ConnectButton.Enabled = true;
                UsersComboBox.Enabled = Enabled;
                ProfManagerButton.Enabled = Enabled;
                passWindow.password = string.Empty;
                confBtn.Enabled = true;
                return;
            }

            passWindow.password = string.Empty;

            ErrorStatus.STATUS = "Connecting...";

            try
            {
                BittrexBridge bridge = new BittrexBridge(UserData.Item1, UserData.Item2);

                Manager = new OrderManager(bridge, ErrorStatus, (int)Properties.Settings.Default["age"], (int)Properties.Settings.Default["frecuency"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            GUIupdater = new Task(() => UpdateGUI(ErrorStatus));
            GUIupdater.Start();

            this.ConnectButton.Text = "Disconnect";
            this.ConnectButton.Enabled = true;
        }

        private void ProfManagerButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            confBtn.Enabled = false;
            ProfManagerButton.Enabled = false;
            PrfWindow.Show();
        }
    }
}