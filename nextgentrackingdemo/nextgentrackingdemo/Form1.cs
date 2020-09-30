using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TVR.Service.Core.IO;
using TVector3 = TVR.Service.Core.Math.Vector3;
using TVR.Service.Core.Tracking;
using TVR.Service.Core.Video;
using System.Security.Permissions;

namespace nextgentrackingdemo
{
    public partial class Form1 : Form
    {
        private volatile bool running = true;

        private Vector3 position = new Vector3(1, 2, 0);

        public Form1()
        {
            InitializeComponent();
            glControl1.MouseWheel += GlControl1_MouseWheel;
        }

        private async void UpdateLoop()
        {
            var conf = ConfigIO.LoadUserConfig();
            var cam = new Camera(conf.CameraInfo);
            var tracker = new TrackingManager(conf);

            while (running)
                if (cam.Update())
                {
                    await tracker.UpdateVideo(cam.HsvFrame, cam.FrameBrightness);
                    imageBox1.Image = cam.Frame;
                    imageBox2.Image = tracker.Trackers[1].Frame;
                    AdvancedTracking(tracker.Trackers[1].TrackedController.Position);
                }
        }

        private void AdvancedTracking(TVector3 vec)
        {
            position = new Vector3(vec.X, vec.Y, vec.Z);
            glControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateLoop();
        }

        float yaw = -1.8f;
        float pitch = 0.4f;
        float zoom = 4.5f;

        float x = 0;
        float y = 0;
        float z = 0;

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GL.LineWidth(1.5f);
            GL.ClearColor(1, 1, 1, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            var perspective = Matrix4.CreatePerspectiveFieldOfView(1.04f, glControl1.Width / (float)glControl1.Height, 0.5f, 100); // Setup Perspective

            var l = Math.Cos(pitch);

            var lookat = Matrix4.LookAt((float)(Math.Cos(yaw) * l) * zoom + x, (float)Math.Sin(pitch) * zoom + y, (float)(Math.Sin(-yaw) * l) * zoom + z, x, y, z, 0, 1, 0); // Setup camera

            GL.MatrixMode(MatrixMode.Projection); // Load Perspective
            GL.LoadIdentity();
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview); // Load Camera
            GL.LoadIdentity();
            GL.LoadMatrix(ref lookat);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height); // Size of window
            GL.Enable(EnableCap.DepthTest); // Enable correct Z Drawings
            GL.DepthFunc(DepthFunction.Less); // Enable correct Z Drawings

            GL.Color4(Color.Gray);
            DrawCross(0, 0, 0, 10);

            GL.Color4(Color.Blue);
            DrawUnits(0, 0, 1, 10);
            DrawCross(0, 0, 10, 0.5f);
            GL.Color4(Color.Green);
            DrawUnits(0, 1, 0, 10);
            DrawCross(0, 10, 0, 0.5f);
            GL.Color4(Color.Red);
            DrawUnits(1, 0, 0, 10);
            DrawCross(10, 0, 0, 0.5f);

            GL.Color4(Color.Blue);
            DrawCross(position.X, position.Y, position.Z, 1.0f);

            label3.Text = yaw + " " + pitch + " " + zoom;

            glControl1.SwapBuffers();
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

        bool down = false;

        int px = 0;
        int py = 0;

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            down = false;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {


            glControl1.Invalidate();

            if (down)
            {
                var dx = e.X - px;
                var dy = e.Y - py;

                yaw -= dx * 0.01f;
                pitch += dy * 0.01f;

                pitch = (float)MathHelper.Clamp(pitch, -Math.PI / 2 + 0.05, Math.PI / 2 - 0.05);

            }

            px = e.X;
            py = e.Y;
        }

        private void GlControl1_MouseWheel(object sender, MouseEventArgs e)
        {

            zoom -= Math.Sign(e.Delta) * 0.25f;
            zoom = MathHelper.Clamp(zoom, 2.0f, 10000.0f);
            glControl1.Invalidate();
        }

        private void glControl1_Scroll(object sender, ScrollEventArgs e)
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
