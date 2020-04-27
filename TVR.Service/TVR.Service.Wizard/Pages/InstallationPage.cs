using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace TVR.Service.Wizard.Pages
{
    public partial class InstallationPage : BasePage
    {
        private const string RuntimeDownloadUrl = "http://github.com/Twometer/twometer-vr/releases/latest/download/tvr-runtime.zip";

        private string zipFileLocation;

        public InstallationPage()
        {
            InitializeComponent();

            NavigatedTo += InstallationPage_NavigatedTo;
        }

        private async void InstallationPage_NavigatedTo(object sender, EventArgs e)
        {
            Context.AllowNext = false;
            Context.NextButtonText = "Next";

            SetStatus("Downloading TwometerVR...");

            zipFileLocation = Path.GetTempFileName();

            var wc = new WebClient();
            wc.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            await wc.DownloadFileTaskAsync(RuntimeDownloadUrl, zipFileLocation);

            SetStatus("Installing...");
            var targetDir = new DirectoryInfo(Context.InstallationPath);
            if (!targetDir.Exists)
                targetDir.Create();

            ZipFile.ExtractToDirectory(zipFileLocation, targetDir.FullName);
            
            SetStatus("Registering with SteamVR...");
            var ps1Path = Path.Combine(targetDir.FullName, "driver", "Install.ps1");
            StartPowershell(ps1Path);

            SetStatus("Complete");

            Context.AllowNext = true;
        }

        private void StartPowershell(string scriptFile)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{scriptFile}\"",
                UseShellExecute = false
            };
            Process.Start(startInfo);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SetPercent(e.ProgressPercentage);
        }

        private void SetStatus(string status)
        {
            lbStatus.Text = "Status: " + status;
        }

        private void SetPercent(int val)
        {
            pbStatus.Value = val;
            lbPercent.Text = val + " %";
        }
    }
}
