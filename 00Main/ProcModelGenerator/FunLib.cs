using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using ModelLib.AmplifiedType;
using EmguCvExtension;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace ProcModelGenerator
{
    using static ModelLib.AmplifiedType.Handler;
    using Writer = Func<string, string>;
    using Img = Image<Gray, byte>;
    using System.Windows.Media.Imaging;
    using System.Windows;

    public static class FunLib
    {
        public static Func<int, Img, Img> Threshold
            => (trsh, img)
            => img.ThresholdBinary(new Gray(trsh), new Gray(255));

        public static Func<int, Img, Img> AdpTHreshold
          => (trsh, img)
          => img.ThresholdAdaptive(new Gray(255), AdaptiveThresholdType.GaussianC, ThresholdType.Binary, trsh, new Gray(0));

        public static Func<int, Img, Img> Median
           => (median, img)
           => img.SmoothMedian(median);

        public static Func<int, Img, Img> Normalize
          => (norm, img)
          => img.Normalize((byte)norm);


        // AccumulWriter -> Writer
        public static Writer PLImagingWriter
            => str
            => "|" + str;

    }

    internal static class Helper
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);


        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }
    }


    public static class PLProtocol
    {
        public static readonly string StrThreshold = "Threshold";
        public static readonly string StrAdpTHreashold = "AdpThreshold";
        public static readonly string StrMedian = "Median";
        public static readonly string StrNormalize = "Normalize";

        public static string With
            (this string src, int param)
            => src + ',' + param.ToString();

    }

}
