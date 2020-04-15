using Emgu.CV;
using Emgu.CV.Structure;
using TVRSvc.Core.Math;
using TVRSvc.Core.Math.Transform;
using TVRSvc.Core.Model;


namespace TVRSvc.Core.Tracking
{
    public class Tracker
    {
        public Controller Controller { get; }

        public TrackerSettings Settings { get; }

        public bool Detected { get; private set; }

        public bool Visualize { get; set; }

        public Image<Gray, byte> Frame { get; private set; }

        private readonly ICameraTransform transform;

        public Tracker(byte controllerId, TrackerSettings settings, ICameraTransform transform)
        {
            Controller = new Controller(controllerId);
            Settings = settings;
            this.transform = transform;
        }

        public void UpdateVideo(Mat frame)
        {
            if (Frame == null)
                Frame = new Image<Gray, byte>(frame.Width, frame.Height);

            RangeFilter(frame);
            Frame = Frame.Erode(1);
            Frame = Frame.Dilate(1);
            Frame = Frame.ThresholdBinary(new Gray(150), new Gray(255));
            Frame = Frame.SmoothGaussian(9);

            var circles = Frame.HoughCircles(new Gray(Settings.CannyThreshold), new Gray(1), 3, Frame.Height / 4, 2, 50)[0];
            Detected = circles.Length > 0;

            if (Detected)
            {
                var circle = circles[0];
                var spherePos = transform.Transform(Frame.Width, Frame.Height, circle);
                Controller.Position = CalcOrigin(spherePos);

                if (Visualize)
                {
                    Frame.Draw(circle, new Gray(128), 4);
                }
            }
        }

        private void RangeFilter(Mat frame)
        {
            var range0 = Settings.ColorRanges[0];
            CvInvoke.InRange(frame, new ScalarArray(range0.Minimum), new ScalarArray(range0.Maximum), Frame);

            for (var i = 1; i < Settings.ColorRanges.Length; i++)
            {
                var range = Settings.ColorRanges[i];
                var mat = new Mat();
                CvInvoke.InRange(frame, new ScalarArray(range.Minimum), new ScalarArray(range.Maximum), mat);
                CvInvoke.BitwiseOr(Frame, mat, Frame);
            }
        }

        /// <summary>
        /// By design, the camera tracks the glowing sphere that is on top of the controller.
        /// However, the origin of rotation is at the bottom of the controller, which is the
        /// point we actually want to track. This method transforms the sphere's position and
        /// finds the position of the origin point at the bottom of the controller.
        /// </summary>
        /// <param name="spherePosition">The position of the sphere</param>
        /// <returns>The controller's origin point</returns>
        private Vec3 CalcOrigin(Vec3 spherePosition)
        {
            return spherePosition;
        }
    }
}
