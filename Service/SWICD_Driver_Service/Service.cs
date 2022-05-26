using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWICD_Driver_Service
{
    public partial class Service : ServiceBase
    {
        Process DriverProcess;
        EventLog eventLog = new EventLog();
        string filename;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            eventLog.Source = "SWICD_Driver_Service";
            eventLog.WriteEntry("SWICD Driver Started.", EventLogEntryType.Information);


            var cwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = Path.Combine(cwd, "SWICD_Driver.exe");

            eventLog.WriteEntry($"SWICD Driver Location: {filename}", EventLogEntryType.Information);

            StartDriver();
            CheckDriverStatusTimer.Enabled = true;
        }

        void Thread_ProcessWorker()
        {
            var stdline = DriverProcess.StandardOutput.ReadLine();
            if (stdline != null)
                eventLog.WriteEntry(stdline, EventLogEntryType.Information);

            var errline = DriverProcess.StandardError.ReadLine();
            if (errline != null)
                eventLog.WriteEntry(errline, EventLogEntryType.Error);
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
        }

        protected override void OnStop()
        {
            if (DriverProcess != null && !DriverProcess.HasExited)
                DriverProcess.Kill();
        }

        private void CheckDriverStatusTimer_Tick(object sender, EventArgs e)
        {
            if (DriverProcess == null || DriverProcess.HasExited)
                StartDriver();
            Thread_ProcessWorker();

        }
    }
}
