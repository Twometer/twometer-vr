using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TVR.Service.Core.Logging;
using TVR.Service.Network.Controllers;
using TVR.Service.Network.Driver;

namespace TVR.Service.Common
{
    public class ServiceHost
    {
        public Services Services { get; private set; }

        private Thread updateThread;
        private Thread broadcastThread;

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private int broadcastDelay;

        public void Start()
        {
            Services = new Services();

            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            (updateThread = new Thread(UpdateLoop)).Start();
            (broadcastThread = new Thread(BroadcastLoop)).Start();

            Services.ControllerServer.PacketReceived += ControllerServer_PacketReceived;
            Services.ControllerServer.StatusChanged += ControllerServer_StatusChanged;

            broadcastDelay = (int)(1000.0f / Services.Config.InputConfig.UpdateRate);
        }

        public void Stop()
        {
            if (Services == null)
                throw new InvalidOperationException("Cannot stop a service that's already stopped!");

            tokenSource.Cancel();
            updateThread?.Join();
            broadcastThread?.Join();
        }

        public Task StopAsync()
        {
            return Task.Run(() => Stop());
        }

        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }

        private async void UpdateLoop()
        {
            while (!token.IsCancellationRequested)
            {
                if (Services.Camera.Update())
                    await Services.TrackingManager.UpdateVideo(Services.Camera.HsvFrame, Services.Camera.FrameBrightness);
            }
        }

        private void BroadcastLoop()
        {
            while (!token.IsCancellationRequested)
            {
                var start = DateTime.Now;
                Services.DriverServer.Broadcast(new DriverPacket() { ControllerStates = Services.TrackingManager.Trackers.Select(t => t.TrackedController).ToArray() });
                var broadcastDuration = (int)(DateTime.Now - start).TotalMilliseconds;

                var timeout = broadcastDelay - broadcastDuration;
                if (timeout > 0)
                    Thread.Sleep(timeout);
            }
        }

        private void ControllerServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.StatusMessage)
            {
                case StatusMessage.Connected:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} connected");
                    break;
                case StatusMessage.BeginCalibrationMode:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} entered calibration mode");
                    break;
                case StatusMessage.MagnetometerCalibration:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} started magnetometer calibration.");
                    break;
                case StatusMessage.ExitCalibrationMode:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} completed calibration");
                    break;
                case StatusMessage.BeginCalculatingOffsets:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} started calculating offets");
                    break;
                case StatusMessage.Ready:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} is ready for use");
                    break;
                case StatusMessage.Reset:
                    LoggerFactory.Current.Log(LogLevel.Info, $"Controller at endpoint {e.ControllerEndpoint} did a factory reset");
                    break;
            }
        }

        private void ControllerServer_PacketReceived(object sender, ControllerInfoPacket e)
        {
            Services.TrackingManager.UpdateMeta(e.ControllerId, e.Qx, e.Qy, e.Qz, e.Qw, e.PressedButtons);
        }
    }
}
