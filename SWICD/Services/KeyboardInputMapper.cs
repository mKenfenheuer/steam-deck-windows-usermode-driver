using neptune_hidapi.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWICD.Config;
using WindowsInput;

namespace SWICD.Services
{
    internal class KeyboardInputMapper
    {
        private static InputSimulator _simulator = new InputSimulator();
        private static NeptuneControllerInputState _lastState = null;
        public static void MapInput(ControllerConfig config, NeptuneControllerInputState input)
        {
            if (_lastState != null)
            {
                foreach (var btn in _lastState.ButtonState.Buttons)
                    if (config.KeyboardMapping[(HardwareButton)btn] != VirtualKeyboardKey.NONE)
                    {
                        if (_lastState.ButtonState[btn] != input.ButtonState[btn])
                        {
                            var key = config.KeyboardMapping[(HardwareButton)btn];
                            if (input.ButtonState[btn])
                            {
                                _simulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)key);
                            } else
                            {
                                _simulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)key);
                            }
                        }
                    }
            }

            _lastState = input;
        }
    }
}
