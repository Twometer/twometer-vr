using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using TVR.Service.Core.Model.Config;

namespace TVR.Service.Core.Video
{
    public class Camera : IDisposable
    {
        private readonly VideoCapture videoCapture;

        public Mat Frame { get; } = new Mat();

        public Mat HsvFrame { get; } = new Mat();

        public double FrameBrightness { get; private set; } = 0.0f;

        public double Exposure
        {
            set
            {
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, value);
            }
        }

        public Camera(CameraInfo camera)
        {
            videoCapture = new VideoCapture(camera.Index, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, camera.Profile.CameraParameters.FrameWidth);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, camera.Profile.CameraParameters.FrameHeight);
            videoCapture.SetCaptureProperty(CapProp.AutoExposure, 0);
        }

        public void Update()
        {
            if (videoCapture.Grab())
            {
                videoCapture.Retrieve(Frame);

                CvInvoke.CvtColor(Frame, HsvFrame, ColorConversion.Bgr2Hsv);
                FrameBrightness = CvInvoke.Mean(HsvFrame).V2;
            }
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}
