using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.AmplifiedType
{
	public static class PartialApplication
	{
		//For Function
		public static Func<T1 , R> Compose<T1, T2, R>( this Func<T2 , R> g , Func<T1 , T2> f )
			=> x => g( f( x ) );

		public static Func<T2 , R> Apply<T1, T2, R>( this Func<T1 , T2 , R> func , T1 t1 )
			=> t2 => func( t1 , t2 );

		public static Func<T2 , T3 , R> Apply<T1, T2, T3, R>( this Func<T1 , T2 , T3 , R> func , T1 t1 )
			=> ( t2 , t3 ) => func( t1 , t2 , t3 );

		public static Func<T2 , T3 , T4 , R> Apply<T1, T2, T3 , T4 , R>( this Func<T1 , T2 , T3 , T4 , R> func , T1 t1 )
			=> ( t2 , t3  , t4) => func( t1 , t2 , t3 , t4 );

		public static Func<T2 , T3 , T4 , T5 , R> Apply<T1, T2, T3 , T4 , T5 , R>( this Func<T1 , T2 , T3 , T4 , T5  , R> func , T1 t1 )
			=> ( t2 , t3 , t4 , t5) => func( t1 , t2 , t3 , t4 , t5);


		public static Func<I1 , I2 , R> Map<I1, I2, T, R>( this Func<I1 , I2 , T> @this , Func<T , R> func )
		   => ( i1 , i2 ) => func( @this( i1 , i2 ) );


		//For Action

		public static Action<T2 > Apply<T1, T2>( this Action<T1 , T2 > func , T1 t1 )
			=> t2 => func( t1 , t2 );

		public static Action<T2 , T3 > Apply<T1, T2, T3>( this Action<T1 , T2 , T3 > func , T1 t1 )
			=> ( t2 , t3 ) => func( t1 , t2 , t3 );

		public static Action<T2 , T3 , T4 > Apply<T1, T2, T3, T4>( this Action<T1 , T2 , T3 , T4 > func , T1 t1 )
			=> ( t2 , t3 , t4 ) => func( t1 , t2 , t3 , t4 );

		public static Action<T2 , T3 , T4 , T5 > Apply<T1, T2, T3, T4, T5>( this Action<T1 , T2 , T3 , T4 , T5 > func , T1 t1 )
			=> ( t2 , t3 , t4 , t5 ) => func( t1 , t2 , t3 , t4 , t5 );


		public static Action<I1 , I2 > Map<I1, I2, T>( this Func<I1 , I2 , T> @this , Action<T > func )
		   => ( i1 , i2 ) => func( @this( i1 , i2 ) );




	}
}
