using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Diagnostics;
using TVR.Service.Core.Logging;
using VR.Service.Core.Native;

namespace TVR.Service.Core.Video
{
    internal class PSEyeVideoSource : IVideoSource
    {
        private readonly int cameraIndex;
        private IntPtr camera;

        private IntPtr rawData;

        public Image<Bgr, byte> BgrFrame { get; private set; }
        public Image<Hsv, byte> HsvFrame { get; private set; }
        public double FrameBrightness { get; private set; }

        public int Framerate { get; set; } = 60;
        public int Exposure { get => PSEye.Exposure(camera); set => PSEye.SetExposure(camera, value); }
        public int Width { get; set; } = 640;
        public int Height { get; set; } = 480;

        public PSEyeVideoSource(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
        }

        public bool Grab()
        {
            if (PSEye.GetFrame(camera, rawData))
            {
                ImageProcessing.BgrToHsv(BgrFrame, HsvFrame);
                FrameBrightness = ImageProcessing.GetBrightness(HsvFrame);
                return true;
            }
            else return false;
        }

        public void Open()
        {
            Loggers.Current.Log(LogLevel.Info, $"Starging PSEYe");
            camera = PSEye.OpenCamera(cameraIndex, Width < 640 ? PsEyeResolution.Qvga : PsEyeResolution.Vga, 75, PsEyeFormat.Bgr);
            if (camera == IntPtr.Zero)
            {
                throw new Exception($"Can't open PSEye camera at index {cameraIndex}");
            }

            Width = PSEye.Width(camera);
            Height = PSEye.Height(camera);

            PSEye.SetAutoGain(camera, false);
            PSEye.SetGain(camera, 15);

            BgrFrame = new Image<Bgr, byte>(Width, Height);
            HsvFrame = new Image<Hsv, byte>(Width, Height);

            rawData = BgrFrame.Mat.DataPointer;
            Loggers.Current.Log(LogLevel.Info, $"PS Eye initialized with resolution {Width}x{Height}");
        }

        public void Dispose()
        {
            BgrFrame?.Dispose();
            HsvFrame?.Dispose();
            PSEye.CloseCamera(camera);
        }
    }
}
