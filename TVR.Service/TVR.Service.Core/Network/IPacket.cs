using System.IO;

namespace TVR.Service.Core.Network
{
    internal interface IPacket
    {
        byte Id { get; }

        void Serialize(Buffer buffer);

        void Deserialize(Buffer buffer);
    }
}
