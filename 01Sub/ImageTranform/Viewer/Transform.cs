using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using ImageTranform;
using SpeedyCoding;
using System.IO;
using Emgu.CV.Stitching;
using Emgu.CV.Util;
using Emgu.CV.UI;


namespace Viewer
{

    using ClrImg = Image<Bgr, byte>;
    using Img = Image<Gray, byte>;
    using static ImageAlingn;
    using System.Drawing;

    public static class Transform
    {
        public static List<byte[]> ToByteMatrix(byte[] src, int w, int h)
        {
            List<byte[]> templist = new List<byte[]>();
            for (int i = 0; i < h ; i++)
            {
                byte[] newint = new byte[w];
                Buffer.BlockCopy(src, i * w , newint, 0, newint.Length);
                templist.Add(newint);
            }

            // var res = templist.Select(x => x.Select(l => (double)l).ToArray()).ToList();

            return templist;
        }


        public static List<ushort[]> ToUshortMatrix( byte[] src , int w , int h)
        {
            List<ushort[]> templist = new List<ushort[]>();
            for (int i = 0; i < h; i++)
            {
                ushort[] newint = new ushort[src.Length / 2];
                Buffer.BlockCopy(src, i * w * 2, newint, 0, newint.Length);
                templist.Add(newint);
            }

           // var res = templist.Select(x => x.Select(l => (double)l).ToArray()).ToList();

            return templist;
        }

        public static List<ushort[]> ToUshortMatrix(byte[][] src )
        {
            var w = src.GetLength(1);
            var h = src.GetLength(0);


            List<ushort[]> templist = new List<ushort[]>();
            for (int i = 0; i < h; i++)
            {
                ushort[] newint = new ushort[ w / 2];
                Buffer.BlockCopy(src, i * w * 2, newint, 0, newint.Length);
                templist.Add(newint);
            }
            return templist;
        }


        public static List<ushort[]> ToUshortMatrix(ushort[] src, int w, int h)
        {
            List<ushort[]> templist = new List<ushort[]>();
            for (int i = 0; i < w * 2; i++)
            {
                ushort[] newint = new ushort[src.Length];
                Buffer.BlockCopy(src, i * w * 2, newint, 0, newint.Length);
                templist.Add(newint);
            }

            // var res = templist.Select(x => x.Select(l => (double)l).ToArray()).ToList();

            return templist;
        }


        public static double[] GetTransMatrix(PointF[] first, PointF[] last)
        {
            var transmat = CvInvoke.GetAffineTransform(first, last);
            var data = transmat.GetData();

            double[] datares = new double[data.Length / 8];
            Buffer.BlockCopy(data, 0, datares, 0, data.Length);
            return datares;
        }

        public static List<IndexData<A>[]> ToIndexData<A>(List<A[]> src)
        {
            var hmax = src.Count();
            var wmax = src[0].Length;

            List<IndexData<A>[]> res = new List<IndexData<A>[]>();

            for (int j = 0; j < hmax; j++)
            {
                IndexData<A>[] tempidx = new IndexData<A>[wmax];
                for (int i = 0; i < wmax; i++)
                {
                    tempidx[i] = new IndexData<A>(i, j, src[j][i]);
                }
                res.Add(tempidx);
            }
            return res;
        }


        public static IndexData<A> TransOperation<A>(double[] mat, IndexData<A> src)
        {
            var neww = mat[0] * src.W + mat[1] * src.H + mat[2];
            var newh = mat[3] * src.W + mat[4] * src.H + mat[5];

            src.W = (int)neww;
            src.H = (int)newh;

            return src;
        }



    }

    public struct IndexData<T>
    {
        public int W;
        public int H;

        public T Data;

        public IndexData(int w, int h, T data)
        {
            W = w;
            H = h;
            Data = data;
        }

    }

}
