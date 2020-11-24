using System.IO;

namespace TVR.Service.Core.Network
{
    internal interface IPacket
    {
        void Serialize(Buffer buffer);

        void Deserialize(Buffer buffer);
    }
}
