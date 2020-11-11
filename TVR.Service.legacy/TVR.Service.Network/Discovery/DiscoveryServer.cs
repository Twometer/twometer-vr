using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.Discovery
{
    public class DiscoveryServer
    {
        private readonly UdpClient udpClient;

        private readonly byte[] discoverySequence = new byte[] { 0x79, 0x65, 0x65, 0x74 };

        private string localIp = null;

        public DiscoveryServer()
        {
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, NetworkConfig.DiscoveryPort));
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            LoggerFactory.Current.Log(LogLevel.Info, $"Discovery server listening on port {NetworkConfig.DiscoveryPort}");
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            IPEndPoint sender = null;
            var data = udpClient.EndReceive(result, ref sender);

            if (data.SequenceEqual(discoverySequence))
            {
                LoggerFactory.Current.Log(LogLevel.Debug, $"Received discovery sequence from {sender}");

                var ip = GetLocalIp();

                // Construct a NUL-terminated C-string for the ESP8266
                var cArray = new byte[Encoding.ASCII.GetByteCount(ip) + 1];
                Encoding.ASCII.GetBytes(ip, 0, ip.Length, cArray, 0);
                udpClient.Send(cArray, cArray.Length, sender);
            }

            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private string GetLocalIp()
        {
            if (localIp != null)
                return localIp;

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return localIp = endPoint.Address.ToString();
            }
        }
    }
}
