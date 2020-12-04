using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using TVR.Service.Core.Model;

namespace TVR.Service.Core.Video
{
    public static class ImageProcessing
    {
        public static double GetBrightness(IInputArray hsvImage)
        {
            return CvInvoke.Mean(hsvImage).V2;
        }

        public static void BgrToHsv(IInputArray bgr, IOutputArray hsv)
        {
            CvInvoke.CvtColor(bgr, hsv, ColorConversion.Bgr2Hsv);
        }

        public static void ColorFilter(Mat hsvFrame, Image<Gray, byte> dstFrame, Mat tmpFrame, ColorProfile colorProfile, double baseBrightness)
        {
            var range0 = colorProfile.Ranges[0];
            CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range0.Min.CvScalar, baseBrightness)), new ScalarArray(range0.Max.CvScalar), dstFrame);

            for (var i = 1; i < colorProfile.Ranges.Length; i++)
            {
                var range = colorProfile.Ranges[i];
                CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range.Min.CvScalar, baseBrightness)), new ScalarArray(range.Max.CvScalar), tmpFrame);
                CvInvoke.BitwiseOr(tmpFrame, dstFrame, dstFrame);
            }

            CvInvoke.Threshold(dstFrame, dstFrame, 10, 255, ThresholdType.Binary);
        }

        private static MCvScalar AdaptMinimum(MCvScalar minimum, double brightness)
        {
            minimum.V2 += brightness;
            return minimum;
        }
    }
}
