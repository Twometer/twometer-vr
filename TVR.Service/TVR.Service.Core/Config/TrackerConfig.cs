using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Config
{
    public class TrackerConfig
    {
        public bool LeftInvertPitch { get; set; }

        public bool RightInvertPitch { get; set; }

        public double PoseResetDelay { get; set; }
        
        public float ZOffset { get; set; }
    }
}
