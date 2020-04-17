using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TVR.Service.Common;

namespace TVR.Service.CLI
{
    public class TVRHost
    {
        private ServiceContext serviceContext;

        private Thread updateThread;
        private Thread broadcastThread;

        private volatile bool running = true;

        public void Start()
        {
            serviceContext = new ServiceContext("tvrconfig.json");

            (updateThread = new Thread(UpdateLoop)).Start();
            (broadcastThread = new Thread(BroadcastLoop)).Start();
        }

        public void Stop()
        {
            running = false;
            updateThread?.Join();
            broadcastThread?.Join();
        }

        private void UpdateLoop()
        {
            while (running)
            {
                serviceContext.Update();
            }
        }

        private void BroadcastLoop()
        {
            while (running)
            {
                serviceContext.Broadcast();

                Thread.Sleep(17); // 60 updates per second for smooth movement
            }
        }

    }
}
