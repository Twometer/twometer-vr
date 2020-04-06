using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Math
{
    public class RollingAverage
    {
        public float Value => dataBuffer.Average();

        private readonly float[] dataBuffer;

        private int idx;

        public RollingAverage(int size)
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
