using System;
using TVR.Service.Common;
using TVR.Service.Core.Logging;

namespace TVR.Service.CLI
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceHost = new ServiceHost();

            LoggerFactory.ForceLogger(new ConsoleLogger());
            LoggerFactory.Current.Log(LogLevel.Info, "TwometerVR starting up...");
            LoggerFactory.Current.Log(LogLevel.Info, "Type 'stop' to shut down the service");

            serviceHost.Start();
            LoggerFactory.Current.Log(LogLevel.Info, "Initialized");

            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd == "stop")
                {
                    LoggerFactory.Current.Log(LogLevel.Info, "Shutting down...");
                    serviceHost.Stop();
                    return;
                }
            }
        }
    }
}
