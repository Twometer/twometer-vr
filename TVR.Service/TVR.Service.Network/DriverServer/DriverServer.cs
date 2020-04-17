using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;
using TVR.Service.Network.Common.Host;

namespace TVR.Service.Network.DriverServer
{
    public class DriverServer : BaseServer<DriverPacket>
    {
        public DriverServer() : base(IPAddress.Loopback, NetworkConfig.DriverPort, receiving: false)
        {
            LoggerFactory.Current.Log(LogLevel.Info, $"Driver server listening on port {NetworkConfig.DriverPort}");
        }
    }
}
