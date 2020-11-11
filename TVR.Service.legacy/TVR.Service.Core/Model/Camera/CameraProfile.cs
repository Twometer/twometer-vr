namespace TVR.Service.Core.Model.Camera
{
    public class CameraProfile
    {
        public string Identifier { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public CameraParameters CameraParameters { get; set; }

        public CalibrationParameters CalibrationParameters { get; set; }

        public ColorProfile[] ColorProfiles { get; set; }
    }
}
