﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Tracking;

namespace TVRSvc.Core.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private const float BrightnessThreshold = 7.5f;

        private float exposure;

        private Camera camera;

        private int frameCounter;
        private int adjustFrames;

        public Calibration(Camera camera)
        {
            this.camera = camera;
        }

        public void Update(Mat frame)
        {
            // First frames deliver wrong values, so wait for camera to adjust
            frameCounter++;
            if (frameCounter < 3)
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

            Debug.WriteLine("Brightness value: " + meanBrightness);

            if (meanBrightness > BrightnessThreshold)
            {
                exposure--;
                adjustFrames = 3;
                camera.Exposure = exposure;
            }
            else
            {
                Debug.WriteLine("Final exposure: " + exposure);
                IsCalibrated = true;
            }
        }

    }
}
