using System.Collections.Generic;

namespace TVR.Service.Core.Model
{
    public class Config
    {
        public int ConfigVersion { get; set; }

        public IDictionary<TrackerColor, ColorProfile> ColorProfiles { get; }

    }
}
