using Emgu.CV;
using Emgu.CV.Structure;
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
        public IConfigProvider ConfigProvider { get; }
        public TrackerManager TrackerManager { get; } = new TrackerManager();
        public IVideoSource VideoSource => videoSource;
        public Image<Gray, byte> DebugFrame => trackingEngine.DebugFrame;

        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly CancellationToken cancellationToken;

        private IVideoSource videoSource;
        private AutoExposure autoExposure;

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
            ConfigProvider = configProvider;
            TrackerManager = new TrackerManager();
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = cancellationTokenSource.Token;
        }

        public void Start()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Loggers.Current.Log(LogLevel.Info, $"Starting TwometerVR v{Version}");

            Loggers.Current.Log(LogLevel.Debug, "Loading config file...");
            ConfigProvider.Load();

            Loggers.Current.Log(LogLevel.Debug, "Opening video stream");
            var videoConfig = ConfigProvider.VideoSourceConfig;
            videoSource = VideoSourceFactory.Create(videoConfig.VideoSourceType, videoConfig.VideoSourceIndex);
            videoSource.Width = videoConfig.FrameWidth;
            videoSource.Height = videoConfig.FrameHeight;
            videoSource.Framerate = videoConfig.Framerate;
            videoSource.Open();

            autoExposure = new AutoExposure(ConfigProvider, videoSource);

            Loggers.Current.Log(LogLevel.Debug, "Initializing tracking engine");
            trackingEngine = new TrackingEngine(TrackerManager, ConfigProvider, videoSource);


            Loggers.Current.Log(LogLevel.Debug, "Setting up network");
            discoveryClient = new DiscoveryClient();
            driverClient = new DriverClient(TrackerManager);
            trackerClient = new TrackerClient(TrackerManager);
            watchdog = new TrackerWatchdog(TrackerManager, ConfigProvider);


            Loggers.Current.Log(LogLevel.Debug, "Initializing update timer");
            updateTimer = new Timer(Update, null, 0, 1000 / ConfigProvider.UserConfig.Input.RefreshRate);


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

            TrackerManager.Clear();
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
                    OnNewFrame();
            }
        }

        private void OnNewFrame()
        {
            if (autoExposure.Finished)
                trackingEngine.Update();
            else
                autoExposure.Update();
        }

        private void Update(object stateInfo)
        {
            watchdog.Update();
            driverClient.SendTrackerStates(TrackerManager.Trackers);
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
