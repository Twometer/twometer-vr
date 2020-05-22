namespace TVR.Service.Network.ControllerServer
{
    public enum ControllerStatus
    {
        Connected,
        BeginCalibrationMode,
        MagnetometerCalibration,
        ExitCalibrationMode,
        BeginCalculatingOffsets,
        Ready,
        Reset
    }
}
