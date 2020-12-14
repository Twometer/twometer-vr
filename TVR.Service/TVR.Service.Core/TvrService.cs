using System.Diagnostics;
using System.Reflection;
using System.Threading;
using TVR.Service.Core.IO;
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
        private readonly IConfigProvider configProvider;
        private readonly TrackerManager trackerManager;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly CancellationToken cancellationToken;

        private IVideoSource videoSource;

        private TrackingEngine trackingEngine;

        private DiscoveryClient discoveryClient;
        private DriverClient driverClient;
        private TrackerClient trackerClient;
        private TrackerWatchdog watchdog;

        private Thread videoThread;
        private Timer updateTimer;

        public TvrService() : this(new FileConfigProvider())
        {
        }

        public TvrService(IConfigProvider configProvider)
        {
            this.configProvider = configProvider;
            this.trackerManager = new TrackerManager();
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = cancellationTokenSource.Token;
        }

        public void Start()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Loggers.Current.Log(LogLevel.Info, $"Starting TwometerVR v{Version}");


            Loggers.Current.Log(LogLevel.Debug, "Opening video stream");
            var videoConfig = configProvider.VideoSourceConfig;
            videoSource = VideoSourceFactory.Create(videoConfig.VideoSourceType, videoConfig.VideoSourceIndex);
            videoSource.Width = videoConfig.FrameWidth;
            videoSource.Height = videoConfig.FrameHeight;
            videoSource.Framerate = videoConfig.Framerate;
            videoSource.Open();


            Loggers.Current.Log(LogLevel.Debug, "Initializing tracking engine");
            trackingEngine = new TrackingEngine(trackerManager, configProvider, videoSource);


            Loggers.Current.Log(LogLevel.Debug, "Setting up network");
            discoveryClient = new DiscoveryClient();
            driverClient = new DriverClient(trackerManager);
            trackerClient = new TrackerClient(trackerManager);
            watchdog = new TrackerWatchdog(trackerManager, configProvider);


            Loggers.Current.Log(LogLevel.Debug, "Initializing update timer");
            updateTimer = new Timer(Update, null, 0, 1 / configProvider.UserConfig.Input.RefreshRate);


            Loggers.Current.Log(LogLevel.Debug, "Starting video procecssing thread");
            videoThread = new Thread(VideoLoop);
            videoThread.Start();


            stopwatch.Stop();
            Loggers.Current.Log(LogLevel.Info, $"Done after {stopwatch.Elapsed}");
        }

        public void Stop()
        {
            Loggers.Current.Log(LogLevel.Info, "Shutting down...");

            cancellationTokenSource.Cancel();
            videoThread.Join();

            trackerManager.Clear();
            videoSource.Dispose();
            updateTimer.Dispose();
            discoveryClient.Close();
            driverClient.Close();
            trackerClient.Close();
        }

        private void VideoLoop()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (videoSource.Grab())
                    trackingEngine.Update();
            }
        }

        private void Update(object stateInfo)
        {
            watchdog.Update();
            driverClient.SendTrackerStates(trackerManager.Trackers);
        }

        public static string Version
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{ver.Major}.{ver.Minor}.{ver.Build}";
            }
        }
    }
}
