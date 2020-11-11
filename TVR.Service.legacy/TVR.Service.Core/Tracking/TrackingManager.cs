using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Model.Config;
using TVR.Service.Core.Model.Device;

namespace TVR.Service.Core.Tracking
{
    public class TrackingManager
    {
        public Tracker[] Trackers { get; }

        private UserConfig config;

        private DateTime? pressBegin = null;
        private bool poseResetExecuted = false;

        public TrackingManager(UserConfig config)
        {
            this.config = config;

            var colorProfiles = config.CameraInfo.Profile.ColorProfiles;
            Trackers = new Tracker[colorProfiles.Length];
            InitTrackers(colorProfiles);
        }

        public Task UpdateVideo(Mat hsvFrame, double meanBrightness)
        {
            var tasks = new List<Task>();
            foreach (var tracker in Trackers)
                tasks.Add(Task.Run(() => { tracker.UpdateVideo(hsvFrame, meanBrightness); }));
            return Task.WhenAll(tasks.ToArray());
        }

        public void UpdateMeta(byte controllerId, float qx, float qy, float qz, float qw, Button[] pressedButtons)
        {
            if (controllerId > Trackers.Length)
                return;
            Trackers[controllerId].UpdateMeta(qx, qy, qz, qw, pressedButtons);
            HandleButtons();
        }

        private void InitTrackers(ColorProfile[] colorProfiles)
        {
            for (byte i = 0; i < colorProfiles.Length; i++)
            {
                var transform = new CameraTransform(config.Offset, config.CameraInfo.Profile.CameraParameters, config.HardwareConfig, config.InputConfig.Latency);
                Trackers[i] = new Tracker(i, colorProfiles[i], transform);
            }
        }

        private void HandleButtons()
        {
            bool areAllButtonsPressed()
            {
                foreach (var ctrl in Trackers.Select(t => t.TrackedController))
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
            if (pressBegin.HasValue && !poseResetExecuted && (DateTime.Now - pressBegin.Value).TotalSeconds > config.InputConfig.PoseResetDelay)
            {
                // ... then reset pose for all controllers
                foreach (var ctrl in Trackers.Select(t => t.TrackedController))
                {
                    ctrl.RotationOffset = ctrl.Rotation.Invert();
                }
                poseResetExecuted = true;
                LoggerFactory.Current.Log(LogLevel.Info, "Pose reset complete");
            }
        }
    }
}
