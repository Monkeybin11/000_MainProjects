using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeSizeImage_Transformation
{
    public static class Helper
    {
        internal static Bitmap ToBitmap_SetPixel(this byte[,] src)
        {
            int h = src.GetLength(0);
            int w = src.GetLength(1);

            Bitmap img = new Bitmap(w, h);

            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    img.SetPixel(i, j, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }
            return img;
        }

        internal static byte[][] CreateJagged(int h, int w)
        {
            byte[][] output = new byte[h][];
            for (int i = 0; i < h; i++)
            {
                output[i] = new byte[w];
            }
            return output;

        }

        internal static Tsrc[,] Reshape<Tsrc>(
           this Tsrc[] src
           , int first
           , int second
           )
        {
            var result = new Tsrc[first, second];
            int idx = 0;
            for (int f = 0; f < first; f++)
            {
                for (int s = 0; s < second; s++)
                {
                    result[f, s] = src[idx++];
                }
            }
            return result;
        }

        internal static TSrc[][] ToJagged<TSrc>(
         this TSrc[,] @this)
        {
            int rowL = @this.Len(0), fcolL = @this.Len(1);

            TSrc[][] output = new TSrc[rowL][];
            for (int j = 0; j < rowL; j++)
            {
                TSrc[] second = new TSrc[fcolL];
                for (int i = 0; i < fcolL; i++)
                {
                    second[i] = @this[j, i];
                }
                output[j] = second;
            }
            return output;
        }
        private static int Len<TSrc>(
         this TSrc[,] src,
         int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src.GetLength(1);
            else return src.GetLength(0);
        }

        private static int Len<TSrc>(
        this TSrc[][] src,
        int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src[0].GetLength(0);
            else return src[0].GetLength(0);
        }


        internal static TSrc[,] ToMat<TSrc>(
          this TSrc[][] @this)
        {
            int rowL = @this.Len(0), colL = @this.Len(1);

            TSrc[,] output = new TSrc[rowL, colL];
            for (int j = 0; j < rowL; j++)
            {
                for (int i = 0; i < colL; i++)
                {
                    output[j, i] = @this[j][i];
                }
            }
            return output;
        }



    }
}
