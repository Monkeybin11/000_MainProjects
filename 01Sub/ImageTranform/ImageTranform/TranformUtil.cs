using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTranform
{
    using static Math;
    using PartialData = List<IdxData<byte>>;

    public static class TranformUtil
    {
        public static int biLinearInterp_int(double nw, double ne, double sw, double se, double p, double q)
        {
            double t, b;

            t = nw + p * (ne - nw);
            b = sw + p * (se - sw);
            return (int)(t + q * (b - t));
        }

        public static byte biLinearInterp_byte(double nw, double ne, double sw, double se, double p, double q)
        {
            double t, b;

            t = nw + p * (ne - nw);
            b = sw + p * (se - sw);
            return (byte)(t + q * (b - t));
        }

        public static PointD GetCenter(this AffinePos pos)
        {
            var xCenter = Abs(pos.LT.X + pos.RB.X) / 2;
            var yCenter = Abs(pos.LB.Y + pos.RT.Y) / 2;


            return new PointD(xCenter, yCenter);
        }

        public static AffinePos MoveToCenter(this AffinePos pos, PointD center)
        {
            var wHalf = Math.Sqrt(Math.Pow((pos.LT.X - pos.RT.X), 2) + Math.Pow((pos.LT.Y - pos.RT.Y), 2));
            var hHalf = Math.Sqrt(Math.Pow((pos.LT.X - pos.LB.X), 2) + Math.Pow((pos.LT.Y - pos.LB.Y), 2));

            var x1 = center.X - wHalf;
            var x2 = center.X - wHalf;
            var x3 = center.X + wHalf;
            var x4 = center.X + wHalf;

            var y1 = center.Y - hHalf;
            var y3 = center.Y + hHalf;
            var y2 = center.Y + hHalf;
            var y4 = center.Y - hHalf;

            return new AffinePos(x1, y1, x2, y2, x3, y3,x4,y4);
        }

        public static AffinePos GetCentered(this AffinePos pos, PointD center)
        {
            var wHalf = Abs(pos.LT.X - pos.RT.X) / 2;
            var hHalf = Abs(pos.LT.Y - pos.RT.Y) / 2;


            var x1 = center.X - wHalf;
            var x2 = center.X - wHalf;
            var x3 = center.X + wHalf;
            var x4 = center.X + wHalf;

            var y1 = center.Y - hHalf;
            var y3 = center.Y + hHalf;
            var y2 = center.Y + hHalf;
            var y4 = center.Y - hHalf;

            return new AffinePos(x1, y1, x2, y2, x3, y3, x4, y4);
        }


        #region 

        public static int[] XYMax(
            this PartialData src)
        => new int[]
            {
                src.Select(x => x.X).Max(),
                src.Select(x => x.Y).Max()
            };
        public static byte[,] UpdateWith(
            this byte[,] src ,  PartialData datas)
        {
            foreach (var data in datas)
            {
                if(data.Y >= 0 &&
                    data.Y < src.GetLength(0)&&
                    data.X >= 0 &&
                    data.X < src.GetLength(1))
                    src[data.Y, data.X] = data.Data;
            }
            return src;
        }

        public static double PosL2(PointD pos1, PointD pos2)
            => Sqrt( Pow(pos1.X - pos2.X, 2) + Pow(pos1.Y - pos2.Y, 2));

        public static TrnsData ToTrnsData
          (this AffinePos srcPos, int w, int h)
        {
            //inner w h 
            var innerh = PosL2(srcPos.LB, srcPos.LT);
            var innerw = PosL2(srcPos.RT, srcPos.LT);


            var trgPos = srcPos.MoveToCenter(new PointD(w / 2.0, h / 2.0)); // ok

            var fcenterX = srcPos.GetCenter().X;
            var fcenterY = srcPos.GetCenter().Y;
            var lcenterX = trgPos.GetCenter().X;
            var lcenterY = trgPos.GetCenter().Y;

           


            var xshift = lcenterX - fcenterX;
            var yshift = lcenterY - fcenterY;

            var dx = srcPos.LT.X - srcPos.RT.X;
            var dy = srcPos.LT.Y - srcPos.RT.Y;

            double radian;
            if (Abs(dx) < 0.0001)
            {
                radian = 1.5708;
            }
            else if (Abs(dy) < 0.0001)
            {
                radian = 3.14159;
            }
            else
            {
                radian = Math.Atan(dy / dx);
            }
            //var angle_degree = 360 - Math.Atan2(dy, dx) * 180 / Math.PI;
            //double radian;
            //angle_degree %= 360;
            //radian = PI / 180.0 * angle_degree;
           
            return new TrnsData()
            {
                H = h,
                W = w,
                XShift = (int)xshift,
                YShift = (int)yshift,
                Innerw = innerw,
                Innterh = innerh,
                XSrcCnter = (int)fcenterX,
                YSrcCnter = (int)fcenterY,
                dX = (int)dx,
                dY = (int)dy,
                Angle = radian,
            };
        }


        public static IEnumerable<IdxData<A>> ToIdxDatas<A>
            (this IEnumerable<IEnumerable<A>> srcs)
            =>srcs.SelectMany((f, j) => f.Select(
                              (s, i) => new IdxData<A>()
                                            {
                                                Y = j,
                                                X = i ,
                                                Data = s
                                             }));

        public static ushort[] ToUShortArray(this byte[] src )
        {
            ushort[] res = new ushort[src.Length/2];
            Buffer.BlockCopy(src, 0, res, 0, src.Length);
            return res;
        }

        public static IdxData<A> ToIdxData<A>(int j, int i, A data)
          => new IdxData<A>()
          {
              Y = j,
              X = i,
              Data = data
          };

        #endregion  

        public static int[,] TointArray(this byte[,,] src)
        {
            var h = src.GetLength(0);
            var w = src.GetLength(1);

            int[,] output = new int[h, w];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    output[j, i] = (int)src[j, i, 0];

                }
            }
            return output;
        }
    }
}
