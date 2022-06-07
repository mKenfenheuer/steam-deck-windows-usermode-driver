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
        public ObservableCollection<HardwareButton> ButtonItems { get; set; } = new ObservableCollection<HardwareButton>(Enum.GetValues(typeof(HardwareButton)).Cast<HardwareButton>());
        public ObservableCollection<EmulatedAxis> AxisItems { get; set; } = new ObservableCollection<EmulatedAxis>(Enum.GetValues(typeof(EmulatedAxis)).Cast<EmulatedAxis>());
        public string AxisText => Regex.Replace(HardwareAxis.ToString(), "([^A-Z])([A-Z])", "$1 $2");
        public bool Inverted { get; set; }
        public HardwareButton ActivationButton { get; set; } = HardwareButton.None;
        public HardwareAxis HardwareAxis { get; set; } = HardwareAxis.None;
        public EmulatedAxis EmulatedAxis { get; set; } = EmulatedAxis.None;
    }
}
