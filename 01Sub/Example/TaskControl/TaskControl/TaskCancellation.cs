using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using SpeedyCoding;
namespace TaskControl
{
	public class TaskCancellation
	{
		TaskCancel TC = new TaskCancel();
		public void main()
		{
			Dictionary<string,Task<LEither<double>>> taskdic = new Dictionary<string, Task<LEither<double>>>();
			Dictionary<string,Func<double,CancellationToken,Task<LEither<double>>>> taskdic2 = new Dictionary<string, Func<double, CancellationToken, Task<LEither<double>>>>();

			taskdic2.Add( "f" , ( input , token ) => new Task<LEither<double>>( () => TC.testfn( input , token )) );
			taskdic2.Add( "s" , ( input , token ) => new Task<LEither<double>>( () => TC.testfn2( input , token )) );
			taskdic2.Add( "t" , ( input , token ) => new Task<LEither<double>>( () => TC.testfn( input , token )) );

			double res = 100;
			var temp1 = taskdic2 [ "f" ]( 100 , TC.ct.Token );
			temp1.Start();

			var temp2 = temp1.Result.Bind( x => taskdic2["s"](x , TC.ct.Token )
													.Act( s => s.Start())
													.Result , "second task is cancelled" );
									//.Bind( x => taskdic2["t"](x,TC.ct.Token) ;
							           


			var tempres1 = temp1.Result;

		}



		
	}

	public class TaskCancel
	{
		public CancellationTokenSource ct = new CancellationTokenSource();

		public Func<double , CancellationToken , LEither<double>> testfn =>
			( input , token ) =>
			{

				for ( int i = 0 ; i < 5 ; i++ )
				{
					if ( token.IsCancellationRequested )
						return (-1.0).ToLEither();
					Thread.Sleep( 500 );
				}
				return (2*input).ToLEither();
			};

		public Func<double , CancellationToken , LEither<double>> testfn2 =>
			( input , token ) =>
			{

				for ( int i = 0 ; i < 5 ; i++ )
				{
					if ( token.IsCancellationRequested ) return (-12.0).ToLEither();
					Thread.Sleep( 500 );
				}
				return (100*input).ToLEither();
			};

	}




	public interface IEither<L, R>
	{
		L Left { get; set; }
		R Right { get; set; }
		bool IsRight { get; set; }

	}

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


		// 1 Bind -> Left (실패시 Left State만 내보내고 로그는 비어있다. )
		// 2 Bind -> Left
		// 3 Bind(Log) -> Left with Log (Left가 들어왔으므로 이 로그 바인드는 로그를 포함해 출력하게 된다. )

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



		// If We Dont need to log . use this 
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

}
