using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVR.Service.Core.Math;

namespace TVR.Service.Core.Model
{
    public class Controller
    {
        public byte Id { get; }

        public Vec3 Position { get; set; }

        public float? ZOffset { get; set; }

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
