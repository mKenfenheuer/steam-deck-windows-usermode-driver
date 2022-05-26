using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWICD_ServiceInstaller
{
    internal class ServiceTest
    {
        Thread ProcessWorker;
        Process DriverProcess;
        string filename;

        public void OnStart()
        {

            EventLog eventLog = new EventLog();
            eventLog.Source = "SWICD_Driver_Service";
            eventLog.WriteEntry("SWICD Driver Started.", EventLogEntryType.Information);


            var cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = Path.Combine(cwd, "SWICD_Driver.exe");

            eventLog.WriteEntry($"SWICD Driver Location: {filename}", EventLogEntryType.Information);

            StartDriver();
        }

        void Thread_ProcessWorker()
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "SWICD_Driver_Service";
            while (true)
            {
                var stdline = DriverProcess.StandardOutput.ReadLine();
                if (stdline != null)
                    eventLog.WriteEntry(stdline, EventLogEntryType.Information);

                var errline = DriverProcess.StandardError.ReadLine();
                if (errline != null)
                    eventLog.WriteEntry(errline, EventLogEntryType.Error);

                Thread.Sleep(100);
            }
        }

        void StartDriver()
        {
            var cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = Path.Combine(cwd, "SWICD_Driver.exe");
            DriverProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = filename,
                    WorkingDirectory = cwd,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                }
            };
            DriverProcess.Start();
            ProcessWorker = new Thread(new ThreadStart(Thread_ProcessWorker));
            ProcessWorker.Start();
        }
    }
}
