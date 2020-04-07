using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Model
{
    public struct ButtonPress
    {
        public byte ControllerId { get; }

        public Button Button { get; }

        public byte ButtonId => (byte)Button;

        public ButtonPress(byte controllerId, Button button)
        {
            ControllerId = controllerId;
            Button = button;
        }
    }
}
