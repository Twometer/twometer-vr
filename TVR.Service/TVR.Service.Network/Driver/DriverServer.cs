using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.Driver
{
    public class DriverServer
    {
        private readonly TcpListener listener;

        private readonly ConcurrentDictionary<Guid, DriverClient> clients = new ConcurrentDictionary<Guid, DriverClient>();

        public int ConnectedClientCount => clients.Count;

        public DriverServer()
        {
            listener = new TcpListener(IPAddress.Loopback, NetworkConfig.DriverPort);
            listener.Start();

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public void Broadcast(IPacket packet)
        {
            var memstream = new MemoryStream();
            var contentWriter = new BinaryWriter(memstream);
            packet.Serialize(contentWriter);

            var payload = memstream.ToArray();

            foreach (var client in clients.Values)
            {
                try
                {
                    client.Writer.Write((short)payload.Length);
                    client.Writer.Write(payload);
                }
                catch
                {
                    LoggerFactory.Current.Log(LogLevel.Warning, $"Driver {client.Endpoint} lost connection");
                    clients.TryRemove(client.Id, out _);
                }
            }
        }

        private void AcceptClients(IAsyncResult result)
        {
            var tcp = listener.EndAcceptTcpClient(result);

            if (tcp != null)
            {
                var id = Guid.NewGuid();
                var client = new DriverClient(id, tcp);
                LoggerFactory.Current.Log(LogLevel.Info, $"Driver connected at {client.Endpoint}");
                clients[id] = client;
            }

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }
    }
}
