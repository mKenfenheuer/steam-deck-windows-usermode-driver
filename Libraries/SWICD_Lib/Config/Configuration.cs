using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD_Lib.Config
{
    public class Configuration
    {
        public List<string> BlacklistedProcesses { get; set; } = new List<string>();
        public List<string> WhitelistedProcesses { get; set; } = new List<string>();
        public OperationMode OperationMode { get; set; } = OperationMode.Blacklist;
        public ControllerConfig DefaultControllerConfig { get; set; } = new ControllerConfig();
        public Dictionary<string, ControllerConfig> PerProcessControllerConfig { get; set; } = new Dictionary<string, ControllerConfig>();
    }
}
