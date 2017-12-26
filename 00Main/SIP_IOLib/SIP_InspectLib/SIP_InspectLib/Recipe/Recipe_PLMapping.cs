using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using ModelLib.AmplifiedType;

namespace SIP_InspectLib.Recipe
{
    using static Handler;
    using static SpeedyCoding.Handler;
    using static ModelLib.AmplifiedType.Handler;
    using static SIP_InspectLib.Indexing.Common;
    using Emgu.CV.Util;
    using Img = Image<Gray, byte>;
    using DataType;
    using SpeedyCoding;

    public class InspctRescipe
	{
		public double RhoLimit;
		public int IntenLowLimt;
		public int IntenHighLimt;
		public int AreaLowLimt;
		public int AreaHighLimt;
		public int Tolerance;
		public int HChipNum;
		public int WChipNum;

		public double[] realLT;
		public double[] realLB;
		public double[] realRT;
		public double[] realRB;

		public bool NeedEdgrCut;
		public int EdgeLimit = 0;
		double XoffSet;
		double YoffSet;

	}


	public class PLMappingResult
	{
		private readonly Func<int,int> Shifter;

		public int Hindex;
		public int Windex;

		public int HidxShfted => Shifter( Hindex );
		public int WidxShfted => Shifter( Windex );

		public string OKNG;
		public double Intensity;
		public double ContourSize;
		public System.Drawing.Rectangle BoxData;
		public System.Drawing.Point PositionError;

		public PLMappingResult( int hindex , int windex , Func<int , int> shifter)
		{
			Hindex = hindex;
			Windex = windex;
			OKNG = "NOPL";
			Intensity = 0;
			ContourSize = 0;
			BoxData = new Rectangle();
			Shifter = shifter;
		}

		public PLMappingResult(
			int hindex
			, int windex
			, string passfail
			, double inten
			, double contsize
			, Rectangle boxData = new Rectangle() )
		{
			Hindex = hindex;
			Windex = windex;
			OKNG = passfail;
			Intensity = inten;
			ContourSize = contsize;
			BoxData = boxData;
		}
	}


	public static class Adaptor
	{
		public static Func<Img , InspctRescipe , Maybe<List<Rectangle>>> ToBoxList
			=> ( img , srcRe )
			=> Just( img ).Lift( FnFindContour( srcRe.AreaHighLimt , srcRe.AreaLowLimt ) )
						  .Lift( SortContour )
						  .Lift( ApplyBox );

		public static Func<InspctRescipe , PosLineEq > EstedChipPosAndEq
			=> srcRe
			=> FnEstChipPos_4PointAndEQ_Rhombus( srcRe.realLT , 
												 srcRe.realLB , 
												 srcRe.realRT , 
												 srcRe.realRB )
											  ( srcRe.HChipNum , 
											    srcRe.WChipNum );
		public static Func<PosLineEq , double[,,]> ToEstedIndex
			=> x
			=> x.IndexPos;

		public static Func< InspctRescipe , Func<Rectangle , double> , PosLineEq , List<Rectangle> , IEnumerable<Maybe<Indexji>>> ToBoxIndex
			=> ( srcRe , sumfunc , poseq , boxs )
			=>
			{
				var indexres = GetIndexOf(srcRe.Tolerance , boxs , poseq.HLineEQs , poseq.VLineEQs);

				var resultGenerator = ImportResult.Apply(srcRe)
												  .Apply(sumfunc)
												  .Apply( poseq.IndexPos )
												  .Apply( boxs)
												  .Apply( indexres );

				return null;
			};

		//public static Func<Recipe_PLMapping , Func<Rectangle , double> , List<Rectangle> , Maybe<PLMappingResult>> ToResualt
		//	=> ( srcRe , boxs )
		//	 =>
		//	 {
		//
		//		 return null;
		//	 };

		private static Func<
				InspctRescipe,
				Func<Rectangle , double>,
				double [ , , ] ,
				List<Rectangle> ,
				IEnumerable<Maybe<Indexji>> ,
				ExResult [ ] [ ] ,
				ExResult [ ] [ ]> ImportResult
			=> ( srcRe , sumFunc , ested , boxlist , boxindices , src )
			=>
			{
				var updator = ResultUpdator  
									   .Apply(sumFunc)
									   .Apply(srcRe.NeedEdgrCut)
									   .Apply(ested)
									   .Apply(src)
									   .Apply( Constrain( srcRe.IntenHighLimt , srcRe.IntenLowLimt , srcRe.EdgeLimit ) );

				PairIndexRect( boxlist , boxindices )
							 .ForEach( updator );

				return src;
			};

		static Action< 
					Func<Rectangle , double> , 
					bool , 
					double [ , , ] , 
					ExResult [ ] [ ] , 
					Constrain , 
					IndexRect> ResultUpdator
			=> ( sumFunc , needEdgeCut ,  ested , src , constrain , idxrec )
			=> idxrec.Index.Match(
				() => Unit() , // NG Case
				idx =>  // Non_NG Case
				{
					var j = idx.j;
					var i = idx.i;
					var ypos = ested[ j,i,0];
					var xpos = ested[ j,i,1];

					var rec = idxrec.Rectangle;
					var intenSum = sumFunc( rec );
					
					if ( needEdgeCut )
					{
						if ( InValidArea( xpos , ypos , 0, 0 , 0 ) ) //여기 고쳐야 한다. << 
						{
							src [ j ] [ i ] = new ExResult( j , i
												 , ( int )ypos - ( int )( rec.Y + rec.Height / 2 )
												 , ( int )xpos - ( int )( rec.X + rec.Width / 2 )
												 , Classifier( intenSum , constrain )
												 , intenSum
												 , rec.Width * rec.Height
												 , rec );
						}
					}
					else
					{
						src [ j ] [ i ] = new ExResult( j , i
										 , ( int )ypos - ( int )( rec.Y + rec.Height / 2 )
										 , ( int )xpos - ( int )( rec.X + rec.Width / 2 )
										 , Classifier( intenSum , constrain )
										 , intenSum
										 , rec.Width * rec.Height
										 , rec );
					}

					/// use cutoff non chip area
					
					return Unit();
				} );

		static Func<double , double , double , double , double , bool> InValidArea
			 => ( x , y , x1,y1 , lnelimit)
			 => ToTuple( x , y ).L2( ToTuple(x1,y1) )  // <<<<<
				 > lnelimit
				 ? false
				 : true;

		static Func<double , Constrain , string> Classifier
			=> ( inten , constrain )
			=> inten < constrain.DwInten / 10 ? "NOPL" :
			   inten < constrain.DwInten ? "LOW" :
			   inten > constrain.UpInten ? "OVER" :
											  "OK";

		static Func<IEnumerable<Rectangle> ,
						IEnumerable<Maybe<Indexji>> ,
						IEnumerable<IndexRect>> PairIndexRect
			=> ( rects , idxs )
				 => idxs.Zip( rects , ToIndexRect );

		static Func<Maybe<Indexji> , Rectangle , IndexRect> ToIndexRect
			=> ( idx , rec )
			=> new IndexRect() { Rectangle = rec , Index = idx };

	}


	public static class Handler
	{
		public static Constrain Constrain( int up , int dw , int edgelimit ) => 
			new Constrain() { UpInten = up , DwInten = dw , ValidLen = edgelimit};
	}

	public class IndexRect
	{
		public Rectangle Rectangle;
		public Maybe<Indexji> Index;
	}

	public class Constrain
	{
		public double ValidLen;
		public int UpInten;
		public int DwInten;
	}

	// Convert to  Indexing 

	// 

}
