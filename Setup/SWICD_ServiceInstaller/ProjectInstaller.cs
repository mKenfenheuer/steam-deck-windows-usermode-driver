using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SWICD_ServiceInstaller
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        protected override void OnAfterInstall(IDictionary savedState)
        {
            var cwd = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);
            var filename = Path.Combine(cwd, "SWICD_Driver_Service.exe");

            Program.InstallService(filename);
            Program.StartService();
            Console.ReadLine();
            base.OnAfterInstall(savedState);
            
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            Program.StopService();
            Program.UninstallService();
            Console.ReadLine();
            base.OnBeforeUninstall(savedState);
        }
    }
}
