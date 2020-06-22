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
        private Services services;
        private Thread updateThread;
        private Thread broadcastThread;

        private CancellationTokenSource tokenSource;
        private CancellationToken token;

        private int broadcastDelay;

        public void Start()
        {
            services = new Services();

            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            (updateThread = new Thread(UpdateLoop)).Start();
            (broadcastThread = new Thread(BroadcastLoop)).Start();

            services.ControllerServer.PacketReceived += ControllerServer_PacketReceived;
            services.ControllerServer.StatusChanged += ControllerServer_StatusChanged;

            broadcastDelay = (int)(1000.0f / services.Config.InputConfig.UpdateRate);
        }

        public void Stop()
        {
            if (services == null)
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

        private void UpdateLoop()
        {
            while (!token.IsCancellationRequested)
            {
                if (services.Camera.Update())
                    services.TrackingManager.UpdateVideo(services.Camera.HsvFrame, services.Camera.FrameBrightness);
            }
        }

        private void BroadcastLoop()
        {
            while (!token.IsCancellationRequested)
            {
                var start = DateTime.Now;
                services.DriverServer.Broadcast(new DriverPacket() { ControllerStates = services.TrackingManager.Trackers.Select(t => t.TrackedController).ToArray() });
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
            services.TrackingManager.UpdateMeta(e.ControllerId, e.Qx, e.Qy, e.Qz, e.Qw, e.PressedButtons);
        }
    }
}
