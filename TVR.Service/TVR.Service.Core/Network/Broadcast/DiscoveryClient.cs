using System.Net;
using System.Net.Sockets;

namespace TVR.Service.Core.Network.Broadcast
{
    internal class DiscoveryClient : BaseClient
    {
        private static readonly byte[] DiscoveryMessage = { 0x80, 0x02, 0x16, 0x39, 0xA0 };

        private string serverIp;

        public DiscoveryClient() : base(NetConfig.BroadcastPort)
        {
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            if (!CheckEquals(data, DiscoveryMessage))
                return;

            Send(new P81DiscoveryReply() { ServerIp = GetIpAddress() }, sender);
        }

        private string GetIpAddress()
        {
            if (serverIp != null)
                return serverIp;

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return serverIp = endPoint.Address.ToString();
            }
        }

        private bool CheckEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for (var i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }
    }
}
