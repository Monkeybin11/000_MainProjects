using EMxLib.device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EMxLib.susceptorexport
{
    public class SusceptorExportServer : ISusceptorExporter
    {
        public int Port { get; set; }
        TcpListener Server { get; set; }
        public bool Connected { get; set; }
        string ipAddr { get; set; }
        Thread tt;
        bool isrunning { get; set; }
        public static string DEV_NAME { get { return "SusceptorExportServer"; } }

        public SusceptorExportServer()
        {
            m_strDevName = DEV_NAME;
            DeviceType = eDeviceType.SusceptorExporter;
            Server = null;
            Connected = false;
            ipAddr = "127.0.0.1";
            Port = 13579;
        }

        public override eDeviceError ConnectDevice(DeviceConnectionInfo param)
        {
            Connect(param.Address, param.TcpPort);
            Connected = true;
            return base.ConnectDevice(param);
        }

        public void Connect(string addr, int port)
        {
            ipAddr = addr;
            Port = port;
            isrunning = true;
            tt = new Thread(Run);
            tt.Start();
            InvokeProcess();

        }

        public override eDeviceError DisconnectDevice()
        {
            RemoveRemoteProcess();
            Release();
            return base.DisconnectDevice();
        }

        public void Release()
        {
            try
            {
                isrunning = false;
                Connected = false;
                Server.Server.Close();
            }
            catch (Exception except)
            {
                EMx.Log.Error(except.ToString());
            }
        }

        TcpClient client = null;
        NetworkStream stream;


        /// <summary>
        /// remove laser writer
        /// </summary>
        private void RemoveRemoteProcess()
        {
            // kill
            foreach (Process proc in Process.GetProcesses())
            {
                string procname = proc.ProcessName.Replace(".vshost", "");
                
                if (procname.ToLower().Equals(EMx.DevExConf.SusceptorExporterProcess.ToLower()))
                {
                    proc.Kill();
                }
            }
        }


        /// <summary>
        /// invoke laser writer
        /// </summary>
        public void InvokeProcess()
        {
            RemoveRemoteProcess();

            string program_path = EMx.DevExConf.SusceptorExporterPath;
            if (File.Exists(program_path))
            {
                Process plato = new Process();
                plato.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(program_path);
                plato.StartInfo.FileName = program_path;
                plato.Start();
            }
        }

        private void Query(SusceptorExportPacket packet)
        {
            if(client.Connected)
            {
                try
                {
                    {
                        if (stream.CanWrite)
                        {
                            byte[] returnbyte = packet.ToBytes();
                            stream.Write(returnbyte, 0, returnbyte.Length);
                            EMx.Log.Info("SusceptorExportServer Stream sended " + packet);
                        }
                        else
                        {
                            EMx.Log.Error("SusceptorExportServer Stream cannot write " + packet);
                        }

                        Thread.Sleep(100);

                        if(stream.CanRead)
                        {
                            double timeouttime = 30000;
                            DateTime now = DateTime.Now;

                            List<byte> DataArray = new List<byte>();
                            byte prevlastbyte = 0xFF;
                            byte lastbyte = 0xFF;

                            while ((DateTime.Now - now).TotalMilliseconds <timeouttime)
                            {
                                if ((prevlastbyte == (byte)'\r') && (lastbyte == (byte)'\n'))
                                {
                                    break;
                                }
                                else
                                {
                                    prevlastbyte = lastbyte;
                                    lastbyte = (byte)stream.ReadByte();
                                    DataArray.Add(lastbyte);
                                    if (DataArray.Count > 10000)
                                    {
                                        EMx.Log.Info("SusceptorExportClient endchar none");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                        
                }
                catch (System.IO.IOException except)
                {
                    EMx.Log.Error("SusceptorExportServer client Timeout " + except.ToString());
                }
                catch (Exception except)
                {
                    EMx.Log.Error("SusceptorExportServer client Exception" + except.ToString());
                }
            }
        }

        public void ValidateSamePacket(byte[] sended , byte[] received)
        {
            if (sended.Length != received.Length)
            {
                EMx.Log.Error("SusceptorExportServer packet legnth does not matching {0} {1}", sended.Length, received.Length);
                return;
            }

            for (int i = 0; i < sended.Length; i++)
            {
                if (sended[i]!= received[i])
                {
                    EMx.Log.Error("SusceptorExportServer packet does not matching {0} {1}", Encoding.UTF8.GetString( sended), Encoding.UTF8.GetString(received));
                    return;
                }
            }
        }
        
        public void Run()
        {
            try
            {
                IPAddress adress = IPAddress.Parse(ipAddr);

                Server = new TcpListener(adress, Port);
                Server.Start();
                

                Console.WriteLine(" Wait for Connection");
                
                while (isrunning)
                {
                    try
                    {
                        if (Server.Pending())
                        {
                            EMx.Log.Info("SusceptorExportServer WaitTcpClient");
                            client = Server.AcceptTcpClient();
                            client.SendBufferSize = 100;
                            client.ReceiveTimeout = 30000;
                            stream = client.GetStream();
                            EMx.Log.Info("SusceptorExportServer Server get stream " + Port.ToString());
                        }
                    }
                    catch (Exception except)
                    {
                        EMx.Log.Error("SusceptorExportServer Server.AcceptTcpClient : " + except.ToString());
                        continue;
                    }

                    if (client == null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    if (client.Connected)
                    {
                        Connected = true;
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("SusceptorExportServer : " + e.ToString());
            }
            finally
            {
                if (stream != null )
                {
                    stream.Close();
                }
                isrunning = false;
            }
        }

        #region cmd

        public override void Close(string msg)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.Close;
            packet.Args = new string[0];
            Query(packet);
        }

        public override void Save(string path)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.Save;
            packet.Args = new string[] { path };
            Query(packet);
        }

        public override void Size(double rngmin, double rngmax)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.Size;
            packet.Args = new string[] { rngmin.ToString(), rngmax.ToString() };
            Query(packet);
        }


        public override void SetAutoRange(double rngmin, double rngmax)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.SetAutoRange;
            packet.Args = new string[] { rngmin.ToString(), rngmax.ToString() };
            Query(packet);
        }


        public override void SetFixedRange(double rngmin, double rngmax)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.SetFixedRange;
            packet.Args = new string[] { rngmin.ToString(), rngmax.ToString() };
            Query(packet);
        }


        public override void SetAverageOffsetRange(double rngmin, double rngmax)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.SetAverageOffsetRange;
            packet.Args = new string[] { rngmin.ToString(), rngmax.ToString() };
            Query(packet);
        }


        public override void SetItem(string item)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.SetItem;
            packet.Args = new string[] { item };
            Query(packet);
        }

        public override void LoadWafer(string path)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.LoadWafer;
            packet.Args = new string[] { path };
            Query(packet);
        }

        public override void LoadSusceptor(string path)
        {
            SusceptorExportPacket packet = new SusceptorExportPacket();
            packet.Commnad = SusceptroExportCommands.LoadSusceptor;
            packet.Args = new string[] { path };
            Query(packet);
        }

        #endregion cmd
    }
}
