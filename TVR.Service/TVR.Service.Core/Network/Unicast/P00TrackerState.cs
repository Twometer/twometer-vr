using System;
using System.Numerics;

namespace TVR.Service.Core.Network.Unicast
{
    internal class P00TrackerState : IPacket
    {
        public byte Id { get; }

        public byte TrackerId { get; }

        public ushort Buttons { get; set; }

        public Quaternion Rotation { get; set; }

        public P00TrackerState(byte id)
        {
            Id = id;
            TrackerId = id;
        }

        public void Deserialize(Buffer buffer)
        {
            Buttons = buffer.ReadUshort();
            Rotation = buffer.ReadQuaternion();
        }

        public void Serialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
