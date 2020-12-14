using System;
using System.IO;
using TVR.Service.Core.Model;
using YamlDotNet.Serialization;

namespace TVR.Service.Core.IO
{
    public class FileConfigProvider : IConfigProvider
    {
        public UserConfig UserConfig { get; private set; }

        public VideoSourceConfig VideoSourceConfig { get; private set; }

        private readonly Deserializer deserializer = new Deserializer();

        public void Load()
        {
            var baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwometerVR");
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            LoadUserConfig(baseDir);
            LoadVideoSourceConfig(baseDir);
        }

        private void LoadUserConfig(string baseDir)
        {
            var configFile = Path.Combine(baseDir, "Config.yml");
            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException($"User config file {configFile} does not exist");
            }
            UserConfig = deserializer.Deserialize<UserConfig>(File.ReadAllText(configFile));
        }

        private void LoadVideoSourceConfig(string baseDir)
        {
            var videoSourceConfigFile = Path.Combine(baseDir, $"{UserConfig.Hardware.VideoSource}.yml");
            if (!File.Exists(videoSourceConfigFile))
            {
                throw new FileNotFoundException($"Config file for video source '{UserConfig.Hardware.VideoSource}' not found at {videoSourceConfigFile}");
            }
            VideoSourceConfig = deserializer.Deserialize<VideoSourceConfig>(File.ReadAllText(videoSourceConfigFile));
        }
    }
}
