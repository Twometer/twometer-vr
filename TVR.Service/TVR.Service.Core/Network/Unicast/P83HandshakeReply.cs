using System;

namespace TVR.Service.Core.Network.Unicast
{
    internal class P83HandshakeReply : IPacket
    {
        public byte Id => 0x83;

        public byte TrackerId { get; set; }

        public void Deserialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Buffer buffer)
        {
            buffer.Write(TrackerId);
        }
    }
}
