using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Network.ControllerServer
{
    public class CalibrationCompletedEventArgs : EventArgs
    {
        public IPEndPoint ControllerEndpoint { get; set; }

    }
}
