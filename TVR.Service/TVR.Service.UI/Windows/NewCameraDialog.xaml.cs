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
            setup.OnStateChanged += Setup_OnStateChanged;

            BeginCaptureLoop();
        }

        private void Setup_OnStateChanged(object sender, CameraSetup.State e)
        {
            switch (e)
            {
                case CameraSetup.State.DetectingCameraParameters:
                    InstructionBox.Text = "Please now switch on the red controller and hold it up from your playing position.";
                    var dialog = new CommonDialog
                    {
                        Title = "TwometerVR Setup Assistant",
                        Caption = "Calibration guide",
                        ContentText = "Please now switch on the red controller and hold it up from your playing position, preferrably in the center and most importantly two meters away from the camera. When you are ready, press OK. A 20-second timer will start after which the calculations will take place. Hold the controller in place until the next dialog box pops up."
                    };
                    dialog.ShowDialog();
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
