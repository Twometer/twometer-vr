using System;
using TVR.Service.Core.Math;

namespace TVR.Service.Core.Tracking
{
    public class CameraSmoothing
    {
        private Vector3 previous;
        private Vector3 current;
        private Vector3 motion;

        private DateTime next;
        private TimeSpan frameTime;

        public CameraSmoothing(int fps)
        {
            frameTime = TimeSpan.FromMilliseconds(1000.0 / fps);
        }

        public void UpdatePosition(Vector3 position)
        {
            previous = current;
            current = position;
            motion = current - previous;
            next = DateTime.Now + frameTime;
        }

        public Vector3 Interpolate()
        {
            var remainingMillis = (next - DateTime.Now).TotalMilliseconds;
            var progress = 1.0 - ((double)remainingMillis / frameTime.TotalMilliseconds);
            return current + motion * progress;
        }

    }
}
