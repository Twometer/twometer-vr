using TVR.Service.Core.Config;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Tracking;

namespace TVR.Service.Core.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private readonly TVRConfig config;
        private readonly Camera camera;
        private readonly TrackerManager manager;

        private float exposure;
        private double prevBrightness;
        private int eqBrightnessCounter;
        private int frameCounter;
        private int adjustFrames;

        public Calibration(TVRConfig config, Camera camera, TrackerManager manager)
        {
            this.config = config;
            this.camera = camera;
            this.manager = manager;
        }

        public void Update(double meanBrightness)
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

            LoggerFactory.Current.Log(LogLevel.Debug, $"Brightness value: {System.Math.Round(meanBrightness, 2)}");

            // Don't let it calibrate forever
            if ((meanBrightness > config.Calibration.BrightnessThreshold || manager.Detected) && EnsureBrightnessChanged(meanBrightness))
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

        private bool EnsureBrightnessChanged(double brightness)
        {
            if (System.Math.Abs(brightness - prevBrightness) < 2.0)
                eqBrightnessCounter++;
            else eqBrightnessCounter = 0;

            prevBrightness = brightness;

            return eqBrightnessCounter < 3;
        }

    }
}
