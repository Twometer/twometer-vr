using Emgu.CV;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;
using TVR.Service.Common;
using TVR.Service.Core.Tracking;

namespace TVR.Service.DebugUI
{
    public partial class MainForm : Form
    {
        private ServiceContext context;

        private int frames;

        public MainForm()
        {
            InitializeComponent();
        }

        // Main Tracker loop
        private void button1_Click(object sender, EventArgs e)
        {
            context = new ServiceContext("tvrconfig.json");
            button1.Enabled = false;

            var manager = context.TrackerManager;
            var tempMat = new Mat();
            Application.Idle += (s, a) =>
            {
                context.Update();

                foreach (var t in context.TrackerManager.Trackers)
                    t.Visualize = checkBox1.Checked;

                if (context.Calibration.IsCalibrated)
                {
                    var controller1 = manager.Trackers[0].Controller;
                    lbTracker1Pos.Text = $"{controller1.Position.ToString()}\n{controller1.Rotation}";
                    lbTracker1Pos.BackColor = controller1.Buttons[Core.Model.Button.A] ? Color.Green : Color.Transparent;
                    lbTracker1Pos.ForeColor = manager.Trackers[0].Detected ? Color.Black : Color.DimGray;

                    var controller2 = manager.Trackers[1].Controller;
                    lbTracker2Pos.Text = $"{controller2.Position.ToString()}\n{controller2.Rotation}";
                    lbTracker2Pos.BackColor = controller2.Buttons[Core.Model.Button.A] ? Color.Green : Color.Transparent;
                    lbTracker2Pos.ForeColor = manager.Trackers[1].Detected ? Color.Black : Color.DimGray;
                }

                if (radioButton1.Checked)
                    imageBox1.Image = context.Camera.Frame;
                else if (radioButton2.Checked)
                    imageBox1.Image = manager.Trackers[0].Frame;
                else if (radioButton3.Checked)
                    imageBox1.Image = manager.Trackers[1].Frame;
                else if (radioButton4.Checked)
                {
                    CvInvoke.BitwiseOr(manager.Trackers[0].Frame, manager.Trackers[1].Frame, tempMat);
                    imageBox1.Image = tempMat;
                }

                lbConnectedControllers.Text = $"Connected controllers: unknown";
                lbConnectedDrivers.Text = $"Connected drivers: {context.DriverServer.ConnectedClientCount}";

                glControl1.Invalidate();

                frames++;
            };

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            context?.Broadcast();
        }

        // Visualization with OpenGL
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (context == null)
                return;

            // Yes, this is immediate mode.
            // Yes, it's 2020.
            // It's just a few lines, I am too lazy to make VBOs and shaders for that

            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, glControl1.Width / (float)glControl1.Height, 0.5f, 100); // Setup Perspective
            var lookat = Matrix4.LookAt(1, 2, 3, 0, 0.5f, 0, 0, 1, 0); // Setup camera
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

            GL.Color4(Color.Red);
            DrawTracker(context.TrackerManager.Trackers[0]);

            GL.Color4(Color.Blue);
            DrawTracker(context.TrackerManager.Trackers[1]);

            glControl1.SwapBuffers();
        }

        private void DrawTracker(Tracker tracker)
        {
            DrawCross(tracker.Controller.Position.X, tracker.Controller.Position.Y, tracker.Controller.Position.Z, 0.3f);

            var forward = Vector3.UnitZ;
            var quat = new Quaternion(tracker.Controller.Rotation.X, tracker.Controller.Rotation.Y, tracker.Controller.Rotation.Z, tracker.Controller.Rotation.W) * Quaternion.Identity;
            var vec = quat * forward;
            vec.Normalize();

            DrawLine(tracker.Controller.Position.X, tracker.Controller.Position.Y, tracker.Controller.Position.Z, tracker.Controller.Position.X - vec.X, tracker.Controller.Position.Y - vec.Y, tracker.Controller.Position.Z - vec.Z);
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

        private void DrawLine(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(x0, y0, z0);
            GL.Vertex3(x1, y1, z1);
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

        // FPS Calculation
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbfps.Text = $"{frames} fps";
            frames = 0;
        }
    }
}
