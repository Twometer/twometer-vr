using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace nextgentrackingdemo.Video
{
    public interface IVideoSource : IDisposable
    {
        double FrameBrightness { get; }

        Image<Hsv, byte> Frame { get; }

        int Framerate { get; set; }

        int Exposure { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        void Open();

        bool Grab();
    }
}
