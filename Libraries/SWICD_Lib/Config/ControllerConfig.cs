using System;

namespace SWICD_Lib.Config
{
    public class ControllerConfig : ICloneable
    {
        public string Executable { get; set; } = null;
        public ProfileSettings ProfileSettings { get; set; } = new ProfileSettings();
        public ButtonMapping ButtonMapping { get; set; } = new ButtonMapping();
        public AxisMapping AxisMapping { get; set; } = new AxisMapping();

        public ControllerConfig()
        {
        }

        public ControllerConfig(string executable)
        {
            Executable = executable;
        }
        public ControllerConfig(string executable, ButtonMapping buttonMapping, AxisMapping axisMapping) : this(executable)
        {
            ButtonMapping = buttonMapping;
            AxisMapping = axisMapping;
        }

        public override string ToString()
        {
            return ProfileSettings.ToString(Executable) + "\r\n" + ButtonMapping.ToString(Executable) + "\r\n" + AxisMapping.ToString(Executable);
        }

        public object Clone()
        {
            ControllerConfig clone = (ControllerConfig)MemberwiseClone();
            clone.Executable = (string)Executable?.Clone();
            clone.ProfileSettings = (ProfileSettings)ProfileSettings.Clone();
            clone.ButtonMapping = (ButtonMapping)ButtonMapping.Clone();
            clone.AxisMapping = (AxisMapping)AxisMapping.Clone();
            return clone;
        }
    }
}