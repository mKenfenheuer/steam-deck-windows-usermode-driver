using System;
using System.Collections.Generic;
using System.Linq;

namespace SWICD_Lib.Config
{
    public class ButtonMapping : ICloneable
    {
        private Dictionary<HardwareButton, EmulatedButton> _mappings = new Dictionary<HardwareButton, EmulatedButton>()
        {
            { HardwareButton.BtnX, EmulatedButton.BtnX },
            { HardwareButton.BtnY, EmulatedButton.BtnY },
            { HardwareButton.BtnA, EmulatedButton.BtnA },
            { HardwareButton.BtnB, EmulatedButton.BtnB },
            { HardwareButton.BtnMenu, EmulatedButton.BtnStart },
            { HardwareButton.BtnOptions, EmulatedButton.BtnBack },
            { HardwareButton.BtnSteam, EmulatedButton.BtnStart },
            { HardwareButton.BtnQuickAccess, EmulatedButton.None },
            { HardwareButton.BtnDpadUp, EmulatedButton.BtnDpadUp },
            { HardwareButton.BtnDpadLeft, EmulatedButton.BtnDpadUp },
            { HardwareButton.BtnDpadRight, EmulatedButton.BtnDpadRight },
            { HardwareButton.BtnDpadDown, EmulatedButton.BtnDpadDown },
            { HardwareButton.BtnL1, EmulatedButton.BtnLB },
            { HardwareButton.BtnR1, EmulatedButton.BtnRB },
            { HardwareButton.BtnL2, EmulatedButton.None },
            { HardwareButton.BtnR2, EmulatedButton.None },
            { HardwareButton.BtnL4, EmulatedButton.BtnA },
            { HardwareButton.BtnR4, EmulatedButton.BtnB },
            { HardwareButton.BtnL5, EmulatedButton.BtnX },
            { HardwareButton.BtnR5, EmulatedButton.BtnY },
            { HardwareButton.BtnRPadPress, EmulatedButton.BtnRS },
            { HardwareButton.BtnLPadPress, EmulatedButton.BtnLS },
            { HardwareButton.BtnRPadTouch, EmulatedButton.None },
            { HardwareButton.BtnLPadTouch, EmulatedButton.None },
            { HardwareButton.BtnRStickPress, EmulatedButton.BtnRS },
            { HardwareButton.BtnLStickPress, EmulatedButton.BtnLS },
            { HardwareButton.BtnRStickTouch, EmulatedButton.None },
            { HardwareButton.BtnLStickTouch, EmulatedButton.None },
        };

        public ButtonMapping(Dictionary<HardwareButton, EmulatedButton> mappings)
        {
            _mappings = mappings;
        }

        public ButtonMapping()
        {
        }

        public EmulatedButton this[HardwareButton button]
        {
            get
            {
                if (_mappings.ContainsKey(button))
                    return _mappings[button];
                return EmulatedButton.None;
            }
            set
            {
                _mappings[button] = value;
            }
        }
        public object Clone()
        {
            var clone = new ButtonMapping(_mappings.ToDictionary(entry => entry.Key,
                                               entry => entry.Value));
            return clone;
        }

        internal string ToString(string executable = null)
        {
            string config = $"[buttons]\r\n";
            if (executable != null)
            {
                config = $"[buttons,{executable}]\r\n";
            }

            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                {
                    config += $"{GetHardwareButtonName(button)}={GetEmulatedButtonName(this[button])}\r\n";
                }

            return config;
        }
        internal string GetEmulatedButtonName(EmulatedButton value) => Enum.GetName(typeof(EmulatedButton), value);
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);

    }
}