using neptune_hidapi.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWICD.Config;
using SWICD.HVDK;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace SWICD.Services
{
    internal class KeyboardMouseInputMapper
    {
        private HIDController _mouseHidController = new HIDController();
        private HIDController _keyboardHidController = new HIDController();
        private KeyboardUtils _keyboardUtils = new KeyboardUtils();
        private NeptuneControllerInputState _lastState = null;
        private DateTime _lastPing = DateTime.UtcNow.AddMinutes(-1);

        private bool LastLeftMouseButtonState = false;
        private bool LastRightMouseButtonState = false;

        public KeyboardMouseInputMapper()
        {
            _mouseHidController.OnLog += new EventHandler<LogArgs>(Log);
            _mouseHidController.VendorID = (ushort)DriversConst.TTC_VENDORID;                //the Tetherscript vendorid
            _mouseHidController.ProductID = (ushort)DriversConst.TTC_PRODUCTID_MOUSEREL;     //the Tetherscript Virtual Mouse Relative Driver productid
            _mouseHidController.Connect();
            _keyboardHidController.OnLog += new EventHandler<LogArgs>(Log);
            _keyboardHidController.VendorID = (ushort)DriversConst.TTC_VENDORID;                //the Tetherscript vendorid
            _keyboardHidController.ProductID = (ushort)DriversConst.TTC_PRODUCTID_KEYBOARD;     //the Tetherscript Virtual Keyboard Driver productid
            _keyboardHidController.Connect();
        }

        internal void ExecuteKeyboardAction(string action)
        {
            string[] keys = action.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            byte modifiers = 0;
            List<byte> pressedKeys = new List<byte>();

            foreach (var key in keys)
            {
                if (key == "NONE")
                    continue;

                if (key.StartsWith("["))
                {
                    int bit = _keyboardUtils.GetModifierKeyCode(key);
                    byte m1;
                    switch (bit)
                    {
                        case 0: m1 = 1; break;
                        case 1: m1 = 2; break;
                        case 2: m1 = 4; break;
                        case 3: m1 = 8; break;
                        case 4: m1 = 16; break;
                        case 5: m1 = 32; break;
                        case 6: m1 = 64; break;
                        case 7: m1 = 128; break;
                        default: m1 = 0; break;
                    }
                    modifiers = (byte)(modifiers | m1);
                }
                else
                {
                    if (pressedKeys.Count() < 6)
                        pressedKeys.Add(_keyboardUtils.GetKeyKeyCode(key));
                }
                SendKeyboardData(modifiers, pressedKeys);
                Thread.Sleep(50);
            }

            foreach (var key in keys.Reverse())
            {
                if (key == "NONE")
                    continue;

                if (key.StartsWith("["))
                {
                    int bit = _keyboardUtils.GetModifierKeyCode(key);
                    byte m1;
                    switch (bit)
                    {
                        case 0: m1 = 1; break;
                        case 1: m1 = 2; break;
                        case 2: m1 = 4; break;
                        case 3: m1 = 8; break;
                        case 4: m1 = 16; break;
                        case 5: m1 = 32; break;
                        case 6: m1 = 64; break;
                        case 7: m1 = 128; break;
                        default: m1 = 0; break;
                    }
                    modifiers = (byte)(modifiers & ~m1);
                }
                else
                {
                    if (pressedKeys.Count() < 6)
                        pressedKeys.Remove(_keyboardUtils.GetKeyKeyCode(key));
                }

                SendKeyboardData(modifiers, pressedKeys);
                Thread.Sleep(50);
            }
        }

        private void SendMouseData(bool leftButton, bool rightButton, sbyte x = 0, sbyte y = 0, bool ignoreMove = true)
        {
            SetFeatureMouseRel MouseRelData = new SetFeatureMouseRel();
            MouseRelData.ReportID = 1;
            MouseRelData.CommandCode = 2;
            byte btns = 0;
            if (leftButton) { btns = 1; };
            if (rightButton) { btns = (byte)(btns | (1 << 1)); }
            if (leftButton) { btns = (byte)(btns | (1 << 2)); }
            MouseRelData.Buttons = btns;  //button states are represented by the 3 least significant bits
            if (!ignoreMove)
            {
                MouseRelData.X = x;
                MouseRelData.Y = y;
            }
            //convert struct to buffer
            byte[] buf = getBytesSFJMouse(MouseRelData, Marshal.SizeOf(MouseRelData));
            //send filled buffer to driver
            _mouseHidController.SendData(buf, (uint)Marshal.SizeOf(MouseRelData));
        }

        private void SendKeyboardData(Byte Modifier, Byte Padding, Byte Key0, Byte Key1, Byte Key2, Byte Key3, Byte Key4, Byte Key5)
        {
            SetFeatureKeyboard KeyboardData = new SetFeatureKeyboard();
            KeyboardData.ReportID = 1;
            KeyboardData.CommandCode = 2;
            KeyboardData.Timeout = 5000 / 50;
            KeyboardData.Modifier = Modifier;
            //padding should always be zero.
            KeyboardData.Padding = Padding;
            KeyboardData.Key0 = Key0;
            KeyboardData.Key1 = Key1;
            KeyboardData.Key2 = Key2;
            KeyboardData.Key3 = Key3;
            KeyboardData.Key4 = Key4;
            KeyboardData.Key5 = Key5;
            //convert struct to buffer
            byte[] buf = getBytesSFJKeyboard(KeyboardData, Marshal.SizeOf(KeyboardData));
            //send filled buffer to driver
            bool success = _keyboardHidController.SendData(buf, (uint)Marshal.SizeOf(KeyboardData));
        }

        private void SendKeyboardData(Byte Modifier, List<byte> pressedKeys)
        {
            byte k1 = 0, k2 = 0, k3 = 0, k4 = 0, k5 = 0, k6 = 0;
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                switch (i)
                {
                    case 0: k1 = pressedKeys[i]; break;
                    case 1: k2 = pressedKeys[i]; break;
                    case 2: k3 = pressedKeys[i]; break;
                    case 3: k4 = pressedKeys[i]; break;
                    case 4: k5 = pressedKeys[i]; break;
                    case 5: k6 = pressedKeys[i]; break;
                }
            }

            SendKeyboardData(Modifier, 0, k1, k2, k3, k4, k5, k6);
        }

        private void KeyboardPing()
        {
            SetFeatureKeyboard KeyboardData = new SetFeatureKeyboard();
            KeyboardData.ReportID = 1;
            KeyboardData.CommandCode = 3;
            //the timeout is how long the driver will wait (milliseconds) without receiving a ping before resetting itself
            //we'll be pinging every 200ms, and loss of ping will cause driver reset in FTimeout.  
            //No more stuck keys requiring reboot to clear.
            //the following fields are not used by the driver for a ping, but we'll zero them anyways
            KeyboardData.Timeout = 5000 / 5; //50 because we count in blocks of 50 in the driver;
            KeyboardData.Modifier = 0;
            KeyboardData.Padding = 0;
            KeyboardData.Key0 = 0;
            KeyboardData.Key1 = 0;
            KeyboardData.Key2 = 0;
            KeyboardData.Key3 = 0;
            KeyboardData.Key4 = 0;
            KeyboardData.Key5 = 0;
            //convert struct to buffer
            byte[] buf = getBytesSFJKeyboard(KeyboardData, Marshal.SizeOf(KeyboardData));
            //send filled buffer to driver
            bool success = _keyboardHidController.SendData(buf, (uint)Marshal.SizeOf(KeyboardData));
        }

        public void Log(object s, LogArgs e)
        {
            LoggingService.LogInformation(e.Msg);
        }

        //for converting a struct to byte array
        private byte[] getBytesSFJKeyboard(SetFeatureKeyboard sfj, int size)
        {
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(sfj, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
        //for converting a struct to byte array
        private byte[] getBytesSFJMouse(SetFeatureMouseRel sfj, int size)
        {
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(sfj, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public void MapMouseInput(ControllerConfig config, NeptuneControllerInputState input)
        {
            bool leftMouse = false;
            bool rightMouse = false;

            foreach (var btn in input.ButtonState.Buttons)
                if (config.MouseMapping[(HardwareButton)btn] != VirtualMouseKey.NONE)
                {
                    var key = config.MouseMapping[(HardwareButton)btn];

                    if (key == VirtualMouseKey.LBUTTON)
                    {
                        leftMouse = input.ButtonState[btn];
                    }
                    else
                    {
                        rightMouse = input.ButtonState[btn];
                    }
                }
            if (rightMouse != LastRightMouseButtonState ||
                leftMouse != LastLeftMouseButtonState)
                SendMouseData(leftMouse, rightMouse, 0, 0, true);

            LastLeftMouseButtonState = leftMouse;
            LastRightMouseButtonState = rightMouse;
        }

        public void MapKeyboardInput(ControllerConfig config, NeptuneControllerInputState input)
        {
            if ((DateTime.UtcNow - _lastPing).TotalMilliseconds > 200)
            {
                KeyboardPing();
                _lastPing = DateTime.UtcNow;
            }

            if (_lastState != null)
            {
                if (!_lastState.ButtonState.Equals(input.ButtonState))
                {
                    _lastState = input;
                    List<byte> pressedKeys = new List<byte>();
                    byte modifiers = 0;

                    foreach (var btn in input.ButtonState.Buttons)
                        if (config.KeyboardMapping[(HardwareButton)btn] != null &&
                            config.KeyboardMapping[(HardwareButton)btn] != String.Empty &&
                            input.ButtonState[btn])
                        {
                            var key = config.KeyboardMapping[(HardwareButton)btn];

                            if (key == "NONE")
                                continue;

                            if (key.StartsWith("["))
                            {
                                int bit = _keyboardUtils.GetModifierKeyCode(key);
                                byte m1;
                                switch (bit)
                                {
                                    case 0: m1 = 1; break;
                                    case 1: m1 = 2; break;
                                    case 2: m1 = 4; break;
                                    case 3: m1 = 8; break;
                                    case 4: m1 = 16; break;
                                    case 5: m1 = 32; break;
                                    case 6: m1 = 64; break;
                                    case 7: m1 = 128; break;
                                    default: m1 = 0; break;
                                }
                                modifiers = (byte)(modifiers | m1);
                            }
                            else
                            {
                                if (pressedKeys.Count() < 6)
                                    pressedKeys.Add(_keyboardUtils.GetKeyKeyCode(key));
                            }
                        }

                    SendKeyboardData(modifiers, pressedKeys);
                }
            }

            _lastState = input;
        }
    }
}

