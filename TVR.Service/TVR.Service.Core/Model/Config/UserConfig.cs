using TVR.Service.Math;

namespace TVR.Service.Core.Model.Config
{
    public class UserConfig
    {
        public CameraInfo CameraInfo { get; set; }

        public Vector3 Offset { get; set; }

        public HardwareConfig HardwareConfig { get; }

        public InputConfig InputConfig { get; }
    }
}
