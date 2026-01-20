using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Core
{
    public enum StatusFlag
    {
        RUN = 0,
        PASS = 1,
        FAIL = 2,
        ERROR = 3,
        STOP = 4,
        STANDBY = 5,
        RESCAN = 6
    }
}
