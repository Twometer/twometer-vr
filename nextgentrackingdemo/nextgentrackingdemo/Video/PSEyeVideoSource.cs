using Emgu.CV;
using Emgu.CV.Structure;
using System;

using static nextgentrackingdemo.Video.Native;

namespace nextgentrackingdemo.Video
{
    public class PSEyeVideoSource : IVideoSource
    {
        private int cameraIndex;
        private IntPtr camera;

        private Image<Bgra, byte> rawFrame;
        private IntPtr data;
        private bool disposedValue;

        public Image<Hsv, byte> Frame { get; private set; }

        public int Framerate { get; set; } = 60;

        public int Exposure { get => CLEyeGetCameraParameter(camera, CLEyeCameraParameter.CLEYE_EXPOSURE); set => CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_EXPOSURE, value); }

        public int Width { get; set; } = 640;

        public int Height { get; set; } = 480;

        public double FrameBrightness { get; private set; }

        public PSEyeVideoSource(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
        }

        public bool Grab()
        {
            data = rawFrame.Mat.DataPointer;
            if (CLEyeCameraGetFrame(camera, data, 500))
            {
                var bgrImg = rawFrame.Convert<Bgr, byte>();
                ImageProcessing.BgrToHsv(bgrImg, Frame);
                FrameBrightness = ImageProcessing.GetBrightness(Frame);
                return true;
            }
            else return false;
        }

        public void Open()
        {
            var id = CLEyeGetCameraUUID(cameraIndex);
            var res = Width == 320 ? CLEyeCameraResolution.CLEYE_QVGA : CLEyeCameraResolution.CLEYE_VGA;
            int width = 0;
            int height = 0;

            camera = CLEyeCreateCamera(id, CLEyeCameraColorMode.CLEYE_COLOR_PROCESSED, res, Framerate);
            CLEyeCameraGetFrameDimensions(camera, ref width, ref height);
            CLEyeSetCameraParameter(camera, CLEyeCameraParameter.CLEYE_GAIN, 0);

            rawFrame = new Image<Bgra, byte>(width, height);
            Frame = new Image<Hsv, byte>(width, height);

            CLEyeCameraStart(camera);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    rawFrame.Dispose();
                    Frame.Dispose();
                }

                CLEyeDestroyCamera(camera);

                disposedValue = true;
            }
        }

        ~PSEyeVideoSource()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
