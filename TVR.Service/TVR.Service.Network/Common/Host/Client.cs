using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TVR.Service.Core.Logging;

namespace TVR.Service.Network.Common.Host
{
    internal class Client
    {
        public Guid Id { get; }

        public EndPoint Endpoint { get; private set; }

        public BinaryWriter Writer { get; }

        private IReceiveCallback callback;

        private NetworkStream stream;

        private byte[] buf = new byte[1024];

        private bool startedTransmit = false;

        public Client(Guid id, TcpClient client, IReceiveCallback callback)
        {
            Id = id;
            stream = client.GetStream();
            Writer = new BinaryWriter(stream);
            Endpoint = client.Client.RemoteEndPoint;
            this.callback = callback;
        }

        public void BeginReceiving()
        {
            stream.BeginRead(buf, 0, sizeof(short), new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            var received = stream.EndRead(result);
            if (received == sizeof(short))
            {
                var packetLen = BitConverter.ToInt16(buf, 0);
                if (packetLen > buf.Length || packetLen < 0)
                {
                    LoggerFactory.Current.Log(LogLevel.Error, $"Packet with invalid length {packetLen} received, dropping");
                    BeginReceiving();
                    return;
                }
                if (packetLen == 0)
                {
                    LoggerFactory.Current.Log(LogLevel.Error, "Zero-length packet received, dropping");
                }
                else
                {
                    if (!startedTransmit)
                        LoggerFactory.Current.Log(LogLevel.Info, $"Client {Endpoint} started transmitting");
                    startedTransmit = true;

                    stream.Read(buf, 0, packetLen);
                    var memstream = new MemoryStream(buf, 0, packetLen);
                    callback?.OnPacket(memstream);
                }
            }
            BeginReceiving();
        }

    }
}
