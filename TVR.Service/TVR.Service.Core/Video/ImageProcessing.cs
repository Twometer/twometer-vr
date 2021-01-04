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

        public static void ColorFilter(Image<Hsv, byte> srcFrame, Image<Gray, byte> dstFrame, Mat tmpFrame, ColorRange[] colorRanges, double baseBrightness)
        {
            var range0 = colorRanges[0];
            CvInvoke.InRange(srcFrame, new ScalarArray(AdaptMinimum(range0.Minimum.CvScalar, baseBrightness)), new ScalarArray(range0.Maximum.CvScalar), dstFrame);

            for (var i = 1; i < colorRanges.Length; i++)
            {
                var range = colorRanges[i];
                CvInvoke.InRange(srcFrame, new ScalarArray(AdaptMinimum(range.Minimum.CvScalar, baseBrightness)), new ScalarArray(range.Maximum.CvScalar), tmpFrame);
                CvInvoke.Add(tmpFrame, dstFrame, dstFrame);
            }

            CvInvoke.Threshold(dstFrame, dstFrame, 10, 255, ThresholdType.Binary);
        }

        private static MCvScalar AdaptMinimum(MCvScalar minimum, double brightness)
        {
            minimum.V2 += brightness;
            if (minimum.V2 > 255)
                minimum.V2 = 255;
            return minimum;
        }
    }
}
