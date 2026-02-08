using System;
using System.Windows;
using System.Windows.Controls;

namespace SWICD.Pages
{
    public partial class KeyboardPickerWindow : Window
    {
        public string SelectedKey { get; private set; }

        public KeyboardPickerWindow(string currentKey)
        {
            InitializeComponent();
            SelectedKey = currentKey ?? "NONE";
            CurrentKeyText.Text = SelectedKey;
        }

        private void KeyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag != null)
            {
                SelectedKey = button.Tag.ToString();
                DialogResult = true;
                Close();
            }
        }
    }
}
