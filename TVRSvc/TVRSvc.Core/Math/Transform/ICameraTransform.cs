using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVRSvc.Core.Math.Transform
{
    public interface ICameraTransform
    {

        Vec3 Transform(int frameWidth, int frameHeight, CircleF obj);

    }
}
