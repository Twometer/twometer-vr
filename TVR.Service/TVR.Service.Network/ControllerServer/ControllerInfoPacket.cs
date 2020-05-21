using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Model;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.ControllerServer
{
    public class ControllerInfoPacket : IPacket
    {
        public byte ControllerId { get; set; }

        public Button[] PressedButtons { get; set; }

        public float Qx { get; set; }

        public float Qy { get; set; }

        public float Qz { get; set; }

        public float Qw { get; set; }

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

            Qx = reader.ReadSingle();
            Qy = reader.ReadSingle();
            Qz = reader.ReadSingle();
            Qw = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            // Never gets sent by the server
        }
    }
}
