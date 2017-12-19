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

namespace PLMapping_SIPCore
{
	using static SIPEngine.Handler;
	using static ModelLib.AmplifiedType.Handler;
	using static SIP_InspectLib.DefectInspect.Handler;
	using static SIP_InspectLib.Indexing.Common;

	using Img = Image<Gray , byte>;

	public class Core_PlMapping
	{
		public void Start( string imgpath , string configpath )
		{

			



		}


		public void LoadImage(string imgpath)
		{ }


		// with maybe
		public Recipe_PLMapping LoadConfig( string configpath )
		{

			return null;
		}

		public static Func<string , IEnumerable<Func<Img , Img>>> CreateModel
			=> recipe 
			=>
		{
			return null;
		};

		public static Func<Img , IEnumerable<Func<Img , Img>> ,  Img> Preprocess
			=> (src, ModelLib)
			=> RunProcessing( src , ModelLib );

		public Func<SIPEngine.Recipe.Recipe_PLMapping , Img , > Indexing
			=> (recipe , img )
		{
			


			// mat 
			// contour 
			// inbox 
			// resultdata
		}

		public void DefectClassify()
		{
			// modify resultdata class 


		}

		public void ExportReadult()
		{

		}



		public Recipe_PLMapping ToInspectRecipe( string path )
		{
			return null;
		}

		public Recipe_PLMapping ToModelRecipe( string path )
		{
			return null;
		}


		//public static IMage


		// receive command

		// send command 

		//a
	}
}
