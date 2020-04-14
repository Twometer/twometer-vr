using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            new Tracker(1, TrackerSettings.Blue)   // Right controller
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

            // TODO: Make this into a config file!
            //       Also, make the mapping of XYZ to yaw, pitch, roll for each controller in a config
            //       Because me was stupid and mounted one PCB inverse to the other
            //       To make stuff even more complicated, the MPU has its base axis in a different plane than our coordinate system
            //       So we have to shift around yaw, pitch and roll here to make it work with SteamVR and TVR

            controller.Yaw = -yaw;
            controller.Pitch = roll;
            controller.Roll = pitch;

            if (controller.Id == 1)
                controller.Pitch *= -1;

            var keys = new List<Button>();
            keys.AddRange(controller.Buttons.Keys);

            foreach (var btn in keys)
                if (pressedButtons?.Contains(btn) == true)
                    controller.Buttons[btn] = true;
                else
                    controller.Buttons[btn] = false;

            HandleButtons();
        }


        private DateTime? pressBegin = null;

        private void HandleButtons()
        {
            bool areAllButtonsPressed()
            {
                foreach (var ctrl in Trackers.Select(t => t.Controller))
                    if (!ctrl.Buttons[Button.A])
                        return false;
                return true;
            }

            // If all buttons are pressed...
            var allPressed = areAllButtonsPressed();
            if (!allPressed)
                pressBegin = null;
            else if (!pressBegin.HasValue)
                pressBegin = DateTime.Now;

            // ...for more than 3 seconds...
            if (pressBegin.HasValue && (DateTime.Now - pressBegin.Value).TotalSeconds > 3)
            {
                // ... then recalibrate all controllers
                foreach (var ctrl in Trackers.Select(t => t.Controller))
                {
                    ctrl.ZOffset = ctrl.Position.Z;
                    ctrl.YawOffset = ctrl.Yaw;
                }
            }
        }

    }
}
