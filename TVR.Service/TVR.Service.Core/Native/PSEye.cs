using System;
using System.Runtime.InteropServices;

namespace VR.Service.Core.Native
{
    internal enum PsEyeResolution : int
    {
        Qvga,
        Vga
    }

    internal enum PsEyeFormat : int
    {
        Bayer,
        Bgr,
        Rgb,
        Gray
    }

    internal class PSEye
    {
        [DllImport("Libraries/libps3eye.dll", EntryPoint = "open_camera", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr OpenCamera(int idx, PsEyeResolution resolution, int fps, PsEyeFormat format);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_auto_gain", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetAutoGain(IntPtr handle, bool autoGain);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_awb", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetAwb(IntPtr handle, bool awb);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_exposure", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetExposure(IntPtr handle, int exposure);
        
        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_gain", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetGain(IntPtr handle, int gain);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_framerate", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetFramerate(IntPtr handle, int fps);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "set_flip_status", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetFlipStatus(IntPtr handle, bool horizontal, bool vertical);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "get_frame", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetFrame(IntPtr handle, IntPtr data);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "exposure", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Exposure(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "stride", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Stride(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "width", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Width(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "height", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Height(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "error_code", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ErrorCode(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "error_string", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern string ErrorString(IntPtr handle);

        [DllImport("Libraries/libps3eye.dll", EntryPoint = "close_camera", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CloseCamera(IntPtr handle);
    }
}
