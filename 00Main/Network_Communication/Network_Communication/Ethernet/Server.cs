using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SpeedyCoding;

namespace Network_Communication.Ethernet
{
    public class Server 
    {
        readonly string ServerIp;
        readonly int Port;
        readonly Action<string> ErrorFunc;
        readonly IRule Rule;

        public Server( string serverIP , int port , IRule rule, Action<string> errorFunc )
        {
            ServerIp = serverIP;
            Port = port;
            Rule = rule;
            ErrorFunc = errorFunc;
        }

        public void Runner()
        {
            IPEndPoint adress = new IPEndPoint(IPAddress.Parse(ServerIp) , Port);

            while ( true )
            {



            }

           
        }

        public void Reciever( TcpClient clinet )
        {
            
        }

        public void Sender( string clinet )
        {
            //


        }

        public void ClinetRequestHandler( TcpClient clinet )
        {
            while ( true )
            {

                string msg = "";

                if ( Rule.CheckMsg( msg ) ) Rule.DoAct( msg );

            }
        }




    }
}
