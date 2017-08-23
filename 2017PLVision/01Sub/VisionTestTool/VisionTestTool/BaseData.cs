using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace VisionTestTool
{
    public class BaseData
    {
        public bool IsBgr;

        private List<Image<Gray,byte>> HistoryImgGray;
        public List<Image<Bgr,byte>>   HistoryImgBgr;
        public Image<Bgr,byte>         WorkingImgColor;
        public Image<Bgr,byte>         RootColor;

        // group Modify
        public List<String> GroupModifyPath;

        public BaseData(bool isbgr)
        {
            IsBgr = isbgr;
            HistoryImgGray = new List<Image<Gray , byte>>() ;
            HistoryImgBgr   = new List<Image<Bgr, byte>>();
            GroupModifyPath = new List<string>();
        }

    }
}
