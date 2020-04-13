using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVRSvc.Core.Math;

namespace TVRSvc.Core.Transform
{
    public interface ICameraTransform
    {

        Vec3 Transform(int frameWidth, int frameHeight, CircleF obj);

    }
}
