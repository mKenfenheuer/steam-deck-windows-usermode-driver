using SWICD.ViewModels;
using SWICD.Config;
using SWICD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWICD.Pages
{
    /// <summary>
    /// Interaktionslogik für ProfileEditPage.xaml
    /// </summary>
    public partial class ProfileEditPage : Page
    {
        public ProfileEditPage(ControllerConfig controllerConfig = null)
        {
            InitializeComponent();
            this.DataContext = new ProfileEditPageViewModel(controllerConfig);
        }

        private void KeyboardPickerButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var model = button?.Tag as KeyboardMappingModel;
            if (model == null) return;

            var picker = new KeyboardPickerWindow(model.EmulatedKeyboardKey);
            picker.Owner = Window.GetWindow(this);
            if (picker.ShowDialog() == true)
            {
                model.EmulatedKeyboardKey = picker.SelectedKey;
            }
        }
    }
}
