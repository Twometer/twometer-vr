using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Core;

namespace TVR.Service.Debug
{
    public partial class Form1 : Form
    {
        private TvrService service;
        private CameraSetup cameraSetup = new CameraSetup();
        private bool online;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!online) return;

            var sb = new StringBuilder();
            foreach (var tracker in service.TrackerManager.Trackers)
            {
                sb.AppendLine("Tracker #" + tracker.TrackerId);
                sb.AppendLine("    Serial No: " + tracker.SerialNo);
                sb.AppendLine("    Class: " + tracker.TrackerClass);
                sb.AppendLine("    Color: " + tracker.TrackerColor);
                sb.AppendLine("    Buttons: " + Convert.ToString(tracker.Buttons, 2));
                sb.AppendLine("    Position: " + tracker.Position);
                sb.AppendLine("    Rotation: " + tracker.Rotation);
                sb.AppendLine("    InRange: " + tracker.InRange);
                sb.AppendLine("    Timeout: " + tracker.TimeSinceLastHeartbeat);
                sb.AppendLine("    Accuracy: " + tracker.TrackingAccuracy);
                service.VideoSource.BgrFrame.Draw(tracker.Circle, new Emgu.CV.Structure.Bgr(255, 255, 255));
            }
            lbTrackerState.Text = sb.ToString();
            imageBox1.Image = service.VideoSource.BgrFrame;
            lbCalibState.Text = $"p_focal_length: {cameraSetup.PFocalLength}\n\npx_per_meter: {cameraSetup.PixelsPerMeter}";

            var calibTracker = service.TrackerManager.Trackers.FirstOrDefault();
            if (calibTracker != null)
                cameraSetup.Update(calibTracker);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            service?.Stop();
        }

        double minh = 999;
        double mins = 999;
        double minv = 999;
        double maxh;
        double maxs;
        double maxv;

        private void imageBox1_Click(object sender, EventArgs e)
        {
            var xScrollOffset = imageBox1.HorizontalScrollBar.Value; // / 100.0 * service.VideoSource.BgrFrame.Width;
            var yScrollOffset = imageBox1.VerticalScrollBar.Value; // / 100.0 * service.VideoSource.BgrFrame.Height;
            var scale = imageBox1.ZoomScale;

            var mousePos = imageBox1.PointToClient(MousePosition);
            var pixX = mousePos.X / scale + xScrollOffset;
            var pixY = mousePos.Y / scale + yScrollOffset;
            var pix = service.VideoSource.HsvFrame[new Point((int)pixX, (int)pixY)];
            CompareAndSet(pix.Hue, ref minh, false);
            CompareAndSet(pix.Satuation, ref mins, false);
            CompareAndSet(pix.Value, ref minv, false);

            CompareAndSet(pix.Hue, ref maxh, true);
            CompareAndSet(pix.Satuation, ref maxs, true);
            CompareAndSet(pix.Value, ref maxv, true);

            lbColorState.Text = $"lH: {minh} lS: {mins} lV: {minv}\n\nhH: {maxh} hS: {maxs} hV: {maxv}";
        }

        private void CompareAndSet(double val, ref double dst, bool greater)
        {
            var cond = greater
                ? (val > dst)
                : (val < dst);

            if (cond) dst = val;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            minh = mins = minv = 999f;
            maxh = maxs = maxv = 0f;
            lbColorState.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cameraSetup.StatusMessageReceived += CameraSetup_StatusMessageReceived;
        }

        private void CameraSetup_StatusMessageReceived(object sender, CameraSetup.StatusMessage e)
        {
            switch (e)
            {
                case CameraSetup.StatusMessage.CountdownChanged:
                    lbStatus.Text = $"Move controller 1m away from cam; sampling will begin in {cameraSetup.TimerSeconds} seconds...";
                    break;
                case CameraSetup.StatusMessage.BeginSamplingCircleDiameter:
                    lbStatus.Text = $"Sampling! Please do not move the controller!";
                    break;
                case CameraSetup.StatusMessage.AwaitingZeroPosition:
                    lbStatus.Text = "Complete. Now please stay at one meter distance from the camera and move all the way to the left side of the frame!";
                    break;
                case CameraSetup.StatusMessage.BeginSamplingPixelsPerMeter:
                    lbStatus.Text = "Horizontal calibration starting. Please move one meter to the right and back again!";
                    break;
                case CameraSetup.StatusMessage.Completed:
                    lbStatus.Text = "Setup completed successfully!";
                    break;
            }
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                service = new TvrService();
                service.Start();
            });
            lbStatus.Text = "Online";
            online = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            cameraSetup.BeginDepthSamplingCountdown();
        }
    }
}
