using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Model;
using TVRSvc.Network.Common;

namespace TVRSvc.Network.ControllerServer
{
    public class ControllerInfoPacket : IPacket
    {
        public byte ControllerId { get; set; }

        public Button[] PressedButtons { get; set; }

        public float AccelX { get; set; }

        public float AccelY { get; set; }

        public float AccelZ { get; set; }

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

            AccelX = reader.ReadSingle();
            AccelY = reader.ReadSingle();
            AccelZ = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            // Never gets sent by the server
        }
    }
}
