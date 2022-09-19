using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SWICD.Config
{
    [Serializable]
    public class KeyboardMapping : ICloneable, ISerializable
    {
        private Dictionary<HardwareButton, string> _mappings = new Dictionary<HardwareButton, string>()
        {
            { HardwareButton.BtnX, "NONE" },
            { HardwareButton.BtnY, "NONE" },
            { HardwareButton.BtnA, "NONE" },
            { HardwareButton.BtnB, "NONE" },
            { HardwareButton.BtnMenu, "NONE" },
            { HardwareButton.BtnOptions, "NONE" },
            { HardwareButton.BtnSteam, "NONE" },
            { HardwareButton.BtnQuickAccess, "NONE" },
            { HardwareButton.BtnDpadUp, "NONE" },
            { HardwareButton.BtnDpadLeft, "NONE" },
            { HardwareButton.BtnDpadRight, "NONE" },
            { HardwareButton.BtnDpadDown, "NONE" },
            { HardwareButton.BtnL1, "NONE" },
            { HardwareButton.BtnR1, "NONE" },
            { HardwareButton.BtnL2, "NONE" },
            { HardwareButton.BtnR2, "NONE" },
            { HardwareButton.BtnL4, "NONE" },
            { HardwareButton.BtnR4, "NONE" },
            { HardwareButton.BtnL5, "NONE" },
            { HardwareButton.BtnR5, "NONE" },
            { HardwareButton.BtnRPadPress, "NONE" },
            { HardwareButton.BtnLPadPress, "NONE" },
            { HardwareButton.BtnRPadTouch, "NONE" },
            { HardwareButton.BtnLPadTouch, "NONE" },
            { HardwareButton.BtnRStickPress, "NONE" },
            { HardwareButton.BtnLStickPress, "NONE" },
            { HardwareButton.BtnRStickTouch, "NONE" },
            { HardwareButton.BtnLStickTouch, "NONE" },
        };

        public KeyboardMapping(Dictionary<HardwareButton, string> mappings)
        {
            _mappings = mappings;
        }

        public KeyboardMapping()
        {
        }
        public KeyboardMapping(SerializationInfo info, StreamingContext context)
        {
            var btns = _mappings.Keys.ToArray();
            foreach (var btn in btns)
            {
                _mappings[btn] = info.GetString(btn.ToString());
            }
        }

        public string this[HardwareButton button]
        {
            get
            {
                if (_mappings.ContainsKey(button))
                    return _mappings[button];
                return "";
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

        public override bool Equals(object obj)
        {
            return obj is KeyboardMapping mapping &&
                   _mappings.EqualsWithValues(mapping._mappings);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var btn in _mappings.Keys)
            {
                info.AddValue(btn.ToString(), _mappings[btn].ToString());
            }
        }
    }
}