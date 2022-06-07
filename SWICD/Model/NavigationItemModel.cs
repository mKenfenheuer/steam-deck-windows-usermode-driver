using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SWICD.Model
{
    internal class NavigationItemModel
    {
        public string Title { get; internal set; }
        public Page Page { get; internal set; }
        public bool Selectable { get; internal set; } = true;
        public bool IsSubItem { get; internal set; } = false;
        public Thickness Margins => IsSubItem ? new Thickness(15,0,0,0) : new Thickness(5,0,0,0);
        public Action<NavigationItemModel> OnClick { get; internal set; }
    }
}
