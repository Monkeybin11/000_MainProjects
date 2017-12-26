using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using SIPEngine.Recipe;
using Emgu.CV;
using Emgu.CV.Structure;
using SIP_InspectLib.Recipe;
using Emgu.CV;
using Emgu.CV.Util;
using SIP_InspectLib.DataType;
using SIP_InspectLib.Recipe;

namespace PLMapping_SIPCore
{

    using static SIP_InspectLib.Recipe.Adaptor;
    using static SIPEngine.Handler;
    using static ModelLib.AmplifiedType.Handler;
    using static SIP_InspectLib.DefectInspect.Handler;
    using static SIP_InspectLib.Indexing.Common;

    using Img = Image<Gray, byte>;
    using ProcFunc = Func<Image<Gray, byte>, Image<Gray, byte>>;
    using System.Drawing;

    public class Core_PlMapping
	{
		public void Start( string imgpath , string configpath )
		{

            var img = LoadImage( imgpath );
            var modellist = CreateModel(configpath);
            var resimg = RunProcessing(img , modellist);



            var inspectrecipe = ToInspctRecipe(configpath); // 
            List<ExResult> outdata = Indexing(inspectrecipe , resimg);

            var poseq =  EstedChipPosAndEq(inspectrecipe);
            var esetedindex = ToEstedIndex(poseq);



            var resisp1 = ToBoxList(resimg , inspectrecipe );


            Func<Rectangle,double> boxsum = new Func<Rectangle, double>( x => 1);


            var ested =  ToBoxIndex(inspectrecipe , boxsum , poseq ,  )




        }

        #region
        public Img LoadImage(string imgpath)
		{
            return default( Img );
        }


		// with maybe
		public InspctRescipe LoadConfig( string configpath )
		{

			return null;
		}

		public static Func<string , IEnumerable<ProcFunc>> CreateModel
			=> recipe 
			=>
		{
			return null;
		};

		public static Func<Img , IEnumerable<Func<Img , Img>> ,  Img> Preprocess
			=> (src, ModelLib)
			=> RunProcessing( src , ModelLib );


        public Func<string, InspctRescipe> ToInspctRecipe
           => path
           =>
           {
               return null;
           };



        #endregion



        public Func<InspctRescipe, Img , List<ExResult> > Indexing
            => ( recipe, img )
            =>

        {
            // 
            //  
            // classify 도 여기서 하자. 
            // 
            return null;
            // mat 
            // contour 
            // inbox 
            // resultdata
        };



		public void DefectClassify()
		{
			// modify resultdata class 


		}

		public void ExportReadult()
		{

		}



		public InspctRescipe ToInspectRecipe( string path )
		{
			return null;
		}

		public InspctRescipe ToModelRecipe( string path )
		{
			return null;
		}


		//public static IMage


		// receive command

		// send command 

		//a
	}
}
