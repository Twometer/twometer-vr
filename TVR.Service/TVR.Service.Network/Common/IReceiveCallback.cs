using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Network.Common
{
    public interface IReceiveCallback
    {

        void OnPacket(MemoryStream data);

    }
}
