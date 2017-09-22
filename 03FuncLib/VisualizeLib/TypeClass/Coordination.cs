using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using SpeedyCoding;

namespace VisualizeLib.TypeClass
{
	public interface Polar : Crd2D
	{ 
		double R 	{get;set;}
		double Rho	{get;set;}

	}
	public class PlrCrd : Polar
	{
		public double R   { get; set; }
		public double Rho { get; set; }

		public PlrCrd( double r , double rho )
		{
			R = r;
			Rho = rho;
		}
	}
	public class PlrUnit : Polar
	{
		public double R   { get { return default( double ); } set { } }
		public double Rho { get { return default( double ); } set { } }
	}


	public interface Cartesian : Crd2D
	{
		double X { get; set; }
		double Y { get; set; }
	}

	public class CrtnCrd : Cartesian
	{
		public double X {get;set;}
		public double Y {get;set;}

		public CrtnCrd( double x , double y )
		{
			X = x;
			Y = y;
		}
	}

	public class CrtnUnit : Cartesian
	{
		public double X { get { return default( double ); } set { } }
		public double Y { get { return default( double ); } set { } }
	}

	public static class Crd2D_Property
	{
		public static Cartesian ToCartesian<A>(
		this A src )
		where A : Crd2D
		{
			var polar = src as PlrCrd;
			var crtn = src as CrtnCrd;

			if ( crtn != null ) return crtn;
			else if ( polar == null ) return new CrtnUnit();
			return new CrtnCrd(
						       polar.R * Math.Cos( polar.Rho ), // x
						       polar.R * Math.Sin( polar.Rho ) // y
						       );
		}

		public static Polar ToPolar<A>(
		this A src )
		where A : Crd2D
		{
			var polar = src as PlrCrd;
			var crtn = src as CrtnCrd;

			if ( polar != null ) return polar;
			else if ( crtn == null ) return new PlrUnit();
			return new PlrCrd(
							   Math.Sqrt( crtn.X* crtn.X + crtn.Y * crtn.Y )  , // x
							   Math.Atan2(crtn.Y, crtn.X)// y
							   );
		}


		public static A Add<A,B>(
		this A v1 ,
		A v2 )
		where A : class , Crd2D 
		where B : class , Crd2D
		{
			// v1 , v2 가 crtn 인지 plr 인지 
			var vec1c = (v1 as CrtnCrd);
			var vec1p = (v1 as PlrCrd);
			var vec2c = (v2 as CrtnCrd);
			var vec2p = (v2 as PlrCrd);

			Cartesian c1 = vec1c != null ? vec1c :
						   vec1p != null ? vec1p.ToCartesian() :
						   new CrtnUnit();

			Cartesian c2 = vec2c != null ? vec1c :
						   vec2p != null ? vec1p.ToCartesian() :
						   new CrtnUnit();

			var cr1 = c1 as CrtnCrd;
			var cr2 = c2 as CrtnCrd;

			return new CrtnCrd( cr1.X + cr2.X , cr1.Y + cr2.Y ) as A;
		}


		#region matcher helper

		public static bool ToCondition<T>(
		this T src ) =>
			src == null ? false : true;

		#endregion
	}




}
