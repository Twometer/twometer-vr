using System;
using TVR.Service.Core.IO;
using TVR.Service.Core.Logging;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Video
{
    internal class AutoExposure
    {
        private readonly ExposureConfig config;
        private readonly IVideoSource videoSource;

        public bool Finished { get; private set; }

        private int exposure;
        private int frames;
        private int cooldown;

        private double prevBrightness;
        private int eqBrightnessCounter;

        public AutoExposure(IConfigProvider configProvider, IVideoSource videoSource)
        {
            this.config = configProvider.VideoSourceConfig.ExposureConfig;
            this.videoSource = videoSource;
        }

        public void Readjust()
        {
            frames = 0;
            Finished = false;
        }

        public void Update()
        {
            if (frames == 0)
                Initialize();


            frames++;
            if (frames < config.WarmupFrames)
                return;

            if (cooldown > 0)
            {
                cooldown--;
                return;
            }

            var brightness = videoSource.FrameBrightness;
            if (brightness > config.Threshold && HasBrightnessChanged(brightness) && !IsLimitReached())
            {
                exposure += config.Step;
                cooldown = config.CooldownFrames;
                videoSource.Exposure = exposure;
                Loggers.Current.Log(LogLevel.Debug, $"Auto exposure level: {exposure}");
            }
            else
            {
                Finished = true;
                Loggers.Current.Log(LogLevel.Debug, $"Auto exposure finished with level {exposure}");
            }
        }

        private void Initialize()
        {
            Loggers.Current.Log(LogLevel.Debug, "Running auto exposure");
            exposure = config.Start;
            videoSource.Exposure = exposure;
        }

        private bool IsLimitReached()
        {
            if (config.Step > 0)
                return exposure > config.Limit;
            else
                return exposure < config.Limit;
        }

        private bool HasBrightnessChanged(double brightness)
        {
            if (Math.Abs(brightness - prevBrightness) < config.MaximumChange)
                eqBrightnessCounter++;
            else eqBrightnessCounter = 0;

            prevBrightness = brightness;

            return eqBrightnessCounter < config.StableFrames;
        }
    }
}
