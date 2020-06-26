using Emgu.CV;
using Emgu.CV.Structure;
using System.Linq;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Model.Device;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    public class Tracker
    {
        public Image<Gray, byte> Frame { get; private set; }

        public Controller TrackedController { get; }

        public ColorProfile ColorProfile { get; }

        public bool Detected { get; private set; }

        private readonly CameraTransform transform;

        private readonly Mat tempFrame = new Mat();

        public Tracker(byte controllerId, ColorProfile colorProfile, CameraTransform transform)
        {
            TrackedController = new Controller(controllerId);
            ColorProfile = colorProfile;
            this.transform = transform;
        }

        public void UpdateVideo(Mat hsvFrame, double brightness)
        {
            if (Frame == null)
                Frame = new Image<Gray, byte>(hsvFrame.Width, hsvFrame.Height);
            ImageProcessing.ColorFilter(hsvFrame, Frame, tempFrame, ColorProfile, brightness);
            ImageProcessing.Erode(Frame, 1);
            ImageProcessing.SmoothGaussian(Frame, 7);

            var circles = ImageProcessing.HoughCircles(Frame, 80, 1, 3, Frame.Width / 2, 3, 80);
            Detected = circles.Length > 0;

            if (!Detected)
                return;

            Frame.Draw(circles[0], new Gray(128), 4);

            TrackedController.Position = transform.Transform(Frame.Width, Frame.Height, circles[0]);
        }

        public void UpdateMeta(float qx, float qy, float qz, float qw, Button[] pressedButtons)
        {
            TrackedController.Rotation = new Quaternion(-qy, qz, qx, qw);

            foreach (var btn in TrackedController.Buttons.Keys)
                TrackedController.Buttons[btn] = pressedButtons?.Contains(btn) == true;
        }

    }
}
