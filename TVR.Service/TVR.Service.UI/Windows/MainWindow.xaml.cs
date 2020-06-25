using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TVR.Service.Common;
using TVR.Service.Core;
using TVR.Service.Core.IO;
using TVR.Service.Core.Math;
using TVR.Service.Core.Model.Config;
using TVR.Service.UI.Windows;

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
                    FileManager.Instance.ProfilesFolder.Create();
                    CameraProfileIO.WriteCameraProfile(newCameraDialog.CameraProfile);
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
                var dialog = new CommonDialog
                {
                    Title = "TwometerVR Error",
                    Caption = "Failed to start",
                    ContentText = "Cannot find configuration files! Make sure that the config store was not corrupted or alternatively, reconfigure the service."
                };
                dialog.ShowDialog();
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                var dialog = new CommonDialog
                {
                    Title = "TwometerVR Error",
                    Caption = "Failed to start",
                    ContentText = e.ToString()
                };
                dialog.ShowDialog();
                Environment.Exit(1);
            }
        }

        private async void StartUpdateLoop()
        {
            while (IsLoaded)
            {
                ServiceStatusLabel.Content = serviceHost.State.ToString();
                CurrentCamLabel.Content = $"Camera: {serviceHost.Services.Config.CameraInfo.Profile.Model}";

                await Task.Delay(250);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void RestartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            await serviceHost.StopAsync();
            await serviceHost.StartAsync();
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
            var dialog = new CommonDialog
            {
                Title = "About TwometerVR",
                Caption = "About",
                ContentText = $"Product: TwometerVR Service\n\nFrontend Version: {Assembly.GetExecutingAssembly().GetName().Version}\nCore Version: {TVRCore.Version}\n\nmade by Twometer\nReleased under the MIT license"
            };
            dialog.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Really close TwometerVR Service?", "Close service?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                serviceHost.Stop();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void DebugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new DebugWindow(serviceHost).Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartUpdateLoop();
        }
    }
}
