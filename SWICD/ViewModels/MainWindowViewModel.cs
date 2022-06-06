using SWICD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.ViewModels
{
    internal class MainWindowViewModel
    {
        public ObservableCollection<NavigationItemModel> NavigationItems { get; set; }

        public MainWindowViewModel()
        {
            NavigationItems = new ObservableCollection<NavigationItemModel>();
            NavigationItems.Add(new NavigationItemModel()
            {
                Title = "Settings"
            });
        }
    }
}
