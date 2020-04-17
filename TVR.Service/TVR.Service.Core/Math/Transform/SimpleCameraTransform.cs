using Emgu.CV.Structure;
using System.Drawing;
using TVR.Service.Core.Config;
using TVR.Service.Core.Math.Filters;

namespace TVR.Service.Core.Math.Transform
{
    // This transform is based on
    // https://www.pyimagesearch.com/2015/01/19/find-distance-camera-objectmarker-using-python-opencv/
    public class SimpleCameraTransform : ICameraTransform
    {
        private readonly TVRConfig config;

        private PointF frameCenter;

        private IFilter xAvg;
        private IFilter yAvg;
        private IFilter zAvg;

        public SimpleCameraTransform(TVRConfig config)
        {
            this.config = config;
            xAvg = new RollingAverageFilter(config.Camera.Latency);
            yAvg = new RollingAverageFilter(config.Camera.Latency);
            zAvg = new RollingAverageFilter(config.Camera.Latency);
        }

        public Vec3 Transform(int frameWidth, int frameHeight, CircleF obj)
        {
            if (frameCenter == PointF.Empty)
                frameCenter = new PointF(frameWidth / 2.0f, frameHeight / 2.0f);

            var center = LockToPixel(obj.Center);
            var offset = LockToPixel(new PointF(center.X - frameCenter.X, center.Y - frameCenter.Y));
            var diameter = (float) System.Math.Floor(obj.Radius * 2);


            var Z = ComputeDistance(diameter);
            xAvg.Push(offset.X * Z / config.Camera.PixelsPerMeter);
            yAvg.Push(-offset.Y * Z / config.Camera.PixelsPerMeter);
            zAvg.Push(Z);

            return new Vec3(xAvg.Value, yAvg.Value + config.Camera.HeightAboveGround, zAvg.Value);
        }

        private float ComputeDistance(float diameter)
        {
            return config.Camera.FocalLength * config.Camera.SphereSize / diameter;
        }

        private Point LockToPixel(PointF p)
        {
            return new Point((int)System.Math.Floor(p.X), (int)System.Math.Floor(p.Y));
        }
    }
}
