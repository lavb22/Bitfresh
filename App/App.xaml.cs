using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Bitfresh
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class app : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow Main = new MainWindow();
            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p =>
                             p.ProcessName == proc.ProcessName).Count();
            if (count > 1)
            {
                MessageBox.Show("Already an instance is running.", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Main.fInstClose = true;
                app.Current.Shutdown();
            }

            Main.Show();
        }
    }
}