using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVRSvc.Tracking;
using TVRSvc.Video;

namespace TVRSvc
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var camera = new Camera();
            var manager = new TrackerManager();
            var calibration = new Calibration(camera);

            Application.Idle += (s, a) =>
            {
                var frame = camera.QueryFrame();
                manager.UpdateVideo(frame);

                foreach (var t in manager.Trackers)
                    t.Visualize = checkBox1.Checked;

                if (!calibration.IsCalibrated)
                    calibration.Update(frame);
                else
                    label2.Text = "calibrated: true";

                label1.Text = manager.Trackers[0].Detected ? manager.Trackers[0].Controller.Position.ToString() : "out of range";
                label6.Text = manager.Trackers[1].Detected ? manager.Trackers[1].Controller.Position.ToString() : "out of range";

                if (radioButton1.Checked)
                    imageBox1.Image = frame;
                else if (radioButton2.Checked)
                    imageBox1.Image = manager.Trackers[0].Frame;
                else if (radioButton3.Checked)
                    imageBox1.Image = manager.Trackers[1].Frame;
            };
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
