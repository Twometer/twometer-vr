using System.Diagnostics;

namespace TVR.Service.Core.Logging
{
    public static class Loggers
    {
        public static ILogger Current { get; private set; } = Debugger.IsAttached ? (ILogger)new DebugLogger() : new ConsoleLogger();

        public static void Force(ILogger logger)
        {
            Current = logger;
        }
    }
}
