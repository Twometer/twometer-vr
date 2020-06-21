namespace TVR.Service.Core.Model.Camera
{
    public class CalibrationParameters
    {
        public double BrightnessThreshold { get; set; }

        public int WarmupFrames { get; set; }

        public int CooldownFrames { get; set; }

        public int StableFrames { get; set; }
    }
}
