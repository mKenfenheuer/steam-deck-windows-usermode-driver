﻿using Newtonsoft.Json;
using System;

namespace SWICD.Config
{
    public class ProfileSettings: ICloneable
    {
        public bool DisableLizardMouse { get; set; }
        public bool DisableLizardButtons { get; set; }
        public bool HapticFeedbackEnabled { get; set; }
        public byte HapticFeedbackAmplitude { get; set; }
        public byte HapticFeedbackPeriod { get; set; }
        public bool OnScreenKeyboardEnabled { get; set; }

        [JsonIgnore]
        public bool ToggleInvertLizardMode { get; set; }

        [JsonIgnore]
        public bool ToggleInvertEmulationActive { get; set; }

        [JsonIgnore]
        public bool ToggleInvertLizardButtons { get; set; }

        [JsonIgnore]
        public bool ToggleInvertHaptics { get; set; }

        [JsonIgnore]
        public bool ToggleInvertOnScreenKeyboard { get; set; }

        [JsonIgnore]
        public bool ToggledDisableLizardMouse => ToggleInvertLizardMode && !DisableLizardMouse ? !DisableLizardMouse : DisableLizardMouse;
        [JsonIgnore]
        public bool ToggledDisableLizardButtons => ToggleInvertLizardButtons && !DisableLizardButtons ? !DisableLizardButtons : DisableLizardButtons;

        [JsonIgnore]
        public bool ToggledDisableHaptics => ToggleInvertHaptics && HapticFeedbackEnabled ? HapticFeedbackEnabled : !HapticFeedbackEnabled;
        [JsonIgnore]
        public bool ToggledDisableOnScreenKeyboard => ToggleInvertOnScreenKeyboard && OnScreenKeyboardEnabled ? OnScreenKeyboardEnabled : !OnScreenKeyboardEnabled;

        public bool GetInvertedEmulationEnabled(bool enabled) => ToggleInvertEmulationActive ? !enabled : enabled;

        public object Clone()
        {
            return new ProfileSettings()
            {
                DisableLizardMouse = DisableLizardMouse,
                DisableLizardButtons = DisableLizardButtons,
                HapticFeedbackEnabled = HapticFeedbackEnabled,
                HapticFeedbackAmplitude = HapticFeedbackAmplitude,
                HapticFeedbackPeriod = HapticFeedbackPeriod,
                OnScreenKeyboardEnabled = OnScreenKeyboardEnabled,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is ProfileSettings settings &&
                   DisableLizardMouse == settings.DisableLizardMouse &&
                   DisableLizardButtons == settings.DisableLizardButtons &&
                   HapticFeedbackEnabled == settings.HapticFeedbackEnabled &&
                   HapticFeedbackAmplitude == settings.HapticFeedbackAmplitude &&
                   HapticFeedbackPeriod == settings.HapticFeedbackPeriod &&
                   OnScreenKeyboardEnabled == settings.OnScreenKeyboardEnabled;
        }
    }
}