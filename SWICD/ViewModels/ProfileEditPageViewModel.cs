using SWICD.Model;
using SWICD_Lib.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.ViewModels
{
    internal class ProfileEditPageViewModel : INotifyPropertyChanged
    {
        public string Executable { get; set; }
        public string PageTitle => Executable != null ? $"Controller Profile: {Executable}" : "Default Controller Profile";
        private ControllerConfig ControllerConfig { get; set; }
        public ObservableCollection<ButtonMappingModel> ButtonMappings { get; set; } = new ObservableCollection<ButtonMappingModel>();
        public ObservableCollection<AxisMappingModel> AxisMappings { get; set; } = new ObservableCollection<AxisMappingModel>();

        public bool DisableLizardMode
        {
            get => ControllerConfig.ProfileSettings.DisableLizardMode;
            set
            {
                ControllerConfig.ProfileSettings.DisableLizardMode = value;
                NotifyPropertyChanged(nameof(DisableLizardModeText));
            }
        }
        public string DisableLizardModeText => DisableLizardMode ? "Disabled" : "Enabled";

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
                        EmulatedButton = ControllerConfig.ButtonMapping[button]
                    });

            foreach (HardwareAxis axis in Enum.GetValues(typeof(HardwareButton)))
                if (axis != HardwareAxis.None)
                    AxisMappings.Add(new AxisMappingModel()
                    {
                        HardwareAxis = axis,
                        EmulatedAxis = ControllerConfig.AxisMapping[axis].EmulatedAxis,
                        ActivationButton = ControllerConfig.AxisMapping[axis].ActivationButton,
                        Inverted = ControllerConfig.AxisMapping[axis].Inverted,
                    });
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
