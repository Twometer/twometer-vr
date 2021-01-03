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

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            service = new TvrService();
            service.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (service == null) return;
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
            }
            label1.Text = sb.ToString();
            imageBox1.Image = service.VideoSource.BgrFrame;
            
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

            label2.Text = $"lH: {minh} lS: {mins} lV: {minv}\n\nhH: {maxh} hS: {maxs} hV: {maxv}";
        }

        private void CompareAndSet(double val, ref double dst, bool greater)
        {
            var cond = greater
                ? (val > dst)
                : (val < dst);

            if (cond) dst = val;
        }
    }
}
