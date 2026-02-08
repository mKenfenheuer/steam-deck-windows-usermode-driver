using SWICD.Services;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWICD.HVDK;

namespace SWICD.Model
{
    internal class KeyboardMappingModel : INotifyPropertyChanged
    {
        private string _emulatedKeyboardKey = "NONE";

        public event PropertyChangedEventHandler PropertyChanged;

        public string ButtonText => FontEnumMapper.MapHardwareButtonToFont(HardwareButton);
        public HardwareButton HardwareButton { get; set; }
        public string SelectedKeyboardKeyDisplay => _emulatedKeyboardKey;
        public Action<string> SetAction { get; set; }
        public string EmulatedKeyboardKey
        {
            get => _emulatedKeyboardKey;
            set
            {
                _emulatedKeyboardKey = value;
                if (SetAction != null)
                    SetAction(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedKeyboardKeyDisplay)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmulatedKeyboardKey)));
            }
        }
    }
}
