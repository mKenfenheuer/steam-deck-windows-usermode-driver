using neptune_hidapi.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWICD.Config;
using GregsStack.InputSimulatorStandard;

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
                            var key = (GregsStack.InputSimulatorStandard.Native.VirtualKeyCode)config.KeyboardMapping[(HardwareButton)btn];
                            if (key != GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.LBUTTON &&
                                key != GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.RBUTTON)
                            {
                                if (input.ButtonState[btn])
                                {
                                    _simulator.Keyboard.KeyDown(key);
                                }
                                else
                                {
                                    _simulator.Keyboard.KeyUp(key);
                                }
                            }
                            else
                            {
                                if (input.ButtonState[btn])
                                {
                                    if (key == GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.LBUTTON)
                                    {
                                        _simulator.Mouse.LeftButtonDown();
                                    }
                                    if (key == GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.RBUTTON)
                                    {
                                        _simulator.Mouse.RightButtonDown();
                                    }
                                }
                                else
                                {
                                    if (key == GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.LBUTTON)
                                    {
                                        _simulator.Mouse.LeftButtonUp();
                                    }
                                    if (key == GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.RBUTTON)
                                    {
                                        _simulator.Mouse.RightButtonUp();
                                    }
                                }
                            }
                        }
                    }
            }

            _lastState = input;
        }
    }
}
