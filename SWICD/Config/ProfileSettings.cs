using System;

namespace SWICD.Config
{
    public class ProfileSettings: ICloneable
    {
        public bool DisableLizardMouse { get; set; }
        public bool DisableLizardButtons { get; set; }

        public string ToString(string executable)
        {
            string config = $"[profile]\r\n";
            if (executable != null)
            {
                config = $"[profile,{executable}]\r\n";
            }
            config += $"DisableLizardMouse={DisableLizardMouse}\r\n"; ;
            config += $"DisableLizardButtons={DisableLizardButtons}\r\n"; ;

            return config;
        }

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