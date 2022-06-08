using System.Collections.Generic;

namespace SWICD_Lib.Config
{
    public class GenericSettings
    {
        public List<string> BlacklistedProcesses { get; set; } = new List<string>();
        public List<string> WhitelistedProcesses { get; set; } = new List<string>();
        public OperationMode OperationMode { get; set; } = OperationMode.Combined;
        public bool StartWithWindows { get; set; } = true;
    }
}