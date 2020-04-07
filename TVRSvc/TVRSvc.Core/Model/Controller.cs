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

        public Vec3 Acceleration { get; set; }

        public Vec2 Rotation { get; set; }

        public Controller(byte id)
        {
            Id = id;
        }
    }
}
