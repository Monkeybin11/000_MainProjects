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


namespace MachineLib.DeviceLib
{
	public enum Axis { R = 1, X = 2 }

	public class SgmaStg_XR : RS232Instance, IStgCtrl, ISgmaStg_XR
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


		public SgmaStg_XR()
		{
			Port = new SerialPort()
							.Act( x =>
							{
								x.PortName = "COM6";
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
		}

		public bool Open()
		=> ( bool )RS.Open();

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
		#endregion
	}

	public static class SgmaExt
	{
		public static string ToPos(
		this int pos ,
		Axis axis )
		{
			return (( int )axis).ToString() + "P" + pos.ToString();   
		}

		public static string ToIdx(
		this Axis axis )
		{
			return ( ( int )axis ).ToString();
		}

	}
}
