using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLImagingMachine_Data
{
    public class ScanImgResult<Timg>
    {
        public List<Timg> RawImgList;



        public ScanImgResult()
        {
            RawImgList = new List<Timg>();
        }

    }
}
