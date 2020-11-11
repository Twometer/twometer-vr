using TVR.Service.Core.Math;

namespace TVR.Service.Core.Model.Config
{
    public class UserConfig
    {
        public CameraInfo CameraInfo { get; set; }

        public Vector3 Offset { get; set; }

        public HardwareConfig HardwareConfig { get; set; }

        public InputConfig InputConfig { get; set; }
    }
}
