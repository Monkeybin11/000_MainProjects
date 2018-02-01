using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using SpeedyCoding;
using Accord.Math.Metrics;
using Accord.Math;

namespace ImageTranform
{
    using Img = Image<Bgr, byte>;
    public static class Transfom
    {
        public static double[][] ToAffinenMetrix(PointF[] fisrt, PointF[] last)
        {
            var srcM = new double[][]
           {
                new double[]{ fisrt[0].X , fisrt[1].X, fisrt[2].X},
                new double[]{ fisrt[ 0].Y , fisrt[1].Y, fisrt[2].Y},
                new double[]{ 1,1,1}
           };


            var targetM = new double[][]
            {
                new double[]{ last[0].X , last[1].X, last[2].X},
                new double[]{ last[0].Y , last[1].Y, last[2].Y}
            };
            
            var squareSrc = Matrix.DotWithTransposed(srcM, srcM);
            var squareInv = Matrix.Inverse(squareSrc);
            var output = Matrix.Dot(targetM, squareInv);
            return output;
        }



        public static void GetInfo(IEnumerable<PointF> pointsF, IEnumerable<PointF> pointsD, Img img)
        {
            var mat = CvInvoke.GetPerspectiveTransform(pointsF.ToArray() , pointsD.ToArray());

            //var dst = CvInvoke.WarpPerspective(img , mat ,1 ); _
        }


        public static Mat ToTargetPos(CornerPoints pos , Point center)
        {

            var mx = center.X;
            var my = center.Y;

            var w = Tuple.Create(pos.x1, pos.y1).L2( Tuple.Create(pos.x2,pos.y2) )/2;
            var h = Tuple.Create(pos.x0, pos.y0).L2(Tuple.Create(pos.x1, pos.y1))/2;



            var lb = new PointF((float)(mx - w), (float)(my - h));
            var lt = new PointF((float)(mx - w), (float)(my + h));
            var rt = new PointF((float)(mx + w), (float)(my + h));

            var pf = ToTransPos(pos);
            var pl = new PointF[] { lb, lt, rt };




            var mat = CvInvoke.GetAffineTransform(pf, pl);



            Console.WriteLine();

            return mat;
        }

        public static PointF[] ToTransPos(CornerPoints pos)
        {
            var p0 = new PointF((float)pos.x0, (float)pos.y0);
            var p1 = new PointF((float)pos.x1, (float)pos.y1);
            var p2 = new PointF((float)pos.x2, (float)pos.y2);
            return new PointF[] { p0, p1, p2 };
        }
    }


    public struct CornerPoints
    {
        public double x0;
        public double y0;
        public double x1;
        public double y1;
        public double x2;
        public double y2;
        public double x3;
        public double y3;
        public double x4;
        public double y4;
    }

}
