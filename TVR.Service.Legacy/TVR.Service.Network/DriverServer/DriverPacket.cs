using System.IO;
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
                    writer.Write(c.Id);
                    writer.Write(c.Position.X);
                    writer.Write(c.Position.Y);
                    writer.Write(c.Position.Z - (c.ZOffset ?? 0));

                    var correctedRotation = c.Rotation * c.RotationOffset;
                    writer.Write(correctedRotation.X);
                    writer.Write(correctedRotation.Y);
                    writer.Write(correctedRotation.Z);
                    writer.Write(correctedRotation.W);

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

