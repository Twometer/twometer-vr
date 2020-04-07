using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private IReceiveCallback callback;

        private NetworkStream stream;

        private byte[] buf = new byte[1024];

        public Client(Guid id, TcpClient client, IReceiveCallback callback)
        {
            Id = id;
            stream = client.GetStream();
            Writer = new BinaryWriter(stream);
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
                if (packetLen > buf.Length)
                {
                    throw new IOException("Buffer overflow: Message too long");
                }
                if (packetLen == 0)
                {
                    Debug.WriteLine("Received packet with length 0, dropping.");
                }
                else
                {
                    stream.Read(buf, 0, packetLen);
                    var memstream = new MemoryStream(buf, 0, packetLen);
                    callback?.OnPacket(memstream);
                }
            }
            BeginReceiving();
        }

    }
}
