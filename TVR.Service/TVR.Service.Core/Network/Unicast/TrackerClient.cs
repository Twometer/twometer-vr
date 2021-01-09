using System;
using System.Net;
using TVR.Service.Core.Extensions;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Unicast
{
    internal class TrackerClient : BaseClient
    {
        private readonly TrackerManager trackerManager;

        public TrackerClient(TrackerManager trackerManager) : base(NetConfig.UnicastPort)
        {
            this.trackerManager = trackerManager;
            Loggers.Current.Log(LogLevel.Info, "Tracker client online");
        }

        protected override void OnReceive(byte[] data, IPEndPoint sender)
        {
            var buf = new Buffer(data);
            var pid = buf.ReadByte();

            if (pid <= 0x7F)
            {
                var state = new P00TrackerState(pid);
                state.Deserialize(buf);

                var tracker = trackerManager.GetTracker(state.TrackerId);
                tracker.Rotation = state.Rotation.ICM2SteamVR();
                tracker.Buttons = state.Buttons;
                tracker.LastHeartbeat = DateTime.Now;
            }
            else if (pid == 0x82) // Handshake packet
            {
                var handshake = new P82Handshake();
                handshake.Deserialize(buf);

                var tracker = new Tracker(trackerManager.NewId(), handshake.SerialNo, handshake.TrackerClass, handshake.TrackerColor);
                trackerManager.AddTracker(tracker);

                Send(new P83HandshakeReply() { TrackerId = tracker.TrackerId }, sender);
            }
        }

    }
}
