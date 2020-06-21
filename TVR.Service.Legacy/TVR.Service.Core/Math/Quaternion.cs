namespace TVR.Service.Core.Math
{
    public struct Quaternion
    {
        public static Quaternion Identity { get; } = new Quaternion(0, 0, 0, 1);

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        public float LengthSquared => X * X + Y * Y + Z * Z + W * W;

        public Vec3 Xyz => new Vec3(X, Y, Z);     

        public Quaternion(Vec3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion Invert()
        {
            float lengthSquared = LengthSquared;
            if (lengthSquared != 0.0)
            {
                float num = 1f / lengthSquared;
                return new Quaternion(Xyz * (-num), W * num);
            }
            else
            {
                return this;
            }
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.Xyz * right.W + right.Xyz * left.W + Vec3.Cross(left.Xyz, right.Xyz), left.W * right.W - Vec3.Dot(left.Xyz, right.Xyz));
        }

        public override string ToString()
        {
            return $"X={X}, Y={Y}, Z={Z}, W={W}";
        }
    }
}
