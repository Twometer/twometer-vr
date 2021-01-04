using Emgu.CV.Structure;
using System.Drawing;
using System.Numerics;
using TVR.Service.Core.IO;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Tracking
{
    internal class SimpleCameraTransform : ICameraTransform
    {
        private readonly float sphereSize;
        private readonly TransformConfig transformConfig;
        private readonly PointF frameCenter;
        private readonly RollingAverageFilter xFilter;
        private readonly RollingAverageFilter yFilter;
        private readonly RollingAverageFilter zFilter;

        public SimpleCameraTransform(IConfigProvider configProvider)
        {
            var videoSourceConfig = configProvider.VideoSourceConfig;
            sphereSize = configProvider.UserConfig.Hardware.SphereSize;
            transformConfig = videoSourceConfig.TransformConfig;
            frameCenter = new PointF(videoSourceConfig.FrameWidth / 2.0f, videoSourceConfig.FrameHeight / 2.0f);
            xFilter = new RollingAverageFilter(configProvider.UserConfig.Input.XYPositionSmoothing);
            yFilter = new RollingAverageFilter(configProvider.UserConfig.Input.XYPositionSmoothing);
            zFilter = new RollingAverageFilter(configProvider.UserConfig.Input.ZPositionSmoothing);
        }

        public Vector3 Transform(CircleF circle)
        {
            var relativePos = new PointF(circle.Center.X - frameCenter.X, circle.Center.Y - frameCenter.Y);
            var diameter = circle.Radius * 2.0f;

            var z = zFilter.Push(ComputeDistance(diameter));
            var x = xFilter.Push(-relativePos.X * z / transformConfig.PixelsPerMeter);
            var y = yFilter.Push(-relativePos.Y * z / transformConfig.PixelsPerMeter);

            return new Vector3(x, y, z);
        }

        private float ComputeDistance(double diameter)
        {
            return (float)(transformConfig.PFocalLength * sphereSize / diameter);
        }
    }
}
