using System.Diagnostics;

namespace TVR.Service.Core.Logging
{
    public class LoggerFactory
    {

        public static ILogger Current { get; } = Debugger.IsAttached ? (ILogger)new DebugLogger() : new ConsoleLogger();

    }
}
