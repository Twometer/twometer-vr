using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace nextgentrackingdemo.Video
{
    public class DShowVideoSource : IVideoSource
    {
        private int cameraIndex;

        private VideoCapture videoCapture;

        private Mat rawFrame = new Mat();

        public Image<Hsv, byte> Frame { get; private set; }

        public int Framerate { get; set; } = 30;

        public int Exposure { get => (int)videoCapture.GetCaptureProperty(CapProp.Exposure); set => videoCapture.SetCaptureProperty(CapProp.Exposure, value); }

        public int Width { get; set; } = 1280;

        public int Height { get; set; } = 720;

        public double FrameBrightness { get; private set; }

        public DShowVideoSource(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
        }

        public bool Grab()
        {
            if (videoCapture.Grab())
            {
                videoCapture.Retrieve(rawFrame);
                ImageProcessing.BgrToHsv(rawFrame, Frame);
                FrameBrightness = ImageProcessing.GetBrightness(Frame);
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

            Frame = new Image<Hsv, byte>(Width, Height);
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}
