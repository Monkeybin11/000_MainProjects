using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace ImageTranform
{
    using Img = Image<Bgr, byte>;
    public static class Transfom
    {
        public static void GetInfo(IEnumerable<PointF> pointsF, IEnumerable<PointF> pointsD, Img img)
        {
            var mat = CvInvoke.GetPerspectiveTransform(pointsF.ToArray() , pointsD.ToArray());

            var dst = CvInvoke.WarpPerspective(img , mat , ); _
        }


    }

    public struct CornerPpoints
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
