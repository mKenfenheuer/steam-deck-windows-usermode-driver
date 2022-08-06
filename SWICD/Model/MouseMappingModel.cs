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
    internal class MouseMappingModel
    {
        private EnumComboBoxItem<VirtualMouseKey> _selectedEmulatedButton;

        public ObservableCollection<EnumComboBoxItem<VirtualMouseKey>> MouseItems { get; set; } = new ObservableCollection<EnumComboBoxItem<VirtualMouseKey>>(Enum.GetValues(typeof(VirtualMouseKey)).Cast<VirtualMouseKey>().Select(e => new EnumComboBoxItem<VirtualMouseKey>()
        {
            Value = e,
            Display = FontEnumMapper.MapEmulatedMouseButtonToFont(e),
        }));

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton); // Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<VirtualMouseKey> SelectedMouseButton
        {
            get => _selectedEmulatedButton;
            set
            {
                _selectedEmulatedButton = value;
                if (SetAction != null)
                    SetAction(value.Value);
            }
        }
        public Action<VirtualMouseKey> SetAction { get; set; }
        public VirtualMouseKey EmulatedMouseButton
        {
            get => SelectedMouseButton.Value;
            set
            {
                SelectedMouseButton = new EnumComboBoxItem<VirtualMouseKey>()
                {
                    Value = value,
                    Display = FontEnumMapper.MapEmulatedMouseButtonToFont(value),
                };
            }
        }
    }
}
