using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
        private readonly Mat hierarchy = new Mat();

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

            using (var contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(Frame, contours, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);
                if (contours.Size > 0)
                {
                    var circle = CvInvoke.MinEnclosingCircle(contours[0]);
                    Frame.Draw(circle, new Gray(100), 2);
                    TrackedController.Position = transform.Transform(Frame.Width, Frame.Height, circle);
                }
            }
        }

        public void UpdateMeta(float qx, float qy, float qz, float qw, Button[] pressedButtons)
        {
            TrackedController.Rotation = new Quaternion(qx, qy, qz, qw);
            foreach (var btn in TrackedController.Buttons.Keys)
                TrackedController.Buttons[btn] = pressedButtons?.Contains(btn) == true;
        }

    }
}
