using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.Controllers
{
    public class ControllerServer
    {
        private readonly UdpClient udp;

        public event EventHandler<ControllerInfoPacket> PacketReceived;

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public ControllerServer()
        {
            udp = new UdpClient();
            udp.Client.Bind(new IPEndPoint(IPAddress.Any, NetworkConfig.ControllerPort));
            udp.BeginReceive(new AsyncCallback(ReceiveCallback), null);

            LoggerFactory.Current.Log(LogLevel.Info, $"Controller server listening on UDP port {NetworkConfig.ControllerPort}");
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            IPEndPoint ep = null;
            var message = udp.EndReceive(result, ref ep);
            if (message[0] == 0xFF && message.Length == 2)
            {
                StatusChanged?.Invoke(this, new StatusChangedEventArgs() { ControllerEndpoint = ep, StatusMessage = (StatusMessage)message[1] });
            }
            else
            {
                var reader = new BinaryReader(new MemoryStream(message));
                var packet = new ControllerInfoPacket();
                packet.Deserialize(reader);
                PacketReceived?.Invoke(this, packet);
            }

            udp.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
    }
}
