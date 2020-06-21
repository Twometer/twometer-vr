using System;

namespace TVR.Service.Common
{
    public class ServiceHost
    {
        private Services services;

        public void Start()
        {
            services = new Services();
        }

        public void Stop()
        {
            if (services == null)
                throw new InvalidOperationException("Cannot stop a service that's already stopped!");
        }

    }
}
