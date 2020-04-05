using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Tracking;

namespace TVRSvc.Video
{
    public class Calibration
    {
        public bool IsCalibrated { get; private set; }

        private float exposure;

        private Camera camera;
        private TrackerManager manager;

        public Calibration(Camera camera, TrackerManager manager)
        {
            this.camera = camera;
            this.manager = manager;
        }

        public void Update()
        {
            if (!manager.Detected)
            {
                IsCalibrated = true;
                return;
            }

            exposure -= 0.5f;
            camera.Exposure = exposure;
        }

    }
}
