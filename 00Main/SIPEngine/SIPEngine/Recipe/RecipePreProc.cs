using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;



namespace SIPEngine.Recipe
{
	using Img = Emgu.CV.Image<Gray , byte>;

	public class Recipe_PLMapping
	{
		public const string Median = "Median";
		public const string Normalize = "Normalize";
		public const string Threshold = "Threshold";
		public const string AdpThreshold = "AdpThreshold";
		public const string HistoEq = "HistoEq";
		public const string Gamma = "Gamma";
		public const string Brightness = "Brightness";
		public const string Union = "Union";
		public const string Inverse = "Inverse";
		public const string GBlur = "GBlur";
		public const string Blur = "Blur";
		public const string Morph = "Morph_";
	}

	public static partial class Handler
	{
		public static IEnumerable<Func<Img , Img>> CreateFuncList( Recipe_PLMapping recipe  )
		{
			Func<Img,Img> temp = new Func<Image<Gray, byte>, Image<Gray, byte>>( x => x);

			return new Func<Img , Img>[] { temp };
		}


		//public static Func<Img , Img> Runner;
		//
		//public static List<Func<Img , Img>> ReadFunc(string[][] recipe)
		//{
		//	// only First Function Name List
		//
		//	// Matcher and Get Func
		//
		//
		//}
		//
		//public Func<Img , Img> Matching( string name )
		//{
		//
		//}
		//
		//public Func<Img , Img> ApplyOption(string option)
		//{
		//
		//
		//}

		//public Func<Img , Img> mian()
		//{
		//	string name = "";
		//	string fparm = "200";
		//
		//	var temp = new byte[3,3,1];
		//	var img = new Image<Gray,byte>( temp );
		//
		//
		//
		//	switch ( name )
		//	{
		//		case "Normalized":
		//
		//			int ptemp;
		//
		//			if ( !int.TryParse( fparm , out ptemp ) )
		//			{
		//				return;
		//			}
		//			return new Func<Img , Img>( ximg => ximg.Mul( 255.0 / ptemp ) );
		//
		//
		//		case "AdpThreshold":
		//
		//			int ptemp2;
		//			string fparams = "MeanC,Binary,"
		//
		//
		//			if ( !int.TryParse( fparm , out ptemp ) )
		//			{
		//				return;
		//			}
		//			return new Func<Img , Img>( ximg => ximg.Mul( 255.0 / ptemp ) );
		//
		//			
		//
		//			break;
		//
		//	}
		//
		//
		//}

	}

	public static class FunctionLib
	{
		//public static Dictionary<string,Func<  >>

	}


}
