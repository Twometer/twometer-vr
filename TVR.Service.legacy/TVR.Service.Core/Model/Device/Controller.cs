using System;
using System.Collections.Concurrent;
using TVR.Service.Core.Math;

namespace TVR.Service.Core.Model.Device
{
    public class Controller
    {
        public byte Id { get; }

        public Vector3 Position { get; set; } = Vector3.Zero;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        public Quaternion RotationOffset { get; set; } = Quaternion.Identity;

        public ConcurrentDictionary<Button, bool> Buttons { get; } = new ConcurrentDictionary<Button, bool>();

        public Controller(byte id)
        {
            Id = id;

            foreach (Button btn in Enum.GetValues(typeof(Button)))
                Buttons[btn] = false;
        }
    }
}
