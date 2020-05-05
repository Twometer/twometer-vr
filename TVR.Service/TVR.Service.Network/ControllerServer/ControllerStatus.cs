using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
