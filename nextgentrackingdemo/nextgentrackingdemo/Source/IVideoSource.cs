using Emgu.CV;

namespace nextgentrackingdemo
{
    public interface IVideoSource
    {
        Mat Frame { get; }

        int Framerate { get; set; }

        int Exposure { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        void Open();

        bool Grab();

    }
}
