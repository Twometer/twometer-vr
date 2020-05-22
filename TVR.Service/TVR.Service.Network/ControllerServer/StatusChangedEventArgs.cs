using System;
using System.Net;

namespace TVR.Service.Network.ControllerServer
{
    public class StatusChangedEventArgs : EventArgs
    {
        public IPEndPoint ControllerEndpoint { get; set; }

        public ControllerStatus ControllerStatus { get; set; }
    }
}
