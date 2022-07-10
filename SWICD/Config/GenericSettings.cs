using System;
using System.Collections.Generic;
using System.Linq;

namespace SWICD.Config
{
    public class GenericSettings : ICloneable
    {
        public List<string> BlacklistedProcesses { get; set; } = new List<string>();
        public List<string> WhitelistedProcesses { get; set; } = new List<string>();
        public OperationMode OperationMode { get; set; } = OperationMode.Combined;
        public bool StartWithWindows { get; set; } = true;
        public bool StartMinimized { get; set; } = true;

        public override string ToString()
        {
            string configText = "[general]\r\n";
            foreach (string executable in BlacklistedProcesses)
            {
                configText += $"Blacklist={executable}\r\n";
            }
            foreach (string executable in WhitelistedProcesses)
            {
                configText += $"Whitelist={executable}\r\n";
            }
            configText += $"Mode={OperationMode}\r\n";
            configText += $"StartWithWindows={StartWithWindows}\r\n";
            configText += $"StartMinimized={StartMinimized}\r\n";

            return configText;
        }

        public object Clone()
        {
            var obj = new GenericSettings();
            obj.BlacklistedProcesses = BlacklistedProcesses.ToList();
            obj.WhitelistedProcesses = WhitelistedProcesses.ToList();
            obj.OperationMode = OperationMode;
            obj.StartWithWindows = StartWithWindows;
            return obj;
        }

        public override bool Equals(object obj)
        {
            return obj is GenericSettings settings &&
                   Enumerable.SequenceEqual(BlacklistedProcesses.OrderBy(a => a), settings.BlacklistedProcesses.OrderBy(a => a)) &&
                   Enumerable.SequenceEqual(WhitelistedProcesses.OrderBy(a => a), settings.WhitelistedProcesses.OrderBy(a => a)) &&
                   OperationMode == settings.OperationMode &&
                   StartWithWindows == settings.StartWithWindows;
        }
    }
}