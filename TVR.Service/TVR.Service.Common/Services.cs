using TVR.Service.Core.IO;
using TVR.Service.Core.Model.Config;
using TVR.Service.Core.Video;
using TVR.Service.Network.Controllers;
using TVR.Service.Network.Discovery;
using TVR.Service.Network.Driver;

namespace TVR.Service.Common
{
    public class Services
    {
        public FileManager FileManager { get; }

        public UserConfig Config { get; }

        public Camera Camera { get; }

        public DiscoveryServer DiscoveryServer { get; }

        public DriverServer DriverServer { get; }

        public ControllerServer ControllerServer { get; }

        public Services()
        {
            FileManager = new FileManager();
            Config = ConfigLoader.LoadUserConfig(FileManager);
            Camera = new Camera(Config.CameraInfo);
            DiscoveryServer = new DiscoveryServer();
            DriverServer = new DriverServer();
            ControllerServer = new ControllerServer();
        }
    }
}
