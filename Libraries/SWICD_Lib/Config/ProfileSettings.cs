using System;

namespace SWICD_Lib.Config
{
    public class ProfileSettings: ICloneable
    {
        public bool DisableLizardMode { get; set; }

        public string ToString(string executable)
        {
            string config = $"[profile]\r\n";
            if (executable != null)
            {
                config = $"[profile,{executable}]\r\n";
            }
            config += $"DisableLizardMode={DisableLizardMode}\r\n"; ;

            return config;
        }

        public object Clone()
        {
            return new ProfileSettings()
            {
                DisableLizardMode = DisableLizardMode,  
            };
        }

        public override bool Equals(object obj)
        {
            return obj is ProfileSettings settings &&
                   DisableLizardMode == settings.DisableLizardMode;
        }
    }
}