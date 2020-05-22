using System.IO;

namespace TVR.Service.Network.Common
{
    public interface IReceiveCallback
    {

        void OnPacket(MemoryStream data);

    }
}
