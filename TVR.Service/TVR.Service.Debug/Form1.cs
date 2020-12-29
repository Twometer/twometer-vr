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
                sb.AppendLine("    Buttons: " + tracker.Buttons);
                sb.AppendLine("    Position: " + tracker.Position);
                sb.AppendLine("    Rotation: " + tracker.Rotation);
                sb.AppendLine("    InRange: " + tracker.InRange);
                sb.AppendLine("    Timeout: " + tracker.TimeSinceLastHeartbeat);
            }
            label1.Text = sb.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            service?.Stop();
        }
    }
}
