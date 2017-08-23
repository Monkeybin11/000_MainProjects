using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using SpeedyCoding;
using MachineLib.DeviceLib.DalsaTDICamera;
using MachineLib.DeviceLib.ACS_Stage;

namespace PLImagingMachine_Core
{
    public class Core_Scan
    {
        IDalsaTDICam TdiCam;
        ACSStageController Stg;
        public Maybe<Core_Scan> ConnectHW(string camport , string stgport)
        {
           Stg.Connect( stgport )
                .Bind(  x => TdiCam.Connect( camport ) )
                .Else( () => TdiCam = new DalsaTDICam_Dummy() );

            //TdiCam.ToMaybe().Bind( x => x.)
            return this.ToMaybe();
        }

        

        public Maybe<Core_Scan> ScanStart( string camport , string stgport )
        {
            // clear full image 
            // goto ready pos 
            // 
            var temp = 4.ToMaybe();
            var tmep2 = new Nothing<int,FuncNameLog,string>();
            return this.ToMaybe();
        }



        // 스테이지와 카메라 엔진 완료 됬다. 이제 버튼 하나에 실행되는 동작 하나씩을 만들어야 한다. 

        // 접속 
        // 1,2 인치 스캔 
        // 스캔후 이미지 저장 
        // 이미지 리사이즈 후 합쳐서 최종본 보여주기 
        // 프로세싱 엔진 제작 
        // 저장되있는 이미지를 프로세싱 해서 결과 뿌리기 

        // 추후 : 줌기능() , 테스트 기능 ,알고리즘 테스트로 부터 생성 , 후 적용 
        // 줌기능은 좀더 생각좀 해보자 
        //
        



    }


}
