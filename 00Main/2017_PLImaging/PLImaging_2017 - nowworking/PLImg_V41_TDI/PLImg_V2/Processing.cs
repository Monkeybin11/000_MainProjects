using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedChipPassFail_first;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;
using Emgu.CV.Util;
using WaferandChipProcessing;
using WaferandChipProcessing.Data;
using WaferandChipProcessing.Func;
using EmguCV_Extension;

namespace PLImg_V2
{
    public partial class MainWindow
    {
        EpiCore EPCore;

        void InitCoreEpi()
        {
            EPCore = new EpiCore( 7500 );


            // Regist Event . Event Flow : Drop => Run Event Method on EpiCore.Epi_Event[i].DropEventMethod 
            //                                                -> evtDropDone Triggerd :CoreEpi.SetImage Run
            EPCore.Act( core => core.evtDroppedImg += new ImgForDisplay( EpiDisplayOrigImg ) )
                  .Act( core => core.evtTrsFullImg += new TrsFullImage( EvtTransfullEpiImg ) )
                  .Act( core => core.evtTrsResizedProcedImg += new TrsProcedImage( EpiDisplayProcedImg ) )
                  .Act( core => core.evtTrsIdxImg += new TrsProcedImage( EpiDisplayDefcetIdxImg ) )
                  .Act( core => core.evtStatistic += new TrsStatistic( EpiDisplayStatisticResult ) )
                  .Act( core => core.evtProcTime += new TrsProgress( EpiDisplayProcTime ) );
                  //.Act( core => imgEpIndex.ImageSource = core.IndexViewImg.ToBitmapSource() );
        }

        #region Epi Event

        void EpiDisplayOrigImg( ImgIdxPos pos, Image<Gray, byte> img )
        {


        }


            void EvtTransfullEpiImg( Tuple<ImgIdxPos, Image<Gray, byte>>[] zippedlist )
        {
            //this.Dispatcher.Invoke( (Action)(() => zippedlist.ActLoop( ths => EpiDisplayOrigImg( ths.Item1, ths.Item2 ) )) );
        }

        void EpiDisplayProcedImg( Image<Bgr, byte> procedidximg )
        {
            //this.Dispatcher.Invoke( (Action)(() => imgEpProced.ImageSource = procedidximg.ToBitmapSource()) );
        }



        void EpiDisplayDefcetIdxImg( Image<Bgr, byte> procedIdxImg )
        {
            //this.Dispatcher.Invoke( (Action)(() => imgEpIndex.ImageSource = procedIdxImg.ToBitmapSource()) );
            //imboxEmgu.Image = procedIdxImg;
            //imboxEmgu.Refresh();
        }
        void EpiProgress( int progress )
        {
            //this.Dispatcher.Invoke( ( Action )( () => lblprgEpi.Content = progress.ToString() + " %" ) );
        }

        void EpiDisplayProcTime( int procTime )
        {
            //this.Dispatcher.Invoke( (Action)(() => lblProcTime.Content = procTime.ToString() + " (um)") );
        }


        void EpiDisplayStatisticResult( int[] statisticResult )
        {
            this.Dispatcher.Invoke( (Action)(() =>
            {
                //lblSize1.Content = statisticResult[0].ToString();
                //lblSize2.Content = statisticResult[1].ToString();
                //lblSize3.Content = statisticResult[2].ToString();
                //lblSize4.Content = statisticResult[3].ToString();
            }) );
        }


        #endregion


    }
}
