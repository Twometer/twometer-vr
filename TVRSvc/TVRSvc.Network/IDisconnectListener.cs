using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Network
{
    public interface IDisconnectListener
    {

        void OnDisconnect(Guid clientId);

    }
}
