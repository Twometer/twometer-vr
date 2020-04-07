using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Network.Common.Host
{
    internal class Client
    {
        public Guid Id { get; }

        public BinaryWriter Writer { get; }

        public Client(Guid id, TcpClient client)
        {
            Id = id;
            Writer = new BinaryWriter(client.GetStream());
        }

    }
}
