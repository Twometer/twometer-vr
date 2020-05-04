using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Network.ControllerServer
{
    public class StatusChangedEventArgs : EventArgs
    {
        public IPEndPoint ControllerEndpoint { get; set; }

        public ControllerStatus ControllerStatus { get; set; }

    }
}
