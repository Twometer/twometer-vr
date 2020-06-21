using System.IO;
using TVR.Service.Model.Camera;

namespace TVR.Service.IO
{
    public class CameraLoader
    {
        public static CameraProfile LoadCameraProfile(DirectoryInfo profileFolder, string cameraProfile)
        {
            var path = Path.Combine(profileFolder.FullName, cameraProfile + ".yml");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Cannot load camera profile because {path} does not exist.");

            var data = File.ReadAllText(path);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            return deserializer.Deserialize<CameraProfile>(data);
        }
    }
}
