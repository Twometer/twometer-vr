using System;
using System.Numerics;

namespace TVR.Service.Core.Model
{
    public class Tracker
    {
        public byte TrackerId { get; set; }

        public string SerialNo { get; set; }

        public TrackerClass TrackerClass { get; set; }

        public TrackerColor TrackerColor { get; set; }

        public ushort Buttons { get; set; }

        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public DateTime LastHeartbeat { get; set; }

        public TimeSpan TimeSinceLastHeartbeat => DateTime.Now - LastHeartbeat;

        public bool InRange { get; set; }

        public Tracker(byte trackerId, string serialNo, TrackerClass trackerClass, TrackerColor trackerColor)
        {
            TrackerId = trackerId;
            SerialNo = serialNo;
            TrackerClass = trackerClass;
            TrackerColor = trackerColor;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            LastHeartbeat = DateTime.Now;
        }
    }
}
