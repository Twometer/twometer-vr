using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TVRSvc.Network
{
    public class Server : IDisconnectListener
    {
        private TcpListener listener;

        private readonly ConcurrentDictionary<Guid, Client> clients = new ConcurrentDictionary<Guid, Client>();

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();

            var thread = new Thread(AcceptClients);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Broadcast(DataPacket packet)
        {
            foreach (var client in clients.Values)
                client.Send(packet);
        }

        private void AcceptClients()
        {
            while (true)
            {
                var cl = listener.AcceptTcpClient();
                var id = Guid.NewGuid();
                clients[id] = new Client(id, cl, this);
            }
        }

        public void OnDisconnect(Guid clientId)
        {
            clients.TryRemove(clientId, out var _);
        }
    }
}
