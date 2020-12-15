using TVR.Service.Core.IO;

namespace TVR.Service.Core.Video
{
    internal class AutoExposure
    {
        private readonly IConfigProvider configProvider;
        private readonly IVideoSource videoSource;

        public bool Finished { get; private set; }

        public AutoExposure(IConfigProvider configProvider, IVideoSource videoSource)
        {
            this.configProvider = configProvider;
            this.videoSource = videoSource;
        }

        public void Update()
        {
            var config = configProvider.VideoSourceConfig.ExposureConfig;
        }
    }
}
