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
using ModelLib.AmplifiedType;
using static ImageTranform.TransformMetrix;
using System.IO;

namespace ImageTranform
{
    using static Math;
    using Img = Image<Bgr, byte>;
    using TrsInfoFunc = Func<int, int, AffinePos, AffinePos, TrnsData>;
    using TrsFunc = Func< byte[,],TrnsData, byte[,], byte[,]>;
    using IoByte = IEnumerable<byte>;
    using PartialData = List< IdxData<byte>>;
    using System.Diagnostics;

    public static class ImageAlingn
    {
        //public static Tuple<Bitmap,int,int> Run(MemoryStream bytestream, int w, int h, PointD[] pos3)
        public static Bitmap Run(MemoryStream bytestream, int w, int h, PointD[] pos3)
        {
            var srcPos = new AffinePos(pos3[0], pos3[1], pos3[2], pos3[3]);
            var reslist = new List<PartialData>();
            var trsData = srcPos.ToTrnsData(w, h);

            var xc = trsData.XSrcCnter;
            var yc = trsData.YSrcCnter;

            var xmax = Math.Max(w - xc, xc) * 2;
            var ymax = Math.Max(h - yc, yc) * 2;


            byte[][] R1 = CreateJagged(ymax, xmax);

            // --

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
            //double radian = 0.785398;
            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);
            int newWidth = (int)(w1 * Abs(cosRadian) + h1 * Abs(sinRadian));
            int newHeight = (int)(h1 * Abs(cosRadian) + w1 * Abs(sinRadian)); // ok
            byte[][] R2 = CreateJagged(newWidth, newHeight);


            for (int i = 0; i < R1.Length; i++)
            {

                //bytestream.Seek(i * w, SeekOrigin.Begin);
                //bytestream.Read(data, 0, w);
                try
                {
                    var reshaped = R1[i].Reshape(1, w1).ToJagged();

                    rotateGeneral(ref R2, trsData, reshaped, i , w1,h1);
                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return R2.ToMat().ToBitmap_SetPixel(); 
        }

        public static Bitmap Run_Memory(MemoryStream bytestream, int w, int h, PointD[] pos3)
        {
            var srcPos = new AffinePos(pos3[0], pos3[1], pos3[2], pos3[3]);
            var reslist = new List<PartialData>();
            var trsData = srcPos.ToTrnsData(w, h);

            var xc = trsData.XSrcCnter;
            var yc = trsData.YSrcCnter;

            var xmax = Math.Max(w - xc, xc) * 2;
            var ymax = Math.Max(h - yc, yc) * 2;


            byte[][] R1 = CreateJagged(ymax, xmax);

            // --

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
            //double radian = 0.785398;
            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);
            int newWidth = (int)(w1 * Abs(cosRadian) + h1 * Abs(sinRadian));
            int newHeight = (int)(h1 * Abs(cosRadian) + w1 * Abs(sinRadian)); // ok
            byte[][] R2 = CreateJagged(newWidth, newHeight);


            for (int i = 0; i < R1.Length; i++)
            {

                //bytestream.Seek(i * w, SeekOrigin.Begin);
                //bytestream.Read(data, 0, w);
                try
                {
                    var reshaped = R1[i].Reshape(1, w1).ToJagged();

                    rotateGeneral(ref R2, trsData, reshaped, i, w1, h1);
                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return R2.ToMat().ToBitmap_SetPixel();
        }



        public static Point TransformPoint(PointD[] pos3, int idiolCenterX,int idiolCenterY,Point srcpos)
        {
            var srcPos = new AffinePos(pos3[0], pos3[1], pos3[2], pos3[3]);
            var reslist = new List<PartialData>();
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
            var sourceY = (x - idiolCenterX) * -sinRadian + (y  - idiolCenterY) * cosRadian;
            sourceY += (idiolCenterY);
            //  이부분은 ok

            var isourceX = (int)sourceX;
            var isourceY = (int)sourceY;

            return new Point(isourceX, isourceY);

        }

        public static byte[][] CreateJagged(int h, int w)
        {
            byte[][] output = new byte[h][];
            for (int i = 0; i < h; i++)
            {
                output[i] = new byte[w];
            }
            return output;

        }
        //Create 

        public static Bitmap ToBitmap_SetPixel(this byte[,] src)
        {
            int h = src.GetLength(0);
            int w = src.GetLength(1);

            Bitmap img = new Bitmap(w, h);

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    Color clr = new Color();
                    img.SetPixel(i, j, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }
            return img;
        }

        //Point to 
        public static Func<Func<TrnsData>, List<TrsFunc> , IoByte , int , int, PartialData> RunTranform
            => (fnData, mfn , src , w , h)
            =>
            {
                var trsData = fnData(); //변환을 위한 정보. 
                //var trsfns = mfn.Select(x => x.Apply(trsData));
                var trsed = src.ToArray()
                                .Reshape( h,w);

                byte[,] res = trsed;

                //여기서 미리 최종 결과의 사이즈를 계산한다. 
              

                foreach (var f in mfn)
                {
                    //res = f(R, trsData, res);
                }

                return res.ToJagged()
                          .ToIdxDatas()
                          .ToList();
            };

        public static double[][] ToAffinenMetrix(PointD[] fisrt, PointD[] last)
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
