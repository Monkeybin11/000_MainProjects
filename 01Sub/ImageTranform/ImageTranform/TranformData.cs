using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTranform
{
    using static Math;
    using static TranformUtil;

    public static partial class  TransformHelper
    {
        public static Func<int, int, AffinePos, AffinePos, TrnsData> 
            GetTransRotateData
            => (w, h, src, trg)
            => {
                var fcenterX = src.GetCenter().X;
                var fcenterY = src.GetCenter().Y;
                var lcenterX = trg.GetCenter().X;
                var lcenterY = trg.GetCenter().Y;
                var dx = Abs(src.LT.X - src.RT.X);
                var dy = Abs(src.LT.Y - src.RT.Y);

                return new TrnsData()
                {
                    H = w,
                    W = h,
                    XSrcCnter = (int)fcenterX,
                    YSrcCnter = (int)fcenterY,
                    Angle = Math.PI*2 - Math.Atan2(dy, dx) 
                };
            };
    }

    public struct IdxData<A>
    {
        public int X;
        public int Y;
        public A Data;
    }

    public struct TrnsData
    {
        public double Innerw;
        public double Innterh;

        public int H;
        public int W;
        public int XSrcCnter;
        public int YSrcCnter;
       // public int XTrgCnter;
       // public int YTrgCnter;
        public int XShift;
        public int YShift;
        public int dX;
        public int dY;
        public double Angle; // radian
    }

    public class AffinePos
    {
        public PointD LB;
        public PointD LT;
        public PointD RT;
        public PointD RB;

        public AffinePos(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            LB = new PointD(x1, y1);
            LT = new PointD(x2, y2);
            RT = new PointD(x3, y3);
            RB = new PointD(x4, y4);
        }

        public AffinePos(PointD lb, PointD lt, PointD rt, PointD rb)
        {
            LB = lb;
            LT = lt;
            RT = rt;
            RB = rb;
        }
    }
}
