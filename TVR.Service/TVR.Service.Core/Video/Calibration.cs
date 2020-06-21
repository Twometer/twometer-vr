using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private readonly Camera camera;
        private readonly CalibrationParameters parameters;

        private float exposure;
        private double prevBrightness;
        private int eqBrightnessCounter;
        private int frameCounter;
        private int adjustFrames;

        public Calibration(Camera camera)
        {
            this.camera = camera;
            parameters = camera.CameraInfo.Profile.CalibrationParameters;
        }

        public void Update(double meanBrightness)
        {
            if (frameCounter == 0)
                LoggerFactory.Current.Log(LogLevel.Info, "Calibrating camera...");

            // First frames deliver wrong values, so wait for camera to adjust
            frameCounter++;
            if (frameCounter < parameters.WarmupFrames)
                return;

            // Cooldown between exposure adjustments because the camera takes time
            // to react to those changes
            if (adjustFrames > 0)
            {
                adjustFrames--;
                return;
            }

            LoggerFactory.Current.Log(LogLevel.Debug, $"Mean image brightness: {System.Math.Round(meanBrightness, 2)}");

            // Don't let it calibrate forever
            if ((meanBrightness > parameters.BrightnessThreshold) && EnsureBrightnessChanged(meanBrightness))
            {
                exposure--;
                adjustFrames = parameters.CooldownFrames;
                camera.Exposure = exposure;
            }
            else
            {
                LoggerFactory.Current.Log(LogLevel.Info, $"Calibration completed with exposure offset {exposure}");
                IsCalibrated = true;
            }
        }

        private bool EnsureBrightnessChanged(double brightness)
        {
            if (System.Math.Abs(brightness - prevBrightness) < 2.0)
                eqBrightnessCounter++;
            else eqBrightnessCounter = 0;

            prevBrightness = brightness;

            return eqBrightnessCounter < parameters.StableFrames;
        }

    }
}
