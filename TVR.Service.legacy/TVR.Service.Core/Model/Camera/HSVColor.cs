using Emgu.CV.Structure;

namespace TVR.Service.Core.Model.Camera
{
    public class HSVColor
    {
        public double H { get; set; }

        public double S { get; set; }

        public double V { get; set; }

        public HSVColor(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }

        public HSVColor()
        {
        }

        public MCvScalar ToMCvScalar()
        {
            return new MCvScalar(H, S, V);
        }
    }
}
