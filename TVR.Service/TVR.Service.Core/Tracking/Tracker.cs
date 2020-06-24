using Emgu.CV;
using Emgu.CV.Structure;
using System.Linq;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Model.Config;
using TVR.Service.Core.Model.Device;
using TVR.Service.Core.Video;

namespace TVR.Service.Core.Tracking
{
    public class Tracker
    {
        public Controller TrackedController { get; }

        public ColorProfile ColorProfile { get; }

        public bool Detected { get; private set; }

        private Image<Gray, byte> frame;

        private readonly CameraTransform transform;

        private readonly Mat temp = new Mat();

        public Tracker(byte controllerId, ColorProfile colorProfile, CameraTransform transform)
        {
            TrackedController = new Controller(controllerId);
            ColorProfile = colorProfile;
            this.transform = transform;
        }

        public void UpdateVideo(Mat hsvFrame, double brightness)
        {
            if (frame == null)
                frame = new Image<Gray, byte>(hsvFrame.Width, hsvFrame.Height);
            ImageProcessing.ColorFilter(hsvFrame, frame, temp, ColorProfile, brightness);
            ImageProcessing.SmoothMedian(frame, 3);

            var circles = ImageProcessing.HoughCircles(frame, 85, 1, 3, frame.Width / 2, 3, 80);
            Detected = circles.Length > 0;

            if (!Detected)
                return;

            TrackedController.Position = transform.Transform(frame.Width, frame.Height, circles[0]);
        }

        public void UpdateMeta(float qx, float qy, float qz, float qw, Button[] pressedButtons)
        {
            TrackedController.Rotation = new Quaternion(-qy, qz, qx, qw);

            foreach (var btn in TrackedController.Buttons.Keys)
                TrackedController.Buttons[btn] = pressedButtons?.Contains(btn) == true;
        }

    }
}
