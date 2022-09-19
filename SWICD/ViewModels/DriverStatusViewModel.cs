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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace SWICD.ViewModels
{
    internal class DriverStatusViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LogEntryModel> LogEntries => new ObservableCollection<LogEntryModel>(LoggingService.Instance.LogEntries);
        public string DriverStatusText { get; set; }
        public string EmulationStatusText
        {
            get
            {
                var executable = ControllerService.Instance.DecisionExecutable;

                if (ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Whitelist)
                    return $"Active (Whitelist: {executable})";

                if (!ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Whitelist)
                    return $"Inactive (No whitelisted process running)";

                if (ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Combined)
                    return $"Active (Whitelist: {executable})";

                if (!ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Combined)
                    return $"Inactive (Blacklist: {executable})";

                if (!ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Blacklist)
                    return $"Inactive (Blacklist: {executable})";

                if (ControllerService.Instance.EmulationEnabled &&
                    ControllerService.Instance.Configuration.GenericSettings.OperationMode == Config.OperationMode.Blacklist)
                    return $"Active (No blacklisted process running)";

                return ControllerService.Instance.EmulationEnabled ? "Active" : "Inactive";
            }
        }
        public string DriverVersionText => BuildVersionInfo.Version;
        public string LatestDriverVersionText { get; set; } = "Checking ...";
        public string LatestDriverVersionLink { get; set; }
        public SolidColorBrush LatestDriverVersionColor { get; set; } = new SolidColorBrush(Colors.LightGray);
        public SolidColorBrush DriverStatusColor { get; set; }
        public SolidColorBrush EmulationStatusColor => ControllerService.Instance.EmulationEnabled ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Orange);
        private readonly Dispatcher _dispatcher;
        public CommandHandler CreateSupportPackageClickCommand => new CommandHandler(obj => CreateSupportPackage());
        public CommandHandler OpenGitHubReleaseClickCommand => new CommandHandler(obj => OpenGitHubRelease());

        public event PropertyChangedEventHandler PropertyChanged;

        public DriverStatusViewModel() : this(Dispatcher.CurrentDispatcher)
        {
        }

        public DriverStatusViewModel(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
            LoggingService.Instance.OnNewLogEntry += OnNewLogEntry;
            ControllerService.Instance.OnServiceStartStop += OnServiceStartStop;
            Task.Run(CheckNewVersion);
        }

        private async Task CheckNewVersion()
        {
            var release = await GitHubApi.GetLatestRelease();

            var releaseVersionStr = Regex.Replace(release.TagName, @"[^0-9\.]+([0-9\.]+)[^\.]*", "$1");
            var curVersionStr = Regex.Replace(BuildVersionInfo.Version, @"[^0-9\.]+([0-9\.]+)[^\.]*", "$1");

            Version releaseVersion = new Version(releaseVersionStr);
            Version buildVersion = new Version(curVersionStr);

            _dispatcher.Invoke(() =>
            {
                LatestDriverVersionText = release.TagName;
                LatestDriverVersionLink = release.HtmlUrl;
                LatestDriverVersionColor = new SolidColorBrush(buildVersion <= releaseVersion ? Colors.White : Colors.Yellow);
            });

            NotifyPropertyChanged(nameof(LatestDriverVersionText));
            NotifyPropertyChanged(nameof(LatestDriverVersionColor));
        }
        private void OpenGitHubRelease()
        {
            System.Diagnostics.Process.Start(LatestDriverVersionLink);
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
            if (ControllerService.Instance.FailedInit)
                DriverStatusText = "Failed to initialize. Check Log.";
            else
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
                NotifyPropertyChanged(nameof(LogEntries));
                NotifyPropertyChanged(nameof(EmulationStatusText));
                NotifyPropertyChanged(nameof(EmulationStatusColor));
            });
        }
    }
}
