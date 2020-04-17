using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Logging
{
    public class DebugLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] [{level.ToPrefix()}] {message}");
        }
    }
}
