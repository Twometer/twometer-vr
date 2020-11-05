using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static nextgentrackingdemo.Source.Native;

namespace nextgentrackingdemo.Source
{
    public class PSEyeVideoSource : IVideoSource
    {
        private IntPtr camera;

        private Mat _frame;

        private IntPtr section;

        private IntPtr map;

        public Mat Frame => _frame;

        public int Framerate { get; set; }

        public int Exposure { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool Grab()
        {
            if (CLEyeCameraGetFrame(camera, map, 500))
            {
                return true;
            }
            else return false;
        }

        public void Open()
        {
            var cams = CLEyeGetCameraCount();
            var id = CLEyeGetCameraUUID(0);
            var res = Width == 320 ? CLEyeCameraResolution.CLEYE_QVGA : CLEyeCameraResolution.CLEYE_VGA;
            int width = 0;
            int height = 0;

            camera = CLEyeCreateCamera(id, CLEyeCameraColorMode.CLEYE_COLOR_PROCESSED, res, Framerate);
            CLEyeCameraGetFrameDimensions(camera, ref width, ref height);

            uint imageSize = (uint)(width * height * 4);
            section = CreateFileMapping(new IntPtr(-1), IntPtr.Zero, 0x04, 0, imageSize, null);
            map = MapViewOfFile(section, 0xF001F, 0, 0, imageSize);

            _frame = new Mat(height, width, Emgu.CV.CvEnum.DepthType.Cv8U, 4, section, width * 4);

            CLEyeCameraStart(camera);
        }
    }
}
