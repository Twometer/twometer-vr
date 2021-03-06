﻿using System;

namespace TVR.Service.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.Write($"{level.ToPrefix()} ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}
