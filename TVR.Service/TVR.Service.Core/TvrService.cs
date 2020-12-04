using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;
using TVR.Service.Core.Network.Broadcast;
using TVR.Service.Core.Network.Driver;
using TVR.Service.Core.Network.Unicast;
using TVR.Service.Core.Tracking;
using TVR.Service.Core.Video;

namespace TVR.Service.Core
{
    public class TvrService
    {
        public string Version
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }

        private readonly TrackerManager trackerManager = new TrackerManager();
        private readonly TrackingEngine trackingEngine = new TrackingEngine();

        private DiscoveryClient discoveryClient;
        private DriverClient driverClient;
        private TrackerClient trackerClient;

        private Timer updateTimer;

        private IVideoSource videoSource;

        public void Start()
        {
            Loggers.Current.Log(LogLevel.Info, $"Starting TwometerVR v{Version}");

            discoveryClient = new DiscoveryClient();
            driverClient = new DriverClient();

            trackerClient = new TrackerClient(trackerManager);
            trackerClient.TrackerConnected += TrackerClient_TrackerConnected;
            trackerClient.TrackerUpdated += TrackerClient_TrackerUpdated;

            updateTimer = new Timer(Update, null, 0, 16);

            videoSource = VideoSourceFactory.Create(VideoSourceType.PSEye, 0);
            videoSource.Open();

            // TODO: Make everything configurable
        }

        public void Stop()
        {
            foreach (var tracker in trackerManager.Trackers)
                driverClient.HandleTrackerDisconnect(tracker);

            videoSource.Dispose();
            updateTimer.Dispose();
            discoveryClient.Close();
            driverClient.Close();
            trackerClient.Close();
        }

        private void Update(object stateInfo)
        {
            foreach (var tracker in trackerManager.GetStaleTrackers())
            {
                driverClient.HandleTrackerDisconnect(tracker);
                trackerManager.RemoveTracker(tracker.TrackerId);
            }

            driverClient.HandleStateChange(trackerManager.Trackers);
        }

        private void TrackerClient_TrackerUpdated(P00TrackerState trackerState)
        {
            var tracker = trackerManager.GetTracker(trackerState.TrackerId);
            tracker.Rotation = trackerState.Rotation;
            tracker.LastHeartbeat = DateTime.Now;
        }

        private void TrackerClient_TrackerConnected(Tracker tracker)
        {
            trackerManager.AddTracker(tracker);
        }
    }
}
