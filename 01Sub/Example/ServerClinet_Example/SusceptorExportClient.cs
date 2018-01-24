using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EMxLib.susceptorexport
{
    public enum TcpErrorCode
    {
        None,
        RunIndexDoesNotMatching,
    }

    public delegate void SusceptorExportEventHandler(SusceptorExportPacket packet);

    public class SusceptorExportClient
    {
        public event SusceptorExportEventHandler SusceptorExportEvent;
        private void OnSusceptorEvent(SusceptorExportPacket packet)
        {
            SusceptorExportEvent?.Invoke(packet);
        }

        public int Port { get; set; }
        public TcpClient client { get; set; }

        NetworkStream stream { get; set; }
        byte[] Query { get; set; }
        public bool IsConnected { get; set; }
        string ipAddr { get; set; }
        Thread tt;
        bool IsReleased { get; set; }

        SusceptorExportPacket lastPacket;

        public SusceptorExportClient()
        {
            client = new TcpClient();
            Port = 10001;
            IsConnected = client.Connected;
            IsReleased = false;
            
            Query = new byte[8];
        }
        
        public void Connect(string addr, int port)
        {
            try
            {
                Port = port;
                ipAddr = addr;
                client.Connect(ipAddr, Port);
                client.ReceiveBufferSize = 1024 * 100;
                client.SendBufferSize = 1024 * 100;
                client.SendTimeout = 200;
                client.ReceiveTimeout = 200;

                stream = client.GetStream();
                EMx.Log.Info("LClient Get Stream");
                IsConnected = true;

            }
            catch (Exception except)
            {
                IsConnected = false;
                EMx.Log.Error("LClient Get Stream false " + except.ToString());
            }

            ThreadRun();

        }

        private void ThreadRun()
        {
            EMx.Log.Info("LClient ThreadRun");
            try
            {
                IsConnected = true;
                tt = new Thread(Run);
                tt.Name = "연결되었습니다.";
                tt.Start();
            }
            catch (Exception except)
            {
                IsConnected = false;
                EMx.Log.Error("LClient ThreadRun catch " + except.ToString());
            }
            EMx.Log.Info("LClient ThreadRun complete");
        }

        public void Connect()
        {
            try
            {
                client.Connect(ipAddr, Port);
                client.ReceiveBufferSize = 1024 * 1024 * 100;
                client.SendBufferSize = 1024 * 100;
                client.SendTimeout = 200;
                client.ReceiveTimeout = 200;

                stream = client.GetStream();
                EMx.Log.Info("LClient Get Stream");
                tt = new Thread(Run);
                tt.Name = "Client Communication Thread";
                tt.Start();
            }
            catch (Exception except)
            {
                EMx.Log.Error("LClient Get Stream fail " + except.ToString());

            }

        }

        public void Release()
        {
            try
            {
                IsReleased = true;
                EMx.Log.Info("LClient Release");
                IsConnected = false;
                if (stream != null)
                {
                    stream.Close();
                }
                client.Close();
            }
            catch (Exception except)
            {
                EMx.Log.Error(except.ToString());
            }
        }

        public void Reconnect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(ipAddr, Port);
                client.ReceiveBufferSize = 1024 * 1024 * 100;
                client.SendBufferSize = 1024 * 100;
                client.SendTimeout = 200;
                client.ReceiveTimeout = 200;
                stream = client.GetStream();
            }
            catch (Exception except)
            {
                EMx.Log.Info("retry to reconnect error " + except.ToString());
            }
        }

        public void Run()
        {
            try
            {
                IsConnected = true;
                DateTime NextPingTime = DateTime.Now;
                while (IsConnected)
                {
                    try
                    {
                        if (client != null)
                        {
                            if (client.Connected == false)
                            {
                                try
                                {
                                    EMx.Log.Info("retry to reconnect");
                                    client.Close();
                                    Reconnect();
                                }
                                catch (Exception except)
                                {
                                    EMx.Log.Error("failed to Reconnect");
                                    Thread.Sleep(100);
                                    continue;
                                }
                            }
                        }


                        if (client == null)
                        {
                            EMx.Log.Error("SusceptorExportClient client null : ");
                            client = new TcpClient();
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (client.Connected)
                        {
                            try
                            {
                                List<byte> DataArray = new List<byte>();
                                if (stream.DataAvailable)
                                {
                                    NextPingTime = DateTime.Now.AddSeconds(10);
                                    byte prevlastbyte = 0xFF;
                                    byte lastbyte = 0xFF;
                                    while ((prevlastbyte != (byte)'\r') || (lastbyte != (byte)'\n'))
                                    {
                                        prevlastbyte = lastbyte;
                                        lastbyte = (byte)stream.ReadByte();
                                        //    Console.WriteLine(lastbyte.ToString("x2"));
                                        DataArray.Add(lastbyte);
                                        if (DataArray.Count > 10000)
                                        {
                                            EMx.Log.Info("SusceptorExportClient endchar none");
                                            stream.Close();
                                            continue;
                                        }
                                    }

                                    ;
                                    string str = Encoding.UTF8.GetString(DataArray.Take(DataArray.Count - 2).ToArray());
                                    ParseStream(str);

                                    if (stream.CanWrite)
                                    {
                                        stream.Write(DataArray.ToArray(), 0, DataArray.Count);
                                    }
                                    Thread.Sleep(1);
                                }



                                Thread.Sleep(1);
                            }
                            catch (System.IO.IOException except)
                            {
                                EMx.Log.Error("SusceptorExportClient client Timeout " + except.ToString());

                            }
                            catch (Exception except)
                            {

                                EMx.Log.Error("SusceptorExportClient client Exception" + except.ToString());

                                stream.Close();

                                continue;
                            }
                        }
                        Thread.Sleep(100);

                    }
                    catch (Exception e)
                    {
                        EMx.Log.Error("SusceptorExportClient Inner Loop : " + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                EMx.Log.Error("SusceptorExportClient Loop : " + e.ToString());
            }
            finally
            {
                IsConnected = false;
                EMx.Log.Error("SusceptorExportClient Loop Finished");
            }
        }

        byte[] GetBytes(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return bytes;
        }


        public void ParseStream(string str)
        {
            try
            {
                lastPacket = new SusceptorExportPacket();
                EMx.Log.Info("SusceptorExportClient Stream received " + str);
                lastPacket.Parse(str);

                switch (lastPacket.Commnad)
                {
                    case SusceptroExportCommands.None:
                        EMx.Log.Error("LCLIENT ParseStream NONE " + str);
                        break;
                    case SusceptroExportCommands.Error:
                        EMx.Log.Error("LCLIENT ParseStream Error " + str);
                        break;
                    default:
                        OnSusceptorEvent(lastPacket);
                        break;
                }
            }
            catch (Exception except)
            {
                EMx.Log.Error("LCLIENT ParseStream ERRERRERR " + except.ToString());
            }
        }
        
    }
}
