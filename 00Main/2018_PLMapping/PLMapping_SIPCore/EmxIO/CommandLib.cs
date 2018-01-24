using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SpeedyCoding;
using ModelLib.AmplifiedType;

namespace PLMapping_SIPCore.EmxIO
{
    using static ModelLib.AmplifiedType.Handler;
    using static Encoding;
    using System.Threading.Tasks;


    public enum CmdType
    {
        IDN ,
        AddWafer ,
        GetQueueStatus ,
        GetWaferStatus ,
        RemoveWafer ,
        Clear
    }

    public static class CommonCommand
    {
        public static string IdRetrun => "*IDN?";
        public static string LastError => "ERR?";
        public static string ResetMachine => "*RST";
        public static string TransAlarm => "Alarm";
        public static string DisconnectExit => "Exit";
    }

    public static class VisionCommand
    {
        public static string AddWafer        = "AddWafer";
        public static string GetQStatus      = "GetQueueStatus";
        public static string GetWaferStatus  = "GetWaferStatus";
        public static string RemoveWafer     = "RemoveWafer";
        public static string Clear           = "Clear";
    }


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







    public class Handler
    {
        public bool IsConnected = false;
        public TcpClient Client;
        public NetworkStream Stream;
        static Queue<string> Qlist = new Queue<string>();

        public void main()
        {

            // connect
            // 
            //
            //
            //
            //
            //
            //
        }

        public bool Connect(string hostname, int port)
        {
            Client = new TcpClient();
            Client.Connect(hostname, port);
            Client.ReceiveBufferSize = 1024 * 100;
            Client.SendBufferSize = 1024 * 100;
            Client.SendTimeout = 200;
            Client.ReceiveTimeout = 200;

            Stream = Client.GetStream();

            return Client.Connected
                    ? true
                    : false;
        }


        public void ReceiveRun()
        {
            IsConnected = true;

            while (true)
            {
                Thread.Sleep(500);

                if (IsConnected)
                {
                    List<byte> DataArray = new List<byte>();
                    if (Stream.DataAvailable)
                    {
                        byte prevlastbyte = 0xFF;
                        byte lastbyte = 0xFF;

                        while ((prevlastbyte != (byte)'\r') || (lastbyte != (byte)'\n'))
                        {
                            prevlastbyte = lastbyte;
                            lastbyte = (byte)Stream.ReadByte();
                            //    Console.WriteLine(lastbyte.ToString("x2"));
                            DataArray.Add(lastbyte);
                            if (DataArray.Count > 10000)
                            {
                                Stream.Close();
                                continue;
                            }
                        }

                        string str = Encoding.UTF8.GetString(DataArray.Take(DataArray.Count - 2).ToArray());

                        ActWith(str);

                        //ParseStream( str );
                        if (Stream.CanWrite)
                        {
                            Stream.Write(DataArray.ToArray(), 0, DataArray.Count);
                        }
                    }
                }
            }
        }

        public void ActorRun()
        {
            while (true)
            {
                Thread.Sleep(500);

                if (IsConnected && Qlist.Count > 0)
                {
                    var cmd = Qlist.Dequeue();





                }
            }

        }


        public static void Insert(string cmd)
            => Task.Run(() => Qlist.Enqueue(cmd));

        public static void ActWith(string cmd)
        {
            var strfirst = cmd.Split('|');
            var head = strfirst.First();
            // 헤드가 넘버인지 체크 

            var body = strfirst.Skip(1).Take(Int32.Parse(head));
            // 바디의 카운트가 헤드의 카운트와 같은지 확인.
            // 명령어가 정의되있는 명령어인지 확인. 
            // 다음부터는 



            var tail = "";

            var seperated = tail.Replace('\r', ' ').Trim().Split('\n');


            // 공용

            //switch ()

        }


    }
    

    public static class CommandLibExt
    {
    //public static Queue<T> InsertIn<T>(
    //    this T src, Queue<T> container )
    //    => container.Enqueue( src );
    //
    public static void main()
    { }
    }
}

