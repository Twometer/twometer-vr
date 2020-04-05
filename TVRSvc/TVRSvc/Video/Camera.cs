using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Video
{
    public class Camera : IDisposable
    {
        private readonly VideoCapture videoCapture;

        public Camera()
        {
            videoCapture = new VideoCapture();
            videoCapture.FlipHorizontal = true;
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.AutoExposure, 0);
        }

        public Mat QueryFrame()
        {
            return videoCapture.QueryFrame();
        }

        public void Dispose()
        {
            videoCapture.Dispose();
        }
    }
}
