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
            var trackers = new TrackerManager();
            var calibration = new Calibration(camera, trackers);

            camera.Exposure = -8;

            Application.Idle += (s, a) =>
            {
                var frame = camera.QueryFrame();
                trackers.UpdateVideo(frame);

                /*if (!calibration.IsCalibrated)
                {
                    calibration.Update();
                }
                else
                {
                    label2.Text = "calibrated: true";
                }*/

                label1.Text = trackers.Trackers[0].Detected ? trackers.Trackers[0].Controller.Position.ToString() : "out of range";
                label6.Text = trackers.Trackers[1].Detected ? trackers.Trackers[1].Controller.Position.ToString() : "out of range";

                imageBox1.Image = checkBox1.Checked ? frame : trackers.Trackers[0].Frame as IInputArray;
            };
        }
    }
}
