using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Bitfresh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon AppNotifyIcon;
        private System.Windows.Forms.ContextMenu NotifyMenu;
        private OrderManager Manager;
        private Configure confWindow;

        private bool fOneShot = false;
        public bool fInstClose = false;

        public System.Collections.ObjectModel.ObservableCollection<Order> OrderList { get; set; }
        public statusClass ErrorStatus;

        public MainWindow()
        {
            InitializeComponent();
            ErrorStatus = new statusClass();
            this.StatusLabel.DataContext = ErrorStatus;

            //Initialize List for the ListView

            OrderList = new System.Collections.ObjectModel.ObservableCollection<Order>();
            this.OrdersView.ItemsSource = OrderList;

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

            //Create configuration object
            confWindow = new Configure(ConnectButton, confBtn);
        }

        //Functions related with IconTray

        protected override void OnClosing(CancelEventArgs e)
        {
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
            base.OnClosing(e);
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
            System.Threading.Tasks.Task.Run(()=> closingTask());
        }

        private void conf_Clik(object sender, RoutedEventArgs e)
        {
            ConnectButton.IsEnabled = false;
            confBtn.IsEnabled = false;
            confWindow.Show();
            System.Diagnostics.Debug.WriteLine("Configuracion");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.ConnectButton.Content.ToString() == "Disconnect")
            {
                this.ConnectButton.IsEnabled = false;

                System.Threading.Tasks.Task.Run(() => disconnectingTask());

                return;
            }

            if (string.IsNullOrEmpty(this.ApiKey.Text) || string.IsNullOrEmpty(this.ApiSecret.Password))
            {
                MessageBox.Show("Please enter valid ApiKey and ApiSecret to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            this.ConnectButton.IsEnabled = false;
            confBtn.IsEnabled = false;
            ErrorStatus.STATUS = "Connecting...";

            BittrexBridge bridge = new BittrexBridge(this.ApiKey.Text.Replace(" ", String.Empty), this.ApiSecret.Password.Replace(" ", String.Empty));
            this.ApiSecret.Password = string.Empty;
            this.ApiKey.IsEnabled = false;
            this.ApiSecret.IsEnabled = false;

            Manager = new OrderManager(bridge, OrderList, ErrorStatus);

            this.ConnectButton.Content = "Disconnect";
            this.ConnectButton.IsEnabled = true;

            
        }

        private void closingTask()
        {
            if (Manager != null)
            {
                if (Manager.CheckifTask())
                {
                    AppNotifyIcon.BalloonTipText = "Bitfresh is closing. Please wait until running tasks complete.";
                    AppNotifyIcon.ShowBalloonTip(3000);
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        this.IsEnabled = false;
                    });
                }

                Manager.Dispose();
            }

            NotifyMenu.Dispose();
            AppNotifyIcon.Dispose();

            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Application.Current.Shutdown();
            });

        }

        private void disconnectingTask()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                this.ConnectButton.Content = "Disconnecting";
            });

            Manager.Dispose();

            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                ConnectButton.IsEnabled = true;
                this.ConnectButton.Content = "Connect";
                this.ApiKey.IsEnabled = true;
                this.ApiSecret.IsEnabled = true;
                ErrorStatus.STATUS = "Waiting...";
                this.confBtn.IsEnabled = true;
            });

        }


        public void ShowConnectionError()
        {
            MessageBox.Show("Failed to connecto to servers", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Manager.Dispose();
            ErrorStatus.STATUS = "Waiting...";
            this.ConnectButton.IsEnabled = true;
            this.ConnectButton.Content = "Connect";
            this.ApiKey.IsEnabled = true;
            this.ApiSecret.IsEnabled = true;
        }

        public void AskForBackup()
        {
            MessageBoxResult re = MessageBox.Show("Backup detected: Do you want to restore it?", "Info", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (re == MessageBoxResult.Yes)
            {
                Manager.fRestoreBackup = true;
            }
            else
            {
                Manager.fDeleteBackup = true;
            }

            Manager.startTasks();
        }
    }

    public class Order : INotifyPropertyChanged
    {
        private string id;
        private string exchange;
        private decimal ammount;
        private string type, status;
        private DateTime created;
        private string timeleft;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string ID { get { return id; } set { if (value != this.id) { this.id = value; NotifyPropertyChanged(); } } }
        public string EXCHANGE { get { return this.exchange; } set { if (value != this.exchange) { this.exchange = value; NotifyPropertyChanged(); } } }
        public decimal AMMOUNT { get { return this.ammount; } set { if (value != this.ammount) { this.ammount = value; NotifyPropertyChanged(); } } }
        public string TYPE { get { return this.type; } set { if (value != this.type) { this.type = value; NotifyPropertyChanged(); } } }
        public string STATUS { get { return this.status; } set { if (value != this.status) { this.status = value; NotifyPropertyChanged(); } } }
        public DateTime CREATED { get { return this.created; } set { if (value != this.created) { this.created = value; NotifyPropertyChanged(); } } }
        public string TIMELEFT { get { return this.timeleft.ToString(); } set { if (value != this.timeleft) { this.timeleft = value; NotifyPropertyChanged(); } } }

        public Order(string _id, string _exchange, decimal _ammount, string _type, string _status, DateTime _created, string _timeleft)
        {
            this.ID = _id;
            this.exchange = _exchange;
            this.AMMOUNT = _ammount;
            this.TYPE = _type;
            this.STATUS = _status;
            this.CREATED = _created;
            this.TIMELEFT = _timeleft;
        }
    }

    public class statusClass : INotifyPropertyChanged
    {
        private Dictionary<string, string> statusByOrder;
        private string status;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string STATUS { get { return status; } set { if (value != this.status) { this.status = value; NotifyPropertyChanged(); } } }

        public statusClass()
        {
            status = "Waiting...";
            statusByOrder = new Dictionary<string, string>();
        }

        public string getStatusByOrder(string uid)
        {
            string returnedStatus;

            if (statusByOrder.TryGetValue(uid, out returnedStatus))
            {
                return returnedStatus;
            }

            return "Stand By";
        }

        public void setStatusByOrder(string uid, string status)
        {
            try
            {
                statusByOrder[uid] = status;
            }
            catch (KeyNotFoundException)
            {
                statusByOrder.Add(uid, status);
            }
        }

        public void removeStatusByOrder(string uid)
        {
            statusByOrder.Remove(uid);
        }
    }
}

public static class Constants
{
    public const int MaxDays = 28;
}