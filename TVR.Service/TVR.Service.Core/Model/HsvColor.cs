using Emgu.CV.Structure;

namespace TVR.Service.Core.Model
{
    public struct HsvColor
    {
        public float H { get; set; }

        public float S { get; set; }

        public float V { get; set; }

        public MCvScalar CvScalar => new MCvScalar(H, S, V);

        public HsvColor(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

    }
}
