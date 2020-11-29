using System.Reflection;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;
using TVR.Service.Core.Network.Broadcast;
using TVR.Service.Core.Network.Driver;
using TVR.Service.Core.Network.Unicast;

namespace TVR.Service.Core
{
    public class TvrService
    {
        public string Version
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }

        private readonly TrackerManager trackerManager = new TrackerManager();

        private DiscoveryClient discoveryClient;
        private DriverClient driverClient;
        private TrackerClient trackerClient;

        public void Launch()
        {
            Loggers.Current.Log(LogLevel.Info, $"Starting TwometerVR v{Version}");

            discoveryClient = new DiscoveryClient();
            driverClient = new DriverClient();
            trackerClient = new TrackerClient();

            // TODO start threads, cameras, etc.
        }
    }
}
