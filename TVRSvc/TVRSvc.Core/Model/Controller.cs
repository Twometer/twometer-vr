using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Math;

namespace TVRSvc.Core.Model
{
    public class Controller
    {
        public byte Id { get; }

        public Vec3 Position { get; set; }

        public float? YawOffset { get; set; }
        public float? ZOffset { get; set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public IDictionary<Button, bool> Buttons { get; } = new Dictionary<Button, bool>();

        public Controller(byte id)
        {
            Id = id;

            Buttons[Button.A] = false;
            Buttons[Button.B] = false;
        }
    }
}
