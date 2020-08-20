using System.IO;
using TVR.Service.Core.Model.Device;
using TVR.Service.Core.Tracking;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.Controllers
{
    public class ControllerInfoPacket : IPacket
    {
        public byte ControllerId { get; private set; }

        public Button[] PressedButtons { get; private set; }

        public ImuState ImuState { get; private set; } = new ImuState();

        public void Deserialize(BinaryReader reader)
        {
            ControllerId = reader.ReadByte();
            var pressedCount = reader.ReadByte();
            if (pressedCount > 0)
            {
                PressedButtons = new Button[pressedCount];
                for (var i = 0; i < PressedButtons.Length; i++)
                    PressedButtons[i] = (Button)reader.ReadByte();
            }

            ImuState.Ax = reader.ReadSingle();
            ImuState.Ay = reader.ReadSingle();
            ImuState.Az = reader.ReadSingle();

            ImuState.Gx = reader.ReadSingle();
            ImuState.Gy = reader.ReadSingle();
            ImuState.Gz = reader.ReadSingle();
            
            ImuState.Mx = reader.ReadSingle();
            ImuState.My = reader.ReadSingle();
            ImuState.Mz = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            // Never gets sent by the server
        }
    }
}
