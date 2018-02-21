using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTranform
{
    using System.Drawing;
    using static Math;
    using static TranformUtil;

    using TrsInfoFunc = Func<int, int, AffinePos, AffinePos, TrnsData>;


    public static class TransformMetrix
    {
       //public static Func< byte[,] ,TrnsData, byte[,],int , byte[,]> Translation
       //    => (trg,tData, src,h)
       //    => xytransform(trg,tData, src , h );
       //
       //public static Func<byte[,],TrnsData, byte[,] , int , byte[,]> Rotation
       //  => (trg,tData, src,h)
       //  => rotateGeneral(trg,tData, src , h );

        public static Func<TrnsData> CreateTrnsData
            (AffinePos srcPos, int w, int h)
            => new Func<TrnsData>(() => srcPos.ToTrnsData(w,h) );



        /// <summary>
        /// affine pos , w , h
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
      
         //   정사각형만 가능
        #region  Transform
        public static void xytransform(ref byte[][] target , TrnsData data, byte[][] src , int hidx , int xmax , int ymax )
        {
            // 현재 소스의 크기는 맥스치로 되어있다. 
            //맥스치의 w,h
            var h = src.GetLength(0);
            var w = src[0].GetLength(0);

            //확장 캔버스의 센터 위치 
            var tcenterX = (int)xmax / 2;
            var tcenterY = (int)ymax / 2;

            var xshift =  (int)(data.XSrcCnter - tcenterX);
            var yshift =  (int)(data.YSrcCnter - tcenterY);

            //byte[,] R = new byte[target.GetLength(0),target.GetLength(1)];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (i - xshift >= 0
                        && j - yshift >= 0
                        && i - xshift < target[0].Length
                        && j - yshift < target.Length)
                    {
                        var yidx = (j - yshift + hidx).ToString();
                        var xidx = (i - xshift).ToString();

                        //var str = string.Format("y : {0}   x : {1} = {2}", yidx, xidx, src[j][i]);
                        //Console.WriteLine(str);
                        if (j - yshift + hidx * h < target.Length
                            && i - xshift < target[0].Length)
                        { target[j - yshift + hidx * h][i - xshift] = src[j][i]; }
                    }
                }
            }
            //return R;
        }

        public static int[,] xytransform(AffinePos srcPos, AffinePos trgPos, int[,] src)
        {
            var h = src.GetLength(0);
            var w = src.GetLength(1);
            var fcenterX = srcPos.GetCenter().X;
            var fcenterY = srcPos.GetCenter().Y;
            var lcenterX = trgPos.GetCenter().X;
            var lcenterY = trgPos.GetCenter().Y;
            var xshift = lcenterX - fcenterX;
            var yshift = lcenterY - fcenterY;
            var dx = Abs(srcPos.LT.X - srcPos.RT.X);
            var dy = Abs(srcPos.LT.Y - srcPos.RT.Y);
            if (dx == 0) return null;
            var angle_degree = 360 - Math.Atan2(dy, dx) * 180 / Math.PI;

            int[,] R = new int[(int)(h + Abs(yshift)), (int)(w + Abs(xshift))];

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    if (i - xshift >= 0
                        && j - yshift >= 0
                        && i - xshift < R.GetLength(1)
                        && j - yshift < R.GetLength(0))
                    {
                        R[(int)(j - yshift),(int)( i - xshift)] = src[j, i];
                    }
                }
            }
            return R;
        }

        public static void rotateGeneral(ref byte[][] target, TrnsData data, byte[][] G, int hidx, int oldw, int oldh)
        {

            double radian = data.Angle;
            var newWidth = target[0].Length;
            var newHeight = target.Length;

            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);


            int centerX = oldw / 2;
            int centerY = oldh / 2;
            int diffX = (newWidth - oldw) / 2;
            int diffY = (newHeight - oldh) / 2;
           
            int isourceX, isourceY;          

            int y, x;

            int h = G.Length;

            for (y = 0; y < target.Length; y++)
            {
                for (x = 0; x < target[0].Length; x++)
                {
                    var dx = x - (centerX + diffX);
                    var dy = y - (centerY + diffY);

                    var xres = dx * cosRadian - dy * sinRadian;
                    var yres = dx * sinRadian + dy * cosRadian;

                    var srcX = xres + centerX;
                    var srcY = yres - hidx * h + centerY;


                    isourceX = (int)Math.Round(srcX);
                    isourceY = (int)Math.Round(srcY);

                    try
                    {
                        if (isourceY < G.Length
                            && isourceY >= 0
                            && isourceX < G[0].Length
                            && isourceX >= 0)

                        {
                            target[y][x] = G[isourceY][isourceX];
                        }
                    }
                    catch (Exception es)
                    {
                        Console.WriteLine(es.ToString());
                    }

                }
            }
        }


        public static void rotateGeneral2( ref byte[][] target,TrnsData data, byte[][] G, int hidx, int oldw, int oldh)
        {

            double radian = data.Angle;
            //double radian = 0.785398;
            var newWidth = target[0].Length;
            var newHeight = target.Length;

            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);

           
            int centerX = oldw / 2;
            int centerY = oldh / 2;
            int diffX = (newWidth - oldw) / 2;
            int diffY = (newHeight - oldh) / 2;

            //byte[,] R = new byte[newHeight, newWidth];

            double sourceX, sourceY;
            int isourceX, isourceY;
            int isourceX2, isourceY2;
            double nw, ne, sw, se, p, q;

            int y, x;
            // -- OK 

            int h = G.Length;


            for (y = 0; y < G.Length; y++)
            {
                for (x = 0; x < G[0].Length; x++)
                {
                  
                    sourceX = (x - centerX) * cosRadian + (y + hidx*h- centerY) * (sinRadian);
                    sourceX += (centerX+ diffX);
                    sourceY = (x - centerX) * -sinRadian + (y + hidx*h - centerY) * cosRadian;
                    sourceY += (centerY+diffY);
                    //  이부분은 ok

                    isourceX = (int)Math.Truncate( sourceX);
                    isourceY = (int)Math.Truncate(sourceY);

                    isourceX2 = (int)Math.Ceiling(sourceX);
                    isourceY2 = (int)Math.Ceiling(sourceY);

                    try
                    {
                        if (isourceY < target.Length
                            && isourceX < target[0].Length
                            && isourceY2 < target[0].Length
                            && isourceX2 < target.Length)
                        {
                                target[isourceY][isourceX] = G[y][x];
                                target[isourceY2][isourceX2] = G[y][x];
                                target[isourceY][isourceX2] = G[y][x];
                                target[isourceY2][isourceX] = G[y][x];
                        }
                       

                    }
                    catch (Exception es)
                    {
                        Console.WriteLine(es.ToString());
                    }
                   
                }
            }
           // return target;
        }

        public static int[,] rotateGeneral(int[,] G, int Width, int Height, double angle)
        {

            double radian;
            angle %= 360;
            radian = PI / 180.0 * angle;

            double cosRadian = Cos(radian);
            double sinRadian = Sin(radian);

            int newWidth = (int)(Width * Abs(cosRadian) + Height * Abs(sinRadian));
            int newHeight = (int)(Height * Abs(cosRadian) + Width * Abs(sinRadian));

            int centerX = Width / 2;
            int centerY = Height / 2;
            int diffX = (newWidth - Width) / 2;
            int diffY = (newHeight - Height) / 2;

            int[,] R = new int[newHeight, newWidth];

            double sourceX, sourceY;
            int isourceX, isourceY;
            double nw, ne, sw, se, p, q;

            int y, x;

            for (y = 0; y < newHeight; y++)
            {
                for (x = 0; x < newWidth; x++)
                {
                    sourceX = (x - diffX - centerX) * cosRadian + (y - diffY - centerY) * (-sinRadian);
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

                        R[y, x] = biLinearInterp_int(nw, ne, sw, se, p, q);
                    }
                }
            }
            return R;
        }
        #endregion

    }
}
