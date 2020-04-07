using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            IPEndPoint sender = null;
            var data = udpClient.EndReceive(result, ref sender);

            if (data.SequenceEqual(discoverySequence))
            {
                var ip = GetLocalIp();
                var c_array = new byte[Encoding.ASCII.GetByteCount(ip) + 1]; // Build a NUL-terminated C-array for the ESP8266
                Encoding.ASCII.GetBytes(ip, 0, ip.Length, c_array, 0);

                udpClient.Send(c_array, c_array.Length, sender);
            }

            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Failed to get local IP");
        }

    }
}
