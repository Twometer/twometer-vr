using Emgu.CV.Structure;
using System.Numerics;

namespace TVR.Service.Core.Tracking
{
    internal interface ICameraTransform
    {
        Vector3 Transform(CircleF circle);
    }
}
