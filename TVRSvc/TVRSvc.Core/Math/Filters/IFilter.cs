using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Math.Filters
{
    public interface IFilter
    {
        float Value { get; }

        void Push(float value);

    }
}
