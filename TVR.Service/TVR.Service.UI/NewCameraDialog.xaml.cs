using DirectShowLib;
using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TVR.Service.Core.Model.Camera;
using TVR.Service.Core.Video;

namespace TVR.Service.UI
{
    /// <summary>
    /// Interaction logic for NewCameraDialog.xaml
    /// </summary>
    public partial class NewCameraDialog : Window
    {
        private ImageBox imageBox;
        private VideoCapture videoCapture;
        public CameraProfile CameraProfile { get; } = new CameraProfile();

        public NewCameraDialog()
        {
            InitializeComponent();

            var cameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            foreach (var cam in cameras)
            {
                var camItem = new ComboBoxItem
                {
                    Tag = cam,
                    Content = cam.Name
                };
                CameraComboBox.Items.Add(camItem);
            }

            imageBox = new ImageBox();
            VideoFeedHost.Child = imageBox;
            imageBox.ContextMenuStrip = null;
        }

        private void BeginSetupButton_Click(object sender, RoutedEventArgs e)
        {
            if (CameraComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select your webcam from the dropdown.");
                return;
            }

            BeginSetupButton.IsEnabled = false;
            CameraComboBox.IsEnabled = false;

            InstructionBox.Text = "Please wait while the setup assistant analyzes basic parameters of your camera...";

            var device = (CameraComboBox.SelectedItem as ComboBoxItem).Tag as DsDevice;
            CameraProfile.CameraParameters = new CameraParameters();
            CameraProfile.CalibrationParameters = new CalibrationParameters();
            CameraProfile.Identifier = BuildIdentifier(device);
            CameraProfile.Model = device.Name;

            videoCapture = new VideoCapture(CameraComboBox.SelectedIndex, VideoCapture.API.DShow);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.AutoExposure, 0);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, 0);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
            videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, 720);

            CameraProfile.CameraParameters.FrameWidth = (int)videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            CameraProfile.CameraParameters.FrameHeight = (int)videoCapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);

            BeginCaptureLoop();
        }

        private int warmupFrames = 0;
        private int cooldownFrames = 0;
        private int exposure = 0;
        private Mat hsvFrame = new Mat();
        private double prevBrightness;
        private int stableCounter = 0;

        private void HandleFrame(Mat frame)
        {
            // Process frame            
            ImageProcessing.BgrToHsv(frame, hsvFrame);
            double frameBrightness = ImageProcessing.GetBrightness(hsvFrame);

            if (CameraProfile.CalibrationParameters.WarmupFrames == 0)
            {
                warmupFrames++;
                if (CheckStability(frameBrightness, 30))
                {
                    CameraProfile.CalibrationParameters.WarmupFrames = warmupFrames - 20;
                    CameraProfile.CalibrationParameters.StableFrames = Math.Min((warmupFrames - 30) / 2 + 1, 4);
                }
            }
            else if (CameraProfile.CalibrationParameters.CooldownFrames == 0)
            {
                if (cooldownFrames == 0)
                {
                    videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, -10);
                }
                cooldownFrames++;

                if (CheckStability(frameBrightness, CameraProfile.CalibrationParameters.StableFrames))
                {
                    CameraProfile.CalibrationParameters.CooldownFrames = cooldownFrames;
                }
            }
            else if (CameraProfile.CalibrationParameters.BrightnessThreshold == 0)
            {
                videoCapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, exposure);
                exposure--;
                if ((CheckStability(frameBrightness, CameraProfile.CalibrationParameters.StableFrames) && frameBrightness < 50) || exposure < -30)
                {
                    CameraProfile.CalibrationParameters.BrightnessThreshold = frameBrightness + 5.0f; // Offset for some error room
                    OnCalibParametersDetected();
                }
            }

            // Update live feed
            imageBox.Image = frame;

            prevBrightness = frameBrightness;
        }

        private void OnCalibParametersDetected()
        {
            InstructionBox.Text = "Please now switch on the red controller and hold it up from your playing position.";
            var dialog = new CommonDialog
            {
                Title = "TwometerVR Setup Assistant",
                Caption = "Calibration completed!",
                ContentText = "Please now follow the instructions on the bottom left (your instruction panel) to continue the setup!"
            };
            dialog.ShowDialog();
        }

        private bool CheckStability(double frameBrightness, int th)
        {
            if (Math.Abs(frameBrightness - prevBrightness) < 2.0)
                stableCounter++;
            else stableCounter = 0;
            return stableCounter > th;
        }

        private async void BeginCaptureLoop()
        {
            Mat frame = new Mat();
            while (IsLoaded)
            {
                var grabbed = await Task.Run(() =>
                {
                    if (videoCapture.Grab())
                    {
                        videoCapture.Read(frame);
                        return true;
                    }
                    return false;
                });
                if (grabbed)
                {
                    HandleFrame(frame);
                }
                await Task.Delay(16); // up to 60fps
            }
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
    }
}
