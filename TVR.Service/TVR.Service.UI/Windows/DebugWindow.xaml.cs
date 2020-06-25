using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TVR.Service.Common;

namespace TVR.Service.UI.Windows
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow : Window
    {
        private readonly ServiceHost host;
        private readonly ImageBox imageBox;

        public DebugWindow(ServiceHost host)
        {
            InitializeComponent();
            this.host = host;
            this.imageBox = new ImageBox();
            VideoFeedHost.Child = imageBox;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartVideoFeed();
        }

        private async void StartVideoFeed()
        {
            while (IsLoaded)
            {
                if (RadioButtonRaw.IsChecked == true)
                    imageBox.Image = host.Services.Camera.Frame;
                else if (RadioButtonLeft.IsChecked == true)
                    imageBox.Image = host.Services.TrackingManager.Trackers[0].Frame;
                else if (RadioButtonRight.IsChecked == true)
                    imageBox.Image = host.Services.TrackingManager.Trackers[1].Frame;

                LeftInfoBlock.Text = host.Services.TrackingManager.Trackers[0].TrackedController.Position.ToString();
                RightInfoBlock.Text = host.Services.TrackingManager.Trackers[1].TrackedController.Position.ToString();

                ExposureLabel.Text = $"Exposure: {host.Services.Camera.Exposure}";
                BrightnessLabel.Text = $"Brightness: {host.Services.Camera.FrameBrightness:0.###}";

                await Task.Delay(10);
            }
        }
    }
}
