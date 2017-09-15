using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceCollection;
using ApplicationUtilTool.Communication;
using System.IO.Ports;
using SpeedyCoding;
using ModelLib.Data;
using System.Threading;
using System.Diagnostics;

namespace MachineLib.DeviceLib
{
	public enum Axis { R = 1, X = 2 , W }

	public class SgmaStg_XR : RS232Instance, IStgCtrl, ISgmaStg_XR, ISgmaStg_XR12
	{
		#region Base
	    SerialPort Port;
		RS232 RS;
		public int TimeOut = 20000;

		public string Home { get { return "H:"; } set { } } 
		public string GoAbs	{ get{return "A:";} set { } }
		public string SetSpeed { get{return "D:";} set { } }
		public string Status	{ get{return "!:";} set { } }
		public string StatusOK { get{return "R"; } set { } }
		public string Pos		{ get{return "Q:";} set { } }
		public string Go		{ get{return "G:";} set { } }
		public string Stop		{ get{ return "L:"; } set { } }

		public SgmaStg_XR(string comport)
		{
			Port = new SerialPort()
							.Act( x =>
							{
								x.PortName = comport;
								x.BaudRate = 38400;
								x.DataBits = 8;
								x.Parity = Parity.None;
								x.StopBits = StopBits.One;
								x.Handshake = Handshake.RequestToSend;
							} );

			

			RS = new RS232( this.Port ,
							CommandEndStyle.CR ,
							SendStyle.String ,
							300 );

			Console.WriteLine( RS.Port.IsOpen.ToString() );


		}

		public bool Open()
		=> RS.Open() == true ? true : false; 
		


		public void Close()
		=> RS.Close();

		public bool Send( string cmd )
		{
			return RS.Query( cmd ) == "OK"
					? true
					: false;
		}

		public string Query( string cmd )
		{
			return RS.Query(  cmd  );
		}

		public bool WaitReady( int timeoutSec)
		{
			Stopwatch stw = new Stopwatch();
			stw.Start();
			while ( RS.Query( Status ) != StatusOK )
			{
				Thread.Sleep( 300 );
				if ( timeoutSec > 0 && stw.ElapsedMilliseconds / 1000 > timeoutSec ) return false;
			}
			return true;
		}

		public bool SendAndReady( string cmd , int timeoutSec = 0)
		{
			RS.Query( cmd );
			Stopwatch stw = new Stopwatch();
			stw.Start();
			while ( RS.Query( Status ) != StatusOK )
			{
				Thread.Sleep( 300 );
				if ( timeoutSec > 0 && stw.ElapsedMilliseconds / 1000 > timeoutSec ) return false;
			}
			return true;
		}

		

		#endregion
	}

	public static class SgmaExt
	{
		public static string ToPos(
		this int pos ,
		Axis axis )
		=> pos > 0 ? ( ( int )axis ).ToString() + "+P" + ( Math.Abs( pos ) ).ToString()
				   : ( ( int )axis ).ToString() + "-P" + ( Math.Abs( pos ) ).ToString();


		public static string ToPos(
		this double pos ,
		Axis axis )
		=> pos > 0 ? ( ( int )axis ).ToString() + "+P" + (Math.Abs(pos)).ToString()
				   : ( ( int )axis ).ToString() + "-P" + ( Math.Abs( pos ) ).ToString();



		public static string ToIdx(
		this Axis axis )
		{
			return axis == Axis.W ? "W"
								  : ( ( int )axis).ToString();
		}

		public static string ToSpeed(
			this double speed )
		{
			string spd = ((int)speed).ToString();
			return $"S{spd}R{spd}R10"; 

		}

		public static int Degree2Pulse(
			this double degree
			)
		{
			return ( int )( degree / 0.72 );
		}
	}
}
