using System.Net;

namespace TVR.Service.Core.Network.Driver
{
    internal class DriverClient : BaseClient
    {
        public DriverClient() : base(NetConfig.DriverPort)
        {
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            // Driver does not currently send messages back
        }
    }
}
