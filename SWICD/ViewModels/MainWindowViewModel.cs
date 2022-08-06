using Microsoft.Win32;
using SWICD.Commands;
using SWICD.Model;
using SWICD.Pages;
using SWICD.Services;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SWICD.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel Instance { get; private set; } 
        public ObservableCollection<NavigationItemModel> NavigationItems { get; set; }
        public NavigationItemModel SelectedNavigationItem
        {
            set
            {
                _ = OnNavigationItemSelected(value);
                NotifyPropertyChanged(nameof(SelectedNavigationItem));
            }
            get => null;
        }

        internal void OnWindowClosing(CancelEventArgs e)
        {
            if (Configuration.HasChanges())
            {
                var result = MessageBox.Show(
                    "Do you want to save the configuration now?",
                    "Attention",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                    e.Cancel = true;
                if (result == MessageBoxResult.Yes)
                    SaveConfiguration();
            }
        }

        private void SaveConfiguration()
        {
            ConfigLoader.SaveConfiguration(Configuration, Environment.SpecialFolder.MyDocuments, "SWICD", "app_config.conf");
            Configuration.CreateSnapshot();
        }

        internal void OnDeleteProfile(ControllerConfig controllerConfig)
        {
            Configuration.PerProcessControllerConfig.Remove(controllerConfig.Executable);
            var Navitem = NavigationItems
                .Where(n => n.Page is ProfileEditPage)
                .FirstOrDefault(n => ((n.Page as ProfileEditPage).DataContext as ProfileEditPageViewModel).Executable == controllerConfig.Executable);
            if (Navitem != null)
                NavigationItems.Remove(Navitem);
            NotifyPropertyChanged(nameof(NavigationItems));

            ContentPage = NavigationItems.FirstOrDefault(n => n.Page is DriverStatusPage)?.Page;
            NotifyPropertyChanged(nameof(ContentPage));
        }

        private Configuration Configuration => ControllerService.Instance.Configuration;
        public Page ContentPage { get; set; }

        public MainWindowViewModel()
        {
            Instance = this;
            ControllerService.Instance.Configuration = ConfigLoader.GetConfiguration(Environment.SpecialFolder.MyDocuments, "SWICD", "app_config.conf");

            NavigationItems = new ObservableCollection<NavigationItemModel>();
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Driver Status",
                Page = new DriverStatusPage(),
            });
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Settings",
                Page = new SettingsPage(Configuration.GenericSettings),
            });
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Profiles",
                Selectable = false,
            });
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Default Profile",
                IsSubItem = true,
                Page = new ProfileEditPage(Configuration.DefaultControllerConfig),
            });

            foreach (var profile in Configuration.PerProcessControllerConfig)
            {
                NavigationItems.Add(new NavigationItemModel()
                {
                    Title = profile.Key,
                    IsSubItem = true,
                    Page = new ProfileEditPage(profile.Value),
                });
            }

            SelectedNavigationItem = NavigationItems.First();
            NotifyPropertyChanged(nameof(SelectedNavigationItem));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddProfileClickCommand => new CommandHandler((obj) => _ = OnAddProfileClick());

        public async Task OnAddProfileClick()
        {
            QuestionWindow window = new QuestionWindow("Would you like to add a profile by selecting an executable or by typing its name?", new string[] { "Select executable", "Type executable", "Abort" });
            window.ShowDialog();


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable | *.exe";
            openFileDialog.Title = "Select the game executable";
            if (openFileDialog.ShowDialog() == true)
            {
                string executable = Path.GetFileName(openFileDialog.FileName);
                ControllerConfig config = new ControllerConfig(executable);
                Configuration.PerProcessControllerConfig.Add(executable, config);
                NavigationItems.Add(new NavigationItemModel()
                {
                    Title = executable,
                    IsSubItem = true,
                    Page = new ProfileEditPage(config),
                });
            }
        }

        public async Task OnNavigationItemSelected(NavigationItemModel item)
        {
            ContentPage = item.Page;
            NotifyPropertyChanged(nameof(ContentPage));
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
