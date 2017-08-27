using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialVision
{
    public static class ExtensionTool
    {
        public static List<System.Drawing.Rectangle> GetRectList(
           this double [ , , ] @this ,
           int hSize
           , int wSize )
        {
            int hlimit = @this.GetLength(0), wlimit = @this.GetLength(1);

            return Enumerable.Range( 0 , @this.GetLength( 0 ) )
                        .SelectMany( j => Enumerable.Range( 0 , @this.GetLength( 1 ) )
                                     , ( j , i ) => new System.Drawing.Rectangle(
                                         ( int )( @this [ j , i , 1 ] - wSize / 2 > 0 ? @this [ j , i , 1 ] - wSize / 2 : 0 )
                                         , ( int )( @this [ j , i , 0 ] - hSize / 2 > 0 ? @this [ j , i , 0 ] - hSize / 2 : 0 )
                                         , @this [ j , i , 1 ] + wSize / 2 <= wlimit ? ( int )( @this [ j , i , 1 ] + wSize / 2 ) : ( int )( wlimit - @this [ j , i , 1 ] )
                                         , @this [ j , i , 0 ] + hSize / 2 <= hlimit ? ( int )( @this [ j , i , 0 ] + hSize / 2 ) : ( int )( hlimit - @this [ j , i , 0 ] ) ) )
                        .ToList();
        }
    }
}
