using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SWICD.Config
{
    [Serializable]
    public class ButtonAction
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }
}