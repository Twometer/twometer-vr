using System.IO;
using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.IO
{
    public class CameraProfileIO
    {
        public static CameraProfile LoadCameraProfile(string cameraProfile)
        {
            var path = Path.Combine(FileManager.Instance.ProfilesFolder.FullName, cameraProfile + ".yml");
            var file = new FileInfo(path);
            if (!file.Exists)
                throw new FileNotFoundException($"Cannot load camera profile because {path} does not exist.");

            return LoadCameraProfile(file);
        }

        public static CameraProfile LoadCameraProfile(FileInfo file)
        {
            var data = File.ReadAllText(file.FullName);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            return deserializer.Deserialize<CameraProfile>(data);
        }

        public static void WriteCameraProfile(CameraProfile profile)
        {
            var serializer = new YamlDotNet.Serialization.Serializer();
            var path = Path.Combine(FileManager.Instance.ProfilesFolder.FullName, profile.Identifier + ".yml");
            var data = serializer.Serialize(profile);
            File.WriteAllText(path, data);
        }
    }
}
