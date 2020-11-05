using System;
using System.Runtime.InteropServices;

namespace nextgentrackingdemo.Source
{
    [Serializable]
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct _Guid
    {
        uint data1;
        ushort data2;
        ushort data3;
        
    }
}
