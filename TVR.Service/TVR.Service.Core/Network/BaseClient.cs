﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;

namespace TVR.Service.Core.Network
{
    internal abstract class BaseClient
    {
        private readonly UdpClient client = new UdpClient();

        internal BaseClient(ushort bindPort)
        {
            client.Client.Bind(new IPEndPoint(IPAddress.Any, bindPort));
            client.Client.IOControl(-1744830452, new byte[] { 0 }, new byte[] { 0 });
            BeginReceive();
        }

        protected Task Send(byte[] data, IPEndPoint receiver)
        {
            return client.SendAsync(data, data.Length, receiver);
        }

        public Task Send(IPacket packet, IPEndPoint receiver)
        {
            var buf = new Buffer();
            buf.Write(packet.Id);
            packet.Serialize(buf);
            return Send(buf.ToArray(), receiver);
        }

        protected abstract void OnReceive(byte[] data, IPEndPoint sender);

        public void Close()
        {
            client.Close();
        }

        private void BeginReceive()
        {
            client.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                IPEndPoint sender = null;
                var data = client.EndReceive(result, ref sender);

                if (data.Length <= NetConfig.MaxPacketSize)
                {
                    HandlePacket(data, sender);
                }

                BeginReceive();
            }
            catch (ObjectDisposedException e)
            {
                Loggers.Current.Log(LogLevel.Error, e.Message);
            }
        }

        private void HandlePacket(byte[] data, IPEndPoint sender)
        {
            try
            {
                OnReceive(data, sender);
            }
            catch (Exception)
            {
                // drop packets with errors
            }
        }
    }

}
