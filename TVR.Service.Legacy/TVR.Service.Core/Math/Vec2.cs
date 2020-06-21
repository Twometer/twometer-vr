namespace TVR.Service.Core.Math
{
    public struct Vec2
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float LengthSquared => X * X + Y * Y;

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
