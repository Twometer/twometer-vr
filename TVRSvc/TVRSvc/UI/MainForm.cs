using Emgu.CV;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TVRSvc.Core.Tracking;
using TVRSvc.Core.Video;
using TVRSvc.Network.ControllerServer;
using TVRSvc.Network.Discovery;
using TVRSvc.Network.DriverServer;
using TVRSvc.Service;

namespace TVRSvc
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

            var manager = context.TrackerManager;
            Application.Idle += (s, a) =>
            {
                context.Update();

                foreach (var t in context.TrackerManager.Trackers)
                    t.Visualize = checkBox1.Checked;

                if (context.Calibration.IsCalibrated)
                {
                    var controller1 = manager.Trackers[0].Controller;
                    lbTracker1Pos.Text = manager.Trackers[0].Detected ? $"{controller1.Position.ToString()}\nY={controller1.Yaw} P={controller1.Pitch} R={controller1.Roll}" : "out of range";
                    lbTracker1Pos.BackColor = controller1.Buttons[Core.Model.Button.A] ? Color.Green : Color.Transparent;

                    var controller2 = manager.Trackers[1].Controller;
                    lbTracker2Pos.Text = manager.Trackers[1].Detected ? $"{controller2.Position.ToString()}\nY={controller2.Yaw} P={controller2.Pitch} R={controller2.Roll}" : "out of range";
                    lbTracker2Pos.BackColor = controller2.Buttons[Core.Model.Button.A] ? Color.Green : Color.Transparent;
                }

                if (radioButton1.Checked)
                    imageBox1.Image = context.Camera.Frame;
                else if (radioButton2.Checked)
                    imageBox1.Image = manager.Trackers[0].Frame;
                else if (radioButton3.Checked)
                    imageBox1.Image = manager.Trackers[1].Frame;
                else if (radioButton4.Checked)
                {
                    var mat = new Mat();
                    CvInvoke.BitwiseOr(manager.Trackers[0].Frame, manager.Trackers[1].Frame, mat);
                    imageBox1.Image = mat;
                }

                lbConnectedControllers.Text = $"Connected controllers: {context.ControllerServer.ConnectedClientCount}";
                lbConnectedDrivers.Text = $"Connected drivers: {context.DriverServer.ConnectedClientCount}";

                glControl1.Invalidate();

                frames++;
            };

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

            GL.Color4(Color.Red);
            DrawTracker(context.TrackerManager.Trackers[0]);

            GL.Color4(Color.Blue);
            DrawTracker(context.TrackerManager.Trackers[1]);

            glControl1.SwapBuffers();
        }

        private void DrawTracker(Tracker tracker)
        {
            if (tracker.Detected)
            {
                DrawCross(tracker.Controller.Position.X, tracker.Controller.Position.Y, tracker.Controller.Position.Z, 0.3f);


                var yaw = tracker.Controller.Yaw;
                var pitch = tracker.Controller.Pitch;

                var x = (float)-Math.Sin(MathHelper.DegreesToRadians(yaw)) * (float)Math.Sin(MathHelper.DegreesToRadians(90 - pitch));
                var y = (float)-Math.Sin(MathHelper.DegreesToRadians(-pitch));
                var z = (float)-Math.Cos(MathHelper.DegreesToRadians(yaw)) * (float)Math.Sin(MathHelper.DegreesToRadians(90 - pitch));
                DrawLine(tracker.Controller.Position.X, tracker.Controller.Position.Y, tracker.Controller.Position.Z, tracker.Controller.Position.X + x, tracker.Controller.Position.Y + y, tracker.Controller.Position.Z + z);
                //DrawCross(tracker.Controller.Position.X - x, tracker.Controller.Position.Y - y, tracker.Controller.Position.Z - z, 0.3f);
            }
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
