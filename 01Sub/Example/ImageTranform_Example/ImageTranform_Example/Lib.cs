using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTranform_Example
{
    using static Math;

    public static class Lib
    {
        public static int[,] rotateGeneral(int[,] G, int Width, int Height, double angle)
        {
            angle %= 360;
            double radian = PI / 180.0 * angle;
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



    }
}
