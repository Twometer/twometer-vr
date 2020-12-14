using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using TVR.Service.Core.IO;
using TVR.Service.Core.Model;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    public class TrackingEngine
    {
        // TODO get config in here.
        private TrackerManager trackerManager;
        private IConfigProvider configProvider;
        private IVideoSource videoSource;
        private ICameraTransform cameraTransform;

        private Image<Gray, byte> frame;

        private readonly Mat tempFrame = new Mat();
        private readonly Mat hierarchy = new Mat();

        public void Update()
        {
            EnsureFramebufferInitialized();

            foreach (var tracker in trackerManager.Trackers)
            {
                var colorProfile = configProvider.UserConfig.Hardware.ColorProfiles[tracker.TrackerColor];
                TrackDevice(tracker, colorProfile);
            }
        }

        private void TrackDevice(Tracker device, ColorProfile profile)
        {
            ImageProcessing.ColorFilter(videoSource.HsvFrame, frame, tempFrame, profile, videoSource.FrameBrightness);

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(frame, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);
                device.InRange = contours.Size > 0;
                
                if (device.InRange)
                {
                    var largest = FindLargestContour(contours);
                    var circle = CvInvoke.MinEnclosingCircle(largest);
                    device.Position = cameraTransform.Transform(circle);
                }
            }
        }

        private VectorOfPoint FindLargestContour(VectorOfVectorOfPoint contours)
        {
            var largest = contours[0];
            
            for (var i = 0; i < contours.Size; i++)
            {
                var contour = contours[i];

                if (contour.Size > largest.Size)
                    largest = contour;
            }

            return largest;
        }

        private void EnsureFramebufferInitialized()
        {
            if (frame == null)
                frame = new Image<Gray, byte>(videoSource.HsvFrame.Width, videoSource.HsvFrame.Height);
        }

    }
}
