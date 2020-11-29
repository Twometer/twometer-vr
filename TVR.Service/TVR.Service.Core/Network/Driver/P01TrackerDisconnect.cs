using System;

namespace TVR.Service.Core.Network.Driver
{
    internal class P01TrackerDisconnect : IPacket
    {
        public byte Id => 0x01;

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
