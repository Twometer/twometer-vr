using System;
using System.IO;
using TVR.Service.Core.Logging;

namespace TVR.Service.Core.IO
{
    public class FileManager
    {
        public DirectoryInfo ProfilesFolder { get; }

        public FileInfo ConfigFile { get; }

        public FileManager()
        {
            var baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwometerVR");
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            ProfilesFolder = new DirectoryInfo(Path.Combine(baseDirectory, "CameraProfiles"));
            ConfigFile = new FileInfo(Path.Combine(baseDirectory, "Config.yml"));

            if (!ProfilesFolder.Exists)
                PrepareFirstBoot();
        }

        private void PrepareFirstBoot()
        {
            LoggerFactory.Current.Log(LogLevel.Info, "First startup: Loading defaults...");
            ProfilesFolder.Create();
            // TODO Load default configuration
            // TODO Load default camera profiles
        }
    }
}
