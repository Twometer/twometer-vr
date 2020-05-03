using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;
using TVR.Service.Network.Common.Host;

namespace TVR.Service.Network.ControllerServer
{
    public class ControllerServer
    {
        public event EventHandler<ControllerInfoPacket> PacketReceived;

        public event EventHandler<CalibrationCompletedEventArgs> CalibrationCompleted;

        private UdpClient udp;

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
            if (message.Length == 2 && message[0] == 0xFF && message[1] == 0xFF)
            {
                CalibrationCompleted?.Invoke(this, new CalibrationCompletedEventArgs() { ControllerEndpoint = ep });
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
