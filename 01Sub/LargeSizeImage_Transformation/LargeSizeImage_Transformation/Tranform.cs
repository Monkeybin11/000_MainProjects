using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeSizeImage_Transformation
{
    using System.IO;
    using static LargeSizeImage_Transformation.TrarnsformMatrix;
    using static LargeSizeImage_Transformation.Helper;
    using static Math;

    public static class Tranform
    {
        public static byte[][] Run(MemoryStream bytestream, int w, int h, PointD[] pos3)
        {
            try
            {

                var srcPos = new AffinePos(pos3[0], pos3[1], pos3[2], pos3[3]);
                var trsData = srcPos.ToTrnsData(w, h);

                var xc = trsData.XSrcCnter;
                var yc = trsData.YSrcCnter;

                var xmax = Math.Max(w - xc, xc) * 2;
                var ymax = Math.Max(h - yc, yc) * 2;

                byte[][] R1 = CreateJagged(ymax, xmax);

                for (int i = 0; i < h; i++)
                {
                    var data = new byte[w];
                    bytestream.Seek(i * w, SeekOrigin.Begin);
                    bytestream.Read(data, 0, w);
                    var reshaped = data.Reshape(1, w).ToJagged();
                    xytransform(ref R1, trsData, reshaped, i, xmax, ymax);
                }

                var w1 = R1[0].Length;
                var h1 = R1.Length;

               

                var degree = trsData.Angle * 180 / Math.PI;

                double radian = trsData.Angle;
                double cosRadian = Cos(radian);
                double sinRadian = Sin(radian);
                int newWidth = (int)(w1 * Abs(cosRadian) + h1 * Abs(sinRadian));
                int newHeight = (int)(h1 * Abs(cosRadian) + w1 * Abs(sinRadian));
                byte[][] R2 = CreateJagged(newWidth, newHeight);


                for (int i = 0; i < R1.Length; i++)
                {
                    var reshaped = R1[i].Reshape(1, w1).ToJagged();

                    rotateGeneral(ref R2, trsData, reshaped, i, w1, h1);
                }
                return R2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            
        }

        public static Point TransformPoint(PointD[] pos3, int idiolCenterX, int idiolCenterY, Point srcpos)
        {
            try
            {
                 var srcPos = new AffinePos(pos3[0], pos3[1], pos3[2], pos3[3]);
                 var trsData = srcPos.ToTrnsData(1, 1);

                 var xshift = (int)(trsData.XSrcCnter - idiolCenterX);
                 var yshift = (int)(trsData.YSrcCnter - idiolCenterY);

                 double radian = trsData.Angle;

                 double cosRadian = Cos(radian);
                 double sinRadian = Sin(radian);

                 var x = srcpos.X - xshift;
                 var y = srcpos.Y - yshift;

                 var sourceX = (x - idiolCenterX) * cosRadian + (y - idiolCenterY) * (sinRadian);
                 sourceX += (idiolCenterX);
                 var sourceY = (x - idiolCenterX) * -sinRadian + (y - idiolCenterY) * cosRadian;
                 sourceY += (idiolCenterY);

                 var isourceX = (int)sourceX;
                 var isourceY = (int)sourceY;

                 return new Point(isourceX, isourceY);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default(Point);
            }
        }

    }
}
