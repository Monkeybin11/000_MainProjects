using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThicknessAndComposition_Inspector_IPS_Core
{


	/*

	public class Writer<T, TContent>
	{
		private readonly Lazy<Tuple<T, TContent>> lazy;

		public Writer( Func<Tuple<T , TContent>> factory , IMonoid<TContent> monoid )
		{
			this.lazy = new Lazy<Tuple<T , TContent>>( factory );
			this.Monoid = monoid;
		}

		public T Value
		{
			get { return this.lazy.Value.Item1; }
		}


		public TContent Content
		{
			get { return this.lazy.Value.Item2; }
		}

		public IMonoid<TContent> Monoid { get; }
	}

	public static partial class WriterExtensions
	{
		// Required by LINQ.
		public static Writer<TResult , TContent> SelectMany<TSource, TContent, TSelector, TResult>
			( this Writer<TSource , TContent> source ,
			 Func<TSource , Writer<TSelector , TContent>> selector ,
			 Func<TSource , TSelector , TResult> resultSelector ) =>
				new Writer<TResult , TContent>( () =>
				{
					Writer<TSelector, TContent> selectorResult = selector(source.Value);
					return Tuple.Create(
					resultSelector( source.Value , selectorResult.Value ) ,
					source.Monoid.Binary( source.Content , selectorResult.Content ) );
				} , source.Monoid );

		// Not required, just for convenience.
		public static Writer<TResult , TContent> SelectMany<TSource, TContent, TResult>
			( this Writer<TSource , TContent> source ,
				Func<TSource , Writer<TResult , TContent>> selector ) => source.SelectMany( selector , Functions.False );
	}

	public static partial class WriterExtensions
	{
		// μ: Writer<Writer<T, TContent>> => Writer<T, TContent>
		public static Writer<TResult , TContent> Flatten<TResult, TContent>
			( Writer<Writer<TResult , TContent> , TContent> source ) => source.SelectMany( Functions.Id );

		// η: T -> Writer<T, TContent>
		public static Writer<T , TContent> Writer<T, TContent>
			( this T value , TContent content , IMonoid<TContent> monoid ) =>
				new Writer<T , TContent>( () => Tuple.Create( value , content ) , monoid );

		// φ: Lazy<Writer<T1, TContent>, Writer<T2, TContent>> => Writer<Lazy<T1, T2>, TContent>
		public static Writer<Lazy<T1 , T2> , TContent> Binary<T1, T2, TContent>
			( this Lazy<Writer<T1 , TContent> , Writer<T2 , TContent>> binaryFunctor ) =>
				binaryFunctor.Value1.SelectMany(
					value1 => binaryFunctor.Value2 ,
					( value1 , value2 ) => new Lazy<T1 , T2>( value1 , value2 ) );

		// ι: TUnit -> Writer<TUnit, TContent>
		public static Writer<Unit , TContent> Unit<TContent>
			( Unit unit , TContent content , IMonoid<TContent> monoid ) => unit.Writer( content , monoid );

		// Select: (TSource -> TResult) -> (Writer<TSource, TContent> -> Writer<TResult, TContent>)
		public static Writer<TResult , TContent> Select<TSource, TResult, TContent>
			( this Writer<TSource , TContent> source , Func<TSource , TResult> selector ) =>
				source.SelectMany( value => selector( value ).Writer( source.Content , source.Monoid ) );
	}













	public interface IMonoid<T>
	{
		T Multiply( T value1 , T value2 );

		T Unit();
	}

	public class Int32SumMonoid : IMonoid<int>
	{
		public int Multiply( int value1 , int value2 ) => value1 + value2;

		public int Unit() => 0;
	}

	public class Int32ProductMonoid : IMonoid<int>
	{
		public int Multiply( int value1 , int value2 ) => value1 * value2;

		public int Unit() => 1;
	}

	public class StringConcatMonoid : IMonoid<string>
	{
		public string Multiply( string value1 , string value2 ) => string.Concat( value1 , value2 );

		public string Unit() => string.Empty;
	}

	public class EnumerableConcatMonoid<T> : IMonoid<IEnumerable<T>>
	{
		public IEnumerable<T> Multiply( IEnumerable<T> value1 , IEnumerable<T> value2 ) => value1.Concat( value2 );

		public IEnumerable<T> Unit() => Enumerable.Empty<T>();
	}

	public class BooleanAndMonoid : IMonoid<bool>
	{
		public bool Multiply( bool value1 , bool value2 ) => value1 && value2;

		public bool Unit() => true;
	}

	public class BooleanOrMonoid : IMonoid<bool>
	{
		public bool Multiply( bool value1 , bool value2 ) => value1 || value2;

		public bool Unit() => false;
	}
	*/

}
