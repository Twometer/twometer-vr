using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
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

        public static void SmoothMedian<TColor, TDepth>(Image<TColor, TDepth> image, int kernelSize) where TColor : struct, IColor where TDepth : new()
        {
            CvInvoke.MedianBlur(image, image, kernelSize);
        }

        public static CircleF[] HoughCircles<TColor, TDepth>(Image<TColor, TDepth> image, double cannyThreshold, double accumulatorThreshold, double dp, double minDist, int minRadius = 0, int maxRadius = 0) where TColor : struct, IColor where TDepth : new()
        {
            return CvInvoke.HoughCircles(image, HoughModes.Gradient, dp, minDist, cannyThreshold, accumulatorThreshold, minRadius, maxRadius);
        }

        public static double GetBrightness(Mat hsvImage)
        {
            return CvInvoke.Mean(hsvImage).V2;
        }

        public static void BgrToHsv(Mat bgr, Mat hsv)
        {
            CvInvoke.CvtColor(bgr, hsv, ColorConversion.Bgr2Hsv);
        }

        public static void ColorFilter(Mat hsvFrame, Image<Gray, byte> dstFrame, Mat tmpFrame, ColorProfile colorProfile, double brightness)
        {
            var range0 = colorProfile.ColorRanges[0];
            CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range0.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range0.Maximum.ToMCvScalar()), dstFrame);
            CvInvoke.Threshold(dstFrame, dstFrame, 10, 255, ThresholdType.Binary);

            for (var i = 1; i < colorProfile.ColorRanges.Length; i++)
            {
                var range = colorProfile.ColorRanges[i];
                CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range.Maximum.ToMCvScalar()), tmpFrame);
                CvInvoke.Threshold(tmpFrame, tmpFrame, 10, 255, ThresholdType.Binary);
                
                CvInvoke.BitwiseOr(tmpFrame, dstFrame, dstFrame);
            }
        }

        private static MCvScalar AdaptMinimum(MCvScalar minimum, double brightness)
        {
            minimum.V2 += brightness;
            return minimum;
        }
    }
}
