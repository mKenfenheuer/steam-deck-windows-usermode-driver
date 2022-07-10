using neptune_hidapi.net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.ViGEm.Client.Targets;
using SWICD.Config;
using Nefarius.ViGEm.Client.Targets.Xbox360;

namespace SWICD.Services
{
    internal class InputMapper
    {
        private static NeptuneControllerInputState _lastState = null;
        public static void MapInput(ControllerConfig config, NeptuneControllerInputState input, IXbox360Controller controller)
        {
            foreach (var axis in input.AxesState.Axes)
            {
                EmulatedAxisConfig axisConf = config.AxisMapping[(HardwareAxis)axis];
                if (axisConf.EmulatedAxis != EmulatedAxis.None)
                    if (axisConf.ActivationButton == HardwareButton.None ||
                        input.ButtonState[(NeptuneControllerButton)axisConf.ActivationButton])
                    {
                        var xboxAxis = GetXbox360Axis(axisConf.EmulatedAxis);
                        var xboxSlider = GetXbox360Slider(axisConf.EmulatedAxis);
                        if (xboxAxis != null)
                            controller.SetAxisValue(xboxAxis, axisConf.Inverted ? (Int16)(Int16.MaxValue - input.AxesState[axis]) : input.AxesState[axis]);
                        if (xboxSlider != null)
                            controller.SetSliderValue(xboxSlider, axisConf.Inverted ? (byte)(byte.MaxValue - (input.AxesState[axis] / (double)Int16.MaxValue * byte.MaxValue)) : (byte)(input.AxesState[axis] / (double)Int16.MaxValue * byte.MaxValue));
                    }
            }

            foreach (var button in input.ButtonState.Buttons)
            {
                EmulatedButton emulatedButton = config.ButtonMapping[(HardwareButton)button];
                if (emulatedButton != EmulatedButton.None)
                {
                    var xboxButton = GetXbox360Button(emulatedButton);
                    if (xboxButton != null && input.ButtonState[button])
                        controller.SetButtonState(xboxButton, input.ButtonState[button]);
                }
            }

            if (_lastState != null)
            {
                foreach (var btn in _lastState.ButtonState.Buttons)
                {
                    if (_lastState.ButtonState[btn] != input.ButtonState[btn])
                    {
                        LoggingService.LogDebug($"Button {btn} changed to: {input.ButtonState[btn]}");
                    }
                }
            }

            _lastState = input;
        }

        private static Xbox360Button GetXbox360Button(EmulatedButton emulatedButton)
        {
            switch (emulatedButton)
            {
                case EmulatedButton.None:
                    return null;
                case EmulatedButton.BtnA:
                    return Xbox360Button.A;
                case EmulatedButton.BtnB:
                    return Xbox360Button.B;
                case EmulatedButton.BtnX:
                    return Xbox360Button.X;
                case EmulatedButton.BtnY:
                    return Xbox360Button.Y;
                case EmulatedButton.BtnBack:
                    return Xbox360Button.Back;
                case EmulatedButton.BtnStart:
                    return Xbox360Button.Start;
                case EmulatedButton.BtnGuide:
                    return Xbox360Button.Guide;
                case EmulatedButton.BtnRB:
                    return Xbox360Button.RightShoulder;
                case EmulatedButton.BtnLB:
                    return Xbox360Button.LeftShoulder;
                case EmulatedButton.BtnRS:
                    return Xbox360Button.RightThumb;
                case EmulatedButton.BtnLS:
                    return Xbox360Button.LeftThumb;
                case EmulatedButton.BtnDpadUp:
                    return Xbox360Button.Up;
                case EmulatedButton.BtnDpadDown:
                    return Xbox360Button.Down;
                case EmulatedButton.BtnDpadLeft:
                    return Xbox360Button.Left;
                case EmulatedButton.BtnDpadRight:
                    return Xbox360Button.Right;
                default:
                    return null;
            }
        }

        private static Xbox360Slider GetXbox360Slider(EmulatedAxis emulatedAxis)
        {
            switch (emulatedAxis)
            {
                case EmulatedAxis.LT:
                    return Xbox360Slider.LeftTrigger;
                case EmulatedAxis.RT:
                    return Xbox360Slider.RightTrigger;
                default:
                    return null;
            }
        }

        private static Xbox360Axis GetXbox360Axis(EmulatedAxis emulatedAxis)
        {
            switch (emulatedAxis)
            {
                case EmulatedAxis.LeftStickX:
                    return Xbox360Axis.LeftThumbX;
                case EmulatedAxis.LeftStickY:
                    return Xbox360Axis.LeftThumbY;
                case EmulatedAxis.RightStickX:
                    return Xbox360Axis.RightThumbX;
                case EmulatedAxis.RightStickY:
                    return Xbox360Axis.RightThumbY;
                default:
                    return null;
            }
        }
    }
}
