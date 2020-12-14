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
        private static IPEndPoint DriverEndpoint { get; } = new IPEndPoint(IPAddress.Loopback, NetConfig.DriverPort);

        public DriverClient(TrackerManager trackerManager) : base((ushort) DriverEndpoint.Port)
        {
            trackerManager.TrackerAdded += SendTrackerConnect;
            trackerManager.TrackerRemoved += SendTrackerDisconnect;
            Loggers.Current.Log(LogLevel.Info, "Driver client online");
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            // Driver does not currently send messages back
            throw new InvalidOperationException();
        }

        private void SendTrackerConnect(Tracker tracker)
        {
            var packet = new P00TrackerConnect() { TrackerId = tracker.TrackerId, ModelNo = tracker.ModelNo, TrackerClass = tracker.TrackerClass, TrackerColor = tracker.TrackerColor };
            Send(packet, DriverEndpoint);
        }

        private void SendTrackerDisconnect(Tracker tracker)
        {
            Send(new P01TrackerDisconnect() { TrackerId = tracker.TrackerId }, DriverEndpoint);
        }

        public void SendTrackerStates(IEnumerable<Tracker> trackers)
        {
            var states = trackers.Select(t => P02TrackerStates.TrackerState.FromTracker(t)).ToArray();
            Send(new P02TrackerStates() { States = states }, DriverEndpoint);
        }
    }
}
