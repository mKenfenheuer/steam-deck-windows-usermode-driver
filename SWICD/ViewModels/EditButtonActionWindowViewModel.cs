using SWICD.Model;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWICD.Commands;
using System.Windows;
using System.Data.SqlClient;
using SWICD.Services;
using SWICD.HVDK;

namespace SWICD.ViewModels
{
    internal class EditButtonActionWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EnumComboBoxItem<HardwareButton>> ButtonItems { get; set; } = new ObservableCollection<EnumComboBoxItem<HardwareButton>>(Enum.GetValues(typeof(HardwareButton)).Cast<HardwareButton>().Where(e => e != HardwareButton.None).Select(e => new EnumComboBoxItem<HardwareButton>()
        {
            Value = e,
            Display = FontEnumMapper.MapHardwareButtonToFont(e),
        }));

        public ObservableCollection<EnumComboBoxItem<string>> KeyboardItems { get; set; } = new ObservableCollection<EnumComboBoxItem<string>>(new KeyboardUtils().GetAvailableKeysWithModifiers.Select(e => new EnumComboBoxItem<string>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(e),
        }));

        public ObservableCollection<HardwareButton> TriggerButtons { get; set; } = new ObservableCollection<HardwareButton>();
        public ObservableCollection<string> KeyboardActions { get; set; } = new ObservableCollection<string>();

        public CommandHandler AddTriggerButtonClickedCommand => new CommandHandler((obj) => OnAddTriggerButtonClicked());

        public CommandHandler ClearTriggerButtonsClickedCommand => new CommandHandler((obj) => OnClearTriggerButtonClicked());

        public CommandHandler AddKeyboardButtonClickedCommand => new CommandHandler((obj) => OnAddKeyboardButtonClicked());

        public CommandHandler ClearKeyboardButtonsClickedCommand => new CommandHandler((obj) => OnClearKeyboardButtonClicked());

        public string Type { get => Result.ButtonAction.Type; set => Result.ButtonAction.Type = value; }

        public int SelectedKeyboardButton { get; set; }
        public int SelectedTriggerButton { get; set; }

        public bool IsKeyboardAction
        {
            get => Type == "keyboard-shortcut";
            set
            {
                if (value)
                    Type = "keyboard-shortcut";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsHapticsToggle
        {
            get => Type == "toggle-haptics";
            set
            {
                if (value)
                    Type = "toggle-haptics";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsLizardToggle
        {
            get => Type == "toggle-lizardmode";
            set
            {
                if (value)
                    Type = "toggle-lizardmode";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsLizardButtonsToggle
        {
            get => Type == "toggle-lizardbuttons";
            set
            {
                if (value)
                    Type = "toggle-lizardbuttons";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsLizardButtonsMouseToggle
        {
            get => Type == "toggle-lizardbuttons+mouse";
            set
            {
                if (value)
                    Type = "toggle-lizardbuttons+mouse";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsEmulationToggle
        {
            get => Type == "toggle-emulation";
            set
            {
                if (value)
                    Type = "toggle-emulation";
                NotifyPropertyChanged(nameof(Type));
                NotifyPropertyChanged(nameof(IsKeyboardAction));
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }
        public bool IsLizardEmulationToggle
        {
            get => Type == "toggle-lizardmode+emulation";
            set
            {
                if (value)
                    Type = "toggle-lizardmode+emulation";
                NotifyPropertyChanged(nameof(IsHapticsToggle));
                NotifyPropertyChanged(nameof(IsLizardToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsToggle));
                NotifyPropertyChanged(nameof(IsLizardButtonsMouseToggle));
                NotifyPropertyChanged(nameof(IsEmulationToggle));
                NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            }
        }

        public string ButtonsPreview => String.Join(" + ", TriggerButtons.Select(b => FontEnumMapper.MapHardwareButtonToFont(b)).ToArray());
        public string KeyboardButtonsPreview => String.Join(" + ", KeyboardActions.ToArray());

        public ButtonActionModel Result { get; internal set; }

        public EditButtonActionWindowViewModel(ButtonActionModel buttonActionModel)
        {
            foreach (var btn in buttonActionModel.HardwareButtons)
                TriggerButtons.Add(btn);

            if (buttonActionModel.ButtonAction.Type == "keyboard-shortcut")
                foreach (var key in buttonActionModel.ButtonAction.Data.Split("+".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    KeyboardActions.Add(key);

            TriggerButtons.CollectionChanged += (s, e) => OnChange();
            KeyboardActions.CollectionChanged += (s, e) => OnChange();

            Result = buttonActionModel;

            _ = Task.Run(async () =>
            {
                await Task.Delay(100);

                SelectedKeyboardButton = 0;
                SelectedTriggerButton = 0;
                NotifyPropertyChanged(nameof(SelectedTriggerButton));
                NotifyPropertyChanged(nameof(SelectedKeyboardButton));
            });

            OnChange();
        }

        private void OnChange()
        {
            Result.HardwareButtons = TriggerButtons.ToArray();
            Result.ButtonAction.Type = Type;
            if (Type == "keyboard-shortcut")
                Result.ButtonAction.Data = String.Join("+", KeyboardActions.ToArray());
            else
                Result.ButtonAction.Data = "";

            NotifyPropertyChanged(nameof(Type));
            NotifyPropertyChanged(nameof(IsKeyboardAction));
            NotifyPropertyChanged(nameof(IsLizardToggle));
            NotifyPropertyChanged(nameof(IsEmulationToggle));
            NotifyPropertyChanged(nameof(IsLizardEmulationToggle));
            NotifyPropertyChanged(nameof(ButtonsPreview));
            NotifyPropertyChanged(nameof(KeyboardButtonsPreview));
        }
        private void OnAddTriggerButtonClicked()
        {
            TriggerButtons.Add(ButtonItems[SelectedTriggerButton].Value);
        }
        private void OnClearTriggerButtonClicked()
        {
            TriggerButtons.Clear();
        }
        private void OnAddKeyboardButtonClicked()
        {
            KeyboardActions.Add(KeyboardItems[SelectedKeyboardButton].Value);
        }
        private void OnClearKeyboardButtonClicked()
        {
            KeyboardActions.Clear();
        }

        public EditButtonActionWindowViewModel() : this(new ButtonActionModel()
        {
            HardwareButtons = new HardwareButton[] { HardwareButton.BtnSteam, HardwareButton.BtnQuickAccess },
            ButtonAction = new ButtonAction()
            {
                Type = "keyboard-shortcut",
                Data = "[LCTRL]+[LALT]+DELETE"
            },
        })
        { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
