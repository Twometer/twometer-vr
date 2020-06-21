using System;
using System.IO;

namespace TVR.Service.Core.IO
{
    public class FileManager
    {
        public static FileManager Instance { get; } = new FileManager();

        public DirectoryInfo ProfilesFolder { get; }

        public FileInfo ConfigFile { get; }

        public bool IsFirstStart { get; }

        private FileManager()
        {
            var baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwometerVR");
            if (!Directory.Exists(baseDirectory))
                Directory.CreateDirectory(baseDirectory);

            ProfilesFolder = new DirectoryInfo(Path.Combine(baseDirectory, "CameraProfiles"));
            ConfigFile = new FileInfo(Path.Combine(baseDirectory, "Config.yml"));

            IsFirstStart = !ProfilesFolder.Exists;
        }

    }
}
