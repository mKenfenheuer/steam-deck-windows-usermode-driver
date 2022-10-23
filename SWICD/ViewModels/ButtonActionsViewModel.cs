using Microsoft.Win32;
using SWICD.Commands;
using SWICD.Config;
using SWICD.Model;
using SWICD.Pages;
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
                    OnDelete = OnDeleteButtonAction,
                    OnUpdate = OnUpdateButtonAction,
                });
            };
        }

        public ButtonActionsViewModel()
        {
        }

        private void OnAddButtonAction()
        {
            EditButtonActionWindow window = new EditButtonActionWindow(new ButtonActionModel()
            {
                HardwareButtons = new HardwareButton[] { HardwareButton.BtnSteam, HardwareButton.BtnQuickAccess },
                ButtonAction = new ButtonAction()
                {
                    Type = "keyboard-shortcut",
                    Data = "[LCTRL]+[LALT]+DELETE"
                },
                OnDelete = OnDeleteButtonAction,
                OnUpdate = OnUpdateButtonAction,

            });
            window.ShowDialog();
            var action = window.GetResult();
            ButtonActions.Add(action);
            OnUpdateButtonAction(action);
        }

        private void OnDeleteButtonAction(ButtonActionModel obj)
        {
            ButtonActions.Remove(obj);
            _buttonActions.Clear();
            foreach (var model in ButtonActions)
            {
                _buttonActions[model.HardwareButtons] = model.ButtonAction;
            }
        }

        private void OnUpdateButtonAction(ButtonActionModel action)
        {
            _buttonActions.Clear();
            foreach (var model in ButtonActions)
            {
                _buttonActions[model.HardwareButtons] = model.ButtonAction;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
