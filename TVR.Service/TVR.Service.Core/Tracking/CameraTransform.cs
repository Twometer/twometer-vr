using Emgu.CV.Structure;
using System.Drawing;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Model.Config;

namespace TVR.Service.Core.Tracking
{
    public class CameraTransform
    {
        private readonly CameraParameters cameraParameters;
        private readonly HardwareConfig hardwareConfig;

        private readonly RollingAverageFilter xAvg;
        private readonly RollingAverageFilter yAvg;
        private readonly RollingAverageFilter zAvg;

        private PointF frameCenter;

        public CameraTransform(CameraParameters cameraParameters, HardwareConfig hardwareConfig, int latency)
        {
            this.cameraParameters = cameraParameters;
            this.hardwareConfig = hardwareConfig;
            xAvg = new RollingAverageFilter(latency);
            yAvg = new RollingAverageFilter(latency);
            zAvg = new RollingAverageFilter(latency);
        }

        public Vector3 Transform(int frameWidth, int frameHeight, CircleF obj)
        {
            if (frameCenter == PointF.Empty)
                frameCenter = new PointF(frameWidth / 2.0f, frameHeight / 2.0f);

            var center = LockToPixel(obj.Center);
            var offset = LockToPixel(new PointF(center.X - frameCenter.X, center.Y - frameCenter.Y));
            var diameter = (float)System.Math.Floor(obj.Radius * 2);


            var Z = ComputeDistance(diameter);
            xAvg.Push(-offset.X * Z / cameraParameters.PixelsPerMeter);
            yAvg.Push(-offset.Y * Z / cameraParameters.PixelsPerMeter);
            zAvg.Push(Z);

            return new Vector3(xAvg.Value, yAvg.Value, zAvg.Value);
        }

        private double ComputeDistance(float diameter)
        {
            return cameraParameters.FocalLength * hardwareConfig.SphereSize / diameter;
        }

        private Point LockToPixel(PointF p)
        {
            return new Point((int)System.Math.Floor(p.X), (int)System.Math.Floor(p.Y));
        }

    }
}
