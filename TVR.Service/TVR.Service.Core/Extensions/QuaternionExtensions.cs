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
                var mul = -(1f / lengthSq);
                return new Quaternion(quaternion.X * mul, quaternion.Y * mul, quaternion.Z * mul, quaternion.W * -mul);
            }
            else
            {
                return quaternion;
            }
        }

        /// <summary>
        /// Converts a Quaternion in chip-coordinate-space to SteamVR coordinate space
        /// </summary>
        /// <param name="quaternion">Quaternion in ICM coordinate space</param>
        /// <returns>Quaternion in SteamVR coordinate space</returns>
        public static Quaternion ICM2SteamVR(this Quaternion quaternion)
        {
            return new Quaternion(quaternion.X, quaternion.Y, quaternion.W, -quaternion.Z);
        }
    }
}
