using Emgu.CV;
using System;
using TVR.Service.Core.Config;
using static Emgu.CV.VideoCapture;

namespace TVR.Service.Core.Video
{
    public class Camera : IDisposable
    {
        private readonly VideoCapture videoCapture;

        public Mat Frame { get; } = new Mat();

        public Mat HsvFrame { get; } = new Mat();

        public double Exposure
        {
            set
            {
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, value);
            }
        }

        public Camera(TVRConfig config)
        {
            videoCapture = new VideoCapture(config.Camera.CameraIndex, API.DShow);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, config.Camera.FrameWidth);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, config.Camera.FrameHeight);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.AutoExposure, 0);
        }

        public void Update()
        {
            if (videoCapture.Grab())
                videoCapture.Retrieve(Frame);
            CvInvoke.CvtColor(Frame, HsvFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}
