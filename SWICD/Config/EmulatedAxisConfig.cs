using System;

namespace SWICD.Config
{
    public class EmulatedAxisConfig : ICloneable
    {
        private EmulatedAxis _emulatedAxis;
        public EmulatedAxis EmulatedAxis { get => _emulatedAxis; set => _emulatedAxis = value; }

        private HardwareButton _activationButton;
        public HardwareButton ActivationButton { get => _activationButton; set => _activationButton = value; }

        private bool _inverted;
        public bool Inverted { get => _inverted; set => _inverted = value; }

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

        public EmulatedAxisConfig(string config)
        {
            string[] configs = config.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Enum.TryParse(configs[0], out _emulatedAxis);

            for(int i = 1; i < configs.Length; i++)
            {
                string conf = configs[i];
                if(conf.StartsWith("activate"))
                {
                    Enum.TryParse(conf.Replace("activate=", ""), out _activationButton);
                }

                if(conf.StartsWith("invert"))
                {
                    bool.TryParse(conf.Replace("inverted=", ""), out _inverted);
                }
            }

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

        public override bool Equals(object obj)
        {
            return obj is EmulatedAxisConfig config &&
                   _emulatedAxis == config._emulatedAxis &&
                   _activationButton == config._activationButton &&
                   _inverted == config._inverted;
        }

        public object Clone()
        {
           return new EmulatedAxisConfig(_emulatedAxis, _activationButton, _inverted);
        }
    }
}