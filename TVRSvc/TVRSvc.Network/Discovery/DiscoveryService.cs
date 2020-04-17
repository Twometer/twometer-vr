using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Logging;
using TVRSvc.Network.Common;

namespace TVRSvc.Network.Discovery
{
    public class DiscoveryService
    {
        private UdpClient udpClient;

        private byte[] discoverySequence = new byte[] { 0x79, 0x65, 0x65, 0x74 };

        public DiscoveryService()
        {
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, NetworkConfig.DiscoveryPort));
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            LoggerFactory.Current.Log(LogLevel.Info, $"Discovery service listening on port {NetworkConfig.DiscoveryPort}");
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            IPEndPoint sender = null;
            var data = udpClient.EndReceive(result, ref sender);

            if (data.SequenceEqual(discoverySequence))
            {
                LoggerFactory.Current.Log(LogLevel.Debug, $"Received discovery sequence from {sender}");

                var ip = GetLocalIp();
                var c_array = new byte[Encoding.ASCII.GetByteCount(ip) + 1]; // Build a NUL-terminated C-array for the ESP8266
                Encoding.ASCII.GetBytes(ip, 0, ip.Length, c_array, 0);
                var sent = udpClient.Send(c_array, c_array.Length, sender);
            }

            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private string GetLocalIp()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }

    }
}
