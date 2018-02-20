using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;


namespace SIPEngine.Recipe
{
    using static FuncLib;
    using ModelLib.AmplifiedType;
    using static ModelLib.AmplifiedType.Handler;
    using static ProcFuncKeys;

    using Img = Emgu.CV.Image<Gray, byte>;
    using RecipeBody = IEnumerable<string>;
    using ProcRecipe = Tuple<string, string>;

    using ProcFuncs = Func<Image<Gray,byte>,Image<Gray,byte>>;


    public static class ProcFuncKeys
    {
        // 무조건 파라미터 갯수 1개로 

        public const string keyMedian       = "Median";
        public const string keyNormalize    = "Normalize";
        public const string keyThreshold    = "Threshold";
        public const string keyAdpThreshold = "AdpThreshold";
        public const string keyHistoEq      = "HistoEq";
        public const string keyGamma        = "Gamma";
        public const string keyBrightness   = "Brightness";
        public const string keyUnion        = "Union";
        public const string keyInverse      = "Inverse";
        public const string keyGBlur        = "GBlur";
        public const string keyBlur         = "Blur";
        public const string keyMorph        = "Morph_";
    }

    public static partial class Handler
    {

    }

    public static class FunctionLib
    {
        public const string StartStr = "ImgProStart";
        public const string EndStr = "ImgProEnd";

        /*
        public static Maybe<List<Func<Img, Img>>> GenerateProcFunc(string[] recipe)
        {
            List<Func<Img, Img>> funList = new List<Func<Img, Img>>();

            if (recipe.Last() != "End") return None;

            foreach (var item in recipe)
            {
                if (item == "End") return funList;

                var res = SelectProcFunc(item);

                if (res == null) return None;

                else funList.Add(res);
            }
            return None;
        }
        */

        // With Monad Stream

        //Entry Point

        public static void main(string src)
        {
            var res = Just(src)
                .Bind( RemoveHeadTail )
                .Bind( ToFuncRecipeList )
                .Bind( ToPreProcFuncs );
        }

        public static Maybe<RecipeBody> RemoveHeadTail(string rawrecipe)
        {
            var splited = rawrecipe.Split('|');
            var head = splited.First();
            var tail = splited.Last();
            var body = splited.Skip(1).Take(splited.Length - 2);

            if (head == StartStr && tail == EndStr)
                return Just(body);
            return None;
        }

        public static Maybe<List<ProcRecipe>> ToFuncRecipeList(IEnumerable<string> strlist)
        {
            Func<string, Maybe<string[]>> checkSplit =
                src =>
                {
                    var splited = src.Split(',');
                    int num;
                    return Int32.TryParse(splited.Last(), out num)
                            ? Just(splited)
                            : None;
                };

            List<ProcRecipe> result = new List<ProcRecipe>();

            foreach (var funcdef in strlist)
            {
                var res = Just(funcdef)
                             .Bind(checkSplit);

                if (!res.isJust) return None;
                result.Add( res.ToProcRecipe() );
            }
            return result;
        }

        public static Maybe<List<ProcFuncs>> ToPreProcFuncs(List<ProcRecipe> funparmlist)
        {
            List<ProcFuncs> fnlist = new List<ProcFuncs>();
            foreach (var fnrecipe in funparmlist)
            {
                var result = ConvertToPreProFunc(fnrecipe);
                if (!result.isJust) return None;
                fnlist.Add(result.Value);
            }
            return fnlist;
        }

        public static Maybe<ProcFuncs> ConvertToPreProFunc(ProcRecipe src)
        {
            var name = src.Item1;
            var strparm = src.Item2;
            int parameter;
            if (!Int32.TryParse(strparm, out parameter)) return None;

            switch (name)
            {
                case keyThreshold:
                    return Threshold.Apply(parameter);

                case keyNormalize:
                    return Normalize.Apply(parameter);

                case keyAdpThreshold:
                    return AdpThreshold.Apply(parameter);

                case keyMedian:
                    return Median.Apply(parameter);
                default:
                    return None;
            }
        }




        /// <summary>
        /// Insert String formatted [name , param]
        /// </summary>
        /// <param name="nameparam"></param>
        /// <returns></returns>
        //public static Func<Img, int, Img> SelectProcFunc(string nameparam)
        //{
        //    var name = nameparam.Split('|').First();
        //    var param = nameparam.Split('|').Last();
        //
        //    int parameter;
        //
        //    if (!int.TryParse(param, out parameter)) return null;
        //
        //    switch (name)
        //    {
        //        case "Threshold":
        //            return Threshold.Apply(parameter);
        //
        //        case "Normalize":
        //            return Normalize.Apply(parameter);
        //
        //        case "AdpThreshold":
        //            return AdpThreshold.Apply(parameter);
        //
        //        case "Median":
        //            return Median.Apply(parameter);
        //        default:
        //            return null;
        //    }
        //}
    }

    public static class FunctionLibExt
    {
        public static Tuple<string, string> ToProcRecipe(this Maybe<string[]> src)
            => Tuple.Create(src.Value[0], src.Value[1]);

    }






}
