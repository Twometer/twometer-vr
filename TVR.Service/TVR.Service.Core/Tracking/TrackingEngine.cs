using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Collections.Generic;
using TVR.Service.Core.Model;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    public class TrackingEngine
    {
        // TODO get config in here.
        private Config config;

        private IVideoSource videoSource;

        private readonly IList<Tracker> trackers = new List<Tracker>();

        private Image<Gray, byte> frame;

        private readonly Mat tempFrame = new Mat();
        private readonly Mat hierarchy = new Mat();

        public void Update()
        {
            if (frame == null)
                frame = new Image<Gray, byte>(videoSource.HsvFrame.Width, videoSource.HsvFrame.Height);

            foreach (var tracker in trackers)
            {
                var colorProfile = config.ColorProfiles[tracker.TrackerColor];
                TrackSingleDevice(tracker, colorProfile);
            }
        }

        private void TrackSingleDevice(Tracker device, ColorProfile profile)
        {
            ImageProcessing.ColorFilter(videoSource.HsvFrame.Mat, frame, tempFrame, profile, videoSource.FrameBrightness);

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(frame, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);

                device.Tracking = contours.Size > 0;
                if (device.Tracking)
                {
                    var circle = CvInvoke.MinEnclosingCircle(contours[0]);
                    
                    // TODO transform circle to world space
                }
            }
        }

        public void RegisterTracker(Tracker tracker)
        {
            trackers.Add(tracker);
        }

    }
}
