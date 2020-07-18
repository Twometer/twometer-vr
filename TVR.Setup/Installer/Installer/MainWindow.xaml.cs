using IWshRuntimeLibrary;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ReleaseJson = "https://api.github.com/repos/Twometer/twometer-vr/releases/latest";
        private const string RuntimeName = "tvr-runtime.zip";

        public MainWindow()
        {
            InitializeComponent();
            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TwometerVR")))
            {
                EnterUninstallMode();
            }
            else
            {
                EnterInstallMode();
            }
        }

        private void EnterUninstallMode()
        {
            if (MessageBox.Show("Uninstall TwometerVR from your system?", Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BeginUninstallation();
            }
            else
            {
                Close();
            }
        }

        private void EnterInstallMode()
        {
            if (MessageBox.Show("Install TwometerVR on your system?", Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BeginInstallation();
            }
            else
            {
                Close();
            }
        }

        private async void BeginUninstallation()
        {
            StatusLabel.Content = "Removing...";
            var installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TwometerVR");
            var driverUninstallScript = Path.Combine(installFolder, "driver", "Uninstall.ps1");
            await Task.Run(() =>
            {
                var startInfo = new ProcessStartInfo
                {
                    Verb = "runas",
                    FileName = driverUninstallScript,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                var process = Process.Start(startInfo);
                process.WaitForExit();
            });

            Directory.Delete(installFolder, true);
            StatusLabel.Content = "Uninstalled";
            MessageBox.Show("TwometerVR was uninstalled successfully", "Removed successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void BeginInstallation()
        {
            try
            {
                var webClient = new WebClient();
                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    StatusBar.Value = e.ProgressPercentage;
                };

                var meta = JObject.Parse(await webClient.DownloadStringTaskAsync(ReleaseJson));
                var rtAsset = meta["assets"].Children<JObject>().Where(asset => asset?.Value<string>("name") == RuntimeName).SingleOrDefault();
                if (rtAsset == null)
                {
                    FailInstallation("Could not find the runtime on GitHub");
                    return;
                }

                var runtimeUrl = rtAsset.Value<string>("browser_download_url");
                var runtimeZip = Path.GetTempFileName();
                await webClient.DownloadFileTaskAsync(runtimeUrl, runtimeZip);

                var installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "TwometerVR");
                if (!Directory.Exists(installFolder))
                    Directory.CreateDirectory(installFolder);

                StatusLabel.Content = "Extracting...";
                StatusBar.IsIndeterminate = true;
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(runtimeZip, installFolder);
                });

                StatusLabel.Content = "Registering drivers...";
                StatusBar.IsIndeterminate = false;
                StatusBar.Value = 90;

                var driverInstallScript = Path.Combine(installFolder, "driver", "Install.ps1");

                await Task.Run(() =>
                {
                    var startInfo = new ProcessStartInfo
                    {
                        Verb = "runas",
                        FileName = driverInstallScript,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    var process = Process.Start(startInfo);
                    process.WaitForExit();
                });
                await Task.Delay(500); // User should at least be able to read the message what the program is/was doing

                StatusBar.IsIndeterminate = false;
                StatusBar.Value = 90;
                StatusLabel.Content = "Registering application...";

                var uninstallerDstPath = Path.Combine(installFolder, "uninstall.exe");
                System.IO.File.Copy(Assembly.GetExecutingAssembly().Location, uninstallerDstPath);
                RegisterUninstaller(uninstallerDstPath);

                var mainAppPath = Path.Combine(installFolder, "service", "TVR.Service.UI.exe");
                RegisterApp(mainAppPath);

                StatusBar.Value = 100;
                StatusLabel.Content = "Installation complete :-)";
                MessageBox.Show("TwometerVR was installed successfully on your system!", "Installation successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                FailInstallation(e.Message);
            }
        }

        private void FailInstallation(string message)
        {
            MessageBox.Show($"Error: {message}", "Installation failed", MessageBoxButton.OK, MessageBoxImage.Error);
            StatusLabel.Content = "Installation failed :(";
        }

        private void RegisterApp(string appPath)
        {
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "TwometerVR");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = Path.Combine(appStartMenuPath, "TwometerVR" + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "TwometerVR Service";
            shortcut.TargetPath = appPath;
            shortcut.Save();
        }

        private void RegisterUninstaller(string uninstallerPath)
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = Guid.NewGuid().ToString("B");
                        key = parent.OpenSubKey(guidText, true) ??
                              parent.CreateSubKey(guidText);

                        if (key == null)
                        {
                            throw new Exception("unable to create uninstaller");
                        }

                        Assembly asm = GetType().Assembly;
                        Version v = asm.GetName().Version;

                        key.SetValue("DisplayName", "TwometerVR");
                        key.SetValue("ApplicationVersion", v.ToString());
                        key.SetValue("Publisher", "Twometer Tech");
                        key.SetValue("DisplayIcon", uninstallerPath);
                        key.SetValue("DisplayVersion", v.ToString(2));
                        key.SetValue("URLInfoAbout", "https://twometer.de/vr");
                        key.SetValue("Contact", "support@twometer.de");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", uninstallerPath);
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        "An error occurred writing uninstall information to the registry.  The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);
                }
            }
        }
    }
}
