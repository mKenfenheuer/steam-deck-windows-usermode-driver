using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD_Lib.Config
{
    public class Configuration
    {
        public GenericSettings GenericSettings { get; set; } = new GenericSettings();
        public ControllerConfig DefaultControllerConfig { get; set; } = new ControllerConfig();
        public ButtonActions ButtonActions { get; set; } = new ButtonActions();
        public Dictionary<string, ControllerConfig> PerProcessControllerConfig { get; set; } = new Dictionary<string, ControllerConfig>();
    }
}
