using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonExtension.PatternMatch;

namespace WaferandChipProcessing
{
    public class EpiProcessingParameter
    {
        public int AreaUp = 40000;
        public int AreaDw = 5;
        public int Alpha  = 5;
        public int Beta   = 5;

        public void SetParameter(int up, int dw)
        {
            AreaUp = up;
            AreaDw = dw;
        }

    }

    public static class EpiParameterExtension
    {
        public static double GetPosThreshold(
            this ImgIdxPos src)
        {
            return src.Match()
                      .With( s => s == ImgIdxPos.TM , 244 )
                      .With( s => s == ImgIdxPos.BM , 244 )
                      .Else( 170 )
                      .Do();
        }

    }

}
