using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace TVR.Service.Core.Video
{
    internal class DShowVideoSource : IVideoSource
    {
        private readonly int cameraIndex;
        private VideoCapture videoCapture;

        public Image<Bgr, byte> BgrFrame { get; private set; }
        public Image<Hsv, byte> HsvFrame { get; private set; }
        public double FrameBrightness { get; private set; }

        public int Framerate { get; set; } = 30;
        public int Exposure { get => (int)videoCapture.GetCaptureProperty(CapProp.Exposure); set => videoCapture.SetCaptureProperty(CapProp.Exposure, value); }
        public int Width { get; set; } = 1280;
        public int Height { get; set; } = 720;

        public DShowVideoSource(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
        }

        public bool Grab()
        {
            if (videoCapture.Grab())
            {
                videoCapture.Retrieve(BgrFrame);
                ImageProcessing.BgrToHsv(BgrFrame, HsvFrame);
                FrameBrightness = ImageProcessing.GetBrightness(HsvFrame);
                return true;
            }
            return false;
        }

        public void Open()
        {
            videoCapture = new VideoCapture(cameraIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(CapProp.FrameWidth, Width);
            videoCapture.SetCaptureProperty(CapProp.FrameHeight, Height);
            videoCapture.SetCaptureProperty(CapProp.Exposure, 0);
            videoCapture.SetCaptureProperty(CapProp.AutoExposure, 0);
            videoCapture.SetCaptureProperty(CapProp.Fps, Framerate);

            BgrFrame = new Image<Bgr, byte>(Width, Height);
            HsvFrame = new Image<Hsv, byte>(Width, Height);
        }

        public void Dispose()
        {
            videoCapture?.Dispose();
            BgrFrame?.Dispose();
            HsvFrame?.Dispose();
        }
    }
}
