using Emgu.CV.Structure;
using System.Drawing;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Model.Config;

namespace TVR.Service.Core.Tracking
{
    public class CameraTransform
    {
        private readonly Vector3 offset;
        private readonly CameraParameters cameraParameters;
        private readonly HardwareConfig hardwareConfig;

        private readonly RollingAverageFilter xAvg;
        private readonly RollingAverageFilter yAvg;
        private readonly RollingAverageFilter zAvg;

        private PointF frameCenter;

        public CameraTransform(Vector3 offset, CameraParameters cameraParameters, HardwareConfig hardwareConfig, int latency)
        {
            this.offset = offset;
            this.cameraParameters = cameraParameters;
            this.hardwareConfig = hardwareConfig;
            xAvg = new RollingAverageFilter(latency);
            yAvg = new RollingAverageFilter(latency);
            zAvg = new RollingAverageFilter(System.Math.Min(6, latency * 3));
        }

        public Vector3 Transform(int frameWidth, int frameHeight, CircleF obj)
        {
            if (frameCenter == PointF.Empty)
                frameCenter = new PointF(frameWidth / 2.0f, frameHeight / 2.0f);

            var center = obj.Center;
            var offset = new PointF(center.X - frameCenter.X, center.Y - frameCenter.Y);
            var diameter = obj.Radius * 2f;


            zAvg.Push(ComputeDistance(diameter));
            var Z = zAvg.Value;
            xAvg.Push(-offset.X * Z / cameraParameters.PixelsPerMeter);
            yAvg.Push(-offset.Y * Z / cameraParameters.PixelsPerMeter);

            return new Vector3(xAvg.Value + this.offset.X, yAvg.Value + this.offset.Y, Z + this.offset.Z);
        }

        private double ComputeDistance(float diameter)
        {
            return cameraParameters.FocalLength * hardwareConfig.SphereSize / diameter;
        }
    }
}
