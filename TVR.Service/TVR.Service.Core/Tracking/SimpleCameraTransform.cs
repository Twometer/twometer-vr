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

        public SimpleCameraTransform(IConfigProvider configProvider)
        {
            var videoSourceConfig = configProvider.VideoSourceConfig;
            sphereSize = configProvider.UserConfig.Hardware.SphereSize;
            transformConfig = videoSourceConfig.TransformConfig;
            frameCenter = new PointF(videoSourceConfig.FrameWidth / 2.0f, videoSourceConfig.FrameHeight / 2.0f);
        }

        public Vector3 Transform(CircleF circle)
        {
            var relativePos = new PointF(circle.Center.X - frameCenter.X, circle.Center.Y - frameCenter.Y);
            var diameter = circle.Area * 2.0f;

            var z = ComputeDistance(diameter);
            var x = -relativePos.X * z / transformConfig.PixelsPerMeter;
            var y = -relativePos.Y * z / transformConfig.PixelsPerMeter;

            // TODO: Position smoothing

            return new Vector3(x, y, z);
        }

        private float ComputeDistance(double diameter)
        {
            return (float) (transformConfig.PFocalLength * sphereSize / diameter);
        }
    }
}
