using System;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Unicast
{
    internal class P82Handshake : IPacket
    {
        public byte Id => 0x82;

        public TrackerClass TrackerClass { get; private set; }

        public TrackerColor TrackerColor { get; private set; }

        public string ModelNo { get; private set; }

        public void Deserialize(Buffer buffer)
        {
            TrackerClass = (TrackerClass)buffer.ReadByte();
            TrackerColor = (TrackerColor)buffer.ReadByte();
            ModelNo = buffer.ReadString();
        }

        public void Serialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
