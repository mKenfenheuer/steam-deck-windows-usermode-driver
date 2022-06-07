using SWICD.Properties;
using SWICD.Services;
using SWICD.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWICD.Pages
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon _nIcon = new NotifyIcon(new Container());
        bool _canClose = false;
        public MainWindow()
        {
            InitializeComponent();
            _nIcon.Icon = AppResources.app_icon;
            _nIcon.Text = "SWICD Driver";
            _nIcon.Visible = true;
            _nIcon.DoubleClick += NIcon_DoubleClick;
            _nIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem showItem = new System.Windows.Forms.MenuItem();
            showItem.Text = "Show";
            showItem.Click += ItemShow_Click;
            _nIcon.ContextMenu.MenuItems.Add(showItem);

            System.Windows.Forms.MenuItem toggleItem = new System.Windows.Forms.MenuItem();
            toggleItem.Text = "Pause Driver";
            toggleItem.Click += ToggleItem_Click;
            _nIcon.ContextMenu.MenuItems.Add(toggleItem);

            System.Windows.Forms.MenuItem exitItem = new System.Windows.Forms.MenuItem();
            exitItem.Text = "Exit";
            exitItem.Click += ExitItem_Click;
            _nIcon.ContextMenu.MenuItems.Add(exitItem);

            ControllerService.Instance.OnServiceStartStop += Instance_OnServiceStartStop;
            ControllerService.Instance.Start();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _canClose = true;
            Close();
        }

        private void ToggleItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            if(ControllerService.Instance.Started)
            {
                ControllerService.Instance.Stop();
                item.Text = "Resume Driver";
            } else
            {
                ControllerService.Instance.Start();
                item.Text = "Pause Driver";
            }
        }

        private void ItemShow_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void Instance_OnServiceStartStop(object sender, bool e)
        {
            _nIcon.Icon = e ? AppResources.app_icon_on : AppResources.app_icon_off;
            _nIcon.Text = "SWICD Driver " + (e ? "Running" : "Stopped");
            Icon = ToImageSource(e ? AppResources.app_icon_on : AppResources.app_icon_off);
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        private void NIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            ((MainWindowViewModel)DataContext).OnWindowClosing(e);

            if (!_canClose && !e.Cancel)
            {
                Hide();
                e.Cancel = true;
            }
        }
    }
}
