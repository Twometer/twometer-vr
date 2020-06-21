using System.IO;
using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.IO
{
    public class CameraLoader
    {
        public static CameraProfile LoadCameraProfile(string cameraProfile)
        {
            var path = Path.Combine(FileManager.Instance.ProfilesFolder.FullName, cameraProfile + ".yml");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Cannot load camera profile because {path} does not exist.");

            var data = File.ReadAllText(path);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            return deserializer.Deserialize<CameraProfile>(data);
        }
    }
}
