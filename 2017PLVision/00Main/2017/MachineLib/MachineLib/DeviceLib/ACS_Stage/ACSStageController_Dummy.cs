using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using SpeedyCoding;
using SpeedyCoding;
using static SpeedyCoding.SpeedyCoding_Reflection;

namespace MachineLib.DeviceLib.ACS_Stage
{
    public class ACSStageController_Dummy : IACSStageController
    {
        public string Address
        {
            get
            {
                return "Address";
            }
        }

        public Maybe<IACSStageController> Operator { get; set; }

        public Maybe<IACSStageController> Connect( string connectPath )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public double CurrentPosition( string axis , double pos )
        {
            GetCurrentMethod().Print();
            return 0;
        }

        public Maybe<IACSStageController> MoveAbs( string axis , double pos )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> MoveRel( string axis , double pos )
        {
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> Origin( string axis )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> SetSpeed( double speed )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> StartTrigger( int buffnum )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> StopTrigger( int buffnum )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> TurnOnOff( string axis , bool onSwitch )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }

        public Maybe<IACSStageController> WaitInPos( string axis , double targetPos )
        {
            GetCurrentMethod().Print();
            return this.Delay50().ToMaybe<IACSStageController>();
        }
    }
}
