﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Config;
using TVRSvc.Core.Logging;
using TVRSvc.Core.Tracking;

namespace TVRSvc.Core.Video
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

            if (meanBrightness > config.Calibration.BrightnessThreshold || manager.Detected)
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
