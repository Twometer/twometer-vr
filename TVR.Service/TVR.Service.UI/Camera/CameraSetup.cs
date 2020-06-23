using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Video;

namespace TVR.Service.UI.Camera
{
    public class CameraSetup
    {
        public CameraProfile CameraProfile { get; } = new CameraProfile();

        public VideoCapture VideoCapture { get; private set; }

        public event EventHandler<State> OnStateChanged;

        private DsDevice device;

        // Left is red
        // Right is blue

        private State currentState = State.DetectingCalibrationParameters;
        private int warmupFrames = 0;
        private int cooldownFrames = 0;
        private int exposure = 0;
        private Mat hsvFrame = new Mat();
        private Mat tempFrame = new Mat();
        private Image<Gray, byte> filteredImage;
        private double prevBrightness;
        private int stableCounter = 0;

        public enum State
        {
            DetectingCalibrationParameters,
            DetectingCameraParameters
        }

        public CameraSetup(DsDevice device, int deviceIndex)
        {
            this.device = device;
            InitCapture(deviceIndex);
        }

        private void InitCapture(int deviceIndex)
        {
            CameraProfile.CameraParameters = new CameraParameters();
            CameraProfile.CalibrationParameters = new CalibrationParameters();
            CameraProfile.Identifier = BuildIdentifier(device);
            CameraProfile.Model = device.Name;

            VideoCapture = new VideoCapture(deviceIndex, VideoCapture.API.DShow);
            VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.AutoExposure, 0);
            VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, 0);
            VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
            VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);

            CameraProfile.CameraParameters.FrameWidth = (int)VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            CameraProfile.CameraParameters.FrameHeight = (int)VideoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);

            filteredImage = new Image<Gray, byte>(CameraProfile.CameraParameters.FrameWidth, CameraProfile.CameraParameters.FrameHeight);
        }

        public IInputArray HandleFrame(Mat frame)
        {
            // Process frame            
            ImageProcessing.BgrToHsv(frame, hsvFrame);
            double frameBrightness = ImageProcessing.GetBrightness(hsvFrame);

            switch (currentState)
            {

                case State.DetectingCalibrationParameters:
                    HandleDetectCalibrationParameters(frameBrightness);
                    return frame;
                case State.DetectingCameraParameters:
                    HandleDetectCameraParameters(frameBrightness);
                    return filteredImage;
            }

            return frame;
        }

        private void HandleDetectCameraParameters(double frameBrightness)
        {
            ImageProcessing.ColorFilter(hsvFrame, filteredImage, tempFrame, CameraProfile.ColorProfiles[0], frameBrightness);
            ImageProcessing.SmoothGaussian(filteredImage, 7);
            var circles = ImageProcessing.HoughCircles(filteredImage, 125, 1, 3, filteredImage.Width / 2, 3, 75);
            if (circles.Length == 0)
                return;
            var circle = circles[0];
            /*
             * This has to explain the following steps to the user while calculating the camera parameters:
             * 
             *  1. Place tracker sphere 2 meters away from the camera
             *  2. Calculate Parameters.FocalLength = (circleDiameter * 2m) / 0.04m
             *  3. Stay at 2 meters away from the camera, go to left side of frame with the ball and move 2 meters to the right
             *     and back multiple times
             *      
             *      Code: Correct the XY coordinates for distance
             *            Take the maximums of each run, average those together
             *            Divide the pixel difference from left to right by two (because 2 meters)
             *            That's the px/m
             *            Save that.
             *            Be happy.
             */
            
        }

        private void HandleDetectCalibrationParameters(double frameBrightness)
        {
            if (CameraProfile.CalibrationParameters.WarmupFrames == 0)
            {
                warmupFrames++;
                LoggerFactory.Current.Log(LogLevel.Debug, "Warming up... " + warmupFrames + "; " + frameBrightness);
                if (CheckStability(frameBrightness, 30, 10.0))
                {
                    CameraProfile.CalibrationParameters.WarmupFrames = warmupFrames - 20;
                    CameraProfile.CalibrationParameters.StableFrames = Math.Min((warmupFrames - 30) / 2 + 1, 4);
                    LoggerFactory.Current.Log(LogLevel.Debug, "Warmup complete! " + CameraProfile.CalibrationParameters.WarmupFrames + "; " + CameraProfile.CalibrationParameters.StableFrames);
                }
            }
            else if (CameraProfile.CalibrationParameters.CooldownFrames == 0)
            {
                if (cooldownFrames == 0)
                {
                    VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, -10);
                }
                cooldownFrames++;

                if (CheckStability(frameBrightness, CameraProfile.CalibrationParameters.StableFrames, 2.0))
                {
                    CameraProfile.CalibrationParameters.CooldownFrames = cooldownFrames;
                }
            }
            else if (CameraProfile.CalibrationParameters.BrightnessThreshold == 0)
            {
                VideoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, exposure);
                exposure--;
                if ((CheckStability(frameBrightness, CameraProfile.CalibrationParameters.StableFrames, 2.0) && frameBrightness < 20) || exposure < -30)
                {
                    CameraProfile.CalibrationParameters.BrightnessThreshold = frameBrightness + 5.0f; // Offset for some error room
                    OnCalibParametersDetected();
                }
            }
            prevBrightness = frameBrightness;
        }


        private void OnCalibParametersDetected()
        {
            // TODO: Currently loading default color profiles, add an auto-detect for this
            CameraProfile.ColorProfiles = new ColorProfile[2];
            CameraProfile.ColorProfiles[0] = new ColorProfile()
            {
                ColorRanges = new ColorRange[]
                {
                    new ColorRange() { Minimum = new HSVColor(0, 76, 66), Maximum = new HSVColor(70, 255, 255) },
                    new ColorRange() { Minimum = new HSVColor(151, 76, 66), Maximum = new HSVColor(179, 255, 255) }
                }
            };
            CameraProfile.ColorProfiles[1] = new ColorProfile()
            {
                ColorRanges = new ColorRange[]
                {
                    new ColorRange() { Minimum = new HSVColor(58, 125, 110), Maximum = new HSVColor(137, 255, 255) }
                }
            };

            UpdateState(State.DetectingCameraParameters);
        }

        private bool CheckStability(double frameBrightness, int stability, double threshold)
        {
            if (Math.Abs(frameBrightness - prevBrightness) < threshold)
                stableCounter++;
            else stableCounter = 0;
            return stableCounter > stability;
        }


        private string BuildIdentifier(DsDevice device)
        {
            var builder = new StringBuilder();
            foreach (char chr in device.Name)
            {
                if (char.IsLetterOrDigit(chr))
                    builder.Append(char.ToLower(chr));
                else if (char.IsWhiteSpace(chr))
                    builder.Append('-');
            }
            return builder.ToString();
        }

        private void UpdateState(State newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(this, newState);
        }
    }
}
