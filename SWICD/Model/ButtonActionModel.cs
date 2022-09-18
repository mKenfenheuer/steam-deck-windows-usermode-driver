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

namespace SWICD.Model
{
    internal class ButtonActionModel
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
                    case "toggle-lizardmode":
                        return "Toggle Lizardmode";
                    case "toggle-emulation":
                        return "Toggle Emulation";
                    case "toggle-lizardmode+emulation":
                        return "Toggle Lizardmode & Emulation";
                    default:
                        return "Unknown?!";
                }
            }
        }
        public HardwareButton[] HardwareButtons { get; set; }
        public ButtonAction ButtonAction { get; set; }
        public CommandHandler EditButtonActionClickedCommand => new CommandHandler((obj) => OnEditButtonAction());
        public Action<ButtonActionModel> OnUpdate { get; set; }

        private void OnEditButtonAction()
        {

        }
    }
}
