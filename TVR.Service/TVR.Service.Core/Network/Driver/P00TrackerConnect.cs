using System;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Driver
{
    internal class P00TrackerConnect : IPacket
    {
        public byte Id => 0x00;

        public byte TrackerId { get; set; }

        public TrackerClass TrackerClass { get; set; }

        public TrackerColor TrackerColor { get; set; }

        public string SerialNo { get; set; }

        public void Deserialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Buffer buffer)
        {
            buffer.Write(TrackerId);
            buffer.Write((byte)TrackerClass);
            buffer.Write((byte)TrackerColor);
            buffer.Write(SerialNo);
        }
    }
}
