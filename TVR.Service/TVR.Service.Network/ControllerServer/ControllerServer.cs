using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;
using TVR.Service.Network.Common.Host;

namespace TVR.Service.Network.ControllerServer
{
    public class ControllerServer : BaseServer<ControllerInfoPacket>
    {
        public ControllerServer() : base(IPAddress.Any, NetworkConfig.ControllerPort)
        {
            LoggerFactory.Current.Log(LogLevel.Info, $"Controller server listening on port {NetworkConfig.ControllerPort}");
        }
    }
}
