using System.Linq;

namespace TVR.Service.Core.Math
{
    public class RollingAverageFilter
    {
        public double Value => dataBuffer.Average();

        private readonly double[] dataBuffer;

        private int idx;

        public RollingAverageFilter(int size)
        {
            dataBuffer = new double[size];
        }

        public void Push(double val)
        {
            dataBuffer[idx] = val;
            idx++; idx %= dataBuffer.Length;
        }
    }
}
