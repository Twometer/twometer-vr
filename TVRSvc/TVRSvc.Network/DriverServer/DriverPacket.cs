using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Model;
using TVRSvc.Network.Common;

namespace TVRSvc.Network.DriverServer
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
                    writer.Write(c.Id);
                    writer.Write(c.Position.X);
                    writer.Write(c.Position.Y);
                    writer.Write(c.Position.Z - (c.ZOffset ?? 0));
                    writer.Write(c.Yaw - (c.YawOffset ?? 0));
                    writer.Write(c.Pitch);
                    writer.Write(c.Roll);

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

