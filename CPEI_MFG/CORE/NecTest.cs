using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPEI_MFG.Core
{
    public class NecTest
    {
        public bool NeedFixture { get; set; }
        public bool AutoClose { get; set; }
        public string PowerOn { get; set; }
        public string PowerOnReply { get; set; }
        public string PowerOff { get; set; }
        public string PowerOffReply { get; set; }
        public string ResetOn { get; set; }
        public string ResetOnReply { get; set; }
        public string ResetOff { get; set; }
        public string ResetOffReply { get; set; }
        public bool AutoLed { get; set; }
    }
}