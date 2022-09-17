using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Config
{
    public enum VirtualMouseKey
    {
        NONE = 0,
        //
        // Zusammenfassung:
        //     Left mouse button
        LBUTTON = 1,
        //
        // Zusammenfassung:
        //     Right mouse button
        RBUTTON = 2,
        //
        // Zusammenfassung:
        //     Middle mouse button (three-button mouse) - NOT contiguous with LBUTTON and RBUTTON
        MBUTTON = 4,
    }
}
