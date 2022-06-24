using SWICD.Services;
using SWICD_Lib.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace SWICD.Model
{
    internal class KeyboardMappingModel
    {
        private EnumComboBoxItem<VirtualKeyCode> _selectedEmulatedButton;

        public ObservableCollection<EnumComboBoxItem<VirtualKeyCode>> KeyboardItems { get; set; } = new ObservableCollection<EnumComboBoxItem<VirtualKeyCode>>(Enum.GetValues(typeof(VirtualKeyCode)).Cast<VirtualKeyCode>().Select(e => new EnumComboBoxItem<VirtualKeyCode>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(e),
        }));

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton); // Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<VirtualKeyCode> SelectedKeyboardButton
        {
            get => _selectedEmulatedButton;
            set
            {
                _selectedEmulatedButton = value;
                if (SetAction != null)
                    SetAction(value.Value);
            }
        }
        public Action<VirtualKeyCode> SetAction { get; set; }
        public VirtualKeyCode EmulatedKeyboardKey
        {
            get => SelectedKeyboardButton.Value;
            set
            {
                SelectedKeyboardButton = new EnumComboBoxItem<VirtualKeyCode>()
                {
                    Value = value,
                    Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(value),
                };
            }
        }
    }
}
