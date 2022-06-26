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
    internal class KeyboardMappingModel
    {
        private EnumComboBoxItem<VirtualKeyboardKey> _selectedEmulatedButton;

        public ObservableCollection<EnumComboBoxItem<VirtualKeyboardKey>> KeyboardItems { get; set; } = new ObservableCollection<EnumComboBoxItem<VirtualKeyboardKey>>(Enum.GetValues(typeof(VirtualKeyboardKey)).Cast<VirtualKeyboardKey>().Select(e => new EnumComboBoxItem<VirtualKeyboardKey>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(e),
        }));

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton); // Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<VirtualKeyboardKey> SelectedKeyboardKey
        {
            get => _selectedEmulatedButton;
            set
            {
                _selectedEmulatedButton = value;
                if (SetAction != null)
                    SetAction(value.Value);
            }
        }
        public Action<VirtualKeyboardKey> SetAction { get; set; }
        public VirtualKeyboardKey EmulatedKeyboardKey
        {
            get => SelectedKeyboardKey.Value;
            set
            {
                SelectedKeyboardKey = new EnumComboBoxItem<VirtualKeyboardKey>()
                {
                    Value = value,
                    Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(value),
                };
            }
        }
    }
}
