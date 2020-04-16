using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.CLI
{
    class Program
    {
        public static void Main(string[] args)
        {
            TVRHost vrHost = new TVRHost();
            vrHost.Start();

            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd == "help")
                {
                    Console.WriteLine("help: Show this help");
                    Console.WriteLine("stop: Stop the service");
                }
                else if (cmd == "stop")
                {
                    Console.WriteLine("Shutting down...");
                    vrHost.Stop();
                    return;
                }
            }
        }
    }
}
