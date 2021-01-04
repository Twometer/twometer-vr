using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using TVR.Service.Core.Logging;

namespace TVR.Service.Core.Model
{
    public class TrackerManager
    {
        private int idCounter;

        private readonly IDictionary<byte, Tracker> trackers = new ConcurrentDictionary<byte, Tracker>();

        public delegate void TrackerAddedEventHandler(Tracker tracker);
        public delegate void TrackerRemovedEventHandler(Tracker tracker);

        public event TrackerAddedEventHandler TrackerAdded;
        public event TrackerRemovedEventHandler TrackerRemoved;

        public IEnumerable<Tracker> Trackers => trackers.Values;

        public void AddTracker(Tracker tracker)
        {
            Loggers.Current.Log(LogLevel.Debug, $"Tracker {tracker.SerialNo} connected with id {tracker.TrackerId}");

            TrackerAdded?.Invoke(tracker);
            trackers.Add(tracker.TrackerId, tracker);
        }

        public void RemoveTracker(byte id)
        {
            var tracker = trackers[id];
            Loggers.Current.Log(LogLevel.Debug, $"Tracker {tracker.SerialNo} (id {tracker.TrackerId}) disconnected");

            trackers.Remove(tracker.TrackerId);
            TrackerRemoved?.Invoke(tracker);
        }

        public void Clear()
        {
            foreach (var tracker in Trackers)
                RemoveTracker(tracker.TrackerId);
        }

        public Tracker GetTracker(byte id)
        {
            return trackers[id];
        }

        public byte NewId()
        {
            return (byte)Interlocked.Increment(ref idCounter);
        }

    }
}
