using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;
using ImageProcessingClient;
using Network_Communication.Ethernet;
using System.Threading;
using PLMapping_SIPCore;
using SIP_InspectLib.DataType;
using System.Windows.Forms;
using System.IO;

namespace ImageProcessingClient
{
    using BodyList = IEnumerable<string>;

    enum WaferStatus { Wating , Copying , Processing , Error };

    public class Core
    {
        Client ProcClient;
        Encoding encode = Encoding.GetEncoding("utf-8");
        Queue<WfInfo> WfInfoList = new Queue<WfInfo>();
        WaferStatus WfStatus = WaferStatus.Wating;
        WfInfo WfNow;
        Thread ProcessingThread;
        Core_PlMapping ProcCore = new Core_PlMapping();

        string SaveDir;

        public void Connect(string serverIp, int port)
        {
            ProcClient = new Client(serverIp, port);
            
            ProcClient.evtRecived += Actor;
            ProcClient.BuildClient();
            Processing();

            var fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                SaveDir = fd.SelectedPath;
            }
        }

        Action Processing =>
            () =>
            Task.Run(() =>
            {
                ProcessingThread = Thread.CurrentThread;
                while (true)
                {
                    if (WfInfoList.Count > 0)
                    {
                        WfNow = WfInfoList.Dequeue();

                        var splited2 = File.ReadAllText(WfNow.RecipePath , Encoding.UTF8 );
                        var splited = File.ReadAllText(WfNow.RecipePath, Encoding.UTF8).Split('|'); // preporc , constrain


                        var res = ProcCore.Start(WfNow.WaferPath, splited[0], splited[1])
                        .Match(
                            () => SendTBD() ,
                            x  => SaveResult(x) );
                        SendProcessResult(res);
                        // Start Processing
                    }
                    Thread.Sleep(1000);
                }
            });

        Action<string> Actor
            => str
            =>
            {
                var splited = str.Split('|');
                var cmd = splited[1];
                var body = splited.Skip(2);
                var temp = body.ToArray();
                //여기서 str을 분리하자. 
                // 1. 패킷 길이만큼 읽기
                // 2. 
                //
                //
                //
                //

                switch (cmd)
                {
                    //Common
                    case "*IDN?":
                        IDN(cmd);
                        break;

                    case "ERR?":
                        ERR(cmd);
                        break;

                    case "*RST":
                        RST();
                        break;

                    case "Alarm":
                        Alarm();
                        break;

                    case "Exit":
                        Exit();
                        break;


                    //Processing
                    case "AddWafer":
                        AddWafer(body);
                        break;

                    case "GetQueueStatus":
                        GetQueueStatus();
                        break;

                    case "GetWaferStatus":
                        GetWaferStatus(body);
                        break;

                    case "RemoveWafer":
                        RemoveWafer(body);
                        break;

                    case "Clear":
                        Clear();
                        break;
                }
            };

        Func<string> SendTBD
            => ()
            => "TBD";

        Func<List<ExResult>,string> SaveResult
           => result
           => 
           {

               // SaveResult
               var path = Path.Combine(SaveDir, WfNow.WaferName + ".csv");
               return path;
           };

        #region Common Command
        void IDN(string str)
        {
            string ID = "03";
            string res = str + "|" + ID + "\r\n";
            ProcClient.SendMsg( res.WithCount() );
        }

        void ERR(string str)
        {
            // GetError() : void -> string
            string error = "ID";
            string res = str + "|" + error + "\r\n";
            ProcClient.SendMsg(res.WithCount());
        }

        void RST()
        {


            //초기 설정으로 복구



        }

        void Alarm()
        {

        }

        void Exit()
        {
            Environment.Exit(Environment.ExitCode);
        }
        #endregion


        #region Processing Command
        void AddWafer(BodyList body)
            => WfInfoList.Enqueue( body.ToWfInfo());

        void GetQueueStatus()
        {
            try
            {
                var res = "GetQueueStatus" +
                            WfInfoList.Count.ToString().AddBar() +
                            WfNow.WaferCreateTime.AddBar() +
                            WfNow.WaferName.AddBar() +
                            WfStatus.ToString().AddBar();

                // Wait Wafer
                var wlist = WfInfoList.ToList();

                for (int i = 0; i < WfInfoList.Count; i++)
                {
                    var createTime = wlist[i].WaferCreateTime;
                    var name = wlist[i].WaferName;
                    var status = "Wait";

                    res = res +
                        createTime.AddBar() +
                        name.AddBar() +
                        status.AddBar();
                }

                ProcClient.SendMsg(res.WithCount());
            }
            catch (Exception)
            {
                ProcClient.SendMsg("TBD");
            }
        }

        void GetWaferStatus(BodyList body)
        {
            var key = body.ElementAt(0);

            string res;

            if (WfNow.WaferCreateTime == key)
            {
                res = "GetWaferStatus" +
                   WfNow.WaferCreateTime.AddBar() +
                   WfNow.WaferName.AddBar() +
                   WfStatus.ToString().AddBar();
            }
            else if (WfInfoList.Select(x => x.WaferCreateTime).Contains(key))
            {
                var result = WfInfoList.Where(x => x.WaferCreateTime == key).First();

                res = "GetWaferStatus" +
                        key.AddBar() +
                        result.WaferName.AddBar() +
                        "Wait".AddBar();
            }
            else
            {
                res = "TBD";
            }
            ProcClient.SendMsg(res.WithCount());
        }

        void RemoveWafer(BodyList body )
        {
            try
            {
                var key = body.ElementAt(0);
                WfInfoList = new Queue<WfInfo>(WfInfoList.Where(x => x.WaferCreateTime != key));
            }
            catch (Exception)
            {
                ProcClient.SendMsg("TBD");
            }
        }

        void Clear()
        {
            try
            {
                WfInfoList = new Queue<WfInfo>();
            }
            catch (Exception)
            {
                ProcClient.SendMsg("TBD");
            }
        }
        #endregion


        #region ToMaster

        Action<string> SendProcessResult
            => result
            =>
        {
            var res = "SendProcessResult" +
                  WfNow.WaferCreateTime.AddBar() +
                  WfNow.WaferName.AddBar() +
                  "csv" +
                  result;
            ProcClient.SendMsg(res.WithCount());
        };

        #endregion  

    }

    public static class Ext
    {
        public static string WithCount
            (this string str)
            => Encoding.GetEncoding("utf-8").GetByteCount(str).ToString() + "|" + str;

        public static string AddBar
            (this string str)
            => "|" + str ;
    }

    



}
