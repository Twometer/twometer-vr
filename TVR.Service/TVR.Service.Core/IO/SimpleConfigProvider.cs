using TVR.Service.Core.Model;

namespace TVR.Service.Core.IO
{
    public class SimpleConfigProvider : IConfigProvider
    {
        public UserConfig UserConfig { get; }

        public VideoSourceConfig VideoSourceConfig { get; }

        public SimpleConfigProvider(UserConfig userConfig, VideoSourceConfig videoSourceConfig)
        {
            UserConfig = userConfig;
            VideoSourceConfig = videoSourceConfig;
        }

        public void Load()
        {
        }
    }
}
