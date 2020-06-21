using Newtonsoft.Json;
using System.IO;

namespace TVR.Service.Core.Config
{
    public class TVRConfig
    {
        public CalibrationConfig Calibration { get; set; }

        public TrackerConfig Tracker { get; set; }

        public CameraConfig Camera { get; set; }

        public static TVRConfig Load(string file)
        {
            if (!File.Exists(file))
                return null;

            var data = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<TVRConfig>(data);
        }

    }
}
