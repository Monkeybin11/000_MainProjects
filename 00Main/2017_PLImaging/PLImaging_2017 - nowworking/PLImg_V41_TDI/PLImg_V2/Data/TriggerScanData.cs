using MachineControl.Camera.Dalsa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImg_V2.Data
{
    public class TriggerScanData
    {
        public Dictionary<ScanConfig,double> StartYPos;
        public Dictionary<ScanConfig,double> StartXPos;
        public Dictionary<ScanConfig,double> EndYPos;
        public double XStep_Size       = 22.3;
        public double Scan_Stage_Speed = 3;
        public double Camera_Exposure  = 800;
        public double Camera_LineRate  = 1200;

        public TriggerScanData() { CreateEndPoint(); }
        //public TriggerScanData(
        //    double startYPos        = 49 ,
        //    double startXPos        = 49 ,
        //    double xStep_Size       = 22.6 ,
        //    double scan_Stage_Speed = 3,
        //    double camera_Exposure  = 2400,
        //    double camera_LineRate  = 400
        //    )
        //{
        //    XStep_Size       = xStep_Size ;
        //    Scan_Stage_Speed = scan_Stage_Speed;
        //    Camera_Exposure  = camera_Exposure;
        //    Camera_LineRate  = camera_LineRate;
        //
        //    CreateEndPoint();
        //}

        void CreateEndPoint() {
            StartYPos = new Dictionary<ScanConfig, double>();
            StartYPos.Add( ScanConfig.Trigger_1, 137 );
            StartYPos.Add( ScanConfig.Trigger_2, 157 );
            StartYPos.Add( ScanConfig.Trigger_4, 161 );

            StartXPos = new Dictionary<ScanConfig, double>();
            StartXPos.Add( ScanConfig.Trigger_1, 10 );
            StartXPos.Add( ScanConfig.Trigger_2, 70  );
            StartXPos.Add( ScanConfig.Trigger_4, 20  );

            EndYPos = new Dictionary<ScanConfig , double>();
            EndYPos.Add( ScanConfig.Trigger_1 , 112 );
            EndYPos.Add( ScanConfig.Trigger_2 , 69  );
            EndYPos.Add( ScanConfig.Trigger_4 , 60  );

        }
    }
}
