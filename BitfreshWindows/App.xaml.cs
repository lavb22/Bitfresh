using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BitfreshWindows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p =>
                             p.ProcessName == proc.ProcessName).Count();
            if (count > 1)
            {
                MessageBox.Show("Already an instance is running.", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Main.fInstClose = true;
                Current.Shutdown();
            }

            Main.Show();
        }
    }
}
