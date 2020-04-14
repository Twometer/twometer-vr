using Emgu.CV;
using System;
using static Emgu.CV.VideoCapture;

namespace TVRSvc.Core.Video
{
    public class Camera : IDisposable
    {
        private readonly VideoCapture videoCapture;

        public Mat Frame { get; private set; }

        public double Exposure
        {
            set
            {
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, value);
            }
        }

        public Camera()
        {
            // TODO: Put camera index and resolution into config file
            videoCapture = new VideoCapture(0, API.DShow);
            videoCapture.FlipHorizontal = true;
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.AutoExposure, 0);
        }

        public Mat QueryFrame()
        {
            Frame = videoCapture.QueryFrame();
            return Frame;
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}
