using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace TVR.Service.Core.Model
{
    internal class TrackerManager : ITrackerIdProvider
    {
        private int idCounter;

        private readonly IDictionary<byte, Tracker> trackers = new ConcurrentDictionary<byte, Tracker>();

        public ICollection<Tracker> Trackers => trackers.Values;

        public void AddTracker(Tracker tracker)
        {
            trackers.Add(tracker.TrackerId, tracker);
        }

        public Tracker GetTracker(byte id)
        {
            return trackers[id];
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
