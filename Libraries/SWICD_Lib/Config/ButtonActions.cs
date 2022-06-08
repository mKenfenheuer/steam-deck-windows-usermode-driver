using System;

namespace SWICD_Lib.Config
{
    public class ButtonActions
    {
        public HardwareButton OpenWindowsGameBar { get; set; } = HardwareButton.None;

        public override string ToString()
        {
            return $"[actions]\r\nOpenWindowsGameBar={GetHardwareButtonName(OpenWindowsGameBar)}\r\n";
        }
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);
    }
}