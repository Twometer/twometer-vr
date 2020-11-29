using System;
using System.Collections;
using System.Net;

namespace TVR.Service.Core.Network.Unicast
{
    internal class TrackerClient : BaseClient
    {
        public TrackerClient() : base(NetConfig.UnicastPort)
        {
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            var buf = new Buffer(data);
            var pid = buf.ReadByte();

            if (pid <= 0x7F)
            {
                var state = new P00TrackerState(pid);
                state.Deserialize(buf);
                
            }
            else if (pid == 0x82)
            {
                var handshake = new P82Handshake();
                handshake.Deserialize(buf);


                Send(new P83HandshakeReply(), sender);
            }
        }

    }
}
