using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Math
{
    public struct Vec3
    { 
        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"X={X}, Y={Y}, Z={Z}";
        }
    }
}
