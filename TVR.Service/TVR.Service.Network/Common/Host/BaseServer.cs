using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TVR.Service.Core.Logging;

namespace TVR.Service.Network.Common.Host
{
    public abstract class BaseServer<T> : IReceiveCallback where T : IPacket
    {
        private readonly TcpListener listener;

        private readonly ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();

        public event EventHandler<T> PacketReceived;

        private readonly bool receiving;

        public int ConnectedClientCount => clients.Count;

        public BaseServer(IPAddress addr, int port, bool receiving = true)
        {
            listener = new TcpListener(addr, port);
            listener.Start();

            this.receiving = receiving;

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }


        public void Broadcast(T data)
        {
            var memstream = new MemoryStream();
            var contentWriter = new BinaryWriter(memstream);
            data.Serialize(contentWriter);

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
                    LoggerFactory.Current.Log(LogLevel.Warning, $"Client {client.Endpoint} lost connection");
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
                var client = new Client(id, tcp, this);
                LoggerFactory.Current.Log(LogLevel.Info, $"Client {client.Endpoint} connected");
                if (receiving)
                    client.BeginReceiving();
                clients[id] = client;
            }

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClients), null);
        }

        public void OnPacket(MemoryStream data)
        {
            var reader = new BinaryReader(data);
            var packet = Activator.CreateInstance<T>();
            packet.Deserialize(reader);
            PacketReceived?.Invoke(this, packet);
        }
    }
}
