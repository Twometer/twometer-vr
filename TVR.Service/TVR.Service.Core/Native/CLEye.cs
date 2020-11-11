using System;
using System.Runtime.InteropServices;

namespace TVR.Service.Core.Native
{
    public static class CLEye
    {
        public enum CLEyeCameraColorMode
        {
            CLEYE_MONO_PROCESSED,
            CLEYE_COLOR_PROCESSED,
            CLEYE_MONO_RAW,
            CLEYE_COLOR_RAW,
            CLEYE_BAYER_RAW
        };

        public enum CLEyeCameraResolution
        {
            CLEYE_QVGA,
            CLEYE_VGA
        };

        public enum CLEyeCameraParameter
        {
            // camera sensor parameters
            CLEYE_AUTO_GAIN,            // [false, true]
            CLEYE_GAIN,                 // [0, 79]
            CLEYE_AUTO_EXPOSURE,        // [false, true]
            CLEYE_EXPOSURE,             // [0, 511]
            CLEYE_AUTO_WHITEBALANCE,    // [false, true]
            CLEYE_WHITEBALANCE_RED,     // [0, 255]
            CLEYE_WHITEBALANCE_GREEN,   // [0, 255]
            CLEYE_WHITEBALANCE_BLUE,    // [0, 255]
                                        // camera linear transform parameters
            CLEYE_HFLIP,                // [false, true]
            CLEYE_VFLIP,                // [false, true]
            CLEYE_HKEYSTONE,            // [-500, 500]
            CLEYE_VKEYSTONE,            // [-500, 500]
            CLEYE_XOFFSET,              // [-500, 500]
            CLEYE_YOFFSET,              // [-500, 500]
            CLEYE_ROTATION,             // [-500, 500]
            CLEYE_ZOOM,                 // [-500, 500]
                                        // camera non-linear transform parameters
            CLEYE_LENSCORRECTION1,      // [-500, 500]
            CLEYE_LENSCORRECTION2,      // [-500, 500]
            CLEYE_LENSCORRECTION3,      // [-500, 500]
            CLEYE_LENSBRIGHTNESS        // [-500, 500]
        };

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CLEyeGetCameraCount();

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Guid CLEyeGetCameraUUID(int camId);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CLEyeCreateCamera(Guid camUUID, CLEyeCameraColorMode mode, CLEyeCameraResolution res, float frameRate);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeDestroyCamera(IntPtr camera);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraStart(IntPtr camera);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraStop(IntPtr camera);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraLED(IntPtr camera, bool on);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeSetCameraParameter(IntPtr camera, CLEyeCameraParameter param, int value);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CLEyeGetCameraParameter(IntPtr camera, CLEyeCameraParameter param);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraGetFrameDimensions(IntPtr camera, ref int width, ref int height);

        [DllImport("Libraries\\CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraGetFrame(IntPtr camera, IntPtr pData, int waitTimeout);
    }
}
