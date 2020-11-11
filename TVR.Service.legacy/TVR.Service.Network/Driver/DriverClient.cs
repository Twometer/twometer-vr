using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TVR.Service.Network.Driver
{
    public class DriverClient
    {
        public Guid Id { get; }

        public EndPoint Endpoint { get; private set; }

        public BinaryWriter Writer { get; }

        public DriverClient(Guid id, TcpClient client)
        {
            Id = id;
            Writer = new BinaryWriter(client.GetStream());
            Endpoint = client.Client.RemoteEndPoint;
        }
    }
}
