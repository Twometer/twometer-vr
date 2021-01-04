using System.Linq;

namespace TVR.Service.Core.Tracking
{
    internal class RollingAverageFilter
    {
        public float Value => dataBuffer.Average();

        private readonly float[] dataBuffer;

        private int idx;

        public RollingAverageFilter(int size)
        {
            dataBuffer = new float[size];
        }

        public float Push(float val)
        {
            dataBuffer[idx] = val;
            idx++; 
            idx %= dataBuffer.Length;
            return Value;
        }
    }
}
