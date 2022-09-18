using Microsoft.Win32;
using SWICD.Commands;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.ViewModels
{
    internal class SettingsViewModel : INotifyPropertyChanged
    {
        private GenericSettings _settings { get; set; } = new GenericSettings();

        public bool StartWithWindows
        {
            get => _settings.StartWithWindows;
            set
            {
                _settings.StartWithWindows = value;
                NotifyPropertyChanged(nameof(StartWithWindows));
                NotifyPropertyChanged(nameof(StartWithWindowsText));
            }
        }

        public string StartWithWindowsText => StartWithWindows ? "Enabled" : "Disabled";
        public bool StartMinimized
        {
            get => _settings.StartMinimized;
            set
            {
                _settings.StartMinimized = value;
                NotifyPropertyChanged(nameof(StartMinimized));
                NotifyPropertyChanged(nameof(StartMinimizedText));
            }
        }

        public string StartMinimizedText => StartMinimized ? "Enabled" : "Disabled";
        public OperationMode OperationMode
        {
            get => _settings.OperationMode;
            set => _settings.OperationMode = value;
        }

        public static ushort AmplitudeLeft { get; set; }
        public static ushort AmplitudeRight { get; set; }
        public static ushort PeriodLeft { get; set; }
        public static ushort PeriodRight { get; set; }

        public ObservableCollection<string> WhitelistedProcesses { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> BlacklistedProcesses { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<OperationMode> OperationModeItems { get; set; } = new ObservableCollection<OperationMode>(Enum.GetValues(typeof(OperationMode)).Cast<OperationMode>());
        public string SelectedWhitelistedProcess { get; set; }
        public string SelectedBlacklistedProcess { get; set; }

        public CommandHandler AddWhitelistedProcessClickCommand => new CommandHandler((obj) => OnAddWhitelistedProcessClick());
        public CommandHandler AddBlacklistedProcessClickCommand => new CommandHandler((obj) => OnAddBlacklistedProcessClick());

        private void OnAddWhitelistedProcessClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable | *.exe";
            openFileDialog.Title = "Select the game executable";
            if (openFileDialog.ShowDialog() == true)
            {
                string executable = Path.GetFileName(openFileDialog.FileName);

                WhitelistedProcesses.Add(executable);
                _settings.WhitelistedProcesses.Add(executable);
            }
        }
        private void OnAddBlacklistedProcessClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable | *.exe";
            openFileDialog.Title = "Select the game executable";
            if (openFileDialog.ShowDialog() == true)
            {
                string executable = Path.GetFileName(openFileDialog.FileName);

                BlacklistedProcesses.Add(executable);
                _settings.BlacklistedProcesses.Add(executable);
            }
        }

        public CommandHandler RemoveWhitelistedProcessClickCommand => new CommandHandler((obj) => OnRemoveWhitelistedProcessClick());
        public CommandHandler RemoveBlacklistedProcessClickCommand => new CommandHandler((obj) => OnRemoveBlacklistedProcessClick());

        private void OnRemoveWhitelistedProcessClick()
        {
            _settings.WhitelistedProcesses.Remove(SelectedWhitelistedProcess);
            WhitelistedProcesses.Remove(SelectedWhitelistedProcess);
        }
        private void OnRemoveBlacklistedProcessClick()
        {
            _settings.BlacklistedProcesses.Remove(SelectedBlacklistedProcess);
            BlacklistedProcesses.Remove(SelectedBlacklistedProcess);
        }

        public SettingsViewModel(GenericSettings settings)
        {
            _settings = settings;

            foreach (var process in settings.BlacklistedProcesses)
                BlacklistedProcesses.Add(process);

            foreach (var process in settings.WhitelistedProcesses)
                WhitelistedProcesses.Add(process);

            NotifyPropertyChanged(nameof(OperationMode));
            NotifyPropertyChanged(nameof(StartWithWindows));
            NotifyPropertyChanged(nameof(StartWithWindowsText));
            NotifyPropertyChanged(nameof(OperationModeItems));
            NotifyPropertyChanged(nameof(OperationMode));
        }
        public SettingsViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
