using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
