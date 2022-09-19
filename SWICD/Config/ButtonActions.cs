using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SWICD.Config
{
    [Serializable]
    public class ButtonActions : ICloneable, ISerializable
    {
        public ButtonActions()
        {
        }

        public ButtonActions(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                string[] buttons = entry.Name.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                HardwareButton[] btns = buttons.Select(b => (HardwareButton)Enum.Parse(typeof(HardwareButton), b)).ToArray();

                _actions[btns] = (ButtonAction)info.GetValue(entry.Name, typeof(ButtonAction));

            }
        }

        private Dictionary<HardwareButton[], ButtonAction> _actions { get; set; } = new Dictionary<HardwareButton[], ButtonAction>();

        public ButtonAction this[HardwareButton[] buttons]
        {
            get => _actions[buttons];
            set => _actions[buttons] = value;
        }

        public HardwareButton[][] GetActionButtons => _actions.Keys.ToArray();

        public override bool Equals(object obj)
        {
            return obj is ButtonActions actions &&
                   _actions.EqualsWithValues(actions._actions);
        }

        public object Clone()
        {
            var obj = new ButtonActions();
            obj._actions = _actions.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
            return obj;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (var btn in _actions.Keys)
            {
                try
                {
                    info.AddValue(String.Join("+", btn), _actions[btn], typeof(ButtonAction));
                }
                catch
                { }
            }
        }

        internal void Clear()
        {
            _actions.Clear();
        }
    }
}