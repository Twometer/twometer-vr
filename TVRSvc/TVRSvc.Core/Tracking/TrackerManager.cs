using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Model;

namespace TVRSvc.Core.Tracking
{
    public class TrackerManager
    {
        public Tracker[] Trackers { get; } = new[]
        {
            new Tracker(0, TrackerSettings.Red),   // Left controller
            new Tracker(1, TrackerSettings.Blue)     // Right controller
        };

        public bool Detected => Trackers.Any(t => t.Detected);

        public void UpdateVideo(Mat frame)
        {
            var hsvFrame = new Mat();
            CvInvoke.CvtColor(frame, hsvFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            foreach (var tracker in Trackers)
                tracker.UpdateVideo(hsvFrame);
        }

        public void UpdateMeta(byte controllerId, float yaw, float pitch, float roll, Button[] pressedButtons)
        {
            if (controllerId > Trackers.Length)
                return;

            var controller = Trackers[controllerId].Controller;
            controller.Rotation = new Math.Vec3(pitch, yaw, roll);


            // Don't process any further if there are no button presses
            if (pressedButtons == null) return;


            var keys = new List<Button>();
            keys.AddRange(controller.Buttons.Keys);

            foreach (var btn in keys)
                if (pressedButtons.Contains(btn))
                    controller.Buttons[btn] = true;
                else
                    controller.Buttons[btn] = false;

            if (!controller.ZOffset.HasValue && pressedButtons.Length > 0)
            {
                controller.ZOffset = controller.Position.Z;
            }
        }

    }
}
