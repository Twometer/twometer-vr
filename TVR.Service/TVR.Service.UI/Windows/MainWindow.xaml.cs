using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TVR.Service.Common;
using TVR.Service.Core;
using TVR.Service.UI.Setup;
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

            FirstStartup.CheckFirstStart();

            try
            {
                serviceHost.Start();
            }
            catch (FileNotFoundException)
            {
                CommonDialog.ShowError(this, "Failed to start", "Cannot find configuration files! Make sure that the config store was not corrupted or alternatively, reconfigure the service.");
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                CommonDialog.ShowError(this, "Failed to start", e.ToString());
                Environment.Exit(1);
            }
        }

        private async void StartUpdateLoop()
        {
            while (IsLoaded)
            {
                ServiceStatusLabel.Content = PrettifyHostState(serviceHost.State);
                CurrentCamLabel.Content = $"Camera: {serviceHost.Services.Config.CameraInfo.Profile.Model}";
                ConnectedControllersLabel.Content = $"Controllers: {serviceHost.Services.ControllerServer.ConnectedClientCount}";
                ConnectedDriversLabel.Content = $"SteamVR Clients: {serviceHost.Services.DriverServer.ConnectedClientCount}";
                await Task.Delay(250);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartUpdateLoop();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("If you close the service, TwometerVR controllers will stop working. Continue?", "Close service?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                serviceHost.Stop();
            else
                e.Cancel = true;
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

        private void ConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var conf = new ConfigDialog(serviceHost.Services.Config) { Owner = this };
            conf.ShowDialog();
            if (conf.RequiresRestart)
            {
                // TODO Correctly restart service
            }
        }

        private void DebugMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new DebugWindow(serviceHost).Show();
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
            var aboutMessage = $"Product: TwometerVR Service\n\nFrontend Version: {Assembly.GetExecutingAssembly().GetName().Version}\nCore Version: {TVRCore.Version}\n\nmade by Twometer\nReleased under the MIT license";
            CommonDialog.Show(this, "About TwometerVR", "About", aboutMessage);
        }

        private string PrettifyHostState(HostState state)
        {
            switch (state) {
                case HostState.Starting:
                    return "Initializing...";
                case HostState.Active:
                    return "Service online";
                case HostState.Stopping:
                    return "Shutting down...";
                case HostState.Inactive:
                    return "Service offline";
            }
            throw new ArgumentException("Unknown host state" + state);
        }
    }
}
