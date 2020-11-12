using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace TVR.Service.Core.Video
{
    internal interface IVideoSource : IDisposable
    {
        Image<Bgr, byte> BgrFrame { get; }

        Image<Hsv, byte> HsvFrame { get; }

        double FrameBrightness { get; }

        int Framerate { get; set; }

        int Exposure { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        void Open();

        bool Grab();
    }
}
