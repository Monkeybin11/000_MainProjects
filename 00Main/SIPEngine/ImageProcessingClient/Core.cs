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
        List<WfInfo> WfInfoFinished = new List<WfInfo>();
        WaferStatus WfStatus = WaferStatus.Wating;
        WfInfo WfNow;
        Thread ProcessingThread;
        Core_PlMapping ProcCore = new Core_PlMapping();

        string SaveDir;

        public bool Connect(string serverIp, int port)
        {
            try
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
                return true;
            }
            catch (Exception)
            {
                return false;
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
                        Thread.Sleep(3000);
                        WfNow = WfInfoList.Dequeue();

                        var splited2 = File.ReadAllText(WfNow.RecipePath , Encoding.UTF8 );
                        var splited = File.ReadAllText(WfNow.RecipePath, Encoding.UTF8).Split('|'); // preporc , constrain


                        var res = ProcCore.Start(WfNow.WaferPath, splited[0], splited[1])
                        .Match(
                            () => SendTBD() ,
                            x  => SaveResult(x) );
                        SendProcessResult(res);

                        WfInfoFinished.Add(WfNow);
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
               while (SaveDir == null)
               { Thread.Sleep(100); }
               var path = Path.Combine(SaveDir, WfNow.WaferName+"_"+WfNow.WaferCreateTime + ".csv");

               StringBuilder stb = new StringBuilder();
               var delimiter = ",";
               stb.Append("Y ");
               stb.Append(delimiter);
               stb.Append("X ");
               stb.Append(delimiter);
               stb.Append("Y Error ");
               stb.Append(delimiter);
               stb.Append("X Error ");
               stb.Append(delimiter);
               stb.Append("OK/NG/LOW/OVER");
               stb.Append(delimiter);
               stb.Append("Size");
               stb.Append(delimiter);
               stb.Append("Integrated Intensity");
               stb.Append(delimiter);
               stb.Append(Environment.NewLine);

               stb.Append("(Row)");
               stb.Append(delimiter);
               stb.Append("(Column)");
               stb.Append(delimiter);
               stb.Append("(pixel)");
               stb.Append(delimiter);
               stb.Append("(pixel)");
               stb.Append(delimiter);
               stb.Append(" ");
               stb.Append(delimiter);
               stb.Append("(pixel^2)");
               stb.Append(delimiter);
               stb.Append("(a.u)");
               stb.Append(delimiter);
               stb.Append(Environment.NewLine);

               result = result.OrderBy(x => x.Hindex).ThenBy(x => x.Windex).ToList();

               for (int i = 0; i < result.Count; i++)
               {
                   stb.Append(result[i].Hindex + 1);
                   stb.Append(delimiter);
                   stb.Append(result[i].Windex + 1);
                   stb.Append(delimiter);
                   stb.Append(result[i].HindexError);
                   stb.Append(delimiter);
                   stb.Append(result[i].WindexError);
                   stb.Append(delimiter);
                   stb.Append(result[i].OKNG);
                   stb.Append(delimiter);
                   stb.Append(result[i].ContourSize);
                   stb.Append(delimiter);
                   stb.Append(result[i].Intensity);
                   stb.Append(Environment.NewLine);
               }
               System.IO.File.WriteAllText(path, stb.ToString());


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
                if (WfNow == null)
                {
                    var empty = "GetQueueStatus|0||||";
                    ProcClient.SendMsg(empty.WithCount());
                }
                else
                {
                    var res = "GetQueueStatus" +
                           WfInfoList.Count.ToString().AddBar() +
                           WfNow.WaferCreateTime.AddBar() +
                           WfNow.WaferName.AddBar() +
                           "Processing".AddBar();

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
            }
            catch (Exception)
            {
                ProcClient.SendMsg("TBD");
            }
        }

        void GetWaferStatus(BodyList body)
        {
            var temp = body.ToArray();
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
