using TVR.Service.Core.Model.Camera;
using YamlDotNet.Serialization;

namespace TVR.Service.Core.Model.Config
{
    public class CameraInfo
    {
        public int Index { get; set; }

        public string ProfileName { get; set; }

        [YamlIgnore]
        public CameraProfile Profile { get; set; }
    }
}
