using System.Collections.Generic;
using System.Linq;
using TVR.Service.Core.IO;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Unicast
{
    internal class TrackerWatchdog
    {
        private readonly TrackerManager trackerManager;

        private readonly IConfigProvider configProvider;

        public TrackerWatchdog(TrackerManager trackerManager, IConfigProvider configProvider)
        {
            this.trackerManager = trackerManager;
            this.configProvider = configProvider;
        }

        public void Update()
        {
            var staleTrackers = GetStaleTrackers();

            foreach (var tracker in staleTrackers)
                trackerManager.RemoveTracker(tracker.TrackerId);
        }

        private IEnumerable<Tracker> GetStaleTrackers()
            => trackerManager.Trackers.Where(t => t.TimeSinceLastHeartbeat.TotalSeconds > configProvider.UserConfig.Input.TrackerTimeout);
    }
}
