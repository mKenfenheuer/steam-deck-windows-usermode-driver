using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SWICD_Configurator
{
    internal static class Program
    {
        private static void RunAsAdmin()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            var cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            using (var process = Process.Start(new ProcessStartInfo("notepad.exe", Path.Combine(cwd, "app_config.conf"))
            {
                Verb = "runas"
            }))
            {
                process?.WaitForExit();
            }
        }

        private static bool IsElevated()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (IsElevated())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            else
            {
                RunAsAdmin();
            }
        }
    }
}
