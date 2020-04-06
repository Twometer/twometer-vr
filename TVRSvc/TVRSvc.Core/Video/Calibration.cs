using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Tracking;

namespace TVRSvc.Core.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private const int BrightnessThreshold = 10;

        private float exposure;

        private Camera camera;

        private int frameCounter;

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

            var grayMat = new Mat();
            CvInvoke.CvtColor(frame, grayMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            var meanBrightness = CvInvoke.Mean(grayMat).V0;

            if (meanBrightness > BrightnessThreshold)
            {
                exposure -= 0.5f;
                camera.Exposure = exposure;
            }
            else
            {
                IsCalibrated = true;
            }
        }

    }
}
