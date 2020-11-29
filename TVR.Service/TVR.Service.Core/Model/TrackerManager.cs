using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TVR.Service.Core.Model
{
    internal class TrackerManager
    {
        private int idCounter;

        private readonly IDictionary<byte, Tracker> trackers = new ConcurrentDictionary<byte, Tracker>();

        public void AddTracker(Tracker tracker)
        {
            trackers.Add(tracker.TrackerId, tracker);
        }

        public Tracker GetTracker(byte id)
        {
            return trackers[id];
        }

        public IEnumerable<Tracker> GetStaleTrackers()
        {
            var now = DateTime.Now;
            foreach (var tracker in trackers.Values)
            {
                var timeSinceHeartbeat = now - tracker.LastHeartbeat;
                if (timeSinceHeartbeat.TotalSeconds > 30)
                {
                    yield return tracker;
                }
            }
        }

        public void RemoveTracker(byte id)
        {
            trackers.Remove(id);
        }

        public byte NewId()
        {
            return (byte)Interlocked.Increment(ref idCounter);
        }

    }
}
