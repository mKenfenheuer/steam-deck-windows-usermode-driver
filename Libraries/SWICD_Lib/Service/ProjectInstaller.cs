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

namespace SWICD_Lib
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        protected override void OnAfterInstall(IDictionary savedState)
        {
            var cwd = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);
            var filename = Path.Combine(cwd, "SWICD_Driver_Service.exe");

            InstallService(filename);
            StartService();
            Console.ReadLine();
            base.OnAfterInstall(savedState);
            
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            StopService();
            UninstallService();
            Console.ReadLine();
            base.OnBeforeUninstall(savedState);
        }

        public static void InstallService(string exeFilename)
        {
            File.AppendAllText(@"C:\InstallLog.log", $"Service FileName: {exeFilename}");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new
            System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C sc create SWICDDriver binpath=\"{exeFilename}\" start=auto DisplayName=\"Steam Deck Windows Usermode Controller Driver Service\"";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            File.AppendAllText(@"C:\InstallLog.log", process.StandardOutput.ReadToEnd());
            File.AppendAllText(@"C:\InstallLog.log", process.StandardError.ReadToEnd());

        }

        public static void StartService()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new
            System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C sc start SWICDDriver";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            File.AppendAllText(@"C:\InstallLog.log", process.StandardOutput.ReadToEnd());
            File.AppendAllText(@"C:\InstallLog.log", process.StandardError.ReadToEnd());

        }

        public static void StopService()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new
            System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C sc stop SWICDDriver";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            File.AppendAllText(@"C:\InstallLog.log", process.StandardOutput.ReadToEnd());
            File.AppendAllText(@"C:\InstallLog.log", process.StandardError.ReadToEnd());

        }

        public static void UninstallService()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new
            System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = $"/C sc delete SWICDDriver";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            File.AppendAllText(@"C:\InstallLog.log", process.StandardOutput.ReadToEnd());
            File.AppendAllText(@"C:\InstallLog.log", process.StandardError.ReadToEnd());
        }
    }
}
