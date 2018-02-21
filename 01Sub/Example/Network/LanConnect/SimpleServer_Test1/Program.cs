using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer_Test1
{
    class Program
    {
        static string ServerAdd = "192.168.11.45";
        static void Main(string[] args)
        {
            MultiWay();
        }

        static void MultiWay()
        {
            TcpListener tcpListner = null;
            Socket clientsocket = null;

            try
            {
                IPAddress ipAd = IPAddress.Parse(ServerAdd);
                tcpListner = new TcpListener(ipAd, 5001);
                tcpListner.Start();

                while (true)
                {
                    clientsocket = tcpListner.AcceptSocket();

                    ClientHandler cHandler = new ClientHandler(clientsocket);
                    Task.Run( (Action)cHandler.Chat );
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                clientsocket.Close();
            }
        }
        static void TwoWay()
        {
            NetworkStream stream = null;
            TcpListener tcpListner = null;
            Socket clientsocket = null;
            StreamReader reader = null;
            StreamWriter writer = null;

            try
            {
                IPAddress ipAd = IPAddress.Parse(ServerAdd);

                tcpListner = new TcpListener(ipAd, 5001);
                tcpListner.Start();


                clientsocket = tcpListner.AcceptSocket();

                stream = new NetworkStream(clientsocket);
                Encoding encode = Encoding.GetEncoding("utf-8");


                reader = new StreamReader(stream);
                writer = new StreamWriter(stream, encode) { AutoFlush = true };

                while (true)
                {
                    Console.WriteLine("In While Loop");
                    string str = reader.ReadLine();
                    Console.WriteLine(str);
                    writer.WriteLine("From Server : ", str);
                }
            }
            catch (Exception)
            {

            }
           

        }
    }

    public class ClientHandler
    {
        Socket Socket = null;
        NetworkStream Stream = null;
        StreamReader Raeder = null;
        StreamWriter Writer = null;
        public ClientHandler(Socket socket)
        {
            this.Socket = socket;
        }

        public void Chat()
        {
            Stream = new NetworkStream(Socket);
            Encoding encode = Encoding.GetEncoding("utf-8");

            Raeder = new StreamReader(Stream, encode);
            Writer = new StreamWriter(Stream, encode) { AutoFlush = true};

            while (true)
            {
                try
                {
                    string str = Raeder.ReadLine();
                    Console.WriteLine(str);
                    Writer.WriteLine(str);
                }
                catch (Exception)
                {
                    var ippoart = this.Socket.RemoteEndPoint.ToString();

                    Console.WriteLine("Connection is lost from {0}", ippoart);

                    break;
                }
            }
        }
    }
}
