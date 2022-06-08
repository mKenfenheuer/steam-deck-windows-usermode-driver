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
    internal class ButtonMappingModel
    {
        public ObservableCollection<EmulatedButton> ButtonItems { get; set; } = new ObservableCollection<EmulatedButton>(Enum.GetValues(typeof(EmulatedButton)).Cast<EmulatedButton>());
        public string ButtonText => Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EmulatedButton EmulatedButton { get; set; }
    }
}
