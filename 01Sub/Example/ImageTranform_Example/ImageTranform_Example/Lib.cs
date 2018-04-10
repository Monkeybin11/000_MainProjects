using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LargeSizeImage_Transformation;

namespace ImageTranform_Example
{
    using System.Drawing;
    using static Math;

    public static class Lib
    {
        public static int[,] xytransform(int[,] src , AffinePos srcPos , AffinePos trgPos)
        {
            var h = src.GetLength(0);
            var w = src.GetLength(1);

            var fcenterX = srcPos.GetCenter().X;
            var fcenterY = srcPos.GetCenter().Y;
            var lcenterX = trgPos.GetCenter().X;
            var lcenterY = trgPos.GetCenter().Y;

            var xshift = (int)(lcenterX - fcenterX);
            var yshift = (int)(lcenterY - fcenterY);  

            int[,] R = new int[h+ Abs(yshift), w+Abs(xshift)];
            
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (i - xshift >= 0
                        && j - yshift >= 0
                        && i - xshift < R.GetLength(1)
                        && j - yshift < R.GetLength(0))
                    {
                        R[j - yshift , i - xshift ] = src[j, i];
                    }
                     
                }
            }

            // now R is shifted
            /*
            var dx = Abs(srcPos.LT.X - srcPos.RT.X);
            var dy = Abs(srcPos.LT.Y - srcPos.RT.Y);
            if (dx == 0) return null;
            var angle_degree =  360 - Math.Atan2(dy, dx)*180/Math.PI;
            var rotationResult = rotateGeneral(R, R.GetLength(1), R.GetLength(0), angle_degree);
           
            return rotationResult;
             */
            return R;

        }


        public static int[,] rotateGeneral(int[,] G, int Width, int Height, double angle ,bool isrradian)
        {
            double radian;
            if (isrradian)
            {
                radian = angle;
            }
            else
            {
                angle %= 360;
                radian = PI / 180.0 * angle;
            }
          
           
            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);

            int newWidth = (int)(Width*Abs(cosRadian) + Height *Abs (sinRadian));
            int newHeight = (int)(Height*Abs(cosRadian) + Width *Abs (sinRadian));

            int centerX = Width / 2;
            int centerY = Height / 2;
            int diffX = (newWidth - Width)/2;
            int diffY = (newHeight - Height)/2;

            int[,] R = new int[newHeight, newWidth];

            double sourceX, sourceY;
            int isourceX, isourceY;
            double nw, ne, sw, se, p, q;

            int y, x;

            for ( y = 0; y < newHeight; y++)
            {
                for ( x = 0; x < newWidth; x++)
                {
                    sourceX = (x - diffX - centerX) * cosRadian + (y - diffY - centerY) * ( - sinRadian);
                    sourceX += centerX;
                    sourceY = (x - diffX - centerX) * sinRadian + (y - diffY - centerY) * cosRadian;
                    sourceY += centerY;

                    isourceX = (int)sourceX;
                    isourceY = (int)sourceY;

                   

                    if (isourceX < 0 || isourceX >= Width - 1 || isourceY < 0 || isourceY >= Height - 1)
                        R[y, x] = 0;
                    else
                    {
                        nw = (double)G[isourceY, isourceX];
                        ne = (double)G[isourceY, isourceX];
                        sw = (double)G[isourceY, isourceX];
                        se = (double)G[isourceY, isourceX];
                        p = Abs(sourceX - isourceX);
                        q = Abs(sourceY - isourceY);

                        R[y, x] = biLinearInterp(nw, ne, sw, se, p, q);
                    }
                }
            }
            return R;
        }

        public static int biLinearInterp(double nw, double ne, double sw, double se, double p, double q)
        {
            double t, b;

            t = nw + p * (ne - nw);
            b = sw + p * (se - sw);
            return (int)(t + q * (b - t));  
        }

        public static PointD GetCenter(this AffinePos pos)
        {
            var xCenter = Abs( pos.LT.X - pos.RT.X)/2;
            var yCenter = Abs( pos.LT.Y - pos.RT.Y)/2;


            return new PointD(xCenter, yCenter);
        }

        public static AffinePos GetCentered(this AffinePos pos , Point center)
        {
            var wHalf = Abs(pos.LT.X - pos.RT.X)/2;
            var hHalf= Abs(pos.LT.Y - pos.RT.Y)/2;


            var x1 = center.X - wHalf;
            var x2 = center.X - wHalf;
            var x3 = center.X + wHalf;
            var x4 = center.X + wHalf;

            var y1 = center.Y - hHalf;
            var y3 = center.Y + hHalf;
            var y2 = center.Y + hHalf;
            var y4 = center.Y - hHalf;

            return new AffinePos(x1, y1, x2, y2, x3, y3, x4,y4);
        }
        public static double PosL2(PointD pos1, PointD pos2)
           => Sqrt(Pow(pos1.X - pos2.X, 2) + Pow(pos1.Y - pos2.Y, 2));

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

            return new AffinePos(x1, y1, x2, y2, x3, y3, x4, y4);
        }

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
            if (dx == 0) return default(TrnsData);
            //var angle_degree = 360 - Math.Atan2(dy, dx) * 180 / Math.PI;
            //double radian;
            //angle_degree %= 360;
            //radian = PI / 180.0 * angle_degree;
            var radian = Math.Atan(dy / dx);
            var radian2 = Math.Atan2(dy, dx);
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


    }

    public struct AffinePos
    {
        public PointD LB;
        public PointD LT;
        public PointD RT;
        public PointD RB;

        public AffinePos(double x1, double y1, double x2, double y2, double x3, double y3, double x4,double y4)
        {
            LB = new PointD(x1, y1);
            LT = new PointD(x2, y2);
            RT = new PointD(x3, y3);
            RB = new PointD(x4, y4);
        }


        public AffinePos(PointD lb , PointD lt , PointD rt, PointD rb)
        {
            LB = lb;
            LT = lt;
            RT = rt;
            RB = rb;
        }



    }

  

}
