using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Model;

namespace TVR.Service.Debug
{
    internal class CameraSetup
    {
        public int TimerSeconds { get; private set; } = 30;

        public event EventHandler<StatusMessage> StatusMessageReceived;

        // Setup state
        private CameraDetectionState currentDetectionState = CameraDetectionState.CircleDiameter;
        private bool running = false;

        // Depth calibration
        private double totalCircleDia;
        private double circleDiaSamples;
        private double perceivedCircleSize;

        // Horizontal calibration
        private double circle_x0;
        private int circle0time;
        private double circle_maxX;

        // Output
        public double PFocalLength { get; private set; }
        public double PixelsPerMeter { get; private set; }

        private enum CameraDetectionState
        {
            CircleDiameter,
            AwaitingZeroPosition,
            PixelsPerMeter
        }

        public enum StatusMessage
        {
            CalibrationParametersDetected,
            CountdownChanged,
            BeginSamplingCircleDiameter,
            BeginSamplingPixelsPerMeter,
            AwaitingZeroPosition,
            Completed
        }

        public async void BeginDepthSamplingCountdown()
        {
            while (TimerSeconds > 0)
            {
                TimerSeconds--;
                SendStatusMessage(StatusMessage.CountdownChanged);
                await Task.Delay(1000);
            }

            // After the timer is over, start detection!
            running = true;
            SendStatusMessage(StatusMessage.BeginSamplingCircleDiameter);
        }

        public void Update(Tracker tracker)
        {
            if (!running)
                return;

            var circle = tracker.Circle;

            switch (currentDetectionState)
            {
                case CameraDetectionState.CircleDiameter:
                    totalCircleDia += Math.Floor(circle.Radius * 2);
                    circleDiaSamples++;

                    if (circleDiaSamples > 10)
                    {
                        perceivedCircleSize = totalCircleDia / circleDiaSamples;
                        PFocalLength = (perceivedCircleSize * 1) / 0.04; // TODO: Don't hardcode real-world sphere size of 4cm
                        SendStatusMessage(StatusMessage.AwaitingZeroPosition);
                        currentDetectionState = CameraDetectionState.AwaitingZeroPosition;
                    }

                    break;
                case CameraDetectionState.AwaitingZeroPosition:
                    if (circle.Center.X < perceivedCircleSize)
                    {
                        circle0time++;
                        circle_x0 += circle.Center.X;
                    }
                    else
                    {
                        circle_x0 = 0;
                        circle0time = 0;
                    }

                    if (circle0time > 5)
                    {
                        circle_x0 /= circle0time;
                        SendStatusMessage(StatusMessage.BeginSamplingPixelsPerMeter);
                        currentDetectionState = CameraDetectionState.PixelsPerMeter;
                    }

                    break;

                case CameraDetectionState.PixelsPerMeter:
                    double xOffset = circle.Center.X - circle_x0;
                    if (xOffset > circle_maxX)
                    {
                        circle_maxX = xOffset;
                    }
                    else if (xOffset > 0 && circle_maxX - xOffset > Math.Max(50, circle_maxX / 2))
                    {
                        double pxm = circle_maxX - circle_x0;
                        PixelsPerMeter = pxm;

                        SendStatusMessage(StatusMessage.Completed);
                    }
                    break;
            }
        }

        private void SendStatusMessage(StatusMessage statusMessage)
        {
            StatusMessageReceived?.Invoke(this, statusMessage);
        }
    }
}
