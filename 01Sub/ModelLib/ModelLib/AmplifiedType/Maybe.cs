using System;
using System.Collections.Generic;
using Unit = System.ValueTuple;


namespace ModelLib.AmplifiedType
{
	using static Handler;
	public static partial class Handler
	{
		public static Maybe<A> Just<A>( A value ) => new Maybe.Just<A>( value );
		public static Maybe.Nothing None => Maybe.Nothing.Default; 
	}

	public struct Maybe<A> // Define TypeClass
	{
		readonly A Value;
		readonly bool isJust;
		bool isNothing => isJust;

		Maybe( A value )
		{
			if ( value == null ) throw new ArgumentNullException(
				"Can't initilize with null " );
			isJust = true;
			Value = value;
		} 

		public static implicit operator Maybe<A>(Maybe.Nothing _ ) => new Maybe<A>();
		public static implicit operator Maybe<A>(Maybe.Just<A> just) => new Maybe<A>(just.Value);
		public static implicit operator Maybe<A>(A value) => value == null ? None : Just(value);

		public B Match<B>( Func<B> Nothing , Func<A , B> Just )
		  => isJust ? Just( Value ) : Nothing();

		public IEnumerable<A> AsEnumerable()
		{
			if ( isJust ) yield return Value;
		}





	}

	namespace Maybe // TypeClass Instance Impelemnt 
	{
		public struct Nothing
		{
			internal static readonly Nothing Default = new Nothing();
		}

		public struct Just<T>
		{
			internal T Value { get; }
			internal Just( T value )
			{
				if ( value == null )
					throw new ArgumentNullException( nameof( value )
					   , "Just can't be created with null, use 'Nothing' instead" );
				Value = value;
			}
		}
	}

	public static class MaybeExt
	{
		public static Maybe<B> Bind<A, B>
			( this Maybe<A> self , Func<A , Maybe<B>> f )
			=> self.Match(
				Nothing : () => None ,
				Just : x => f( x ));

		public static IEnumerable<B> Bind<A, B>
			( this Maybe<A> self , Func<A , IEnumerable<B>> f)
			=> self.AsEnumerable().Bind(f);

	}


}


