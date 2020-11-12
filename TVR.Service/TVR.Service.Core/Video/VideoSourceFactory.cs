using System;

namespace TVR.Service.Core.Video
{
    internal static class VideoSourceFactory
    {
        public static IVideoSource Create(VideoSourceType type, int cameraIndex)
        {
            switch (type)
            {
                case VideoSourceType.DirectShow:
                    return new DShowVideoSource(cameraIndex);
                case VideoSourceType.PSEye:
                    return new PSEyeVideoSource(cameraIndex);
                default:
                    throw new ArgumentException($"Unknown video source type {type}");
            }
        }
    }
}
