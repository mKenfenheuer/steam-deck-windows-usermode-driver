using System;
using System.Collections.Generic;
using System.Linq;

namespace SWICD.Config
{
    public class MouseMapping : ICloneable
    {
        private Dictionary<HardwareButton, VirtualMouseKey> _mappings = new Dictionary<HardwareButton, VirtualMouseKey>()
        {
            { HardwareButton.BtnX, VirtualMouseKey.NONE },
            { HardwareButton.BtnY, VirtualMouseKey.NONE },
            { HardwareButton.BtnA, VirtualMouseKey.NONE },
            { HardwareButton.BtnB, VirtualMouseKey.NONE },
            { HardwareButton.BtnMenu, VirtualMouseKey.NONE },
            { HardwareButton.BtnOptions, VirtualMouseKey.NONE },
            { HardwareButton.BtnSteam, VirtualMouseKey.NONE },
            { HardwareButton.BtnQuickAccess, VirtualMouseKey.NONE },
            { HardwareButton.BtnDpadUp, VirtualMouseKey.NONE },
            { HardwareButton.BtnDpadLeft, VirtualMouseKey.NONE },
            { HardwareButton.BtnDpadRight, VirtualMouseKey.NONE },
            { HardwareButton.BtnDpadDown, VirtualMouseKey.NONE },
            { HardwareButton.BtnL1, VirtualMouseKey.NONE },
            { HardwareButton.BtnR1, VirtualMouseKey.NONE },
            { HardwareButton.BtnL2, VirtualMouseKey.NONE },
            { HardwareButton.BtnR2, VirtualMouseKey.NONE },
            { HardwareButton.BtnL4, VirtualMouseKey.NONE },
            { HardwareButton.BtnR4, VirtualMouseKey.NONE },
            { HardwareButton.BtnL5, VirtualMouseKey.NONE },
            { HardwareButton.BtnR5, VirtualMouseKey.NONE },
            { HardwareButton.BtnRPadPress, VirtualMouseKey.NONE },
            { HardwareButton.BtnLPadPress, VirtualMouseKey.NONE },
            { HardwareButton.BtnRPadTouch, VirtualMouseKey.NONE },
            { HardwareButton.BtnLPadTouch, VirtualMouseKey.NONE },
            { HardwareButton.BtnRStickPress, VirtualMouseKey.NONE },
            { HardwareButton.BtnLStickPress, VirtualMouseKey.NONE },
            { HardwareButton.BtnRStickTouch, VirtualMouseKey.NONE },
            { HardwareButton.BtnLStickTouch, VirtualMouseKey.NONE },
        };

        public MouseMapping(Dictionary<HardwareButton, VirtualMouseKey> mappings)
        {
            _mappings = mappings;
        }

        public MouseMapping()
        {
        }

        public VirtualMouseKey this[HardwareButton button]
        {
            get
            {
                if (_mappings.ContainsKey(button))
                    return _mappings[button];
                return VirtualMouseKey.NONE;
            }
            set
            {
                _mappings[button] = value;
            }
        }
        public object Clone()
        {
            var clone = new MouseMapping(_mappings.ToDictionary(entry => entry.Key,
                                               entry => entry.Value));
            return clone;
        }

        internal string ToString(string executable = null)
        {
            string config = $"[mousebuttons]\r\n";
            if (executable != null)
            {
                config = $"[mousebuttons,{executable}]\r\n";
            }

            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                {
                    config += $"{GetHardwareButtonName(button)}={GetEmulatedKeyName(this[button])}\r\n";
                }

            return config;
        }
        internal string GetEmulatedKeyName(VirtualMouseKey value) => Enum.GetName(typeof(VirtualMouseKey), value);
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);

        public override bool Equals(object obj)
        {
            return obj is MouseMapping mapping &&
                   _mappings.EqualsWithValues(mapping._mappings);
        }
    }
}