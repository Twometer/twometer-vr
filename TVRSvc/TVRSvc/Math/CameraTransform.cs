using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Math
{

    // This transform is based on
    // https://www.pyimagesearch.com/2015/01/19/find-distance-camera-objectmarker-using-python-opencv/
    public class CameraTransform
    {
        // Unit: Meters

        private const float FocalLength = 656.25f; // Perceived focal length calculated using formula from web page

        private const float SphereSize = 0.04f; // Real-life Tracker sphere is 4cm in diameter

        private const float FS = FocalLength * SphereSize;

        private const float HorizontalPixelsPerMeter = 590; // How many pixels it takes for one meter in XY direction (after normalization)

        private PointF frameCenter;

        private RollingAverage xAvg = new RollingAverage(8);
        private RollingAverage yAvg = new RollingAverage(8);
        private RollingAverage zAvg = new RollingAverage(8);

        public Vec3 Transform(int frameWidth, int frameHeight, CircleF obj)
        {
            if (frameCenter == PointF.Empty)
                frameCenter = new PointF(frameWidth / 2.0f, frameHeight / 2.0f);

            var offset = new PointF(obj.Center.X - frameCenter.X, obj.Center.Y - frameCenter.Y);
            var diameter = obj.Radius * 2;


            var Z = ComputeDistance(diameter);
            xAvg.Push(offset.X * Z / HorizontalPixelsPerMeter); //   
            yAvg.Push(offset.Y * Z / HorizontalPixelsPerMeter); //   
            zAvg.Push(Z);

            return new Vec3(xAvg.Value, yAvg.Value, zAvg.Value);
        }

        private float ComputeDistance(float diameter)
        {
            return FS / diameter;
        }

    }
}
