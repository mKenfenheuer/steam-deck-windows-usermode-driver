using System;

namespace SWICD_Lib.Config
{
    public class EmulatedAxisConfig
    {
        public EmulatedAxis EmulatedAxis { get; set; }
        public HardwareButton ActivationButton { get; set; }
        public bool Inverted { get; set; }

        public EmulatedAxisConfig(EmulatedAxis emulatedAxis, HardwareButton activationButton, bool inverted)
        {
            EmulatedAxis = emulatedAxis;
            ActivationButton = activationButton;
            Inverted = inverted;
        }

        public EmulatedAxisConfig(EmulatedAxis emulatedAxis, HardwareButton activationButton)
        {
            EmulatedAxis = emulatedAxis;
            ActivationButton = activationButton;
            Inverted = false;
        }

        public EmulatedAxisConfig(EmulatedAxis emulatedAxis, bool inverted)
        {
            EmulatedAxis = emulatedAxis;
            ActivationButton = HardwareButton.None;
            Inverted = inverted;
        }

        public EmulatedAxisConfig(EmulatedAxis emulatedAxis)
        {
            EmulatedAxis = emulatedAxis;
            ActivationButton = HardwareButton.None;
            Inverted = false;
        }

        public override string ToString()
        {
            string config = $"{GetEmulatedAxisName(EmulatedAxis)}";
            if (ActivationButton != HardwareButton.None)
                config += $",activate={GetHardwareButtonName(ActivationButton)}";
            if (Inverted)
                config += $",inverted=true";
            return config;
        }

        internal string GetEmulatedAxisName(EmulatedAxis value) => Enum.GetName(typeof(EmulatedAxis), value);
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);

    }
}