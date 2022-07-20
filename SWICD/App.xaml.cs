using SWICD.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SWICD
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                LoggingService.LogCritical(eventArgs.Exception.ToString());
            };
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1 && e.Args[0] == "autostart-try")
            {
                if (!ControllerService.Instance.Configuration.GenericSettings.StartWithWindows)
                {
                    Application.Current.Shutdown();
                    return;
                }
            }
            MainWindow wnd = new MainWindow();
            if (!ControllerService.Instance.Configuration.GenericSettings.StartMinimized)
                wnd.Show();
        }
    }
}
