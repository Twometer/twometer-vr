using System.Linq;

namespace TVR.Service.Core.Math.Filters
{
    public class RollingAverageFilter : IFilter
    {
        public float Value => dataBuffer.Average();

        private readonly float[] dataBuffer;

        private int idx;

        public RollingAverageFilter(int size)
        {
            dataBuffer = new float[size];
        }

        public void Push(float val)
        {
            dataBuffer[idx] = val;
            idx++; idx %= dataBuffer.Length;
        }
    }
}
