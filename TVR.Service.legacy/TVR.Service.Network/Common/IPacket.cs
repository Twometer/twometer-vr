using System.IO;

namespace TVR.Service.Network.Common
{
    public interface IPacket
    {
        void Serialize(BinaryWriter writer);

        void Deserialize(BinaryReader reader);
    }
}
