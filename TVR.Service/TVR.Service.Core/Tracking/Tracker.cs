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
            ColorFilter(hsvFrame, brightness);
            ImageProcessing.SmoothGaussian(frame, 7);

            var circles = ImageProcessing.HoughCircles(frame, 125, 1, 3, frame.Width / 2, 3, 75);
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
        

        private void ColorFilter(Mat hsvFrame, double brightness)
        {
            var range0 = ColorProfile.ColorRanges[0];
            CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range0.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range0.Maximum.ToMCvScalar()), frame);

            for (var i = 1; i < ColorProfile.ColorRanges.Length; i++)
            {
                var range = ColorProfile.ColorRanges[i];
                CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range.Maximum.ToMCvScalar()), temp);
                CvInvoke.BitwiseOr(frame, temp, frame);
            }
        }

        private MCvScalar AdaptMinimum(MCvScalar minimum, double brightness)
        {
            minimum.V2 += brightness;
            return minimum;
        }
    }
}
