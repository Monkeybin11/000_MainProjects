using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeSizeImage_Transformation
{
    public struct TrnsData
    {
        public double Innerw;
        public double Innterh;

        public int H;
        public int W;
        public int XSrcCnter;
        public int YSrcCnter;
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
    public class PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }


    }
}
