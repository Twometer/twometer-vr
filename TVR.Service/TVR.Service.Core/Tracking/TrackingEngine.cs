﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using TVR.Service.Core.IO;
using TVR.Service.Core.Model;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    internal class TrackingEngine
    {
        private readonly TrackerManager trackerManager;
        private readonly IConfigProvider configProvider;
        private readonly IVideoSource videoSource;

        private Image<Gray, byte> frame;

        private readonly Mat tempFrame = new Mat();
        private readonly Mat hierarchy = new Mat();

        public Image<Gray, byte> DebugFrame => frame;

        public TrackingEngine(TrackerManager trackerManager, IConfigProvider configProvider, IVideoSource videoSource)
        {
            this.trackerManager = trackerManager;
            this.configProvider = configProvider;
            this.videoSource = videoSource;
        }

        public void Update()
        {
            EnsureFramebufferInitialized();

            foreach (var tracker in trackerManager.Trackers)
            {
                var colorProfile = configProvider.UserConfig.Hardware.ColorProfiles[tracker.TrackerColor];
                TrackDevice(tracker, colorProfile);
            }
        }

        private void TrackDevice(Tracker device, ColorRange[] colorRanges)
        {
            if (device.CameraTransform == null)
            {
                device.CameraTransform = new SimpleCameraTransform(configProvider);
            }

            var adaptiveBrightness = configProvider.VideoSourceConfig.UseAdaptiveBrightness ? videoSource.FrameBrightness : 0;
            ImageProcessing.ColorFilter(videoSource.HsvFrame, frame, tempFrame, colorRanges, adaptiveBrightness);

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(frame, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxNone);
                device.InRange = contours.Size > 0;

                if (device.InRange)
                {
                    var largest = FindLargestContour(contours);
                    var circle = CvInvoke.MinEnclosingCircle(largest);

                    device.TrackingAccuracy = (float)(CvInvoke.ContourArea(largest) / circle.Area);
                    device.Circle = circle;

                    // Only update position if we have some confidence in what we do
                    if (device.TrackingAccuracy > configProvider.UserConfig.Input.MinAccuracy)
                    {
                        device.Position = device.CameraTransform.Transform(circle) + configProvider.UserConfig.Offset;
                    }
                    else
                    {
                        device.InRange = false;
                    }
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
