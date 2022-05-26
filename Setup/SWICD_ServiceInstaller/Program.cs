using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace SWICD_ServiceInstaller
{
    [RunInstaller(true)]
    internal class Program
    {
        static void Main(string[] args)
        {
            var cwd = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var filename = Path.Combine(cwd, "SWICD_Driver_Service.exe");

            if (args[0] == "--install")
            {
                InstallService(filename);
                StartService();
            }
            else if (args[0] == "--uninstall")
            {
                StopService();
                UninstallService();
            }
            else if (args[0] == "--test")
            {
                ServiceTest test = new ServiceTest();
                test.OnStart();
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Nothing to do. Specify --install or --uninstall as argument.");
            }
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
