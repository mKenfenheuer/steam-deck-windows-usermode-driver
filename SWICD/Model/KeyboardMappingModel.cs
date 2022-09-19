using SWICD.Services;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWICD.HVDK;

namespace SWICD.Model
{
    internal class KeyboardMappingModel
    {
        private EnumComboBoxItem<string> _selectedEmulatedButton;

        public ObservableCollection<EnumComboBoxItem<string>> KeyboardItems { get; set; } = new ObservableCollection<EnumComboBoxItem<string>>(new string[] { "NONE" }.Concat(new KeyboardUtils().GetAvailableKeysWithModifiers).Select(e => new EnumComboBoxItem<string>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(e),
        }));

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton); // Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<string> SelectedKeyboardKey
        {
            get => _selectedEmulatedButton;
            set
            {
                _selectedEmulatedButton = value;
                if (SetAction != null)
                    SetAction(value.Value);
            }
        }
        public Action<string> SetAction { get; set; }
        public string EmulatedKeyboardKey
        {
            get => SelectedKeyboardKey.Value;
            set
            {
                SelectedKeyboardKey = new EnumComboBoxItem<string>()
                {
                    Value = value,
                    Display = FontEnumMapper.MapEmulatedKeyboardKeyToFont(value),
                };
            }
        }
    }
}
