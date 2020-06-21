using System.IO;
using TVR.Service.Model.Config;

namespace TVR.Service.IO
{
    public class ConfigLoader
    {
        public static UserConfig LoadUserConfig(DirectoryInfo cameraProfileFolder, string configFile)
        {
            if (!File.Exists(configFile))
                throw new FileNotFoundException($"Cannot load user config because {configFile} does not exist.");

            var data = File.ReadAllText(configFile);
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            var config = deserializer.Deserialize<UserConfig>(data);
            config.CameraInfo.Profile = CameraLoader.LoadCameraProfile(cameraProfileFolder, config.CameraInfo.ProfileName);
            return config;
        }
    }
}
