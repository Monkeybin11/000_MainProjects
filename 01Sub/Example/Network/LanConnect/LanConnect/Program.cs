using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LanConnect
{
    class Program
    {
        static void Main( string[] args )
        {
            Run();
            /*
            if ( args.Length < 4 )
            {
                Console.WriteLine( "Usage : {0}  <bindIp> <BindPort> <ServerIp> <Message>" , Process.GetCurrentProcess().ProcessName );
                return;
            }

            //string bindIp = args[0];
            string bindIp = "169.254.23.213";
            int bindPort = Convert.ToInt32(args[1]);
            string serverIp = args[2];
            string message  = args[3];
            const int serverProt = 5425;


            try
            {
                IPEndPoint clientAddress = new IPEndPoint(IPAddress.Parse(bindIp) , bindPort);
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(bindIp) , bindPort);

                Console.WriteLine( "Client : {0} , Server : {1}", clientAddress.ToString(), serverAddress.ToString() );

                TcpClient client = new TcpClient(clientAddress);
                client.Connect(serverAddress);

                byte[] data = Encoding.Default.GetBytes(message);

                NetworkStream stream = client.GetStream();

                stream.Write( data, 0, data.Length );
                Console.WriteLine("Send : {0}" , message);

                data = new byte[256];

                string responsData = "";

                int bytes = stream.Read(data,0,data.Length);
                responsData = Encoding.Default.GetString( data, 0, bytes );
                Console.WriteLine( "Received : {0}", responsData );

                stream.Close();
                client.Close();
            }
            catch ( SocketException ex )
            {
                Console.WriteLine( ex.ToString() );
            }

            Console.WriteLine( "Clinet exit" );
            */
        }

        public static void Run()
        {
            //IPEndPoint localAdress = new IPEndPoint(IPAddress.Parse("169.254.23.213") , 5425);
            IPEndPoint localAdress = new IPEndPoint(IPAddress.Parse("192.168.11.45") , 5425);
            //IPEndPoint localAdress = new IPEndPoint(IPAddress.Any , 5425);
            TcpListener server = new TcpListener(localAdress);
            server.Start();

            byte[] buff = new byte[1024];

            while ( true )
            {
                Thread.Sleep( 1000 );
                Console.WriteLine( "start" );
                TcpClient client = server.AcceptTcpClient();

                var stream = client.GetStream();

                int nbyte;
                while ( true )
                {

                    while ( ( nbyte = stream.Read( buff, 0, buff.Length ) ) > 0 )
                    {

                        var str = Encoding.ASCII.GetString(buff,0,nbyte);

                        str = "Recived String is " + str;

                        var outputdata = Encoding.ASCII.GetBytes(str);

                        stream.Write( outputdata, 0, outputdata.Length );

                        var outstr = Encoding.ASCII.GetString( buff , 0, nbyte );

                        Console.WriteLine( "Raed string is" );
                        Console.WriteLine( outstr );
                    }   

                    if ( !client.Connected )
                    {

                        stream.Close();
                        client.Close();
                        client = server.AcceptTcpClient();
                        stream = client.GetStream();
                    }
                    break;

                }
          
            }
        }

    }
}
