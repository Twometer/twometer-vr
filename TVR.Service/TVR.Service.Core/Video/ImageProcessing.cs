using Emgu.CV;
using Emgu.CV.CvEnum;

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
    }
}
