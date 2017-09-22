using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System.Windows.Media;
using System.Windows.Controls;
using static EmguCV_Extension.Vision_Tool;
using static EmguCV_Extension.Preprocessing;
using static UtilTool.UI.Corrdinate;
using EmguCV_Extension;
using System.Drawing;
using AccordLibBased.Clustering;
using AccordLibBased.FeatureExtract;
using System.Diagnostics;

namespace WaferandChipProcessing
{
    public partial class MainCore
    {
        void Register_ProcMethod()
        {
            Proc_Method_List = new Dictionary<SampleType , Func<Image<Gray , byte> , Image<Gray , byte>>>();
            Proc_Method_List.Add( SampleType.None , CreateMethod_None() );
            Proc_Method_List.Add( SampleType._1B6R , CreateMethod_1B6R() );
            Proc_Method_List.Add( SampleType._A , CreateMethod_A() );
            Proc_Method_List.Add( SampleType._B , CreateMethod_BC() );
            Proc_Method_List.Add( SampleType._C , CreateMethod_BC() );
            Proc_Method_List.Add( SampleType._D , CreateMethod_D() );
            Proc_Method_List.Add( SampleType._BlueLD , CreateMethod_BlueLD() );
            Proc_Method_List.Add( SampleType.Fullested , CreateMethod_FullEst() );
            Proc_Method_List.Add( SampleType.Lumence_0620 , CreateMethod_Lumence0620() );
            Proc_Method_List.Add( SampleType.Lumence_0620_2mm , CreateMethod_Lumence0620_2mm() );
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_None()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                var backInten     = BackgroundInten(img);
                var thresImg      = DoThreshold(img , (int)backInten + 5 );
                return thresImg;
            } );
            return method;
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_1B6R()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                var thresImg      = DoThreshold(img , (int)BackgroundInten(img) + 5 );
                return thresImg;
            } );
            return method;
        }
        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_A()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                // Not Work
                return null;
            } );
            return method;
        }
        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_BC()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                //var morped =  OpenRect(CloseRect( DoThreshold( img , 23) , 3) ,3);
                var morped =  DoThreshold( TempMatch_Ce( img , TemplateImg) , 120) ;

                for (int i = 0; i < 8; i++)
                {
                    morped = ErodeHori(morped , 3);
                }


                for (int i = 0; i < 8; i++)
                {
                    morped = DilateRect(morped , 3);
                }
                //
                //for (int i = 0; i < 6; i++)
                //{
                //    morped = DilateVerti(morped , 3);
                //}
                return morped;
            } );
            return method;
        }
        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_D()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                var imgg = DilateRect(DilateRect(CloseRect(DoThreshold(img , BackgroundInten(img) + 1) , 5),3),3);
                return DilateRect(DilateRect(CloseRect(DoThreshold(img , BackgroundInten(img) + 1) , 5),3),3);
            } );
            return method;
        }
        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_BlueLD()
        {
            var method = new Func<Image<Gray,byte>,Image<Gray,byte>>((img)=>
            {
                var backInten     = BackgroundInten(img);
                var thresImg      = DoThreshold(img , (int)backInten + 5 );
                return thresImg;
            } );
            return method;
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_FullEst()
        {
            var method = new Func<Image<Gray, byte>, Image<Gray, byte>>((img) =>
            {
                return img;
            });
            return method;
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_Lumence0620()
        {
            var method = new Func<Image<Gray, byte>, Image<Gray, byte>>((img) =>
            {
                return img.Median(5)
                         //.Normalize(120)
                         .Threshold(120)
                         .DilateRect();
            });
            return method;
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_Lumence0620_2mm()
        {
            var method = new Func<Image<Gray, byte>, Image<Gray, byte>>((img) =>
            {
                return img.Threshold(60)
                          .DilateRect();
            });
            return method;
        }

        Func<Image<Gray , byte> , Image<Gray , byte>> CreateMethod_Lumence0620_2mm_bck()
        {
            var method = new Func<Image<Gray, byte>, Image<Gray, byte>>((img) =>
            {
                //var clustered = ( img.ToBitmap() );
                Feature_Extract<byte,byte> Fe = new Feature_Extract<byte, byte>();
                K_Mean kmean = new K_Mean();

                var clustered = kmean.test(img.ToBitmap());

                double min = 255;
                foreach ( var item in clustered[ "center" ] )
                {
                    if ( item [ 0 ] < min ) min = item [ 0 ];
                }
                var clustedImg = new Image<Gray,byte>( clustered["image"]);


                var imgdata = clustedImg.Data;
                int rownum = imgdata.Len();
                int colnum = imgdata.Len(1);
                byte[][] lbpdata = rownum.JArray<byte>(colnum);
                for ( int j = 0 ; j < rownum ; j++ )
                {
                    var rows = new byte[colnum];
                    for ( int i = 0 ; i < colnum ; i++ )
                    {
                        rows [ i ] = imgdata [ j , i , 0 ];
                    }
                    lbpdata [ j ] = rows;
                }

                var lbp = new Image<Gray,byte> ( Fe.LBP(lbpdata)
                                                    .ConvertToImgData() );

                var tempimg1 = lbp.Normalize(60);
                var tempimg2 = tempimg1.Normalize(60);
                var tempimg3 = tempimg2.Normalize(60);




                return lbp.Normalize(60)
                          .Normalize(60)
                          .DilateRect()
                          .DilateRect()
                          .ErodeRect()
                          .ErodeRect();
            });
            return method;
        }
    }
}
