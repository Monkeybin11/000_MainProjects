using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using SpeedyCoding;

namespace EmguCvExtension
{
    public static class Conversion
    {
        public static Image<TColor , TDepth> HStack<TColor, TDepth>(
          this Image<TColor , TDepth> src ,
          Image<TColor , TDepth> rightSrc )
          where TColor : struct, IColor
          where TDepth : new()
        {
            return new Image<TColor , TDepth>( src.Data.Concate_H( rightSrc.Data ) );
        }

        public static Image<TColor , TDepth> HStack<TColor, TDepth>(
            this Image<TColor , TDepth> src ,
            Image<TColor , TDepth> rightSrc ,
            int clipping )
            where TColor : struct, IColor
            where TDepth : new()
        {
            return new Image<TColor , TDepth>( src.Data.Concate_H( rightSrc.Data , clipping ) );
        }



        public static Image<TColor , TDepth> VStack<TColor, TDepth>(
             this Image<TColor , TDepth> src ,
             Image<TColor , TDepth> bottomSrc )
             where TColor : struct, IColor
             where TDepth : new()
        {
            return new Image<TColor , TDepth>( src.Data.Concate_V( bottomSrc.Data ) );
        }


        public static int [ ] [ ] [ ] Points2Arrays(
           this Point [ ] [ ] input )
        {
            return ( from rows in input
                     select ( from row in rows

                              select new int [ ] { row.X , row.Y } ).ToArray() ).ToArray();
        }

        public static Point [ ] [ ] Arrays2Points(
            this int [ ] [ ] [ ] input )
        {
            return ( from rows in input
                     select ( from row in rows
                              select new Point( row [ 0 ] , row [ 1 ] ) ).ToArray() ).ToArray();

        }

        public static byte [ , , ] Gray2Bgr(
            this byte [ , , ] graysrc
            )
        {
            return new Image<Gray , byte>( graysrc ).Convert<Bgr , byte>().Data;
        }

        public static byte [ , , ] Bgr2Gray(
           this byte [ , , ] graysrc
           )
        {
            return new Image<Bgr , byte>( graysrc ).Convert<Gray , byte>().Data;
        }

        public static byte [ ] [ ] ConvertToJagged(
            this Image<Gray , byte> @this )
        {
            var rowNum = @this.Height;
            var colNum = @this.Width;

            byte[][] output = new byte[rowNum][];

            for ( int j = 0 ; j < rowNum ; j++ )
            {
                for ( int i = 0 ; i < colNum ; i++ )
                {
                    output [ j ] = new byte [ ] { @this.Data [ j , i , 0 ] };
                }
            }
            return output;
        }

        public static byte [ , ] ConvertToMatrix(
            this Image<Gray , byte> @this )
        {
            var rowNum = @this.Height;
            var colNum = @this[0].Width;

            byte[,] output = new byte[rowNum,colNum];

            for ( int j = 0 ; j < rowNum ; j++ )
            {
                for ( int i = 0 ; i < colNum ; i++ )
                {
                    output [ j , i ] = @this.Data [ j , i , 0 ];
                }
            }
            return output;
        }

        public static byte [ , , ] ConvertToImgData(
            this byte [ ] [ ] @this )
        {
            var rowNum = @this.GetLength(0);
            var colNum = @this[0].GetLength(0);

            byte[,,] output = new byte[rowNum,colNum,1];

            for ( int j = 0 ; j < @this.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < @this [ 0 ].GetLength( 0 ) ; i++ )
                {
                    output [ j , i , 0 ] = @this [ j ] [ i ];
                }
            }
            return output;
        }



    }
}
