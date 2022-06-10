using SWICD.Model;
using SWICD.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace SWICD.ViewModels
{
    internal class DriverStatusViewModel :INotifyPropertyChanged
    {
        public ObservableCollection<LogEntryModel> LogEntries { get; set; } = new ObservableCollection<LogEntryModel>();
        public string DriverStatusText { get; set; }
        public string DriverVersionText => BuildVersionInfo.Version;
        public SolidColorBrush DriverStatusColor { get; set; }
        private readonly Dispatcher _dispatcher;

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
