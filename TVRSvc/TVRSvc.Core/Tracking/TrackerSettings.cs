using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Tracking
{
    public struct TrackerSettings
    {
        public static TrackerSettings Blue { get; } = new TrackerSettings(new MCvScalar(58, 125, 110), new MCvScalar(137, 255, 255), 100);

        public static TrackerSettings Red { get; } = new TrackerSettings(new[] {
            new ColorRange(new MCvScalar(0, 76, 56), new MCvScalar(70, 255, 255)),
            new ColorRange(new MCvScalar(151, 76, 56), new MCvScalar(179, 255, 255))
        }, 100);

        public ColorRange[] ColorRanges { get; }

        public float CannyThreshold { get; }

        public TrackerSettings(ColorRange[] colorRanges, float cannyThreshold)
        {
            ColorRanges = colorRanges;
            CannyThreshold = cannyThreshold;
        }

        public TrackerSettings(MCvScalar minimum, MCvScalar maximum, float cannyThreshold)
        {
            ColorRanges = new ColorRange[] { new ColorRange(minimum, maximum) };
            CannyThreshold = cannyThreshold;
        }
    }
}
