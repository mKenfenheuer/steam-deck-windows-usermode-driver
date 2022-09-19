using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SWICD.HVDK
{
    class KeyboardUtils
    {

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        protected List<string> FKeys = new List<string>();
        protected List<string> FModifiers = new List<string>();

        public string[] GetAvailableKeys => FKeys.Where(k => !k.Contains("dummy")).ToArray();
        public string[] GetAvailableKeysWithModifiers => FModifiers.Concat(FKeys.ToArray()).Where(k => !k.Contains("dummy")).ToArray();

        public KeyboardUtils()
        {
            AddModifierKeyCodes();
            AddKeyKeyCodes();
        }

        public Boolean AppActivate(string Name, int PauseAfterActivation)
        {
            if (Name != "")
            {   //is it already the foreground window?
                IntPtr selectedWindow = GetForegroundWindow();
                StringBuilder WinCaptionEx = new StringBuilder(260);
                GetWindowText(selectedWindow, WinCaptionEx, WinCaptionEx.Capacity);
                if (WinCaptionEx.ToString() == Name)
                {
                    return true;
                }
                else
                {
                    IntPtr w = FindWindow(null, Name);
                    if (w != IntPtr.Zero)
                    {
                        SetForegroundWindow(w);
                        System.Threading.Thread.Sleep(PauseAfterActivation);
                        return true;
                    }
                }
            }
            return false;
        }

        //keys and modifers must be converted to keycodes
        //this is based specs in /common/hut1_12v2.pdf
        //special keycodes for modifiers like shift and control

        public Byte GetModifierKeyCode(string modifier)
        {
            int i = FModifiers.IndexOf(modifier);
            if (i == -1) { return 0; } else { return (byte)i; };
        }

        protected void AddModifierKeyCodes()
        {
            //the [ and ] and text is arbitrary. You can call the modifiers whatever you like, as long as the list order is preserved
            FModifiers.Add("[LCTRL]");
            FModifiers.Add("[LSHIFT]");
            FModifiers.Add("[LALT]");
            FModifiers.Add("[LWIN]");
            FModifiers.Add("[RCTRL]");
            FModifiers.Add("[RSHIFT]");
            FModifiers.Add("[RALT]");
            FModifiers.Add("[RWIN]");
        }

        public Byte GetKeyKeyCode(string key)
        {
            int i = FKeys.IndexOf(key);
            if (i == -1) { return 0; } else { return (byte)i; };
        }

        protected void AddKeyKeyCodes()
        {
            FKeys.Add("dummy1");
            FKeys.Add("dummy2");
            FKeys.Add("dummy3");
            FKeys.Add("dummy4");
            FKeys.Add("a");
            FKeys.Add("b");
            FKeys.Add("c");
            FKeys.Add("d");
            FKeys.Add("e");
            FKeys.Add("f");
            FKeys.Add("g");
            FKeys.Add("h");
            FKeys.Add("i");
            FKeys.Add("j");
            FKeys.Add("k");
            FKeys.Add("l");
            FKeys.Add("m");
            FKeys.Add("n");
            FKeys.Add("o");
            FKeys.Add("p");
            FKeys.Add("q");
            FKeys.Add("r");
            FKeys.Add("s");
            FKeys.Add("t");
            FKeys.Add("u");
            FKeys.Add("v");
            FKeys.Add("w");
            FKeys.Add("x");
            FKeys.Add("y");
            FKeys.Add("z");
            FKeys.Add("1");
            FKeys.Add("2");
            FKeys.Add("3");
            FKeys.Add("4");
            FKeys.Add("5");
            FKeys.Add("6");
            FKeys.Add("7");
            FKeys.Add("8");
            FKeys.Add("9");
            FKeys.Add("0");
            FKeys.Add("ENTER");
            FKeys.Add("ESCAPE");
            FKeys.Add("BACKSPACE");
            FKeys.Add("TAB");
            FKeys.Add("SPACEBAR");
            FKeys.Add("-");
            FKeys.Add("=");
            FKeys.Add("[");
            FKeys.Add("]");
            FKeys.Add("\\");
            FKeys.Add("");
            FKeys.Add(";");
            FKeys.Add("dummy5");
            FKeys.Add("`");
            FKeys.Add(",");
            FKeys.Add(".");
            FKeys.Add("/");
            FKeys.Add("CAPSLOCK");
            FKeys.Add("F1");
            FKeys.Add("F2");
            FKeys.Add("F3");
            FKeys.Add("F4");
            FKeys.Add("F5");
            FKeys.Add("F6");
            FKeys.Add("F7");
            FKeys.Add("F8");
            FKeys.Add("F9");
            FKeys.Add("F10");
            FKeys.Add("F11");
            FKeys.Add("F12");
            FKeys.Add("PRINTSCREEN");
            FKeys.Add("SCROLLLOCK");
            FKeys.Add("PAUSE");
            FKeys.Add("INSERT");
            FKeys.Add("HOME");
            FKeys.Add("PAGEUP");
            FKeys.Add("DELETE");
            FKeys.Add("END");
            FKeys.Add("PAGEDOWN");
            FKeys.Add("RIGHTARROW");
            FKeys.Add("LEFTARROW");
            FKeys.Add("DOWNARROW");
            FKeys.Add("UPARROW");
            FKeys.Add("NUMLOCK");
            FKeys.Add("K/");
            FKeys.Add("K*");
            FKeys.Add("K-");
            FKeys.Add("K+");
            FKeys.Add("KENTER");
            FKeys.Add("K1");
            FKeys.Add("K2");
            FKeys.Add("K3");
            FKeys.Add("K4");
            FKeys.Add("K5");
            FKeys.Add("K6");
            FKeys.Add("K7");
            FKeys.Add("K8");
            FKeys.Add("K9");
            FKeys.Add("K0");
            FKeys.Add("K.");
            FKeys.Add("F13");
            FKeys.Add("F14");
            FKeys.Add("F15");
            FKeys.Add("F16");
            FKeys.Add("F17");
            FKeys.Add("F18");
            FKeys.Add("F19");
            FKeys.Add("F20");
            FKeys.Add("F21");
            FKeys.Add("F22");
            FKeys.Add("F23");
            FKeys.Add("F24"); //115
        }
    }
}


