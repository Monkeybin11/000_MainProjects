using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using ModelLib.AmplifiedType;
using PLMapping_SIPCore;

namespace TestConsole
{
    using SIP_InspectLib.Recipe;
    using System.IO;
    using static ApplicationUtilTool.FileIO.XmlTool;

    class Program
	{
		static void Main( string [ ] args )
		{
            //ExportXmlInspect();
            var core = new Core_PlMapping();

            //var imgpath     =  @"\\192.168.10.10\임시폴더\s소재우\PLMappingTest\test3.png";
            var imgpath     =  @"\\192.168.10.10\임시폴더\s소재우\PLMappingTest\B-6CD0PPR.png";
            var modelpath   =  @"\\192.168.10.10\임시폴더\s소재우\PLMappingTest\model.csv";
            var inspectpath =  @"\\192.168.10.10\임시폴더\s소재우\PLMappingTest\test.xml";

            var res = core.Start( imgpath, modelpath, inspectpath );
		}

        public static void ExportXmlInspect()
        {
            string outpath = @"D:\temp\test.xml";
            var temp = new InspctRecipe()
            {
                 RhoLimit       = 0,
                 IntenLowLimt   = 2500,
                 IntenHighLimt  = 14000,
                 AreaLowLimt    = 900,
                 AreaHighLimt   = 2,
                 Tolerance      = 3,
                 HChipNum       = 562,
                 WChipNum       = 691,
                 realLT         = new double[] { 13.5051227678571,12.6714732142857  },
                 realLB         = new double[] { 14926.9954464286,10.3372544642857  },
                 realRT         = new double[] { 13.8385825892857,12142.3277678571  },
                 realRB         = new double[] { 14928.3292857143,12141.6608482143  },
                 NeedEdgrCut    = false,
                 EdgeLimit      = 0,
                 XoffSet        = 0,
                 YoffSet        = 0
             };
            WriteXmlClass( temp, Path.GetDirectoryName(outpath) , Path.GetFileName(outpath));
        }
	}
}
