using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.Model.Config
{
    public class CameraInfo
    {
        public int Index { get; set; }

        public string ProfileName { get; set; }

        public CameraProfile Profile { get; set; }
    }
}
