using System.Numerics;

namespace TVR.Service.Core.Extensions
{
    public static class QuaternionExtensions
    {
        public static Quaternion Invert(this Quaternion quaternion)
        {
            var lengthSq = quaternion.LengthSquared();
            if (lengthSq != 0.0)
            {
                var mul = -(1 / lengthSq);
                return new Quaternion(quaternion.X * mul, quaternion.Y * mul, quaternion.Z * mul, quaternion.W * -mul);
            }
            else
            {
                return quaternion;
            }
        }
    }
}
