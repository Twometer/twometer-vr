using TVR.Service.Model.Camera;

namespace TVR.Service.Model.Config
{
    public class CameraInfo
    {
        public int Index { get; set; }

        public string ProfileName { get; set; }

        public CameraProfile Profile { get; set; }
    }
}
