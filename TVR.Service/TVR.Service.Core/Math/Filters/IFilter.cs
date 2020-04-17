using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Math.Filters
{
    public interface IFilter
    {
        float Value { get; }

        void Push(float value);

    }
}
