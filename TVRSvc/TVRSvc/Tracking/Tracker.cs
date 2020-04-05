using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Model;

namespace TVRSvc.Tracking
{
    public class Tracker
    {
        public Controller Controller0 { get; } = new Controller(0);

        public Controller Controller1 { get; } = new Controller(1);

        public void Update(Mat frame)
        {

        }

    }
}
