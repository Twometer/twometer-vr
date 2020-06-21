using System.IO;
using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.IO
{
    public class CameraLoader
    {
        public static CameraProfile LoadCameraProfile(FileManager fileManager, string cameraProfile)
        {
            var path = Path.Combine(fileManager.ProfilesFolder.FullName, cameraProfile + ".yml");
            if (!File.Exists(path))
                throw new FileNotFoundException($"Cannot load camera profile because {path} does not exist.");

            var data = File.ReadAllText(path);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            return deserializer.Deserialize<CameraProfile>(data);
        }
    }
}
