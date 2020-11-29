using System.Net;
using TVR.Service.Core.Logging;

namespace TVR.Service.Core.Network.Driver
{
    internal class DriverClient : BaseClient
    {
        public DriverClient() : base(NetConfig.DriverPort)
        {
            Loggers.Current.Log(LogLevel.Info, "Driver client online");
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            // Driver does not currently send messages back
        }
    }
}
