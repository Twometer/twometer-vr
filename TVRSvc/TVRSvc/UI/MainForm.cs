using Emgu.CV;
using Emgu.CV.UI;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVRSvc.Core.Tracking;
using TVRSvc.Core.Video;
using TVRSvc.Network;

namespace TVRSvc
{
    public partial class MainForm : Form
    {
        private const int NetworkPort = 12741;

        private TrackerManager manager;

        private int frames;

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manager = new TrackerManager();

            var camera = new Camera();
            var calibration = new Calibration(camera);
            var server = new Server();
            server.Start(NetworkPort);

            Application.Idle += (s, a) =>
            {
                var frame = camera.QueryFrame();
                manager.UpdateVideo(frame);

                foreach (var t in manager.Trackers)
                    t.Visualize = checkBox1.Checked;

                if (!calibration.IsCalibrated)
                {
                    calibration.Update(frame);
                    label1.Text = label6.Text = "calibrating camera...";
                }
                else
                {
                    label1.Text = manager.Trackers[0].Detected ? manager.Trackers[0].Controller.Position.ToString() : "out of range";
                    label6.Text = manager.Trackers[1].Detected ? manager.Trackers[1].Controller.Position.ToString() : "out of range";
                }

                if (radioButton1.Checked)
                    imageBox1.Image = frame;
                else if (radioButton2.Checked)
                    imageBox1.Image = manager.Trackers[0].Frame;
                else if (radioButton3.Checked)
                    imageBox1.Image = manager.Trackers[1].Frame;

                glControl1.Invalidate();

                if (manager.Detected)
                    server.Broadcast(new DataPacket() { ControllerStates = manager.Trackers.Select(t => t.Controller).ToArray() });

                frames++;
            };
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (manager == null)
                return;

            // Yes, this is immediate mode.
            // Yes, it's 2020.
            // It's just a few lines, I am too lazy to make VBOs and shaders for that

            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, glControl1.Width / (float)glControl1.Height, 1, 10000); // Setup Perspective
            var lookat = Matrix4.LookAt(1, 3, 5, 0, 0, 0, 0, 1, 0); // Setup camera
            GL.MatrixMode(MatrixMode.Projection); // Load Perspective
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview); // Load Camera
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height); // Size of window
            GL.Enable(EnableCap.DepthTest); // Enable correct Z Drawings
            GL.DepthFunc(DepthFunction.Less); // Enable correct Z Drawings


            GL.Color4(Color.Black);
            DrawCross(0, 0, 0, 5);
            DrawUnits(0, 0, 1, 5);
            DrawUnits(0, 1, 0, 5);
            DrawUnits(1, 0, 0, 5);

            GL.Color4(Color.Blue);
            DrawTracker(manager.Trackers[0]);

            GL.Color4(Color.Red);
            DrawTracker(manager.Trackers[1]);

            glControl1.SwapBuffers();
        }

        private void DrawTracker(Tracker tracker)
        {
            if (tracker.Detected)
                DrawCross(tracker.Controller.Position.X, tracker.Controller.Position.Y, tracker.Controller.Position.Z, 0.3f);
        }

        private void DrawCross(float x, float y, float z, float size)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(-size + x, y, z);
            GL.Vertex3(size + x, y, z);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(x, -size + y, z);
            GL.Vertex3(x, size + y, z);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(x, y, -size + z);
            GL.Vertex3(x, y, size + z);
            GL.End();
        }

        private void DrawUnits(float x, float y, float z, float d)
        {
            float ox = 0;
            float oy = 0;
            float oz = 0;

            float yd = y == 0 ? 0.2f : 0;
            float zd = y != 0 ? 0.2f : 0;

            for (var i = 0; i < d; i++)
            {
                ox += x;
                oy += y;
                oz += z;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(ox - zd, oy - yd, oz);
                GL.Vertex3(ox + zd, oy + yd, oz);
                GL.End();
            }
            ox = 0; oy = 0; oz = 0;
            for (var i = 0; i < d; i++)
            {
                ox -= x;
                oy -= y;
                oz -= z;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(ox - zd, oy - yd, oz);
                GL.Vertex3(ox + zd, oy + yd, oz);
                GL.End();
            }
        }

        bool moving = false;

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }


        int lx;
        int ly;

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving)
            {



            }

            lx = e.X;
            ly = e.Y;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbfps.Text = $"{frames} fps";
            frames = 0;

        }
    }
}
