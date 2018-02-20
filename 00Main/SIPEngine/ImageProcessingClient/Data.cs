using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingClient
{
    public static partial class ExtMain
    {
        public static WfInfo ToWfInfo
            (this IEnumerable<string> body)
        {
            var splited = body.ToArray();
            var res = new WfInfo()
            {
                WaferCreateTime = splited[0],
                WaferName = splited[1],
                RecipePath = splited[2],
                WaferFileCount = splited[3],
                WaferPath = splited[4],
                MakeaCopy = splited[5]
            };
            return res;
        }
    }

    public class WfInfo 
    {
        public string WaferCreateTime;
        public string WaferName;
        public string RecipePath;
        public string WaferFileCount;
        public string WaferPath;
        public string MakeaCopy;

        private string[] MemberList
        {
            get
            {
                return new string[]
                        {
                            WaferCreateTime,
                            WaferName,
                            RecipePath,
                            WaferFileCount,
                            WaferPath,
                            MakeaCopy
                        };
            }
        }
    }
}
