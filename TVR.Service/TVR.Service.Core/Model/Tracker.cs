using System;
using TVR.Service.Core.Math;

namespace TVR.Service.Core.Model
{
    public struct Tracker
    {
        public byte TrackerId { get; set; }

        public string ModelNo { get; set; }

        public TrackerClass TrackerClass { get; set; }

        public TrackerColor TrackerColor { get; set; }

        public ushort Buttons { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public DateTime LastHeartbeat { get; set; }
    }
}
