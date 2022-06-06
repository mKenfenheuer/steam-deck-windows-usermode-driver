using SWICD.Model;
using SWICD.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SWICD.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
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
        public Page ContentPage { get; set; }

        public MainWindowViewModel()
        {
            NavigationItems = new ObservableCollection<NavigationItemModel>();
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Settings"
            });
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Profiles"
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task OnNavigationItemSelected(NavigationItemModel item)
        {
            ContentPage = new SettingsPage();
            NotifyPropertyChanged(nameof(ContentPage));
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
