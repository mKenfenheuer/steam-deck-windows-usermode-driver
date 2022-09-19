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

namespace SWICD.ViewModels
{
    internal class ProfileEditPageViewModel : INotifyPropertyChanged
    {
        public string Executable { get; set; }
        public string PageTitle => Executable != null ? $"Controller Profile: {Executable}" : "Default Controller Profile";
        private ControllerConfig ControllerConfig { get; set; }
        public ObservableCollection<KeyboardMappingModel> KeyboardMappings { get; set; } = new ObservableCollection<KeyboardMappingModel>();
        public ObservableCollection<MouseMappingModel> MouseMappings { get; set; } = new ObservableCollection<MouseMappingModel>();
        public ObservableCollection<ButtonMappingModel> ButtonMappings { get; set; } = new ObservableCollection<ButtonMappingModel>();
        public ObservableCollection<AxisMappingModel> AxisMappings { get; set; } = new ObservableCollection<AxisMappingModel>();

        public Visibility DeleteButtonVisible => Executable != null ? Visibility.Visible : Visibility.Hidden;
        public CommandHandler DeleteButtonClickCommand => new CommandHandler((obj) => OnDeleteButtonClick());

        private void OnDeleteButtonClick()
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete the profile for {Executable}?",
                "Attention",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if(result == MessageBoxResult.Yes)
            {
                MainWindowViewModel.Instance.OnDeleteProfile(ControllerConfig);
            }
        }

        public bool DisableLizardMouse
        {
            get => ControllerConfig.ProfileSettings.DisableLizardMouse;
            set
            {
                ControllerConfig.ProfileSettings.DisableLizardMouse = value;
                NotifyPropertyChanged(nameof(DisableLizardMouseText));
            }
        }

        public bool HapticFeedbackEnabled
        {
            get => ControllerConfig.ProfileSettings.HapticFeedbackEnabled;
            set
            {
                ControllerConfig.ProfileSettings.HapticFeedbackEnabled = value;
                NotifyPropertyChanged(nameof(HapticfeedbackEnabledText));
            }
        }

        public byte HapticFeedbackPeriod
        {
            get => ControllerConfig.ProfileSettings.HapticFeedbackPeriod;
            set
            {
                ControllerConfig.ProfileSettings.HapticFeedbackPeriod = value;
                NotifyPropertyChanged(nameof(HapticFeedbackPeriodText));
            }
        }

        public byte HapticFeedbackAmplitude
        {
            get => ControllerConfig.ProfileSettings.HapticFeedbackAmplitude;
            set
            {
                ControllerConfig.ProfileSettings.HapticFeedbackAmplitude = value;
                NotifyPropertyChanged(nameof(HapticFeedbackAmplitudeText));
            }
        }
        public string HapticFeedbackPeriodText => $"Period ({HapticFeedbackPeriod})";
        public string HapticFeedbackAmplitudeText => $"Amplitude ({HapticFeedbackAmplitude})";
        public string HapticfeedbackEnabledText => !HapticFeedbackEnabled ? "Haptic Feedback Disabled" : "Haptic Feedback Enabled";
        public string DisableLizardMouseText => DisableLizardMouse ? "Mouse Movement Disabled" : " Mouse Movement Enabled";

        public bool DisableLizardButtons
        {
            get => ControllerConfig.ProfileSettings.DisableLizardButtons;
            set
            {
                ControllerConfig.ProfileSettings.DisableLizardButtons = value;
                NotifyPropertyChanged(nameof(DisableLizardButtonsText));
            }
        }
        public string DisableLizardButtonsText => DisableLizardButtons ? "Buttons Disabled" : "Buttons Enabled";

        public ProfileEditPageViewModel()
        {
            ControllerConfig = new ControllerConfig();
            PopulateMappings();
        }

        public ProfileEditPageViewModel(ControllerConfig controllerConfig)
        {
            if (controllerConfig == null)
                controllerConfig = new ControllerConfig();
            Executable = controllerConfig.Executable;
            ControllerConfig = controllerConfig;
            PopulateMappings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PopulateMappings()
        {
            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                    ButtonMappings.Add(new ButtonMappingModel()
                    {
                        HardwareButton = button,
                        EmulatedButton = ControllerConfig.ButtonMapping[button],
                        SetAction = val => ControllerConfig.ButtonMapping[button] = val,
                    });


            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                    KeyboardMappings.Add(new KeyboardMappingModel()
                    {
                        HardwareButton = button,
                        EmulatedKeyboardKey = ControllerConfig.KeyboardMapping[button],
                        SetAction = val => ControllerConfig.KeyboardMapping[button] = val,
                    });

            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                    MouseMappings.Add(new MouseMappingModel()
                    {
                        HardwareButton = button,
                        EmulatedMouseButton = ControllerConfig.MouseMapping[button],
                        SetAction = val => ControllerConfig.MouseMapping[button] = val,
                    });

            foreach (HardwareAxis axis in Enum.GetValues(typeof(HardwareAxis)))
                if (axis != HardwareAxis.None)
                    AxisMappings.Add(new AxisMappingModel()
                    {
                        HardwareAxis = axis,
                        EmulatedAxis = ControllerConfig.AxisMapping[axis].EmulatedAxis,
                        ActivationButton = ControllerConfig.AxisMapping[axis].ActivationButton,
                        Inverted = ControllerConfig.AxisMapping[axis].Inverted,
                        SetAxisAction = val => ControllerConfig.AxisMapping[axis].EmulatedAxis = val,
                        SetActivationButtonAction = val => ControllerConfig.AxisMapping[axis].ActivationButton = val,
                    });
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
