using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Model
{
    public struct ButtonPress
    {
        public int ControllerId { get; }

        public int ButtonId { get; }

        public ButtonPress(int controllerId, int buttonId)
        {
            ControllerId = controllerId;
            ButtonId = buttonId;
        }
    }
}
