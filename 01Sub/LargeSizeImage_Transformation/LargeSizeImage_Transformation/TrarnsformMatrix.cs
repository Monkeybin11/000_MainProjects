using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeSizeImage_Transformation
{
    using System.Drawing;
    using static Math;
    using static TranformUtil;

    internal static class TrarnsformMatrix
    {
        public static void xytransform(ref byte[][] target, TrnsData data, byte[][] src, int hidx, int xmax, int ymax)
        {
           
            var h = src.GetLength(0);
            var w = src[0].GetLength(0);
          
            var tcenterX = (int)xmax / 2;
            var tcenterY = (int)ymax / 2;

            var xshift = (int)(data.XSrcCnter - tcenterX);
            var yshift = (int)(data.YSrcCnter - tcenterY);

           

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

                    
                        if (j - yshift + hidx * h < target.Length
                            && i - xshift < target[0].Length)
                        { target[j - yshift + hidx * h][i - xshift] = src[j][i]; }
                    }
                }
            }
            //return R;
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

       
            double sourceX, sourceY;
            int isourceX, isourceY;
            int isourceX2, isourceY2;

            int y, x;

            int h = G.Length;


            for (y = 0; y < G.Length; y++)
            {
                for (x = 0; x < G[0].Length; x++)
                {

                    sourceX = (x - centerX) * cosRadian + (y + hidx * h - centerY) * (sinRadian);
                    sourceX += (centerX + diffX);
                    sourceY = (x - centerX) * -sinRadian + (y + hidx * h - centerY) * cosRadian;
                    sourceY += (centerY + diffY);
                    //  이부분은 ok

                    isourceX = (int)Math.Truncate(sourceX);
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
        }


    }
}
