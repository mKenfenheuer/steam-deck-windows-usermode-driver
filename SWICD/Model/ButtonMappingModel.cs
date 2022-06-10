using SWICD_Lib.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWICD.Model
{
    internal class ButtonMappingModel
    {
        public ObservableCollection<EnumComboBoxItem<EmulatedButton>> ButtonItems { get; set; } = new ObservableCollection<EnumComboBoxItem<EmulatedButton>>(Enum.GetValues(typeof(EmulatedButton)).Cast<EmulatedButton>().Select(e => new EnumComboBoxItem<EmulatedButton>()
        {
            Value = e,
            Display = GetDisplayText(e),
        }));

        private static string GetDisplayText(EmulatedButton e)
        {
            string text = e.ToString().Replace("Btn", "");
            if (text.Length == 1)
            {
                text = text.ToLower();
            }

            if (e == EmulatedButton.BtnStart)
            {
                return "m";
            }

            if (e == EmulatedButton.BtnBack)
            {
                return "v";
            }

            if (e == EmulatedButton.BtnGuide)
            {
                return "g";
            }

            if (e == EmulatedButton.BtnDpadDown)
            {
                return "X";
            }

            if (e == EmulatedButton.BtnDpadLeft)
            {
                return "A";
            }

            if (e == EmulatedButton.BtnDpadRight)
            {
                return "D";
            }

            if (e == EmulatedButton.BtnDpadUp)
            {
                return "W";
            }

            if (e == EmulatedButton.BtnLB)
            {
                return "[";
            }

            if (e == EmulatedButton.BtnRB)
            {
                return "]";
            }

            if (e == EmulatedButton.BtnLS)
            {
                return "<";
            }

            if (e == EmulatedButton.BtnRS)
            {
                return ">";
            }

            return text;
        }

        public string ButtonText => Regex.Replace(HardwareButton.ToString().Replace("Btn", ""), "([^A-Z])([A-Z])", "$1 $2");
        public HardwareButton HardwareButton { get; set; }
        public EnumComboBoxItem<EmulatedButton> SelectedEmulatedButton { get; set; }
        public EmulatedButton EmulatedButton
        {
            get => SelectedEmulatedButton.Value; set => SelectedEmulatedButton = new EnumComboBoxItem<EmulatedButton>()
            {
                Value = value,
                Display = GetDisplayText(value),
            };
        }
    }
}
