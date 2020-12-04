using System.Net;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Network.Unicast
{
    internal class TrackerClient : BaseClient
    {
        private readonly ITrackerIdProvider idProvider;

        public delegate void TrackerUpdatedEventHandler(P00TrackerState trackerState);
        public delegate void TrackerConnectedEventHandler(Tracker tracker);

        public event TrackerUpdatedEventHandler TrackerUpdated;
        public event TrackerConnectedEventHandler TrackerConnected;

        public TrackerClient(ITrackerIdProvider idProvider) : base(NetConfig.UnicastPort)
        {
            this.idProvider = idProvider;
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

                TrackerUpdated?.Invoke(state);
            }
            else if (pid == 0x82)
            {
                var handshake = new P82Handshake();
                handshake.Deserialize(buf);

                var tracker = new Tracker(idProvider.NewId(), handshake.ModelNo, handshake.TrackerClass, handshake.TrackerColor);
                TrackerConnected?.Invoke(tracker);

                Send(new P83HandshakeReply() { TrackerId = tracker.TrackerId }, sender);
            }
        }

    }
}
