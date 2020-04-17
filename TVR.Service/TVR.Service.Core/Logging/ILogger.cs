using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVR.Service.Core.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}
