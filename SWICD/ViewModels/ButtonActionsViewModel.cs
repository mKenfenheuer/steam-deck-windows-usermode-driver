using Microsoft.Win32;
using SWICD.Commands;
using SWICD.Config;
using SWICD.Model;
using SWICD.Services;
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
    internal class ButtonActionsViewModel : INotifyPropertyChanged
    {
        private ButtonActions _buttonActions { get; set; } = new ButtonActions();
        public CommandHandler AddButtonActionClickedCommand => new CommandHandler((obj) => OnAddButtonAction());
        public ObservableCollection<ButtonActionModel> ButtonActions { get; set; } = new ObservableCollection<ButtonActionModel>();
        public ButtonActionsViewModel(ButtonActions buttonActions)
        {
            _buttonActions = buttonActions;
            foreach (var btns in _buttonActions.GetActionButtons)
            {
                var action = _buttonActions[btns];
                ButtonActions.Add(new ButtonActionModel()
                {
                    HardwareButtons = btns,
                    ButtonAction = action,
                    OnUpdate = OnUpdateButtonAction,
                });
            };
        }

        public ButtonActionsViewModel()
        {
        }

        private void OnAddButtonAction()
        {

        }
        private void OnUpdateButtonAction(ButtonActionModel model)
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
