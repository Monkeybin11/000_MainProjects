using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;

namespace Network_Communication.Ethernet
{
    public class Client
    {
        public event Action<string> evtRecived;

        TcpClient client;
        string ServerAddress;
        int Port;

        StreamWriter Writer;
        StreamReader Raeder;
        Encoding EncodeFormat;


        public Client(string serverIP, int port, string encode = "utf-8")
        {
            ServerAddress = serverIP;
            Port = port;
        }

        public void BuildClient()
        {
            client = new TcpClient();
            client.Connect(ServerAddress, Port);
            EncodeFormat = Encoding.GetEncoding("utf-8");

            var netStream = client.GetStream();

            Writer = new StreamWriter(netStream, EncodeFormat) { AutoFlush = true };
            Raeder = new StreamReader(netStream, EncodeFormat);
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var temp = Raeder.ReadLine();
                        evtRecived(temp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        MessageBox.Show("Connection Lost")      ;
                        break;
                    }
                }
            });
        }

        public void SendMsg(string msg)
            =>  Writer.WriteLine(msg);
    }
}
