using Emgu.CV;
using TVR.Service.Core.Config;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Tracking;

namespace TVR.Service.Core.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private TVRConfig config;
        private Camera camera;
        private TrackerManager manager;

        private float exposure;
        private int frameCounter;
        private int adjustFrames;

        public Calibration(TVRConfig config, Camera camera, TrackerManager manager)
        {
            this.config = config;
            this.camera = camera;
            this.manager = manager;
        }

        public void Update(Mat frame)
        {
            if (frameCounter == 0)
                LoggerFactory.Current.Log(LogLevel.Info, "Calibrating camera...");

            // First frames deliver wrong values, so wait for camera to adjust
            frameCounter++;
            if (frameCounter < config.Calibration.WarmupFrames)
                return;

            // Cooldown between exposure adjustments because the camera takes time
            // to react to those changes
            if (adjustFrames > 0)
            {
                adjustFrames--;
                return;
            }

            var grayMat = new Mat();
            CvInvoke.CvtColor(frame, grayMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            var meanBrightness = CvInvoke.Mean(grayMat).V0;

            LoggerFactory.Current.Log(LogLevel.Debug, $"Brightness value: {System.Math.Round(meanBrightness, 2)}");

            // Don't let it calibrate forever
            if ((meanBrightness > config.Calibration.BrightnessThreshold || manager.Detected) && exposure > -30)
            {
                exposure--;
                adjustFrames = config.Calibration.CooldownFrames;
                camera.Exposure = exposure;
            }
            else
            {
                LoggerFactory.Current.Log(LogLevel.Info, $"Calibration completed with exposure {exposure}");
                IsCalibrated = true;
            }
        }

    }
}
