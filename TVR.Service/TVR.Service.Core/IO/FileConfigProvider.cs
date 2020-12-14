using System;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.IO
{
    public class FileConfigProvider : IConfigProvider
    {
        public UserConfig UserConfig { get; private set; }

        public VideoSourceConfig VideoSourceConfig { get; private set; }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}
