using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Model;
using TVR.Service.Network.Common;

namespace TVR.Service.Network.DriverServer
{
    public class DriverPacket : IPacket
    {
        public Controller[] ControllerStates { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            // Never received by the server
        }

        public void Serialize(BinaryWriter writer)
        {
            // Controller states

            if (ControllerStates != null)
            {
                writer.Write((byte)ControllerStates.Length);
                foreach (var c in ControllerStates)
                {
                    var offset = c.RotOffset ?? new Core.Math.Vec3(0, 0, 0);

                    writer.Write(c.Id);
                    writer.Write(c.Position.X);
                    writer.Write(c.Position.Y);
                    writer.Write(c.Position.Z - (c.ZOffset ?? 0));
                    writer.Write(c.Yaw - offset.X);
                    writer.Write(c.Pitch - offset.Y);
                    writer.Write(c.Roll - offset.Z);

                    writer.Write((byte)c.Buttons.Keys.Count);
                    foreach (var btn in c.Buttons)
                    {
                        writer.Write((byte)btn.Key);
                        writer.Write(btn.Value);
                    }
                }
            }
            else
            {
                writer.Write((byte)0);
            }
        }

    }
}

