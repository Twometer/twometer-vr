using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Tracking;
using TVRSvc.Core.Video;
using TVRSvc.Network.ControllerServer;
using TVRSvc.Network.Discovery;
using TVRSvc.Network.DriverServer;

namespace TVRSvc.Service
{
    public class ServiceContext
    {
        public TrackerManager TrackerManager { get; }

        public Camera Camera { get; }

        public Calibration Calibration { get; }

        public DiscoveryService Discovery { get; }

        public DriverServer DriverServer { get; }

        public ControllerServer ControllerServer { get; }

        public ServiceContext()
        {
            TrackerManager = new TrackerManager();
            Camera = new Camera();
            Calibration = new Calibration(Camera, TrackerManager);
            Discovery = new DiscoveryService();
            DriverServer = new DriverServer();
            ControllerServer = new ControllerServer();

            ControllerServer.PacketReceived += ControllerServer_PacketReceived;
        }

        private void ControllerServer_PacketReceived(object sender, ControllerInfoPacket e)
        {
            TrackerManager.UpdateMeta(e.ControllerId, e.Yaw, e.Pitch, e.Roll, e.PressedButtons);
        }

        public void Update()
        {
            var frame = Camera.QueryFrame();
            TrackerManager.UpdateVideo(frame);

            if (!Calibration.IsCalibrated)
            {
                Calibration.Update(frame);
            }
            else
            {
                if (TrackerManager.Detected)
                    DriverServer.Broadcast(new DriverPacket() { ControllerStates = TrackerManager.Trackers.Select(t => t.Controller).ToArray() });
            }
        }

    }
}
