using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Network.Common;
using TVRSvc.Network.Common.Host;

namespace TVRSvc.Network.ControllerServer
{
    public class ControllerServer : BaseServer<ControllerInfoPacket>
    {
        public ControllerServer() : base(IPAddress.Any, NetworkConfig.ControllerPort)
        {
        }
    }
}
