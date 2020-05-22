using Emgu.CV.Structure;

namespace TVR.Service.Core.Tracking
{
    public struct ColorRange
    {
        public MCvScalar Minimum { get; }

        public MCvScalar Maximum { get; }

        public ColorRange(MCvScalar minimum, MCvScalar maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
