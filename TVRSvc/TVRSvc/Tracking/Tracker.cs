using Emgu.CV;
using Emgu.CV.Structure;
using TVRSvc.Math.Transform;
using TVRSvc.Model;

namespace TVRSvc.Tracking
{
    public class Tracker
    {
        public Controller Controller { get; }

        public TrackerSettings Settings { get; }

        public bool Detected { get; private set; }

        public Image<Gray, byte> Frame { get; private set; }

        private readonly ICameraTransform transform;

        public Tracker(int controllerId, TrackerSettings settings)
        {
            Controller = new Controller(controllerId);
            Settings = settings;
            transform = new SimpleCameraTransform();
        }

        public void UpdateVideo(Mat frame)
        {
            if (Frame == null)
                Frame = new Image<Gray, byte>(frame.Width, frame.Height);

            CvInvoke.InRange(frame, new ScalarArray(Settings.Minimum), new ScalarArray(Settings.Maximum), Frame);
            Frame = Frame.SmoothGaussian(9);

            var circles = Frame.HoughCircles(new Gray(Settings.CannyThreshold), new Gray(1), 2, Frame.Height / 4, 8, 500)[0];
            Detected = circles.Length > 0;

            if (Detected)
            {
                var circle = circles[0];
                Controller.Position = transform.Transform(Frame.Width, Frame.Height, circle);
            }
        }


    }
}
