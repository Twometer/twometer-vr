using YamlDotNet.Serialization;

namespace TVR.Service.Core.Math
{
    public struct Vector3
    {
        public static Vector3 Zero { get; } = new Vector3(0, 0, 0);

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        [YamlIgnore]
        public float LengthSquared => X * X + Y * Y + Z * Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }

        public override string ToString()
        {
            return $"X={X}, Y={Y}, Z={Z}";
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
    }
}
