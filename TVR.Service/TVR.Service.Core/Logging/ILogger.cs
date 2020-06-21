namespace TVR.Service.Core.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
