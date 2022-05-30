using SWICD_Lib.Config;
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
using System.Windows.Forms;

namespace SWICD_Driver_Service
{
    public partial class Service : ServiceBase
    {
        
        Process DriverProcess;
        string InstallationDirectory;
        List<string> DriverLog = new List<string>();
        Thread _driverLogWorker;
        Thread _driverManagementWorker;
        bool _running = true;
        EventLog log = new EventLog()
        {
            Source = "SWICD_Driver",
        };

        bool IsDriverRunning => !DriverProcess?.HasExited ?? false;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            InstallationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _driverLogWorker = new Thread(new ThreadStart(DriverLogWorker));
            _driverLogWorker.IsBackground = true;
            _driverLogWorker.Start();
            _driverManagementWorker = new Thread(new ThreadStart(DriverManagementWorker));
            _driverManagementWorker.IsBackground = true;
            _driverManagementWorker.Start();
        }        

        void DriverLogWorker()
        {
            while (_running)
            {
                try
                {
                    if (DriverProcess != null)
                    {
                        try
                        {
                            var stdline = DriverProcess.StandardOutput.ReadLine();
                            if (stdline != null)
                            {
                                DriverLog.Add(stdline);
                                log.WriteEntry(stdline, EventLogEntryType.Information);
                                File.AppendAllText(Path.Combine("C:\\", "DriverLog.log"), stdline + "\r\n");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                        }

                        try
                        {
                            var errline = DriverProcess.StandardError.ReadLine();
                            if (errline != null)
                            {
                                log.WriteEntry(errline, EventLogEntryType.Error);
                                DriverLog.Add(errline);
                                File.AppendAllText(Path.Combine("C:\\", "DriverLog.log"), errline + "\r\n");
                            }

                        }
                        catch (Exception ex)
                        {
                            log.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.WriteEntry(ex.ToString(), EventLogEntryType.Error);

                }
                Thread.Sleep(100);
            }

        }

        void StartDriver()
        {
            var filename = Path.Combine(InstallationDirectory, "SWICD_Driver.exe");
            DriverProcess = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = filename,
                    WorkingDirectory = InstallationDirectory,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                }
            };
            DriverProcess.Start();
        }

        protected override void OnStop()
        {
            if (DriverProcess != null && !DriverProcess.HasExited)
            {
                DriverProcess.Kill();
            }
            _running = false;
        }

        private void DriverManagementWorker()
        {
            while (_running)
            {
                try
                {
                    if (!IsDriverRunning)
                        StartDriver();
                }
                catch (Exception ex)
                {
                    log.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
