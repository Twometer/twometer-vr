using System;

namespace TVR.Service.Core.Network.Broadcast
{
    internal class P81DiscoveryReply : IPacket
    {
        public byte Id => 0x81;

        public string ServerIp { get; set; }

        public void Deserialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Buffer buffer)
        {
            buffer.Write(ServerIp);
        }
    }
}
