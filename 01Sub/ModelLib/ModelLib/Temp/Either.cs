using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib.TypeClass
{
	public interface IEither<L,R>
	{
		L Left { get; set; }
		R Right { get; set; }
		bool IsRight { get; set; }

	}
	#region Either
	public class Either<L,R> : IEither<L,R>
	{
		public L Left { get; set; }
		public R Right { get; set; }
		public bool IsRight { get; set; }

		public Either( R right )
		{
			Right = right;
			IsRight	 = true;
		}

		public Either( L left , bool isleft = false)
		{
			Left = left;
			IsRight = false;
		}
	}

	// Log Either



	public static class Either_Ext
	{ 
		public static Either<L , R> ToEither<L,R> (
		this R val)
		{
			return new Either<L , R>( val );
		}

		

		public static Either<L,B> Bind<A,B,L>(
		this Either<L,A> src,
		Func<A , B> func,
		L left)
		{
			if ( src.IsRight ) return func( src.Right ).ToEither<L,B>();
			else
			{
				return new Either<L,B>( left );
			}
		}
	}

	#endregion

	#region LEither

	public class LEither<R> : IEither<string , R>
	{
		public string Left { get; set; }
		public R Right { get; set; }
		public bool IsRight { get; set; }

		public LEither( R right )
		{
			Right = right;
			IsRight = true;
		}

		public LEither( string left , bool isleft = false )
		{
			Left = left;
			IsRight = false;
		}

		public LEither()
		{
			IsRight = false;
		}
	}
	
	public static class LEither_Property
	{
		public static LEither<R> ToLEither<R>(
		this R val )
		{
			return new LEither<R>( val );
		}

		// this method is for time logging Either Type
		public static LEither<B> Bind<A, B>(
		this LEither<A> src ,
		Func<A , B> func ,
		string log ) 
		{
			if ( src.IsRight )
			{
				try { return func( src.Right ).ToLEither<B>(); }
				catch { }
			}
			var time = DateTime.Now.ToString("yyMMdd_HH mm ss");
			string fulllog = "Error( " + time + " ) : " + log;
			return new LEither<B>( fulllog );
		}

		// If We Dont need to log . use tthis 
		// 1 Bind -> Left (실패시 Left State만 내보내고 로그는 비어있다. )
		// 2 Bind -> Left
		// 3 Bind(Log) -> Left with Log (Left가 들어왔으므로 이 로그 바인드는 로그를 포함해 출력하게 된다. )

		public static LEither<B> Bind<A, B>(
		this LEither<A> src ,
		Func<A , B> func )
		{
			if ( src.IsRight )
			{
				try { return func( src.Right ).ToLEither<B>(); }
				catch { }
			}
			return new LEither<B>();
		}
	}

	#endregion
}
