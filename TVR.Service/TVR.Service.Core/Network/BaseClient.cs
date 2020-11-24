using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace TVR.Service.Core.Network
{
    internal abstract class BaseClient
    {
        private readonly UdpClient client = new UdpClient();

        internal BaseClient(ushort port)
        {
            client.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            BeginReceive();
        }

        protected Task Send(byte[] data, IPEndPoint receiver)
        {
            return client.SendAsync(data, data.Length, receiver);
        }

        protected abstract void OnReceive(byte[] data, IPEndPoint sender);

        private void BeginReceive()
        {
            client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            IPEndPoint sender = null;
            var data = client.EndReceive(result, ref sender);

            if (data.Length <= NetConfig.MaxPacketSize)
                OnReceive(data, sender);

            BeginReceive();
        }
    }

}
