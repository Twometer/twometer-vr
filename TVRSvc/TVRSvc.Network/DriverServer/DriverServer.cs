using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Logging;
using TVRSvc.Network.Common;
using TVRSvc.Network.Common.Host;

namespace TVRSvc.Network.DriverServer
{
    public class DriverServer : BaseServer<DriverPacket>
    {
        public DriverServer() : base(IPAddress.Loopback, NetworkConfig.DriverPort, receiving: false)
        {
            LoggerFactory.Current.Log(LogLevel.Info, $"Driver server listening on port {NetworkConfig.DriverPort}");
        }
    }
}
