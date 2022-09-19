using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SWICD.Config
{
    [Serializable]
    public class AxisMapping : ICloneable, ISerializable
    {
        private Dictionary<HardwareAxis, EmulatedAxisConfig> _mappings = new Dictionary<HardwareAxis, EmulatedAxisConfig>()
        {
            { HardwareAxis.LeftStickX, new EmulatedAxisConfig(EmulatedAxis.LeftStickX) },
            { HardwareAxis.LeftStickY, new EmulatedAxisConfig(EmulatedAxis.LeftStickY) },
            { HardwareAxis.RightStickX, new EmulatedAxisConfig(EmulatedAxis.RightStickX) },
            { HardwareAxis.RightStickY, new EmulatedAxisConfig(EmulatedAxis.RightStickY) },
            { HardwareAxis.LeftPadX, new EmulatedAxisConfig(EmulatedAxis.LeftStickX, HardwareButton.BtnLPadTouch) },
            { HardwareAxis.LeftPadY, new EmulatedAxisConfig(EmulatedAxis.LeftStickY, HardwareButton.BtnLPadTouch) },
            { HardwareAxis.RightPadX, new EmulatedAxisConfig(EmulatedAxis.RightStickX, HardwareButton.BtnRPadTouch) },
            { HardwareAxis.RightPadY, new EmulatedAxisConfig(EmulatedAxis.RightStickY, HardwareButton.BtnRPadTouch) },
            { HardwareAxis.RightPadPressure, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.LeftPadPressure, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.L2, new EmulatedAxisConfig(EmulatedAxis.LT) },
            { HardwareAxis.R2, new EmulatedAxisConfig(EmulatedAxis.RT) },
            { HardwareAxis.GyroAccelX, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.GyroAccelY, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.GyroAccelZ, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.GyroRoll, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.GyroPitch, new EmulatedAxisConfig(EmulatedAxis.None) },
            { HardwareAxis.GyroYaw, new EmulatedAxisConfig(EmulatedAxis.None) },
        };

        public AxisMapping(Dictionary<HardwareAxis, EmulatedAxisConfig> dictionary)
        {
            _mappings = dictionary;
        }

        public AxisMapping(SerializationInfo info, StreamingContext context)
        {
            var btns = _mappings.Keys.ToArray();
            foreach (var btn in btns)
            {
                _mappings[btn] = (EmulatedAxisConfig)info.GetValue(btn.ToString(), typeof(EmulatedAxisConfig));
            }
        }

        public AxisMapping()
        {
        }

        public EmulatedAxisConfig this[HardwareAxis axis]
        {
            get
            {
                if (_mappings.ContainsKey(axis))
                    return _mappings[axis];
                return new EmulatedAxisConfig(EmulatedAxis.None);
            }
            set
            {
                _mappings[axis] = value;
            }
        }

        public object Clone()
        {
            var clone = new AxisMapping(_mappings.ToDictionary(entry => entry.Key,
                                               entry => (EmulatedAxisConfig)entry.Value.Clone()));
            return clone;
        }

        public override bool Equals(object obj)
        {
            return obj is AxisMapping mapping &&
                   _mappings.EqualsWithValues(mapping._mappings);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var btn in _mappings.Keys)
            {
                info.AddValue(btn.ToString(), _mappings[btn]);
            }
        }
    }
}