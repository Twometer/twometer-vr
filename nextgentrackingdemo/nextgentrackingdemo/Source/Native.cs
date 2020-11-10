using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace nextgentrackingdemo.Source
{
    public static class Native
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

        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CLEyeGetCameraCount();
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern Guid CLEyeGetCameraUUID(int camId);
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CLEyeCreateCamera(Guid camUUID, CLEyeCameraColorMode mode, CLEyeCameraResolution res, float frameRate);
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeDestroyCamera(IntPtr camera);
       
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraStart(IntPtr camera);
       
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraStop(IntPtr camera);
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraLED(IntPtr camera, bool on);
    
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeSetCameraParameter(IntPtr camera, CLEyeCameraParameter param, int value);
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int CLEyeGetCameraParameter(IntPtr camera, CLEyeCameraParameter param);
      
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraGetFrameDimensions(IntPtr camera, ref int width, ref int height);
       
        [DllImport("CLEyeMulticam.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CLEyeCameraGetFrame(IntPtr camera, IntPtr pData, int waitTimeout);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UnmapViewOfFile(IntPtr hMap);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);
    }
}
