using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bitfresh_WindowsForm
{
    internal static class Program
    {
        public static MainWindow MainUI;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainUI = new MainWindow();

            if (!MainUI.fMonoFramework)
            {
                Process proc = Process.GetCurrentProcess();
                int count = Process.GetProcesses().Where(p =>
                                 p.ProcessName == proc.ProcessName).Count();
                if (count > 1)
                {
                    MessageBox.Show(MainUI, "Already an instance is running.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MainUI.fInstClose = true;
                    Application.Exit();
                    return;
                }
            }

            Application.Run(MainUI);
        }
    }
}