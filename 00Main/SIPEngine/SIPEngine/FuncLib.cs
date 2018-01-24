using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace SIPEngine
{
    using Img = Image<Gray, byte>;
    public static class FuncLib
    {
        public static Func<int, Img, Img> Threshold
            => ( param, img )
            => img.ThresholdBinary( new Gray( param ), new Gray( 255 ) );

        public static Func<int, Img, Img> Normalize
           => ( param, img )
           =>  img.Sub( new Gray( 0 ) ).Mul( 255.0 / ( double )( param ) );

        public static Func<int, Img, Img> Median
          => ( param, img )
          => img.SmoothMedian( param );

        public static Func<int, Img, Img> AdpThreshold
          => ( param, img )
          => img.ThresholdAdaptive( new Gray( 255 ) , AdaptiveThresholdType.GaussianC , ThresholdType.Binary , param , new Gray(0) );
    }
}
