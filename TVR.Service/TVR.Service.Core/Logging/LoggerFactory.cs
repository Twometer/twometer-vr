using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Logging
{
    public class LoggerFactory
    {

        public static ILogger Current { get; } = Debugger.IsAttached ? (ILogger)new DebugLogger() : new ConsoleLogger();

    }
}
