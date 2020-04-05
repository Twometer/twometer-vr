using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Math
{
    public struct Vec2
    {
        public float X { get; set; }

        public float Y { get; set; }

        public Vec2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"X={X}, Y={Y}";
        }
    }
}
