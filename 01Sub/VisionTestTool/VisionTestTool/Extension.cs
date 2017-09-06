using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTestTool
{
    public static class Extension
    {
        public static TSrc [ , , ] PickChenel<TSrc>(
            this TSrc [ , , ] src 
            , int idx)
        {
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            int c = src.GetLength(2);

            TSrc[,,] output = new TSrc[w, h, 1];

            for ( int j = 0 ; j < w ; j++ )
            {
                for ( int i = 0 ; i < h ; i++ )
                {
                    output [ j , i , 0 ] = src [ j , i , idx ];
                }
            }
            return output;
        }

        public static TSrc [ , , ] GraytoBgr<TSrc>(
           this TSrc [ , , ] src
           , TSrc [ , , ] dst
           , int idx )
        {
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            for ( int j = 0 ; j < w ; j++ )
            {
                for ( int i = 0 ; i < h ; i++ )
                {
                    dst [j , i , idx ] = src [ j , i , 0 ];
                }
            }
            return dst;
        }

    }
}
