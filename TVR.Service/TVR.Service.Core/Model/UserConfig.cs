using System.Collections.Generic;
using System.Numerics;

namespace TVR.Service.Core.Model
{
    public struct UserConfig
    {
        public int ConfigVersion { get; set; }

        public HardwareConfig Hardware {get;set;}

        public Vector3 Offset { get; set; }

        public InputConfig Input { get; set; }
    }

    public struct HardwareConfig
    {
        public string VideoSource { get; set; }

        public float SphereSize { get; set; }

        public IDictionary<TrackerColor, ColorRange[]> ColorProfiles { get; set; }
    }

    public struct InputConfig
    {
        public TrackerButton PoseResetButton { get; set; }

        public float PoseResetDelay { get; set; }

        public int RefreshRate { get; set; }

        public int XYPositionSmoothing { get; set; }
        
        public int ZPositionSmoothing { get; set; }

        public int TrackerTimeout { get; set; }

        public float MinAccuracy { get; set; }
    }
}
