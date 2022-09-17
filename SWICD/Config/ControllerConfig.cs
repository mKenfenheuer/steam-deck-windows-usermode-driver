using System;
using System.Collections.Generic;

namespace SWICD.Config
{
    public class ControllerConfig : ICloneable
    {
        public string Executable { get; set; } = null;
        public ProfileSettings ProfileSettings { get; set; } = new ProfileSettings();
        public ButtonMapping ButtonMapping { get; set; } = new ButtonMapping();
        public AxisMapping AxisMapping { get; set; } = new AxisMapping();
        public KeyboardMapping KeyboardMapping { get; set; } = new KeyboardMapping();
        public MouseMapping MouseMapping { get; set; } = new MouseMapping();

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
            return ProfileSettings.ToString(Executable) + "\r\n" + ButtonMapping.ToString(Executable) + "\r\n" + MouseMapping.ToString(Executable) +"\r\n" + KeyboardMapping.ToString(Executable) + "\r\n" + AxisMapping.ToString(Executable);
        }

        public object Clone()
        {
            ControllerConfig clone = (ControllerConfig)MemberwiseClone();
            clone.Executable = (string)Executable?.Clone();
            clone.ProfileSettings = (ProfileSettings)ProfileSettings.Clone();
            clone.ButtonMapping = (ButtonMapping)ButtonMapping.Clone();
            clone.AxisMapping = (AxisMapping)AxisMapping.Clone();
            clone.KeyboardMapping = (KeyboardMapping)KeyboardMapping.Clone();
            clone.MouseMapping = (MouseMapping)MouseMapping.Clone();
            return clone;
        }

        public override bool Equals(object obj)
        {
            return obj is ControllerConfig config &&
                   Executable == config.Executable &&
                   EqualityComparer<ProfileSettings>.Default.Equals(ProfileSettings, config.ProfileSettings) &&
                   EqualityComparer<ButtonMapping>.Default.Equals(ButtonMapping, config.ButtonMapping) &&
                   EqualityComparer<KeyboardMapping>.Default.Equals(KeyboardMapping, config.KeyboardMapping) &&
                   EqualityComparer<MouseMapping>.Default.Equals(MouseMapping, config.MouseMapping) &&
                   EqualityComparer<AxisMapping>.Default.Equals(AxisMapping, config.AxisMapping);
        }
    }
}