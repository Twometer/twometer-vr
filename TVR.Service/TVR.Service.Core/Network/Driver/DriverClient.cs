using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Driver
{
    internal class DriverClient : BaseClient
    {
        private readonly TrackerManager trackerManager;

        private IPEndPoint driverEndpoint;

        public DriverClient(TrackerManager trackerManager) : base(NetConfig.DriverPort)
        {
            this.trackerManager = trackerManager;
            trackerManager.TrackerAdded += SendTrackerConnect;
            trackerManager.TrackerRemoved += SendTrackerDisconnect;
            Loggers.Current.Log(LogLevel.Info, "Driver client online");
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            if (data.Length == 1 && data[0] == 0x03) // Request information packet
            {
                driverEndpoint = sender;
                Loggers.Current.Log(LogLevel.Info, "Driver registered");

                foreach (var tracker in trackerManager.Trackers)
                {
                    SendTrackerConnect(tracker);
                }
            }
        }

        private void SendTrackerConnect(Tracker tracker)
        {
            if (driverEndpoint == null) return;

            var packet = new P00TrackerConnect() { TrackerId = tracker.TrackerId, SerialNo = tracker.SerialNo, TrackerClass = tracker.TrackerClass, TrackerColor = tracker.TrackerColor };
            Send(packet, driverEndpoint);
        }

        private void SendTrackerDisconnect(Tracker tracker)
        {
            if (driverEndpoint == null) return;
            
            Send(new P01TrackerDisconnect() { TrackerId = tracker.TrackerId }, driverEndpoint);
        }

        public void SendTrackerStates(IEnumerable<Tracker> trackers)
        {
            if (driverEndpoint == null) return;

            var states = trackers.Select(t => P02TrackerStates.TrackerState.FromTracker(t)).ToArray();
            Send(new P02TrackerStates() { States = states }, driverEndpoint);
        }
    }
}
