using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Network
{
    public class Client
    {
        private Guid id;

        private TcpClient client;

        private IDisconnectListener listener;

        private BinaryWriter writer;

        public Client(Guid id, TcpClient client, IDisconnectListener listener)
        {
            this.id = id;
            this.client = client;
            this.listener = listener;
            this.writer = new BinaryWriter(client.GetStream());
        }

        public void Send(DataPacket data)
        {
            try
            {
                var memstream = new MemoryStream();
                var contentWriter = new BinaryWriter(memstream);
                data.Serialize(contentWriter);

                var payload = memstream.ToArray();

                writer.Write(payload.Length);
                writer.Write(payload);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                listener.OnDisconnect(id);
            }
        }

    }
}
