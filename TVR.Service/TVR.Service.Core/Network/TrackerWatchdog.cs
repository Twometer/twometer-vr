using System.Collections.Generic;
using System.Linq;
using TVR.Service.Core.Model;
using TVR.Service.Core.Network.Driver;

namespace TVR.Service.Core.Network
{
    internal class TrackerWatchdog
    {
        private readonly UserConfig config;

        private readonly TrackerManager trackerManager;

        private readonly DriverClient driverClient;

        public void Update()
        {
            var staleTrackers = GetStaleTrackers();

            foreach (var tracker in staleTrackers)
            {
                driverClient.HandleTrackerDisconnect(tracker);
                trackerManager.RemoveTracker(tracker.TrackerId);
            }
        }

        private IEnumerable<Tracker> GetStaleTrackers()
            => trackerManager.Trackers.Where(t => t.TimeSinceLastHeartbeat.TotalSeconds > config.Input.TrackerTimeout);
    }
}
