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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TVR.Service.Common;
using TVR.Service.Core.IO;

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
                MessageBox.Show("No configurations!");
                // TODO: Show initial configuration dialog
            }
            Start();
        }

        private async void Start()
        {
            ServiceStatusLabel.Content = "Starting...";
            await serviceHost.StartAsync();
            ServiceStatusLabel.Content = "Active";
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void RestartMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ServiceStatusLabel.Content = "Shutting down...";
            await serviceHost.StopAsync();
            Start();
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
