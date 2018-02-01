using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using ImageTranform;
using SpeedyCoding;

namespace Viewer
{
    using ClrImg = Image<Bgr, byte>;
    using Img = Image<Gray, byte>;
    using static Transfom;
    using System.IO;
    using Emgu.CV.Stitching;
    using Emgu.CV.Util;
    using Emgu.CV.UI;
    using static Transform;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    ClrImg img = new ClrImg(new byte[160000, 100 * i, 3]);
                }
                catch (Exception)
                {
                    Console.WriteLine(i.ToString());
                }
            }
            Console.WriteLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var cpos = new CornerPoints()
            {
                x0 = 10,
                y0 = -4.14213562,
                x1 = -4.14213562,
                y1 = 10,
                x2 = 10,
                y2 = 24.14213562,
                x3 = 24.14213562,
                y3 = 10,
            };

            var rightpos = new CornerPoints()
            {
                x0 = -10,
                y0 = -10,
                x1 = -10,
                y1 = 10,
                x2 = 10,
                y2 = 10,
                x3 = 10,
                y3 = -10,
            };

            var current = ToTransPos(cpos);
            var target = ToTransPos(rightpos);

            var mat = CvInvoke.GetAffineTransform(current, target);

            var inputmat = new Mat();

            double[] xy = new double[] { 10, -4.14213562 };




            //var res = mat.Dot(2,1,DepthType.Cv64F , 1 , ,1);

            Console.WriteLine();




        }

        //Split Image
        private void button3_Click(object sender, EventArgs e)
        {
            string path = @"C:\Data\testImage.png";

            Image<Gray, byte> img = new Image<Gray, byte>(path);

            var w = img.Width;
            var h = img.Height;

            var wstep = w / 3;
            var hstep = h / 2;

            Rectangle rect1 = new Rectangle(0, 0, wstep + 100, hstep + 100);
            Rectangle rect2 = new Rectangle(wstep, 0, wstep + 100, hstep + 100);
            Rectangle rect3 = new Rectangle(wstep * 2 - 100, 0, wstep + 100, hstep + 100);
            Rectangle rect4 = new Rectangle(0, hstep - 100, wstep + 100, hstep + 100);
            Rectangle rect5 = new Rectangle(wstep - 100, hstep - 100, wstep + 100, hstep + 100);
            Rectangle rect6 = new Rectangle(wstep * 2 - 100, hstep - 100, wstep + 100, hstep + 100);

            img.ROI = rect1;
            var img1 = img.Copy();

            img.ROI = rect2;
            var img2 = img.Copy();

            img.ROI = rect3;
            var img3 = img.Copy();

            img.ROI = rect4;
            var img4 = img.Copy();

            img.ROI = rect5;
            var img5 = img.Copy();

            img.ROI = rect6;
            var img6 = img.Copy();

            List<Img> imglist = new List<Img>();
            imglist.Add(img1);
            imglist.Add(img2);
            imglist.Add(img3);
            imglist.Add(img4);
            imglist.Add(img5);
            imglist.Add(img6);

            string basepath = @"C:\Data\res" + "\\";

            for (int i = 0; i < imglist.Count; i++)
            {
                imglist[i].Save(basepath + i.ToString() + ".png");
            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            int centerx = 2809 / 2;
            int centery = 2739 / 2;

            var pp0 = Tuple.Create(954, 2139);
            var pp1 = Tuple.Create(600, 375);
            var pp2 = Tuple.Create(2363, 18);

            var innerw = pp1.L2(pp2) / 2;
            var innerh = pp0.L2(pp1) / 2;




            var p0 = new PointF((float)954, (float)2139);
            var p1 = new PointF((float)600, (float)375);
            var p2 = new PointF((float)2363, (float)18);

            var ptarget0 = new PointF((float)(centerx - innerw), (float)(centery - innerh));
            var ptarget1 = new PointF((float)(centerx - innerw), (float)(centery + innerh));
            var ptarget2 = new PointF((float)(centerx + innerw), (float)(centery + innerh));


            var srcPoints = new PointF[] { p0, p1, p2 };
            var trgPoints = new PointF[] { ptarget0, ptarget1, ptarget2 };

            var transmat = CvInvoke.GetAffineTransform(srcPoints, trgPoints);

            var data = transmat.GetData();

            // ---

            var afinmat = ToAffinenMetrix(srcPoints, trgPoints);
            // ---



            double[] datares = new double[data.Length / 8];
            Buffer.BlockCopy(data, 0, datares, 0, data.Length);


            string[] files = Directory.GetFiles(@"C:\Data\res");



            List<Img> imglist = new List<Img>();

            for (int i = 0; i < files.Length; i++)
            {
                imglist.Add(new Img(files[i]));
            }

            for (int i = 0; i < imglist.Count; i++)
            {
                var w = imglist[i].Width;
                var h = imglist[i].Height;

                var size = new Size(w * 3, h * 3);
                //var size = new Size(w, h);

                var tempimg = imglist[i].Copy();

                CvInvoke.WarpAffine(imglist[i], tempimg, transmat, size);
                tempimg.Save(@"C:\Data\transres\" + i.ToString() + ".png");
            }
        }

        //string Basepath = @"C:\Data\transres\";
        string Basepath = @"C:\Data\res\";


        private void button5_Click(object sender, EventArgs e)
        {
            string[] paths = Directory.GetFiles(Basepath);

            List<Mat> matlist = new List<Mat>();

            for (int i = 0; i < paths.Length; i++)
            {
                var tempmat = new Mat(paths[i], ImreadModes.Color);

                matlist.Add(tempmat);
            }


            using (VectorOfMat vmsrc = new VectorOfMat(matlist.ToArray()))
            {
                //Image<Bgr, byte> res = new Image<Bgr, byte>(28090, 27390);
                Mat result = new Mat();
                Stitcher stitcher = new Stitcher(false);


                Stitcher.Status stitchStatus = stitcher.Stitch(vmsrc, result);

                ImageViewer.Show(result);
                //result.Save(Basepath + "testresult.png");

            }





        }

        // 여기서 커스텀으로 적용한다. 
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                var img = new Image<Gray, ushort>(path);

                var w = img.Width;
                var h = img.Height;


                var flatmat = img.Data.Flatten();

                var srcmat = ToUshortMatrix(flatmat, w, h); // Ushort Matrix
                var idxdata = ToIndexData(srcmat); // IdxData Matrix

                // Get TransMatrix
                var trsMat = GetTransMat2();
                var trsMatcv = GetTransMat().Item2;

                // Update idx
                var transedres = idxdata.Select(x => x.Select(l => TransOperation(trsMat, l)).ToArray()).ToList();


                var xmax = transedres.Select(x => x.Select(k => k.W).Max()).Max(); 
                var ymax = transedres.Select(x => x.Select(k => k.H).Max()).Max();

                var xmin = transedres.Select(x => x.Select(k => k.W).Min()).Min();
                var ymin = transedres.Select(x => x.Select(k => k.H).Min()).Min();

                var xshift = 0 - xmin;
                var yshift = 0 - ymin;

                var wLen = xmax - xmin;
                var hLen = ymax - ymin;

                ushort[][] result = new ushort[hLen][].Select( x => new ushort[wLen]).ToArray();

                for (int j = 0; j < transedres.Count; j++)
                {
                    for (int i = 0; i < transedres[j].Length; i++)
                    {
                        var x = transedres[j][i].W;
                        var y = transedres[j][i].H;

                        result[y][x] = transedres[j][i].Data;
                    }
                }

                /////// For image /////

                for (int j = 0; j < transedres.Count; j++)
                {
                    for (int i = 0; i < transedres[j].Length; i++)
                    {
                        var x = transedres[j][i].W;
                        var y = transedres[j][i].H;

                        result[y][x] = (byte)result[y][x];
                    }
                }

            }
        }


        #endregion  

        // emgu와 커스텀 비교하는 코드
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                var img = new Image<Gray, byte>(path);

                var w = img.Width;
                var h = img.Height;


                var flatmat = img.Data.Flatten();

                var srcmat =  ToByteMatrix(flatmat, w, h); // byte Matrix
                var idxdata = ToIndexData(srcmat); // IdxData Matrix

                // Get TransMatrix
                var trsMat = GetTransMat2();
                Mat trsMatcv = GetTransMat_Small_Mat();

                int[] idx = new int[] { 0 };
                int[] idx2 = new int[] { 1 };
                var emgudata = trsMatcv.GetData(idx);
                var emgudata2 = trsMatcv.GetData(idx2);


                double[] up = new double[3];
                double[] dw = new double[3];

                Buffer.BlockCopy(emgudata, 0, up, 0, idx.Length);
                Buffer.BlockCopy(emgudata2, 0, dw, 0, idx.Length);



               

                //var datatemp1 = BitConverter.ToDouble (emgudata);
                //var datatemp2 = BitConverter.ToInt64  (emgudata);
                //var datatemp3 = BitConverter.ToUInt64 (emgudata);

                // Update idx
                var transedres = idxdata.Select(x => x.Select(l => TransOperation(trsMat, l)).ToArray()).ToList();


                //trsMatcv.Save(@"C:\Data\test.txt");

                 var xmax = transedres.Select(x => x.Select(k => k.W).Max()).Max();
                var ymax = transedres.Select(x => x.Select(k => k.H).Max()).Max();

                var xmin = transedres.Select(x => x.Select(k => k.W).Min()).Min();
                var ymin = transedres.Select(x => x.Select(k => k.H).Min()).Min();

                var xshift = 0 - xmin;
                var yshift = 0 - ymin;

                var wLen = xmax - xmin+1;
                var hLen = ymax - ymin+1;

                //--------
                var testimg = img.Copy();

                CvInvoke.WarpAffine(img, testimg, trsMatcv, new Size(wLen, hLen));

                testimg.Save(@"C:\Data\testimg2_EMgucv.png");

                //--------


                byte[][] result = new byte[hLen][].Select(x => new byte[wLen]).ToArray();

                for (int j = 0; j < transedres.Count; j++)
                {
                    for (int i = 0; i < transedres[j].Length; i++)
                    {
                        var x = transedres[j][i].W + xshift ;
                        var y = transedres[j][i].H + yshift ;

                        result[y][x] = transedres[j][i].Data;
                    }
                }
                //여기서는 이 result 를 flatten해서 보내면 된다. !
                /////// For image /////

                var h0 = result.Count();
                var w0 = result[0].Length;

                byte[,,] imgdata = new byte[h0, w0, 1];

                for (int j = 0; j < h0; j++)
                {
                    for (int i = 0; i < w0; i++)
                    {
                        imgdata[j, i, 0] = result[j][i];
                    }
                }

                Image<Gray, byte> tempimg = new Img(imgdata);

                var dirpath = Path.GetDirectoryName(ofd.FileName);

                tempimg.Save( dirpath + "\\result_Custom.png" );

               

            }
        }


        // Matrix Test
        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                var img = new Image<Gray, byte>(path);

                var w = img.Width;
                var h = img.Height;


                var flatmat = img.Data.Flatten();

                var srcmat = ToByteMatrix(flatmat, w, h); // byte Matrix
                var idxdata = ToIndexData(srcmat); // IdxData Matrix

                // Get TransMatrix
                var trsMat = GetTransMat_Small45do();
                Mat trsMatcv = GetTransMat_Small45do_Cv();

                int[] idx = new int[] { 0 };
                int[] idx2 = new int[] { 1 };
                var emgudata = trsMatcv.GetData(idx);
                var emgudata2 = trsMatcv.GetData(idx2);


                double[] up = new double[3];
                double[] dw = new double[3];

                Buffer.BlockCopy(emgudata, 0, up, 0, idx.Length);
                Buffer.BlockCopy(emgudata2, 0, dw, 0, idx.Length);





                //var datatemp1 = BitConverter.ToDouble (emgudata);
                //var datatemp2 = BitConverter.ToInt64  (emgudata);
                //var datatemp3 = BitConverter.ToUInt64 (emgudata);

                // Update idx
                var transedres = idxdata.Select(x => x.Select(l => TransOperation(trsMat, l)).ToArray()).ToList();


                //trsMatcv.Save(@"C:\Data\test.txt");

                var xmax = transedres.Select(x => x.Select(k => k.W).Max()).Max();
                var ymax = transedres.Select(x => x.Select(k => k.H).Max()).Max();

                var xmin = transedres.Select(x => x.Select(k => k.W).Min()).Min();
                var ymin = transedres.Select(x => x.Select(k => k.H).Min()).Min();

                var xshift = 0 - xmin;
                var yshift = 0 - ymin;

                var wLen = xmax - xmin + 1;
                var hLen = ymax - ymin + 1;

                //--------
                var testimg = img.Copy();

                CvInvoke.WarpAffine(img, testimg, trsMatcv, new Size(wLen, hLen));

                testimg.Save(@"C:\Data\testimg2_EMgucv.png");

                //--------


                byte[][] result = new byte[hLen][].Select(x => new byte[wLen]).ToArray();

                for (int j = 0; j < transedres.Count; j++)
                {
                    for (int i = 0; i < transedres[j].Length; i++)
                    {
                        var x = transedres[j][i].W + xshift;
                        var y = transedres[j][i].H + yshift;

                        result[y][x] = transedres[j][i].Data;
                    }
                }
                //여기서는 이 result 를 flatten해서 보내면 된다. !
                /////// For image /////

                var h0 = result.Count();
                var w0 = result[0].Length;

                byte[,,] imgdata = new byte[h0, w0, 1];

                for (int j = 0; j < h0; j++)
                {
                    for (int i = 0; i < w0; i++)
                    {
                        imgdata[j, i, 0] = result[j][i];
                    }
                }

                Image<Gray, byte> tempimg = new Img(imgdata);

                var dirpath = Path.GetDirectoryName(ofd.FileName);

                tempimg.Save(dirpath + "\\result_Custom.png");

            }
        }


            ///////////////////////////////////////

            double[] GetTransMat_Small45do()
        {

            var pf0 = new PointF((float)1, (float)0);
            var pf1 = new PointF((float)1, (float)1);
            var pf2 = new PointF((float)1, (float)2);

            var pl0 = new PointF((float)0, (float)0);
            var pl1 = new PointF((float)1, (float)1);
            var pl2 = new PointF((float)2, (float)2);



            var srcPoints = new PointF[] { pf0, pf1, pf2 };
            var trgPoints = new PointF[] { pl0, pl1, pl2 };


            var afinmat = ToAffinenMetrix(srcPoints, trgPoints);
            // ---

            return afinmat.Flatten();
        }


        Mat GetTransMat_Small45do_Cv()
        {

            var pf0 = new PointF((float)10, (float)0);
            var pf1 = new PointF((float)10, (float)10);
            var pf2 = new PointF((float)10, (float)20);

            var pl0 = new PointF((float)0, (float)0);
            var pl1 = new PointF((float)1, (float)1);
            var pl2 = new PointF((float)2, (float)2);



            var srcPoints = new PointF[] { pf0, pf1, pf2 };
            var trgPoints = new PointF[] { pl0, pl1, pl2 };


            var afinmat = CvInvoke.GetAffineTransform(srcPoints, trgPoints);
            // ---

            return afinmat;
        }


        double[] GetTransMat_Small2()
        {

            var pf0 = new PointF((float)1, (float)0);
            var pf1 = new PointF((float)1, (float)1);
            var pf2 = new PointF((float)1, (float)2);

            var pl0 = new PointF((float)0, (float)1);
            var pl1 = new PointF((float)1, (float)2);
            var pl2 = new PointF((float)2, (float)3);



            var srcPoints = new PointF[] { pf0, pf1, pf2 };
            var trgPoints = new PointF[] { pl0, pl1, pl2 };


            var afinmat = ToAffinenMetrix(srcPoints, trgPoints);
            // ---

            return afinmat.Flatten();
        }

        double[] GetTransMat_Small()
        {

            var pf0 = new PointF((float)59, (float)22);
            var pf1 = new PointF((float)77, (float)13);
            var pf2 = new PointF((float)67, (float)41);

            var pl0 = new PointF((float)27, (float)44);
            var pl1 = new PointF((float)47, (float)44);
            var pl2 = new PointF((float)27, (float)64);



            var srcPoints = new PointF[] { pf0, pf1, pf2 };
            var trgPoints = new PointF[] { pl0, pl1, pl2 };


            var afinmat = ToAffinenMetrix(srcPoints, trgPoints);
            // ---

            return afinmat.Flatten();
        }

        Mat GetTransMat_Small_Mat()
        {

            var pf0 = new PointF((float)59, (float)22);
            var pf1 = new PointF((float)77, (float)13);
            var pf2 = new PointF((float)67, (float)41);

            var pl0 = new PointF((float)27, (float)44);
            var pl1 = new PointF((float)47, (float)44);
            var pl2 = new PointF((float)27, (float)64);



            var srcPoints = new PointF[] { pf0, pf1, pf2 };
            var trgPoints = new PointF[] { pl0, pl1, pl2 };


            var afinmat = CvInvoke.GetAffineTransform(srcPoints, trgPoints);
            // ---

            return afinmat;
        }




        /// <summary>
        /// ///////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        double[] GetTransMat2()
        {
            int centerx = 2809 / 2;
            int centery = 2739 / 2;

            var pp0 = Tuple.Create(954, 2139);
            var pp1 = Tuple.Create(600, 375);
            var pp2 = Tuple.Create(2363, 18);

            var innerw = pp1.L2(pp2) / 2;
            var innerh = pp0.L2(pp1) / 2;



            var p0 = new PointF((float)954, (float)2139);
            var p1 = new PointF((float)600, (float)375);
            var p2 = new PointF((float)2363, (float)18);

            var ptarget0 = new PointF((float)(centerx - innerw), (float)(centery - innerh));
            var ptarget1 = new PointF((float)(centerx - innerw), (float)(centery + innerh));
            var ptarget2 = new PointF((float)(centerx + innerw), (float)(centery + innerh));


            var srcPoints = new PointF[] { p0, p1, p2 };
            var trgPoints = new PointF[] { ptarget0, ptarget1, ptarget2 };


            var afinmat = ToAffinenMetrix(srcPoints, trgPoints);
            // ---

            return afinmat.Flatten();
        }

        Tuple<double[], Mat> GetTransMat()
        {
            int centerx = 2809 / 2;
            int centery = 2739 / 2;

            var pp0 = Tuple.Create(954, 2139);
            var pp1 = Tuple.Create(600, 375);
            var pp2 = Tuple.Create(2363, 18);

            var innerw = pp1.L2(pp2) / 2;
            var innerh = pp0.L2(pp1) / 2;

            var p0 = new PointF((float)954, (float)2139);
            var p1 = new PointF((float)600, (float)375);
            var p2 = new PointF((float)2363, (float)18);

            var ptarget0 = new PointF((float)(centerx - innerw), (float)(centery - innerh));
            var ptarget1 = new PointF((float)(centerx - innerw), (float)(centery + innerh));
            var ptarget2 = new PointF((float)(centerx + innerw), (float)(centery + innerh));


            var srcPoints = new PointF[] { p0, p1, p2 };
            var trgPoints = new PointF[] { ptarget0, ptarget1, ptarget2 };

            var transmat = CvInvoke.GetAffineTransform(srcPoints, trgPoints);

            var data = transmat.GetData();

            double[] datares = new double[data.Length / 8];
            Buffer.BlockCopy(data, 0, datares, 0, data.Length);

            return Tuple.Create(datares, transmat);
        }




    }
}
