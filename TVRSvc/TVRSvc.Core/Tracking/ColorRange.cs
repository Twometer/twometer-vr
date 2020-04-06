using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Tracking
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
