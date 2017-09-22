using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace PLImg_V2
{
    public class CamParm
    {
        private static CamParm Instance;

        public CamParm()
        {
            if ( Instance == null )
            {
                Instance = new CamParm();
            }
        }

        public CamParm GetInstance()
        {
            object key = new object();
            if( Instance == null)
            {
                Instance = new CamParm();
            }
            return Instance;
        }

    }
}
