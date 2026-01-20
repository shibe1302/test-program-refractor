using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.CORE
{
    class Other
    {
    }
    public class LaserInfo
    {
        public string SetNum { get; set; }
        public int Mode { get; set; }
        public int SnNum { get; set; }
    }

    public class NustreamInfo
    {
        public string LogPath { get; set; }
        public string ModelName { get; set; }
        public string LogName { get; set; }
        public IntPtr HApmpt { get; set; }
    }

    public class EndInfo
    {
        public bool Finished { get; set; }
        public bool Result { get; set; }
        public bool ResultPass { get; set; }
        public bool ResultFail { get; set; }
        public bool ResultGolden { get; set; }
        public string ErrorCode { get; set; }
        public int IqTestNum { get; set; }
    }
}
