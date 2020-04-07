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

        public Button Button { get; }

        public int ButtonId => (int)Button;

        public ButtonPress(int controllerId, Button button)
        {
            ControllerId = controllerId;
            Button = button;
        }
    }
}
