using SWICD.Services;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWICD.Model
{
    internal class ButtonMappingModel
    {
        private EnumComboBoxItem<EmulatedButton> _selectedEmulatedButton;

        public ObservableCollection<EnumComboBoxItem<EmulatedButton>> ButtonItems { get; set; } = new ObservableCollection<EnumComboBoxItem<EmulatedButton>>(Enum.GetValues(typeof(EmulatedButton)).Cast<EmulatedButton>().Select(e => new EnumComboBoxItem<EmulatedButton>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedButtonToFont(e),
        }));

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton); // Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<EmulatedButton> SelectedEmulatedButton
        {
            get => _selectedEmulatedButton;
            set
            {
                _selectedEmulatedButton = value;
                if (SetAction != null)
                    SetAction(value.Value);
            }
        }
        public Action<EmulatedButton> SetAction { get; set; }
        public EmulatedButton EmulatedButton
        {
            get => SelectedEmulatedButton.Value;
            set
            {
                SelectedEmulatedButton = new EnumComboBoxItem<EmulatedButton>()
                {
                    Value = value,
                    Display = FontEnumMapper.MapEmulatedButtonToFont(value),
                };
            }
        }
    }
}
