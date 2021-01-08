using TVR.Service.Core.Video;

namespace TVR.Service.Core.Model
{
    public struct VideoSourceConfig
    {
        public VideoSourceType VideoSourceType { get; set; }

        public int VideoSourceIndex { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public int Framerate { get; set; }

        public TransformConfig TransformConfig { get; set; }

        public ExposureConfig ExposureConfig { get; set; }

        public float[] Distortion { get; set; }

        public bool UseAdaptiveBrightness { get; set; }
    }

    public struct TransformConfig
    {
        public float PFocalLength { get; set; }

        public float PixelsPerMeter { get; set; }
    }

    public struct ExposureConfig
    {
        public int Start { get; set; }

        public int Limit { get; set; }

        public int Step { get; set; }

        public int WarmupFrames { get; set; }

        public int CooldownFrames { get; set; }

        public int StableFrames { get; set; }

        public double MaximumChange { get; set; }

        public double Threshold { get; set; }
    }
}
