using System;
using System.Collections.Generic;
using static System.Math;

namespace ModelLib.Data
{
	public static partial class Handler
	{
		public static ValPos ValPos( int x , int y , double value = 0 ) => new ValPos( x , y , value );
	}


	public struct ValPos
	{
		public int X;
		public int Y;
		public double Value;

		public ValPos( int x , int y , double val )
		{
			X = x;
			Y = y;
			Value = val;
		}

		public static bool operator <( ValPos a , ValPos b )
			=> ( Pow( a.X , 2 ) + Pow( a.Y , 2 ) ) < ( Pow( b.X , 2 ) + Pow( b.Y , 2 ) );

		public static bool operator >( ValPos a , ValPos b )
			=> ( Pow( a.X , 2 ) + Pow( a.Y , 2 ) ) > ( Pow( b.X , 2 ) + Pow( b.Y , 2 ) );

		public static implicit operator Tuple<int , int , double>( ValPos valpos )
			=> Tuple.Create( valpos.X , valpos.Y , valpos.Value );

		public static explicit operator ValPos( Tuple<int , int , double> data )
			=> new ValPos( data.Item1 , data.Item2 , data.Item3 );

		public static ValPos operator +( ValPos a , double b )
			=> new ValPos( a.X , a.Y , a.Value + b );

		public static ValPos operator -( ValPos a , double b )
			=> new ValPos( a.X , a.Y , a.Value - b );

		public static ValPos operator *( ValPos a , double b )
			=> new ValPos( a.X , a.Y , a.Value * b );

		public static ValPos operator /( ValPos a , double b )
			=> new ValPos( a.X , a.Y , b != 0 ? a.Value / b : double.MaxValue );
	}


	public static class ValPosExt
	{
		public static ValPos AddPos( this ValPos self , ValPos target )
			=> new ValPos( self.X + target.X , self.Y + target.Y , self.Value );

		public static ValPos SubPos( this ValPos self , ValPos target )
			=> new ValPos( self.X - target.X , self.Y - target.Y , self.Value );
	}
}
