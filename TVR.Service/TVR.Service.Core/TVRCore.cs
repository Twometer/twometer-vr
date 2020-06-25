using System;
using System.Reflection;

namespace TVR.Service.Core
{
    public class TVRCore
    {
        public static Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
