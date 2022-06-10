using SWICD_Lib.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWICD.Model
{
    internal class AxisMappingModel
    {
        public ObservableCollection<EnumComboBoxItem<HardwareButton>> ButtonItems { get; set; } = new ObservableCollection<EnumComboBoxItem<HardwareButton>>(Enum.GetValues(typeof(HardwareButton)).Cast<HardwareButton>().Select(e => new EnumComboBoxItem<HardwareButton>()
        {
            Value = e,
            Display = GetButtonDisplayText(e),
        }));

        private static string GetButtonDisplayText(HardwareButton e)
        {
            return Regex.Replace(e.ToString().Replace("Btn",""), "([^ ])([A-Z])", "$1 $2");
        }

        public ObservableCollection<EnumComboBoxItem<EmulatedAxis>> AxisItems { get; set; } = new ObservableCollection<EnumComboBoxItem<EmulatedAxis>>(Enum.GetValues(typeof(EmulatedAxis)).Cast<EmulatedAxis>().Select(e => new EnumComboBoxItem<EmulatedAxis>()
        {
            Value = e,
            Display = GetAxisDisplayText(e),
        }));

        private static string GetAxisDisplayText(EmulatedAxis e)
        {
            return Regex.Replace(e.ToString(), "([^ ])([A-Z])", "$1 $2");
        }

        public string AxisText => Regex.Replace(HardwareAxis.ToString(), "([^A-Z])([A-Z])", "$1 $2");
        public bool Inverted { get; set; }
        public EnumComboBoxItem<HardwareButton> SelectedActivationButton { get; set; }
        public HardwareButton ActivationButton
        {
            get => SelectedActivationButton.Value; set => SelectedActivationButton = new EnumComboBoxItem<HardwareButton>()
            {
                Value = value,
                Display = GetButtonDisplayText(value),
            };
        }
        public HardwareAxis HardwareAxis { get; set; } = HardwareAxis.None;
        public EnumComboBoxItem<EmulatedAxis> SelectedEmulatedAxis { get; set; }
        public EmulatedAxis EmulatedAxis
        {
            get => SelectedEmulatedAxis.Value; set => SelectedEmulatedAxis = new EnumComboBoxItem<EmulatedAxis>()
            {
                Value = value,
                Display = GetAxisDisplayText(value),
            };
        }
    }
}
