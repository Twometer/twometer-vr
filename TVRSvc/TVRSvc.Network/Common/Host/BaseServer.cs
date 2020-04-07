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
    public abstract class BaseServer<T> where T : IPacket
    {
        private TcpListener listener;

        private readonly ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();

        public BaseServer(IPAddress addr, int port)
        {
            listener = new TcpListener(addr, port);
            listener.Start();

            var thread = new Thread(AcceptClients);
            thread.IsBackground = true;
            thread.Start();
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

        private void AcceptClients()
        {
            while (true)
            {
                var cl = listener.AcceptTcpClient();
                var id = Guid.NewGuid();
                clients[id] = new Client(id, cl);
            }
        }
    }
}
