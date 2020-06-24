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
using TVR.Service.UI.Camera;

namespace TVR.Service.UI
{
    /// <summary>
    /// Interaction logic for NewCameraDialog.xaml
    /// </summary>
    public partial class NewCameraDialog : Window
    {
        public CameraProfile CameraProfile => setup.CameraProfile;

        private readonly ImageBox imageBox;
        private CameraSetup setup;

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
                MessageBox.Show("Please select your webcam from the dropdown.", "Invalid selection", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            BeginSetupButton.IsEnabled = false;
            CameraComboBox.IsEnabled = false;

            InstructionBox.Text = "Please wait while the setup assistant analyzes basic parameters of your camera...";

            var device = (CameraComboBox.SelectedItem as ComboBoxItem).Tag as DsDevice;
            setup = new CameraSetup(device, CameraComboBox.SelectedIndex);
            setup.StatusMessageReceived += Setup_StatusMessageReceived;

            BeginCaptureLoop();
        }

        private void Setup_StatusMessageReceived(object sender, CameraSetup.StatusMessage e)
        {
            switch (e)
            {
                case CameraSetup.StatusMessage.CalibrationParametersDetected:
                    InstructionBox.Text = "Please now switch on the red controller and hold it up from your playing position.";
                    var dialog = new CommonDialog
                    {
                        Title = "TwometerVR Setup Assistant",
                        Caption = "Calibration guide",
                        ContentText = "Please now switch on the red controller and hold it up from your playing position, preferrably in the center and most importantly a meter away from the camera.\n\nWhen you are ready, press OK. A 30-second timer will then start after which the calculations will take place.\n\nMore instructions will be shown in the bottom left corner!"
                    };
                    dialog.ShowDialog();
                    setup.BeginCamParamsStep1Countdown();
                    break;
                case CameraSetup.StatusMessage.CountdownChanged:
                    InstructionBox.Text = $"Sampling of the depth will begin in {setup.TimerSeconds} seconds...";
                    break;
                case CameraSetup.StatusMessage.BeginSamplingCircleDiameter:
                    InstructionBox.Text = $"Sampling! Please do not move the controller!";
                    break;
                case CameraSetup.StatusMessage.AwaitingZeroPosition:
                    InstructionBox.Text = "Complete. Now please stay at 2 meter distance from the camera and move all the way to the left side of the frame!";
                    break;
                case CameraSetup.StatusMessage.BeginSamplingPixelsPerMeter:
                    InstructionBox.Text = "Horizontal calibration starting. Please move two meter to the right and back again!";
                    break;
                case CameraSetup.StatusMessage.Completed:
                    InstructionBox.Text = "Setup completed successfully!";
                    var success = new CommonDialog
                    {
                        Title = "TwometerVR Setup Assistant",
                        Caption = "Congratulations",
                        ContentText = "Your camera is successfully calibrated! Now you can enjoy your favorite VR games :3"
                    };
                    success.ShowDialog();
                    DialogResult = true;
                    break;
            }
        }

        private void HandleFrame(Mat frame)
        {
            imageBox.Image = setup.HandleFrame(frame);    
        }

        private async void BeginCaptureLoop()
        {
            Mat frame = new Mat();
            while (IsLoaded)
            {
                var grabbed = await Task.Run(() =>
                {
                    if (setup.VideoCapture.Grab())
                    {
                        setup.VideoCapture.Read(frame);
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


    }
}
