using Emgu.CV.Structure;

namespace TVR.Service.Core.Math.Transform
{
    public interface ICameraTransform
    {
        Vec3 Transform(int frameWidth, int frameHeight, CircleF obj);
    }
}
