using System.Linq;
using TVR.Service.Core.Config;
using TVR.Service.Core.Logging;
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
            ControllerServer.StatusChanged += ControllerServer_StatusChanged; ;
        }

        private void ControllerServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.ControllerStatus)
            {
                case ControllerStatus.Connected:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} connected");
                    break;
                case ControllerStatus.BeginCalibrationMode:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} entered calibration mode");
                    break;
                case ControllerStatus.MagnetometerCalibration:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} started magnetometer calibration.");
                    break;
                case ControllerStatus.ExitCalibrationMode:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} completed calibration");
                    break;
                case ControllerStatus.BeginCalculatingOffsets:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} started calculating offets");
                    break;
                case ControllerStatus.Ready:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} is ready for use");
                    break;
                case ControllerStatus.Reset:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} did a factory reset");
                    break;
            }
        }

        private void ControllerServer_PacketReceived(object sender, ControllerInfoPacket e)
        {
            TrackerManager.UpdateMeta(e.ControllerId, e.Qx, e.Qy, e.Qz, e.Qw, e.PressedButtons);
        }

        public void Update()
        {
            Camera.Update();
            TrackerManager.UpdateVideo(Camera.HsvFrame, Camera.FrameBrightness);
            if (!Calibration.IsCalibrated)
                Calibration.Update(Camera.FrameBrightness);
        }

        public void Broadcast()
        {
            DriverServer.Broadcast(new DriverPacket() { ControllerStates = TrackerManager.Trackers.Select(t => t.Controller).ToArray() });
        }
    }
}
