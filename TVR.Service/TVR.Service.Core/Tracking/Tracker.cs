using Emgu.CV;
using Emgu.CV.Structure;
using TVR.Service.Core.Math;
using TVR.Service.Core.Math.Transform;
using TVR.Service.Core.Model;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    public class Tracker
    {
        public Controller Controller { get; }

        public TrackerSettings Settings { get; }

        public bool Detected { get; private set; }

        public bool Visualize { get; set; }

        public Image<Gray, byte> Frame { get; private set; }

        private readonly ICameraTransform transform;

        private readonly Mat tempMat = new Mat();

        public Tracker(byte controllerId, TrackerSettings settings, ICameraTransform transform)
        {
            Controller = new Controller(controllerId);
            Settings = settings;
            this.transform = transform;
        }

        public void UpdateVideo(Mat frame, double brightness)
        {
            if (Frame == null)
                Frame = new Image<Gray, byte>(frame.Width, frame.Height);

            RangeFilter(frame, brightness);
            ImageProcessing.Erode(Frame, 1);
            ImageProcessing.Dilate(Frame, 1);
            ImageProcessing.ThresholdBinary(Frame, new Gray(150), new Gray(255));
            ImageProcessing.SmoothGaussian(Frame, 9);

            var circles = ImageProcessing.HoughCircles(Frame, Settings.CannyThreshold, 1, 3, Frame.Width / 2, 2, 50);
            Detected = circles.Length > 0;

            if (Detected)
            {
                var circle = circles[0];
                var spherePos = transform.Transform(Frame.Width, Frame.Height, circle);
                Controller.Position = CalcOrigin(spherePos);

                if (Visualize)
                    Frame.Draw(circle, new Gray(128), 4);
            }
        }

        private void RangeFilter(Mat frame, double brightness)
        {
            var range0 = Settings.ColorRanges[0];
            CvInvoke.InRange(frame, new ScalarArray(AdaptMinimum(range0.Minimum, brightness)), new ScalarArray(range0.Maximum), Frame);

            for (var i = 1; i < Settings.ColorRanges.Length; i++)
            {
                var range = Settings.ColorRanges[i];
                CvInvoke.InRange(frame, new ScalarArray(AdaptMinimum(range.Minimum, brightness)), new ScalarArray(range.Maximum), tempMat);
                CvInvoke.BitwiseOr(Frame, tempMat, Frame);
            }
        }

        private MCvScalar AdaptMinimum(MCvScalar minimum, double brightness)
        {
            minimum.V2 += brightness;
            return minimum;
        }

        /// <summary>
        /// By design, the camera tracks the glowing sphere that is on top of the controller.
        /// However, the origin of rotation is where the IMU is placed, which is the
        /// point we actually want to track. This method transforms the sphere's position and
        /// finds the position of the origin point at the position of the IMU.
        /// </summary>
        /// <param name="spherePosition">The position of the sphere</param>
        /// <returns>The controller's origin point</returns>
        private Vec3 CalcOrigin(Vec3 spherePosition)
        {
            return spherePosition;
        }
    }
}
