using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RS232_LibTest
{
    public delegate void TransStr( string str );
    public enum sigmaCom { }
    public class Sigmakoki
    {
        public string MovePosSetCommand { get { return "A:1{0}P{1}"; } set {; } }
        public string MoveSettedPosCommand { get { return "G:"; } set {; } }
        public string OriginCommand { get { return "H:1"; } set {; } }
        public string StatusCommand { get { return "Q:"; } set {; } }
        public string ForceStopCommand { get { return "L:E"; } set {; } }

        public event TransStr evtTransStr;

        SerialPort Port = new SerialPort();
        RS232<sigmaCom> sl;
        public Sigmakoki()
        {
            Port.PortName = "COM6";
            Port.BaudRate = 38400;
            Port.DataBits = 8;
            Port.Parity = Parity.None;
            //Port.Handshake = Handshake.None;
            Port.StopBits = StopBits.One;
            Port.Handshake = Handshake.RequestToSend;
            //Port.Encoding = Encoding.UTF8;
            //Port.DataReceived += Port_DataReceived;
            sl = new RS232<sigmaCom>( Port );
            sl.Open();

            var res = Port.IsOpen;
            Console.WriteLine( "Open is " + res.ToString() );
        }

        private void Port_DataReceived( object sender, SerialDataReceivedEventArgs e )
        {
            try
            {

                var port = sender as SerialPort;
                var size = port.ReadBufferSize;
                var buffdata = port.ReadExisting();
                evtTransStr( buffdata );
            }
            catch ( Exception )
            {
                Console.WriteLine( "err" );
            }
        }

        public void Origin()
        {
            Write( OriginCommand );
        }

        public void MoveAbsPos( int pos )
        {
            Write( String.Format( MovePosSetCommand, pos > 0 ? "+" : "-", (int)Math.Abs( pos ) ) );
            Write( MoveSettedPosCommand );
        }

        public void wait(int pos )
        {
            Stopwatch sw = new Stopwatch();
            Thread.Sleep( 1000 );
            string res = "";
            Console.WriteLine( "Wait start" );

            
            while (res != "R" ) {
                sw.Start();
                res = Queary( "!:" );
                Thread.Sleep( 100 );

                sw.Stop();
                var passtime = sw.ElapsedMilliseconds;
                Console.WriteLine( "Pass Time : " + passtime );
                if ( passtime > 20000 )
                {
                    break;
                };
            };
            Console.WriteLine( "Wait Done" );
        }

      

        public void SetXSpeed( int speed, int acc )
        {
            string input = String.Format("D:1S{0}F{1}R{2}", speed, speed, acc);
            Write( input );
        }

        public void Write( string command )
        {
            sl.Send( command + "\r" );
            //sl.Send( command + "\r\n" );
        }

        public string Queary( string command )
        {
            return sl.query( command + "\r" );
        }

        public void Close()
        {
            sl.Close();
        }
     
    


    }
}
