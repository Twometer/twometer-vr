using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using TVR.Service.Core.Model.Camera;

namespace TVR.Service.Core.Video
{
    public class ImageProcessing
    {
        public static void Erode<TColor, TDepth>(Image<TColor, TDepth> image, int iterations) where TColor : struct, IColor where TDepth : new()
        {
            CvInvoke.Erode(image, image, null, new Point(-1, -1), iterations, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }

        public static void Dilate<TColor, TDepth>(Image<TColor, TDepth> image, int iterations) where TColor : struct, IColor where TDepth : new()
        {
            CvInvoke.Dilate(image, image, null, new Point(-1, -1), iterations, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
        }

        public static void ThresholdBinary<TColor, TDepth>(Image<TColor, TDepth> image, TColor threshold, TColor maxValue) where TColor : struct, IColor where TDepth : new()
        {
            CvInvoke.Threshold(image, image, threshold.MCvScalar.V0, maxValue.MCvScalar.V0, ThresholdType.Binary);
        }

        public static void SmoothGaussian<TColor, TDepth>(Image<TColor, TDepth> image, int kernelSize) where TColor : struct, IColor where TDepth : new()
        {
            CvInvoke.GaussianBlur(image, image, new Size(kernelSize, kernelSize), 0, 0);
        }

        public static CircleF[] HoughCircles<TColor, TDepth>(Image<TColor, TDepth> image, double cannyThreshold, double accumulatorThreshold, double dp, double minDist, int minRadius = 0, int maxRadius = 0) where TColor : struct, IColor where TDepth : new()
        {
            return CvInvoke.HoughCircles(image, HoughModes.Gradient, dp, minDist, cannyThreshold, accumulatorThreshold, minRadius, maxRadius);
        }
    }
}
