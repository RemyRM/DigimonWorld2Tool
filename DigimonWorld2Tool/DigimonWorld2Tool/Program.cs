using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigimonWorld2Tool
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(ApplicationApplicationExit);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomainProcessExit);
            Application.Run(new DigimonWorld2ToolForm());
            Application.Run(new Views.MainWindow());
        }

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private static void ApplicationApplicationExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
