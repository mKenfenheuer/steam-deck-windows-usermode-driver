using System;
using System.Collections.Generic;
using System.Linq;

namespace SWICD.Config
{
    public class KeyboardMapping : ICloneable
    {
        private Dictionary<HardwareButton, VirtualKeyboardKey> _mappings = new Dictionary<HardwareButton, VirtualKeyboardKey>()
        {
            { HardwareButton.BtnX, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnY, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnA, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnB, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnMenu, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnOptions, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnSteam, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnQuickAccess, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnDpadUp, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnDpadLeft, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnDpadRight, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnDpadDown, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnL1, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnR1, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnL2, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnR2, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnL4, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnR4, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnL5, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnR5, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnRPadPress, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnLPadPress, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnRPadTouch, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnLPadTouch, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnRStickPress, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnLStickPress, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnRStickTouch, VirtualKeyboardKey.NONE },
            { HardwareButton.BtnLStickTouch, VirtualKeyboardKey.NONE },
        };

        public KeyboardMapping(Dictionary<HardwareButton, VirtualKeyboardKey> mappings)
        {
            _mappings = mappings;
        }

        public KeyboardMapping()
        {
        }

        public VirtualKeyboardKey this[HardwareButton button]
        {
            get
            {
                if (_mappings.ContainsKey(button))
                    return _mappings[button];
                return VirtualKeyboardKey.NONE;
            }
            set
            {
                _mappings[button] = value;
            }
        }
        public object Clone()
        {
            var clone = new KeyboardMapping(_mappings.ToDictionary(entry => entry.Key,
                                               entry => entry.Value));
            return clone;
        }

        internal string ToString(string executable = null)
        {
            string config = $"[keyboardkeys]\r\n";
            if (executable != null)
            {
                config = $"[keyboardkeys,{executable}]\r\n";
            }

            foreach (HardwareButton button in Enum.GetValues(typeof(HardwareButton)))
                if (button != HardwareButton.None)
                {
                    config += $"{GetHardwareButtonName(button)}={GetEmulatedKeyName(this[button])}\r\n";
                }

            return config;
        }
        internal string GetEmulatedKeyName(VirtualKeyboardKey value) => Enum.GetName(typeof(VirtualKeyboardKey), value);
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);

        public override bool Equals(object obj)
        {
            return obj is KeyboardMapping mapping &&
                   _mappings.EqualsWithValues(mapping._mappings);
        }
    }
}