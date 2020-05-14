﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Config;
using TVR.Service.Core.Math.Transform;
using TVR.Service.Core.Model;


namespace TVR.Service.Core.Tracking
{
    public class TrackerManager
    {
        public Tracker[] Trackers { get; }

        public bool Detected => Trackers.Any(t => t.Detected);

        private readonly TVRConfig config;

        private Mat hsvFrame = new Mat();

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

        public void UpdateVideo(Mat frame)
        {
            CvInvoke.CvtColor(frame, hsvFrame, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);

            foreach (var tracker in Trackers)
                tracker.UpdateVideo(hsvFrame);
        }

        public void UpdateMeta(byte controllerId, float yaw, float pitch, float roll, Button[] pressedButtons)
        {
            if (controllerId > Trackers.Length)
                return;

            var controller = Trackers[controllerId].Controller;

            // To make stuff complicated, The MPU has its base axis in a different plane than our coordinate system
            // So we have to shift around yaw, pitch and roll here to make it work with SteamVR and TVR
            controller.Yaw = yaw;
            controller.Pitch = roll;
            controller.Roll = pitch;

            if (config.Tracker.LeftInvertPitch && controller.Id == 0)
                controller.Pitch *= -1;
            if (config.Tracker.RightInvertPitch && controller.Id == 1)
                controller.Pitch *= -1;

            foreach (var btn in controller.Buttons.Keys)
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

            // ...for more than X seconds...
            if (pressBegin.HasValue && (DateTime.Now - pressBegin.Value).TotalSeconds > config.Tracker.PoseResetDelay)
            {
                // ... then reset pose for all controllers
                foreach (var ctrl in Trackers.Select(t => t.Controller))
                {
                    ctrl.YawOffset = ctrl.Yaw;
                }
            }
        }

    }
}
