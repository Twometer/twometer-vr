using TVR.Service.Core.Model;

namespace TVR.Service.Core.IO
{
    public interface IConfigProvider
    {
        UserConfig UserConfig { get; }

        VideoSourceConfig VideoSourceConfig { get; }

        void Load();
    }
}
