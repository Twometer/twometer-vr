using Emgu.CV;
using System;
using System.Linq;
using TVR.Service.Core.Config;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Math;
using TVR.Service.Core.Math.Transform;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Tracking
{
    public class TrackerManager
    {
        public Tracker[] Trackers { get; }

        public bool Detected => Trackers.Any(t => t.Detected);

        private readonly TVRConfig config;

        public TrackerManager(TVRConfig config)
        {
            this.config = config;
            Trackers = new[]
            {
                new Tracker(0, TrackerSettings.Red, new SimpleCameraTransform(config)),   // Left controller
                new Tracker(1, TrackerSettings.Blue, new SimpleCameraTransform(config))   // Right controller
            };
            foreach (var t in Trackers)
                t.Controller.ZOffset = config.Tracker.ZOffset;
        }

        public void UpdateVideo(Mat hsvFrame, double meanBrightness)
        {
            foreach (var tracker in Trackers)
                tracker.UpdateVideo(hsvFrame, meanBrightness);
        }

        public void UpdateMeta(byte controllerId, float qx, float qy, float qz, float qw, Button[] pressedButtons)
        {
            if (controllerId > Trackers.Length)
                return;

            var controller = Trackers[controllerId].Controller;

            // To make stuff complicated, The MPU has its base axis in a different plane than our coordinate system
            // So we have to shift around the quaternion here to make it work with SteamVR
            controller.Rotation = new Quaternion(-qy, qz, qx, qw);

            foreach (var btn in controller.Buttons.Keys)
                controller.Buttons[btn] = pressedButtons?.Contains(btn) == true;

            HandleButtons();
        }


        private DateTime? pressBegin = null;
        private bool poseResetExecuted = false;

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
            {
                poseResetExecuted = false;
                pressBegin = null;
            }
            else if (!pressBegin.HasValue)
                pressBegin = DateTime.Now;

            // ...for more than X seconds...
            if (pressBegin.HasValue && !poseResetExecuted && (DateTime.Now - pressBegin.Value).TotalSeconds > config.Tracker.PoseResetDelay)
            {
                // ... then reset pose for all controllers
                foreach (var ctrl in Trackers.Select(t => t.Controller))
                {
                    ctrl.RotationOffset = ctrl.Rotation.Invert();
                }
                poseResetExecuted = true;
                LoggerFactory.Current.Log(LogLevel.Info, "Pose reset complete");
            }
        }

    }
}
