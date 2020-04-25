using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Config;
using TVR.Service.Core.Tracking;
using TVR.Service.Core.Video;
using TVR.Service.Network.ControllerServer;
using TVR.Service.Network.Discovery;
using TVR.Service.Network.DriverServer;

namespace TVR.Service.Common
{
    public class ServiceContext
    {
        public TVRConfig Config { get; set; }

        public TrackerManager TrackerManager { get; }

        public Camera Camera { get; }

        public Calibration Calibration { get; }

        public DiscoveryService Discovery { get; }

        public DriverServer DriverServer { get; }

        public ControllerServer ControllerServer { get; }

        public ServiceContext(string configFile)
        {
            Config = TVRConfig.Load(configFile);
            TrackerManager = new TrackerManager(Config);
            Camera = new Camera(Config);
            Calibration = new Calibration(Config, Camera, TrackerManager);
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
        }

        public void Broadcast()
        {
            //if (TrackerManager.Detected || ControllerServer.ConnectedClientCount >= 2)
            DriverServer.Broadcast(new DriverPacket() { ControllerStates = TrackerManager.Trackers.Select(t => t.Controller).ToArray() });
        }

    }
}
