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
    internal class MouseInputMapper
    {
        private static InputSimulator _simulator = new InputSimulator();
        private static NeptuneControllerInputState _lastState = null;
        public static void MapInput(ControllerConfig config, NeptuneControllerInputState input)
        {
            if (_lastState != null)
            {
                foreach (var btn in _lastState.ButtonState.Buttons)
                    if (config.MouseMapping[(HardwareButton)btn] != VirtualMouseKey.NONE)
                    {
                        if (_lastState.ButtonState[btn] != input.ButtonState[btn])
                        {
                            var key = config.MouseMapping[(HardwareButton)btn];

                            if (input.ButtonState[btn])
                            {
                                if (key == VirtualMouseKey.LBUTTON)
                                {
                                    _simulator.Mouse.LeftButtonDown();
                                }
                                if (key == VirtualMouseKey.RBUTTON)
                                {
                                    _simulator.Mouse.RightButtonDown();
                                }
                            }
                            else
                            {
                                if (key == VirtualMouseKey.LBUTTON)
                                {
                                    _simulator.Mouse.LeftButtonUp();
                                }
                                if (key == VirtualMouseKey.RBUTTON)
                                {
                                    _simulator.Mouse.RightButtonUp();
                                }
                            }
                        }
                    }
            }

            _lastState = input;
        }
    }
}
