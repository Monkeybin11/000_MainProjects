using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Forms;
using static Emgu.CV.CvEnum.Inter;
using System.Diagnostics;
using EmguCV_Extension;
using static EmguCV_Extension.Vision_Tool;
using System.Drawing;

namespace ProcessingLib
{
    public class PreProcessing
    {
        object key = new object();

        ProcessingLib ProcLib = new ProcessingLib();

        public ProcessingData PreProcessSingle(byte[,,] inputimg , int imgIdx)
        {
            lock ( key )
            {
                var dataBox = new ProcessingData();

                // Task1
                var oriImg = new Image<Gray,byte>(inputimg);
                var colorImg = new Image<Bgr,byte>(inputimg);
                dataBox.OriginalImg[imgIdx] = oriImg;
                dataBox.ColorImg[imgIdx] = colorImg;

                // Task2
                var preprocedimg = ProcLib.DummpyProc( oriImg );

                // Task3
                var boxlist = new List<Rectangle>();

                var applybox = FnApplyBox(50000,10);
                var findcntr = FnFindContour(30000,5);

                var cntrlist = findcntr( preprocedimg );
                boxlist = applybox( cntrlist );

                dataBox.BoxSeprList[imgIdx] = boxlist;
                return dataBox;
            }
        }
    }


    public class ProcessingData  
    {
        //Image  Index is Index of List
        public Image<Gray,byte>[] OriginalImg;
        public Image<Bgr,byte>[]  ColorImg;
        public Image<Gray,byte>[] PreProcedImg;
        public Image<Gray,byte>[] BoxedImg;

        public Image<Bgr,byte>  IndexImg;
        public Image<Gray,byte> RzOriImg;
        public Image<Bgr,byte>  RzBoxImg;

        public List<Rectangle>[] BoxSeprList;
        public List<Rectangle> BoxList;
    }

    public class EpiDefectClassify : IDefectClassify<Image<Gray, byte>, Image<Bgr, byte>, Rectangle, Tuple<string, double>[]> 
    {
        public StringBuilder Result { get; set; }
        public List<Tuple<string, double>[]> ResultData { get; set; }

        public List<Tuple<string, double>[]>[] Cnv2Result
            ( List<Rectangle>[] src
            , int resolution )
        {
            return src.Select( x => x.Rect2Result( resolution ) )
                      .ToArray();
        }

        public List<Tuple<string, double>[]>[] Cnv2Result
            ( Image<Gray, byte> imgsrc
            , List<Rectangle>[] src
            , int resolution )
        {
            return null;
        }

        public List<Tuple<string, double>[]> CombResult
            ( List<Tuple<string, double>[]>[] resultList 
            , int[] offsetList )
        {
            List<Tuple<string, double>[]>[] shited = new List<Tuple<string, double>[]>[ resultList.GetLength(0) ];
            for ( int i = 0; i < resultList.GetLength(0); i++ )
            {
                shited[i] = resultList[i].Select( x => x.ShiftResult( offsetList[i] ) ).ToList();
            }
            return null;
        }

        public Image<Bgr, byte> CombBoxedImg( Image<Bgr, byte>[] inputlist )
        {
            // Combine Boxed Image
            return null;
        }

        public Image<Gray, byte> CombGrayImg( Image<Gray, byte>[] inputlist )
        {
            throw new NotImplementedException();
        }

       

        public Image<Bgr, byte>[] DrawBoxedImg( Image<Bgr, byte>[] inputlist )
        {
            throw new NotImplementedException();
        }

        // --- with box input
        // BoxInfo => Result
        // => 


        // --- with result
        // Draw result on each image , resize each image ,combine each image 
        // with offset, shift result 


    }

    public class ProcessingLib
    {
        public Func<Image<Gray, byte>, Image<Gray, byte>> DummpyProc 
            => x => x.ThresholdBinary( new Gray( 120 ), new Gray( 255 ) );

    }



    public interface IDefectClassify<Tgrayimg, Tcolorimg,Tsrc, Tresult>
    {
        List< Tresult > ResultData { get; set; }
        StringBuilder Result { get; set; }

        // Convert
        List<Tresult>[] Cnv2Result( List<Tsrc>[] src , int resolution);
        List<Tresult>[] Cnv2Result( Tgrayimg imgsrc, List<Tsrc>[] src , int resolution);

        // Combine
        List<Tresult> CombResult( List<Tresult>[] resultList , int[] offsetList );
        Tgrayimg CombGrayImg( Tgrayimg[] inputlist );
        Tcolorimg CombBoxedImg( Tcolorimg[] inputlist );
        Tcolorimg[] DrawBoxedImg( Tcolorimg[] inputlist );
    }

    public static class EpiExtension
    {
        public static List<Tuple<string, double>[]> Rect2Result(
            this List<Rectangle> src 
            ,double resolution)
        {
            return src.Select( item =>
                       {
                           var cenX = (double)(item.X + item.Width)/2.0;
                           var cenY = (double)(item.Y + item.Height)/2.0;
                           var radius = (double)Math.Sqrt( item.Width * item.Height);
                           var realSize = (double) (item.Width * item.Height) * resolution * resolution;
                       
                           var output = new Tuple<string, double>[]
                                        {
                                            Tuple.Create("CenterY",cenY)
                                            ,Tuple.Create("CenterY",cenX)
                                            ,Tuple.Create("Radius",radius)
                                            ,Tuple.Create("RealSize",realSize)
                                        };
                           return output;
                       } ).ToList();
        }

        public static Tuple<string, double>[] ShiftResult(
            this Tuple<string, double>[] src 
            , int shift)
        {
            src[1] = Tuple.Create( src[1].Item1, src[1].Item2 + shift );
            return src;
        }
    }
}
