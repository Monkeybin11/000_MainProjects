using System;
using System.Text;

namespace PLMapping_SIPCore.EmxIO
{
	using static Encoding;
	public static class CommandFunc
	{
		public static string AddSep
			( this string cmd)
			=> cmd + "|";

		public static Func<bool , string , string> ToComd
			=> ( withsep , cmd )
			=>
			{
				var res = withsep
							? cmd + Environment.NewLine
							: cmd.Remove( cmd.Length - 1 );

				return Default.GetByteCount(res).ToString().AddSep() + res;
			};
	}



	public static class CommonCommand
	{
		public static string IdRetrun		=> "*IDN?";
		public static string LastError		=> "ERR?" ;
		public static string ResetMachine	=> "*RST" ;
		public static string TransAlarm		=> "Alarm";
		public static string DisconnectExit => "Exit" ;
	}

	public static class VisionCommand
	{
		public static string AddWafer        = "AddWafer";
		public static string GetQStatus      = "GetQueueStatus";
		public static string GetWaferStatus  = "GetWaferStatus";
		public static string RemoveWafer     = "RemoveWafer";
		public static string Clear			 = "Clear";
	}


}
