using TVR.Service.Core.Video;

namespace TVR.Service.Core.Model
{
    public struct VideoSourceConfig
    {
        public VideoSourceType VideoSourceType { get; set; }

        public int VideoSourceIndex { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public ExposureConfig ExposureConfig { get; set; }

        public float[] Distortion { get; set; }
    }

    public struct ExposureConfig
    {
        public float Start { get; set; }

        public float Limit { get; set; }

        public float Step { get; set; }

        public int WarmupFrames { get; set; }

        public int WaitFrames { get; set; }

        public int StableFrames { get; set; }

        public float Threshold { get; set; }
    }
}
