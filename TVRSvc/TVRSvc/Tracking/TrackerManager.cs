using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Model;

namespace TVRSvc.Tracking
{
    public class TrackerManager
    {
        public Tracker[] Trackers { get; } = new[]
        {
            new Tracker(0, TrackerSettings.Blue),
            // new Tracker(1, new TrackerSettings())
        };

        public bool Detected => Trackers.Any(t => t.Detected);

        public void UpdateVideo(Mat frame)
        {
            foreach (var tracker in Trackers)
                tracker.UpdateVideo(frame);
        }

        public void UpdateMeta()
        {
            // TODO: Parse network-based metadata frames
        }

    }
}
