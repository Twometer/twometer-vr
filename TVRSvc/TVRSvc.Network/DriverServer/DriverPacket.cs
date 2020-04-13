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
                    writer.Write(c.Rotation.X);
                    writer.Write(c.Rotation.Y);
                    writer.Write(c.Rotation.Z);
                }
            }
            else
            {
                writer.Write((byte)0);
            }


            // Button presses
            var buttonPresses = ControllerStates?
                .Where(c => c.PressedButtons != null)
                .Select(c => c.PressedButtons.Select(btn => new ButtonPress(c.Id, btn)))
                .SelectMany(press => press)
                .ToArray();

            if (buttonPresses == null)
            {
                writer.Write((byte)0);
                return;
            }

            writer.Write(buttonPresses.Length);
            foreach (var b in buttonPresses)
            {
                writer.Write(b.ControllerId);
                writer.Write(b.ButtonId);
            }
        }

    }
}
