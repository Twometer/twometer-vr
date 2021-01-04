using System;
using System.Numerics;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Driver
{
    internal class P02TrackerStates : IPacket
    {
        public byte Id => 0x02;

        public TrackerState[] States { get; set; }

        public void Deserialize(Buffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Buffer buffer)
        {
            buffer.Write((byte)States.Length);
            foreach (var state in States)
            {
                buffer.Write(state.TrackerId);
                buffer.Write(state.Buttons);
                buffer.Write(state.Position);
                buffer.Write(state.Rotation);
                buffer.Write((byte)(state.InRange ? 1 : 0));
            }
        }

        internal struct TrackerState
        {
            public byte TrackerId { get; }

            public ushort Buttons { get; }

            public Vector3 Position { get; }

            public Quaternion Rotation { get; }

            public bool InRange { get; }

            public TrackerState(byte trackerId, ushort buttons, Vector3 position, Quaternion rotation, bool inRange)
            {
                TrackerId = trackerId;
                Buttons = buttons;
                Position = position;
                Rotation = rotation;
                InRange = inRange;
            }

            public static TrackerState FromTracker(Tracker tracker)
            {
                return new TrackerState(tracker.TrackerId, tracker.Buttons, tracker.Position, tracker.Rotation, tracker.InRange);
            }
        }
    }
}
