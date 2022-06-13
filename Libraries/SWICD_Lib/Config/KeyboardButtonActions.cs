using System;
using System.Collections.Generic;
using System.Linq;

namespace SWICD_Lib.Config
{
    public class KeyboardButtonActions : ICloneable
    {
        public KeyboardButtonActions()
        {
            _actions[new HardwareButton[] { HardwareButton.BtnSteam }] = "{WIN}{G}";
        }

        private Dictionary<HardwareButton[], string> _actions { get; set; } = new Dictionary<HardwareButton[], string>();

        public string this[HardwareButton[] buttons]
        {
            get => _actions[buttons];
            set => _actions[buttons] = value;
        }

        public override string ToString()
        {
            return $"[keyboard-actions]\r\n{String.Join("\r\n", _actions.Select(p => RenderConfigLine(p)))}\r\n";
        }

        private string RenderConfigLine(KeyValuePair<HardwareButton[], string> pair)
        {
            return $"{String.Join("+", pair.Key)}={pair.Value}";
        }
        internal string GetHardwareButtonName(HardwareButton value) => Enum.GetName(typeof(HardwareButton), value);

        public override bool Equals(object obj)
        {
            return obj is KeyboardButtonActions actions &&
                   _actions.EqualsWithValues(actions._actions);
        }

        public object Clone()
        {
            var obj = new KeyboardButtonActions();
            obj._actions = _actions.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
            return obj;
        }
    }
}