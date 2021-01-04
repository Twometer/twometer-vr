using Emgu.CV;
using Emgu.CV.Structure;
using System;
using static TVR.Service.Core.Native.CLEye;

namespace TVR.Service.Core.Video
{
    internal class PSEyeVideoSource : IVideoSource
    {
        private readonly int cameraIndex;
        private IntPtr camera;

        private Image<Bgra, byte> rawFrame;
        private IntPtr rawData;

        public Image<Bgr, byte> BgrFrame { get; private set; }
        public Image<Hsv, byte> HsvFrame { get; private set; }
        public double FrameBrightness { get; private set; }

        public int Framerate { get; set; } = 60;
        public int Exposure { get => CLEyeGetCameraParameter(camera, CLEyeCameraParameter.CLEYE_EXPOSURE); set => CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_EXPOSURE, value); }
        public int Width { get; set; } = 640;
        public int Height { get; set; } = 480;

        public PSEyeVideoSource(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
        }

        public bool Grab()
        {
            if (CLEyeCameraGetFrame(camera, rawData, 500))
            {
                CvInvoke.CvtColor(rawFrame, BgrFrame, Emgu.CV.CvEnum.ColorConversion.Bgra2Bgr);
                ImageProcessing.BgrToHsv(BgrFrame, HsvFrame);
                FrameBrightness = ImageProcessing.GetBrightness(HsvFrame);
                return true;
            }
            else return false;
        }

        public void Open()
        {
            var camId = CLEyeGetCameraUUID(cameraIndex);
            if (camId == Guid.Empty)
                throw new Exception($"Can't find PSEye camera at index {cameraIndex}");

            var resolution = Width < 640 ? CLEyeCameraResolution.CLEYE_QVGA : CLEyeCameraResolution.CLEYE_VGA;

            var width = 0;
            var height = 0;

            camera = CLEyeCreateCamera(camId, CLEyeCameraColorMode.CLEYE_COLOR_PROCESSED, resolution, Framerate);

            CLEyeCameraGetFrameDimensions(camera, ref width, ref height);
            Width = width;
            Height = height;

            CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_AUTO_EXPOSURE, 0);
            CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_AUTO_GAIN, 0);
            CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_GAIN, 15);

            rawFrame = new Image<Bgra, byte>(width, height);
            BgrFrame = new Image<Bgr, byte>(width, height);
            HsvFrame = new Image<Hsv, byte>(width, height);

            rawData = rawFrame.Mat.DataPointer;

            CLEyeCameraStart(camera);
        }

        public void Dispose()
        {
            rawFrame?.Dispose();
            BgrFrame?.Dispose();
            HsvFrame?.Dispose();
            CLEyeDestroyCamera(camera);
        }
    }
}
