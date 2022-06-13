using SWICD.Commands;
using SWICD.Model;
using SWICD.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace SWICD.ViewModels
{
    internal class DriverStatusViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LogEntryModel> LogEntries { get; set; } = new ObservableCollection<LogEntryModel>();
        public string DriverStatusText { get; set; }
        public string DriverVersionText => BuildVersionInfo.Version;
        public SolidColorBrush DriverStatusColor { get; set; }
        private readonly Dispatcher _dispatcher;
        public CommandHandler CreateSupportPackageClickCommand => new CommandHandler(obj => CreateSupportPackage());

        public event PropertyChangedEventHandler PropertyChanged;

        public DriverStatusViewModel() : this(Dispatcher.CurrentDispatcher)
        {
        }

        public DriverStatusViewModel(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
            LoggingService.Instance.OnNewLogEntry += OnNewLogEntry;
            ControllerService.Instance.OnServiceStartStop += OnServiceStartStop;
        }
        private void CreateSupportPackage()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path = Path.Combine(folder, $"SWICD_Support_{DateTime.UtcNow.Ticks}.zip");
            using (ZipArchive archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                var logEntry = archive.CreateEntry("debug_log.log");
                WriteString(logEntry, LoggingService.Instance.GetLogString());

                var confEntry = archive.CreateEntry("app_config.conf");
                WriteString(confEntry, ControllerService.Instance.Configuration.ToString());

                var versionEntry = archive.CreateEntry("app_version.txt");
                WriteString(versionEntry, BuildVersionInfo.Version);
            }

            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", path));
        }

        private void WriteString(ZipArchiveEntry entry, string text)
        {
            var writer = new StreamWriter(entry.Open());
            writer.Write(text);
            writer.Flush();
            writer.Close();
        }

        private void OnServiceStartStop(object sender, bool e)
        {
            DriverStatusText = e ? "Running" : "Stopped";
            DriverStatusColor = new SolidColorBrush(e ? Colors.Green : Colors.Red);
            NotifyPropertyChanged(nameof(DriverStatusText));
            NotifyPropertyChanged(nameof(DriverStatusColor));
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void OnNewLogEntry(object sender, LogEntryModel e)
        {
            _dispatcher.Invoke(() =>
            {
                LogEntries.Add(e);
            });
        }
    }
}
