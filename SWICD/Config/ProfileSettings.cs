using Newtonsoft.Json;
using System;

namespace SWICD.Config
{
    public class ProfileSettings: ICloneable
    {
        public bool DisableLizardMouse { get; set; }
        public bool DisableLizardButtons { get; set; }

        [JsonIgnore]
        public bool ToggleInvertLizardMode { get; set; }

        [JsonIgnore]
        public bool ToggleInvertEmulationActive { get; set; }

        [JsonIgnore]
        public bool ToggledDisableLizardMouse => ToggleInvertLizardMode && !DisableLizardMouse ? !DisableLizardMouse : DisableLizardMouse;
        [JsonIgnore]
        public bool ToggledDisableLizardButtons => ToggleInvertLizardMode && !DisableLizardButtons ? !DisableLizardButtons : DisableLizardButtons;

        public bool GetInvertedEmulationEnabled(bool enabled) => ToggleInvertEmulationActive ? !enabled : enabled;

        public object Clone()
        {
            return new ProfileSettings()
            {
                DisableLizardMouse = DisableLizardMouse,
                DisableLizardButtons = DisableLizardButtons
            };
        }

        public override bool Equals(object obj)
        {
            return obj is ProfileSettings settings &&
                   DisableLizardMouse == settings.DisableLizardMouse &&
                   DisableLizardButtons == settings.DisableLizardButtons;
                
        }
    }
}