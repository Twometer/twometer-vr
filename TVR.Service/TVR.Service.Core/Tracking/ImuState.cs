using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Tracking
{
    public class ImuState
    {
        public float Ax { get; set; }
        public float Ay { get; set; }
        public float Az { get; set; }

        public float Gx { get; set; }
        public float Gy { get; set; }
        public float Gz { get; set; }

        public float Mx { get; set; }
        public float My { get; set; }
        public float Mz { get; set; }
    }
}
