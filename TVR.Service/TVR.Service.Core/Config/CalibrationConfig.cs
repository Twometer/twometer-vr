using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Config
{
    public class CalibrationConfig
    {
        public float BrightnessThreshold { get; set; }

        public int WarmupFrames { get; set; }

        public int CooldownFrames { get; set; }
    }
}
