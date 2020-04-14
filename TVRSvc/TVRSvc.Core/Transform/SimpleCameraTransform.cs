using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Math;

namespace TVRSvc.Core.Transform
{

    // This transform is based on
    // https://www.pyimagesearch.com/2015/01/19/find-distance-camera-objectmarker-using-python-opencv/
    public class SimpleCameraTransform : ICameraTransform
    {
        // Unit: Meters
        // TODO: Make this into a config file

        private const float FocalLength = 656.25f; // Perceived focal length calculated using formula from web page

        private const float SphereSize = 0.04f; // Real-life Tracker sphere is 4cm in diameter

        private const float FS = FocalLength * SphereSize;

        private const float HorizontalPixelsPerMeter = 590; // How many pixels it takes for one meter in XY direction (after normalization)

        private const float HeightAboveGround = 1.35f; // Height of the camera above the ground in meters

        private const int Latency = 3; // Latency of the transform in frames. Higher values mean slower response time but smoother movement

        private PointF frameCenter;

        private RollingAverage xAvg = new RollingAverage(Latency);
        private RollingAverage yAvg = new RollingAverage(Latency);
        private RollingAverage zAvg = new RollingAverage(Latency);

        public Vec3 Transform(int frameWidth, int frameHeight, CircleF obj)
        {
            if (frameCenter == PointF.Empty)
                frameCenter = new PointF(frameWidth / 2.0f, frameHeight / 2.0f);

            var center = LockToPixel(obj.Center);
            var offset = LockToPixel(new PointF(center.X - frameCenter.X, center.Y - frameCenter.Y));
            var diameter = (float) System.Math.Floor(obj.Radius * 2);


            var Z = ComputeDistance(diameter);
            xAvg.Push(offset.X * Z / HorizontalPixelsPerMeter);
            yAvg.Push(-offset.Y * Z / HorizontalPixelsPerMeter);
            zAvg.Push(Z);

            return new Vec3(xAvg.Value, yAvg.Value + HeightAboveGround, zAvg.Value);
        }

        private float ComputeDistance(float diameter)
        {
            return FS / diameter;
        }

        private Point LockToPixel(PointF p)
        {
            return new Point((int)System.Math.Floor(p.X), (int)System.Math.Floor(p.Y));
        }

    }
}
