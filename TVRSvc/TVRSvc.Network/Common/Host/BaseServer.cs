using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TVRSvc.Network.Common.Host
{
    public abstract class BaseServer<T> : IReceiveCallback where T : IPacket
    {
        private readonly TcpListener listener;

        private readonly ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();

        public event EventHandler<T> PacketReceived;

        public BaseServer(IPAddress addr, int port)
        {
            listener = new TcpListener(addr, port);
            listener.Start();

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
                    client.Writer.Write(payload.Length);
                    client.Writer.Write(payload);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Disconnect occurred: ");
                    Console.WriteLine(e);

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
