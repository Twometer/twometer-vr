using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Config
{
    public class CameraProfile
    {
        public string Identifier { get; }
        
        public string Name { get; }

        public string Manufacturer { get; }

        public CameraParameters CameraParameters { get; }

        public CalibrationConfig CalibrationConfig { get; }

        public ColorProfile[] ColorProfiles { get; }
    }
}
