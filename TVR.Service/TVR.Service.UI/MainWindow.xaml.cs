using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TVR.Service.Common;
using TVR.Service.Core.IO;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Config;

namespace TVR.Service.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceHost serviceHost = new ServiceHost();

        public MainWindow()
        {
            InitializeComponent();

            if (FileManager.Instance.IsFirstStart)
            {
                var welcomeWindow = new WelcomeWindow();
                var newCameraDialog = new NewCameraDialog();
                if (welcomeWindow.ShowDialog() == true && newCameraDialog.ShowDialog() == true)
                {
                    var defaultUserConfig = new UserConfig
                    {
                        CameraInfo = new CameraInfo() { Index = 0, ProfileName = newCameraDialog.CameraProfile.Identifier },
                        Offset = Vector3.Zero,
                        InputConfig = new InputConfig() { Latency = 2, PoseResetDelay = 0.2, UpdateRate = 120 },
                        HardwareConfig = new HardwareConfig() { SphereDistance = 0.055, SphereSize = 0.04 }
                    };
                    ConfigIO.WriteUserConfig(defaultUserConfig);
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            try
            {
                serviceHost.Start();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Cannot find config files!");
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to start the service!\n" + e.ToString());
                Environment.Exit(1);
            }

            ServiceStatusLabel.Content = "Active";
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void RestartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ServiceStatusLabel.Content = "Shutting down...";
            await serviceHost.StopAsync();
            ServiceStatusLabel.Content = "Starting...";
            await serviceHost.StartAsync();
            ServiceStatusLabel.Content = "Active";
        }

        private void RecalibrateMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GitHubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Twometer/twometer-vr");
        }

        private void WikiMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Twometer/twometer-vr/wiki");
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Product: TwometerVR Service\nVersion: 2.0");
        }
    }
}
