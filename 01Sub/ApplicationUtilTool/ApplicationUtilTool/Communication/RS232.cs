using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using System.Threading;

namespace ApplicationUtilTool.Communication
{
	public enum CommandEndStyle { Non , CR , LF , CRLF }
	public enum SendStyle { String , ASCII , UTF8 }
    public class RS232
    {
        SerialPort Port;
		string End;
		byte[] Delimiter;
		Func<string,byte[]> ToByteArr;
		Action<string> Send;
		public Func<string,string> Query;
	    


		public RS232( SerialPort port , CommandEndStyle crlfstyle , SendStyle sendstyle , int reciveDelay)
        {
            Port = port;

			End = crlfstyle == CommandEndStyle.CR ? "\r" :
				  crlfstyle == CommandEndStyle.LF ? "\n" :
				  crlfstyle == CommandEndStyle.CRLF ? "\r\n" :
													 string.Empty;

			Delimiter = crlfstyle == CommandEndStyle.CR ? new byte [ ] { 0x0d } :
					    crlfstyle == CommandEndStyle.LF ? new byte [ ] { 0x0a } :
					    crlfstyle == CommandEndStyle.CRLF ? new byte [ ] { 0x0d , 0x0a } :
															new byte [0];

			ToByteArr = sendstyle == SendStyle.ASCII ? ToASCII : ToUTF8 ;

			Send = sendstyle == SendStyle.String ? WriteString : WriteArr;

			Query = text =>
			{
				Send( text );
				Thread.Sleep( reciveDelay );
				return Read();
			}; 
		}

        public bool? Open() 
		=> Port.IsOpen ? Port.Act( x => x.Close() )
                             .Map( x => { x.Open(); return true as bool?; } ) 
                       : null;

		public void Close()
	   => Port.Close();


		Func<string> Read =>
		() => Port.ReadExisting().Replace( "\r" , string.Empty ).Replace( "\n" , string.Empty );


		#region Function options 

		Action<string> WriteString =>
		text => Port.Write( text + End );

		Action <string> WriteArr =>
		text =>
		{
			var arr = ToByteArr(text);
			Port.Write( arr , 0 , arr.Length );
			Port.Write( Delimiter , 0 , Delimiter.Length );
		};

		Func<string,byte[]> ToASCII =>
		text => Encoding.ASCII.GetBytes( text.Trim() );

		Func<string,byte[]> ToUTF8 =>
		text => Encoding.UTF8.GetBytes( text.Trim() );

		#endregion
    }
}
