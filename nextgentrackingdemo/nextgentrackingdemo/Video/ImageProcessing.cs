using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using TVR.Service.Core.Model.Camera;

namespace nextgentrackingdemo.Video
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

        public static void ColorFilter(IInputArray hsvFrame, Image<Gray, byte> dstFrame, Mat tmpFrame, ColorProfile colorProfile, double brightness)
        {
            var range0 = colorProfile.ColorRanges[0];
            CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range0.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range0.Maximum.ToMCvScalar()), dstFrame);

            for (var i = 1; i < colorProfile.ColorRanges.Length; i++)
            {
                var range = colorProfile.ColorRanges[i];
                CvInvoke.InRange(hsvFrame, new ScalarArray(AdaptMinimum(range.Minimum.ToMCvScalar(), brightness)), new ScalarArray(range.Maximum.ToMCvScalar()), tmpFrame);
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
