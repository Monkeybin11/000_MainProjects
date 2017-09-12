using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RS232_LibTest
{
    public class RS232<TcommandList> where TcommandList : struct, IComparable, IConvertible, IFormattable
    {
        public SerialPort Port;
        public event Action<string> evtReadDone;

        public RS232( SerialPort port )
        {
            Port = port;
            //Port.DataReceived += new SerialDataReceivedEventHandler( ReadDone );
        }

        public bool? Open()
        {
            Port.Open();
            return true;
        }

        public void Close()
        {
            Port.Close();
            Port.Dispose();
        }

        private void ReadDone( object sender, SerialDataReceivedEventArgs e )
        {
            Console.WriteLine( "REad" );
            var port = sender as SerialPort;
            var size = port.ReadBufferSize;
            string lineReadIn = "";
            while ( port.BytesToRead > 0 )
            {
                lineReadIn += port.ReadExisting();
                Thread.Sleep( 25 );
            }
            Console.WriteLine( "RS232 : " + lineReadIn.ToString() );


            //try
            //{
            //    evtReadDone( (sender as SerialPort).ReadExisting() );
            //}
            //catch ( Exception )
            //{
            //    evtReadDone( null );
            //    throw;
            //}
        }
        byte[] Delimiter = new byte[] { 0x0d , 0x0a };

        public string query( string text )
        {
            Port.WriteLine( text );
            Thread.Sleep( 300 );
            var res = Port.ReadExisting().Replace("\r" , string.Empty ).Replace("\n" , string.Empty);
            Console.WriteLine( "Query Rescived : " + res );
            return res;
        }

        public void Send( string text )
        {
           // lock ( Port )
           // {
           //     try
           //     {
           //         
           //         var arr = System.Text.Encoding.ASCII.GetBytes(text);
           //         Port.Write( arr, 0, arr.Length );
           //         Port.Write( Delimiter, 0, Delimiter.Length );
           //
           //
           //         List<byte> recvPacket = new List<byte>();
           //
           //         Thread.Sleep( 1 );
           //
           //         DateTime TimeoutTime = DateTime.Now.AddMilliseconds(100);
           //         while ( TimeoutTime > DateTime.Now )
           //         {
           //             if ( Port.BytesToRead > 0 )
           //             {
           //                 byte[] readbyte = new byte[Port.BytesToRead];
           //                 Port.Read( readbyte, 0, readbyte.Length );
           //                 recvPacket.AddRange( readbyte );
           //             }
           //
           //             if ( CheckDelimiter( recvPacket ) )
           //             {
           //                 break;
           //             }
           //         }
           //         if ( recvPacket.Count > Delimiter.Length )
           //         {
           //             var res = Encoding.ASCII.GetString( recvPacket.ToArray(), 0, recvPacket.Count - Delimiter.Length );
           //         }
           //     }
           //     catch ( ArgumentOutOfRangeException e )
           //     {
           //         Console.WriteLine( "error = " + e.Message );
           //     }
           // }
           lock(Port)
            {
                Port.WriteLine( text );
            }
           
            //byte[] Delimiter = new byte[] { 0x0d };
            //var arr = Encoding.ASCII.GetBytes(text.Trim());
            //Port.Write( arr, 0, arr.Length );
            //Port.Write( Delimiter, 0, Delimiter.Length );
        }
        
        private bool CheckDelimiter( List<byte> recvPacket )
        {
            if ( recvPacket.Count >= Delimiter.Length )
            {
                for ( int i = 0; i < Delimiter.Length; i++ )
                {
                    if ( recvPacket[recvPacket.Count - i - 1] != Delimiter[Delimiter.Length - i - 1] )
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        public void Send( string text, double value )
        {
            text = text + " " + value.ToString();
            byte[] Delimiter = new byte[] { 0x0d };
            var arr = Encoding.ASCII.GetBytes(text.Trim());
            Port.Write( arr, 0, arr.Length );
            Port.Write( Delimiter, 0, Delimiter.Length );
        }

        public void Send( TcommandList command, double value )
        {
            var text = command.ToString() + " " + value.ToString();
            byte[] Delimiter = new byte[] { 0x0d };
            var arr = Encoding.ASCII.GetBytes(text.Trim());
            Port.Write( arr, 0, arr.Length );
            Port.Write( Delimiter, 0, Delimiter.Length );
        }

    }
}
