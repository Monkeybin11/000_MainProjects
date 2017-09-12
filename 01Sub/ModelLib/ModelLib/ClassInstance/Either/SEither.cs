using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.TypeClass;
using ModelLib.Data;
using System.Diagnostics;

namespace ModelLib.ClassInstance
{
	public class SEither : IEither<string , IStgCtrl>
	{
		public int time;
		public string Left { get; set; }
		public IStgCtrl Right { get; set; }
		public bool IsRight { get; set; }

		public SEither()
		{
			IsRight = false;
		}

		public SEither( string log )
		{
			IsRight = false;
			Left = log;
		}

		public SEither( IStgCtrl sr , int tim = 20000 )
		{
			Right = sr;
			time = tim;
		}

		public SEither( IStgCtrl sr , bool pass , int tim = 20000)
		{
			Right = sr;
			IsRight = pass;
			time = tim;
		}
	}

	public static class SEither_Ext
	{
		public static SEither ToSEither(
			this IStgCtrl src  )
		=> new SEither( src );

		public static SEither ToSEither(
			this IStgCtrl src ,
			int time )
		=> new SEither( src , time );
		
		public static SEither Bind(
			this SEither src ,
			Func<SEither , SEither> func )
		{
			if ( src.IsRight )
			{
				int passtime = 0;
				Stopwatch stw = new Stopwatch();
				func( src );
				while ( passtime < src.time )
				{
					stw.Start();
					if ( src.Right.Query( src.Right.Status )
						 == src.Right.StatusOK ) return src; // Right
					else stw.Stop();
					passtime = ( int )stw.ElapsedMilliseconds;
				}
			}
			return new SEither(); // Left No Log
		}

		public static SEither Bind(
			this SEither src ,
			Func<SEither , SEither> func ,
			string log)
		{
			if ( src.IsRight )
			{
				int passtime = 0;
				Stopwatch stw = new Stopwatch();
				func( src );
				while ( passtime > src.time )
				{
					stw.Start();
					if ( src.Right.Query( src.Right.Status ) 
						 == src.Right.StatusOK ) return src; // Right
					else stw.Stop();
					passtime = ( int )stw.ElapsedMilliseconds;
				}
			}
			var timenow = DateTime.Now.ToString("yyMMdd_HH mm ss");
			string fulllog = "Error( " + timenow + " ) : " + log;

			return new SEither( fulllog ); // Left
		}

	}

}
