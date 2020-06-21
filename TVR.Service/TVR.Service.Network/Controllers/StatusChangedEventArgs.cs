using System;
using System.Net;

namespace TVR.Service.Network.Controllers
{
    public class StatusChangedEventArgs : EventArgs
    {
        public IPEndPoint ControllerEndpoint { get; set; }

        public StatusMessage StatusMessage { get; set; }
    }
}
