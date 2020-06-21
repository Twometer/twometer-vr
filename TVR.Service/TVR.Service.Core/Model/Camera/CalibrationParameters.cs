namespace TVR.Service.Model.Camera
{
    public class CalibrationParameters
    {
        public double BrightnessThreshold { get; set; }

        public double WarmupFrames { get; set; }

        public double CooldownFrames { get; set; }

        public double StableFrames { get; set; }
    }
}
