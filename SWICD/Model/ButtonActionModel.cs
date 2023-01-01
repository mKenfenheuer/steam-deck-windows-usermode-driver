using SWICD.Services;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWICD.Commands;
using SWICD.Pages;
using System.ComponentModel;

namespace SWICD.Model
{
    public class ButtonActionModel : INotifyPropertyChanged
    {
        public string TriggerText => String.Join(" + ", HardwareButtons.Select(b => FontEnumMapper.MapHardwareButtonToFont(b)).ToArray());
        public string ActionText
        {
            get
            {
                switch (ButtonAction.Type)
                {
                    case "keyboard-shortcut":
                        return ButtonAction.Data;
                    case "toggle-haptics":
                        return "Toggle Controller Rumble";
                    case "toggle-lizardmode":
                        return "Toggle Mouse Lizardmode";
                    case "toggle-lizardbuttons":
                        return "Toggle Buttons Lizardmode";
                    case "toggle-lizardbuttons+mouse":
                        return "Toggle Buttons & Mouse Lizardmode";
                    case "toggle-emulation":
                        return "Toggle Emulation";
                    case "toggle-lizardmode+emulation":
                        return "Toggle Lizardmode & Emulation";
                    case "toggle-onscreenkeyboard":
                        return "Toggle OnScreenKeyboard";
                    default:
                        return "Unknown?!";
                }
            }
        }
        public HardwareButton[] HardwareButtons { get; set; }
        public ButtonAction ButtonAction { get; set; }
        public CommandHandler EditButtonActionClickedCommand => new CommandHandler((obj) => OnEditButtonAction());
        public CommandHandler DeleteButtonActionClickedCommand => new CommandHandler((obj) => OnDeleteButtonAction());
        public Action<ButtonActionModel> OnDelete { get; set; }
        public Action<ButtonActionModel> OnUpdate { get; internal set; }

        private void OnEditButtonAction()
        {
            EditButtonActionWindow window = new EditButtonActionWindow(this);
            window.ShowDialog();
            NotifyPropertyChanged(nameof(ActionText));
            NotifyPropertyChanged(nameof(TriggerText));
            OnUpdate(this);
        }
        private void OnDeleteButtonAction()
        {
            OnDelete(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
