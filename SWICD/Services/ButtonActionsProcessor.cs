using neptune_hidapi.net;
using SWICD.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Services
{
    internal class ButtonActionsProcessor
    {
        private NeptuneControllerInputState _lastState = null;
        public void ProcessInput(ButtonActions config, ControllerConfig currentConfig, NeptuneControllerInputState input)
        {
            if (_lastState != null)
            {
                foreach (var buttons in config.GetActionButtons)
                {
                    if (!buttons.All(b => _lastState.ButtonState[(NeptuneControllerButton)b]) && buttons.All(b => input.ButtonState[(NeptuneControllerButton)b]))
                    {
                        var buttonAction = config[buttons];
                        switch (buttonAction.Type)
                        {
                            case "keyboard-shortcut":
                                ControllerService.Instance.KeyboardMouseInputMapper.ExecuteKeyboardAction(buttonAction.Data);
                                break;
                            case "toggle-haptics":
                                currentConfig.ProfileSettings.ToggleInvertHaptics = !currentConfig.ProfileSettings.ToggleInvertHaptics;
                                break;
                            case "toggle-lizardmode":
                                currentConfig.ProfileSettings.ToggleInvertLizardMode = !currentConfig.ProfileSettings.ToggleInvertLizardMode;
                                break;
                            case "toggle-lizardbuttons":
                                currentConfig.ProfileSettings.ToggleInvertLizardButtons = !currentConfig.ProfileSettings.ToggleInvertLizardButtons;
                                break;
                            case "toggle-lizardbuttons+mouse":
                                currentConfig.ProfileSettings.ToggleInvertLizardMode = !currentConfig.ProfileSettings.ToggleInvertLizardMode;
                                currentConfig.ProfileSettings.ToggleInvertLizardButtons = !currentConfig.ProfileSettings.ToggleInvertLizardButtons;
                                break;
                            case "toggle-lizardmode+emulation":
                                currentConfig.ProfileSettings.ToggleInvertLizardMode = !currentConfig.ProfileSettings.ToggleInvertLizardMode;
                                currentConfig.ProfileSettings.ToggleInvertLizardButtons = !currentConfig.ProfileSettings.ToggleInvertLizardButtons;
                                currentConfig.ProfileSettings.ToggleInvertEmulationActive = !currentConfig.ProfileSettings.ToggleInvertEmulationActive;
                                break;
                            case "toggle-emulation":
                                currentConfig.ProfileSettings.ToggleInvertEmulationActive = !currentConfig.ProfileSettings.ToggleInvertEmulationActive;
                                break;
                            case "toggle-onscreenkeyboard":
                                ControllerService.Instance.OnScreenKeyboardProcessor.ToggleOnScreenKeyboard();
                                break;
                        }
                    }
                }
            }
            _lastState = input;
        }
    }
}
