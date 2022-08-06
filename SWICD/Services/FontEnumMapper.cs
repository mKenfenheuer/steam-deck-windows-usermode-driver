using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Services
{
    internal class FontEnumMapper
    {
        public static string MapEmulatedButtonToFont(EmulatedButton emulated)
        {
            switch (emulated)
            {
                case EmulatedButton.BtnX:
                    return "\u21D0";
                case EmulatedButton.BtnY:
                    return "\u21D1";
                case EmulatedButton.BtnA:
                    return "\u21D3";
                case EmulatedButton.BtnB:
                    return "\u21D2";
                case EmulatedButton.BtnRB:
                    return "\u2199";
                case EmulatedButton.BtnLB:
                    return "\u2198";
                case EmulatedButton.BtnBack:
                    return "\u21FA";
                case EmulatedButton.BtnStart:
                    return "\u21FB";
                case EmulatedButton.BtnLS:
                    return "\u21DE";
                case EmulatedButton.BtnRS:
                    return "\u21DF";
                case EmulatedButton.BtnGuide:
                    return "\uE001";
                case EmulatedButton.BtnDpadDown:
                    return "\u21A1";
                case EmulatedButton.BtnDpadLeft:
                    return "\u219E";
                case EmulatedButton.BtnDpadRight:
                    return "\u21A0";
                case EmulatedButton.BtnDpadUp:
                    return "\u219F";
                case EmulatedButton.None:
                    return "NONE";
            }
            return "\u2753";
        }

        internal static string MapEmulatedMouseButtonToFont(VirtualMouseKey e)
        {
            switch (e)
            {
                case VirtualMouseKey.NONE:
                    return "NONE";
                case VirtualMouseKey.LBUTTON:
                    return "\u27F5";
                case VirtualMouseKey.RBUTTON:
                    return "\u27F6";
                case VirtualMouseKey.MBUTTON:
                    return "\u27F7";
            }
            return "\u2753";
        }

        internal static string MapEmulatedKeyboardKeyToFont(VirtualKeyboardKey e)
        {
            return e.ToString();
        }

        public static string MapEmulatedAxisToFont(EmulatedAxis emulated)
        {
            switch (emulated)
            {
                case EmulatedAxis.LeftStickY:
                    return "\u21C5";
                case EmulatedAxis.LeftStickX:
                    return "\u21C4";
                case EmulatedAxis.RightStickX:
                    return "\u21C6";
                case EmulatedAxis.RightStickY:
                    return "\u21C3";
                case EmulatedAxis.LT:
                    return "\u2196";
                case EmulatedAxis.RT:
                    return "\u2197";
                case EmulatedAxis.None:
                    return "NONE";
            }
            return "\u2753";
        }

        public static string MapHardwareAxisToFont(HardwareAxis axis)
        {
            switch (axis)
            {
                case HardwareAxis.LeftStickY:
                    return "\u21C5";
                case HardwareAxis.LeftStickX:
                    return "\u21C4";
                case HardwareAxis.RightStickX:
                    return "\u21C6";
                case HardwareAxis.RightStickY:
                    return "\u21C3";
                case HardwareAxis.L2:
                    return "\u21B2";
                case HardwareAxis.R2:
                    return "\u21B3";
                case HardwareAxis.LeftPadX:
                    return "\u21EB";
                case HardwareAxis.LeftPadY:
                    return "\u21EA";
                case HardwareAxis.LeftPadPressure:
                    return "\u21DC PRESS";
                case HardwareAxis.RightPadPressure:
                    return "\u21DD PRESS";
                case HardwareAxis.RightPadX:
                    return "\u21ED";
                case HardwareAxis.RightPadY:
                    return "\u21EC";
                case HardwareAxis.None:
                    return "NONE";
            }

            return axis.ToString();
        }

        public static string MapHardwareButtonToFont(HardwareButton btn)
        {
            switch (btn)
            {
                case HardwareButton.BtnX:
                    return "\u21D0";
                case HardwareButton.BtnY:
                    return "\u21D1";
                case HardwareButton.BtnA:
                    return "\u21D3";
                case HardwareButton.BtnB:
                    return "\u21D2";
                case HardwareButton.BtnR1:
                    return "\u2199";
                case HardwareButton.BtnL1:
                    return "\u2198";
                case HardwareButton.BtnL2:
                    return "\u21B2";
                case HardwareButton.BtnR2:
                    return "\u21B3";
                case HardwareButton.BtnL4:
                    return "\u2201";
                case HardwareButton.BtnR4:
                    return "\u2202";
                case HardwareButton.BtnL5:
                    return "\u2203";
                case HardwareButton.BtnR5:
                    return "\u2204";
                case HardwareButton.BtnDpadDown:
                    return "\u21A1";
                case HardwareButton.BtnDpadLeft:
                    return "\u219E";
                case HardwareButton.BtnDpadRight:
                    return "\u21A0";
                case HardwareButton.BtnDpadUp:
                    return "\u219F";
                case HardwareButton.BtnOptions:
                    return "\u21FB";
                case HardwareButton.BtnMenu:
                    return "\u21FA";
                case HardwareButton.BtnQuickAccess:
                    return "\u00b7\u00b7\u00b7";
                case HardwareButton.BtnLStickPress:
                    return "\u21DE PRESS";
                case HardwareButton.BtnRStickPress:
                    return "\u21DF PRESS";
                case HardwareButton.BtnLStickTouch:
                    return "\u21DE TOUCH";
                case HardwareButton.BtnRStickTouch:
                    return "\u21DF TOUCH";
                case HardwareButton.BtnLPadPress:
                    return "\u21DC PRESS";
                case HardwareButton.BtnRPadPress:
                    return "\u21DD PRESS";
                case HardwareButton.BtnLPadTouch:
                    return "\u21DC TOUCH";
                case HardwareButton.BtnRPadTouch:
                    return "\u21DD TOUCH";
                case HardwareButton.BtnSteam:
                    return "STEAM";
                case HardwareButton.None:
                    return "NONE";

            }

            return btn.ToString();
        }
    }
}
