using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BitfreshWindows
{
    /// <summary>
    /// Interaction logic for Configure.xaml
    /// </summary>
    public partial class Configure : Window
    {
        public int frec, age;
        private Button cntc, conf;

        public Configure(Button conbutton, Button confbutton)
        {
            InitializeComponent();
            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            cntc = conbutton;
            conf = confbutton;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            this.enableBttons();
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["frecuency"] = frecValue.Value;
            Properties.Settings.Default["age"] = ageValue.Value;
            Properties.Settings.Default.Save();
            this.enableBttons();
            this.Hide();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            this.enableBttons();
            this.Hide();
        }

        private void enableBttons()
        {
            cntc.IsEnabled = true;
            conf.IsEnabled = true;
        }
    }
}