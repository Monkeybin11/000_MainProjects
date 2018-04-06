using ModelLib.AmplifiedType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;



namespace SIP_InspectLib.DefectInspect
{
	using static ModelLib.AmplifiedType.Handler;
	using static Enumerable;
	using static DefectInspect.Handler;
	using DataType;
	using SpeedyCoding;
    using SIP_InspectLib.Recipe;
    using static IntenArea;

	public static class IntenArea
	{
		public static Func<double , double , double [ , , ] , byte [ , , ] , ExResult , ExResult> FnCheckLowOver
			=> ( uplimit , dwlimit , estedChipP , indexingImage , res )
			=>
			{
				if ( res == null ) return res;
				if ( res.OKNG == "OK" )
				{
					int xs = (int)estedChipP[ res.Hindex , res.Windex , 1] - 3;
					int ys = (int)estedChipP[ res.Hindex , res.Windex , 0] - 3;

					if ( res.Intensity < dwlimit )
						res.OKNG = "LOW";
					else if ( res.Intensity > uplimit )
						res.OKNG = "OVER";
				}
				return res;
			};


		public static ExResult [ ] [ ] NgResultInitializer( int h , int w )
			=> Range( 0 , h ).Select( j =>
								 Range( 0 , w ).Select( i =>
						 				new ExResult( j , i )).ToArray() )
								   .ToArray();

		public static Func<
				ImgPData ,
				Func<Rectangle , double> ,
				Tuple<double , double> ,
				double ,
				Constrain ,
				double [ , , ] ,
				IEnumerable<Maybe<Indexji>> ,
				List<Rectangle> ,
				ExResult [ ] [ ] ,
				ExResult [ ] [ ] > ImportResult
			=> ( posData , sumIndieBox , center , limit , contrain , ested , boxindices , boxlist , src )
			=>
			{
				var updator = ResultUpdator
								.Apply(sumIndieBox)
								.Apply(center)
								.Apply(limit)
								.Apply(ested)
								.Apply(src)
								.Apply( Constrain(posData.IntenSumUPLimit , posData.IntenSumDWLimit));

				PairIndexRect( boxlist , boxindices )
							 .Lift( updator );

				return src;
			};


		/// <summary>
		/// Func<rect double > = FnSumInsideBox( Image ) : Func is curriedform with paramter image. Use this Method with FnSumInsideBox with partial application 
		/// </summary>
		private static Func<
			Func<Rectangle , double>,
			Tuple<double , double> , 
			double ,
			double [ , , ] ,
			ExResult [ ] [ ] , 
			Constrain , 
			IndexRect,
			ValueTuple> ResultUpdator
			=> ( sumIndieBox , center , limit , ested , src , constrain , idxrec )
			=> idxrec.Index.Match(
				() => Unit() , // NG Case
				idx =>  // Non_NG Case
				{
					var j = idx.j;
					var i = idx.i;
					var ypos = ested[ j,i,0];
					var xpos = ested[ j,i,1];

					var rec = idxrec.Rectangle;
					var intenSum = sumIndieBox( rec );

					if ( InValidArea( center , limit , xpos , ypos ) )
					{
						src [ j ] [ i ] = new ExResult( j , i
											 , ( int )ypos - ( int )( rec.Y + rec.Height / 2 )
											 , ( int )xpos - ( int )( rec.X + rec.Width / 2 )
											 , Classifier( intenSum , constrain )
											 , intenSum
											 , rec.Width * rec.Height
											 , rec );
					}
					return Unit();
				} );

		public static Func<Tuple<double,double> , double , double , double , bool> InValidArea
			 => ( center , limit , x , y )
			 => x.ToTuple( y ).L2( center )
				 > limit
				 ? false
				 : true;

		private static Func<double , Constrain , string> Classifier
			=> ( inten , constrain )
			=> inten < constrain.DwInten / 10 ? "NOPL" :
			   inten < constrain.DwInten ? "LOW" :
			   inten > constrain.UpInten ? "OVER" :
											  "OK";

		private static Func<IEnumerable<Rectangle> ,
						IEnumerable<Maybe<Indexji>> ,
						IEnumerable<IndexRect>> PairIndexRect
			=> ( rects , idxs )
				 => idxs.Zip( rects , ToIndexRect );

		private static Func<Maybe<Indexji> , Rectangle , IndexRect> ToIndexRect
			=> ( idx , rec )
			=> new IndexRect() { Rectangle = rec , Index = idx };
	}

	public static class Handler
	{
		public static Constrain Constrain( int up , int dw ) => new Constrain() { UpInten = up , DwInten = dw };

        public static ExResult[][] ResultInitializer( InspctRecipe src ) => NgResultInitializer( src.HChipNum, src.WChipNum );
    }

	public class IndexRect
	{
		public Rectangle Rectangle;
		public Maybe<Indexji> Index;
	}

	public class Constrain
	{
		public static double ValidLen => 8127;
		public int UpInten;
		public int DwInten;
	}
}
