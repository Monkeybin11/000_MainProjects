using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using SpeedyCoding;
using ModelLib.AmplifiedType;
using SIPEngine.Recipe;
using Emgu.CV;
using Emgu.CV.Structure;
using SIP_InspectLib.Recipe;
using Emgu.CV.Util;
using SIP_InspectLib.DataType;


namespace PLMapping_SIPCore
{
    using static SIPEngine.Recipe.FunctionLib;
    using static System.IO.Path;
    using static ApplicationUtilTool.FileIO.XmlTool;
    using static ApplicationUtilTool.FileIO.CsvTool;
    using static PLMapping_SIPCore.Adaptor.IOWithServer;
    using static SIP_InspectLib.Recipe.Adaptor;
    using static SIPEngine.Handler;
    using static ModelLib.AmplifiedType.Handler;
    using static SIP_InspectLib.DefectInspect.Handler;
    using static SIP_InspectLib.Indexing.Common;

    using Img = Image<Gray, byte>;
    using ProcFunc = Func<Image<Gray, byte>, Image<Gray, byte>>;
    using System.Drawing;

    public class Core_PlMapping
	{
		public Maybe<List<ExResult>> Start( string imgpath , string procpath ,  string configpath )
		{

            var img = LoadImage( imgpath );
            Func<Rectangle,double> boxsum = FnSumInsideBox(img.Data);     //여기서 에버리지로 할건지, 합으로 할껀지 정할 수 있다. 

            var inspectrecipe = ToInspctRecipe(configpath);
            var poseq         = EstedChipPosAndEq(inspectrecipe);
            var esetedindex   = ToEstedIndex(poseq);

            var doproc  = RunProcessing.Apply(img);

            //var recp = ReadCsv2String(procpath , order0Dirction: false);


            var resimg = Just(procpath)
                                 .Bind(RemoveHeadTail)
                                 .Bind(ToFuncRecipeList)
                                 .Bind(ToPreProcFuncs)
                                 .Bind(Preprocess.Apply(img));

            if (!resimg.isJust) return None;
                            
           
            var resisp1 = ToBoxList(  inspectrecipe , resimg.Value);
            // 컨투어 찾고, 소팅후 박스로 (끝)


            var resGenerator =  ToExResult(inspectrecipe , boxsum , poseq , resisp1.Value );
            // 박스 리스트에 대해 대응되는 인덱싱 리스트 (끝) 여기까지 체크 완료. 

            var exresults = ResultInitializer(inspectrecipe) // 인덱싱 초기화만 되있음. 
                                .Map( resGenerator )
                                .Flatten()
                                .ToList(); // 끝 여기서 모든 결과를 만들었다. (끝)\

            //var counter = exresults.Where(x => x.OKNG == "OK").Count();

            return Just(exresults);
        }

        #region
        public Img LoadImage(string imgpath)
		 => new Image<Gray, byte>( imgpath );


        public static Func<Img, IEnumerable<Func<Img, Img>>, Maybe<Img>> Preprocess
            => (src, ModelLib)
            => Just(ModelLib.FoldL(src));


        public Func<string, InspctRescipe> ToInspctRecipe
           => path
           =>
           {
               var abspath = GetUNCPath( path );
               var name    = GetFileName(abspath);
               var dir     = GetDirectoryName(abspath);
               var recipe  = ReadXmlClas( default(InspctRescipe) , dir , name );
               return recipe;
           };



        #endregion
	}
}
