using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SWICD.Config
{
    [Serializable]
    public class ButtonMapping : ICloneable, ISerializable
    {
        private Dictionary<HardwareButton, EmulatedButton> _mappings = new Dictionary<HardwareButton, EmulatedButton>()
        {
            { HardwareButton.BtnX, EmulatedButton.BtnX },
            { HardwareButton.BtnY, EmulatedButton.BtnY },
            { HardwareButton.BtnA, EmulatedButton.BtnA },
            { HardwareButton.BtnB, EmulatedButton.BtnB },
            { HardwareButton.BtnMenu, EmulatedButton.BtnBack },
            { HardwareButton.BtnOptions, EmulatedButton.BtnStart },
            { HardwareButton.BtnSteam, EmulatedButton.BtnGuide },
            { HardwareButton.BtnQuickAccess, EmulatedButton.BtnGuide },
            { HardwareButton.BtnDpadUp, EmulatedButton.BtnDpadUp },
            { HardwareButton.BtnDpadLeft, EmulatedButton.BtnDpadLeft },
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

        public ButtonMapping(SerializationInfo info, StreamingContext context)
        {
            var btns = _mappings.Keys.ToArray();
            foreach (var btn in btns)
            {
                EmulatedButton button = EmulatedButton.None;
                Enum.TryParse(info.GetString(btn.ToString()), out button);
                _mappings[btn] = button;
            }
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

        public override bool Equals(object obj)
        {
            return obj is ButtonMapping mapping &&
                   _mappings.EqualsWithValues(mapping._mappings);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach(var btn in _mappings.Keys)
            {
                info.AddValue(btn.ToString(), _mappings[btn].ToString());
            }
        }
    }
}