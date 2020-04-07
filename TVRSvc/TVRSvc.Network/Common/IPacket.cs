using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Network.Common
{
    public interface IPacket
    {
        void Serialize(BinaryWriter writer);

    }
}
