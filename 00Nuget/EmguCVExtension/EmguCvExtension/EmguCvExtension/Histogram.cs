using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;

namespace EmguCvExtension
{
    public static class Histogram
    {
        public static DenseHistogram ShowHisto(
           this byte [ ] @this
           , int binsize
           , int min
           , int max )
        {
            int srcLen = @this.GetLength( 0 );
            var histsrc =  new Matrix<float>( 1 , srcLen )
                            .Act( mat =>
                                    mat.Data
                                       .Map( data =>
                                       {
                                           return Enumerable.Range( 0,srcLen )
                                              .Select( i => (float)@this[i] )
                                              .ToArray<float>();
                                       } )
                            );
            return new DenseHistogram( binsize , new RangeF( min , max ) )
                        .Act( densehist =>
                                densehist.Calculate(
                                    new Matrix<float> [ 1 ] { histsrc }
                                    , true
                                    , null
                                    ) );
        }

        public static DenseHistogram ShowHisto(
            this int [ ] @this
            , int binsize
            , int min
            , int max )
        {
            int srcLen = @this.GetLength( 0 );
            var histsrc =  new Matrix<float>( 1 , srcLen )
                            .Act( mat =>
                                    mat.Data
                                       .Map( data =>
                                       {
                                           return Enumerable.Range( 0,srcLen )
                                              .Select( i => (float)@this[i] )
                                              .ToArray<float>();
                                       } )
                            );
            return new DenseHistogram( binsize , new RangeF( min , max ) )
                        .Act( densehist =>
                                densehist.Calculate(
                                    new Matrix<float> [ 1 ] { histsrc }
                                    , true
                                    , null
                                    ) );
        }

        public static DenseHistogram ShowHisto(
            this double [ ] @this
            , int binsize
            , int min
            , int max )
        {
            int srcLen = @this.GetLength( 0 );
            var histsrc =  new Matrix<float>( 1 , srcLen );


            for ( int i = 0 ; i < srcLen ; i++ )
            {
                histsrc [ 0 , i ] = ( float )@this [ i ];
            }

            return new DenseHistogram( binsize , new RangeF( min , max ) )
                        .Act( densehist =>
                                densehist.Calculate(
                                    new Matrix<float> [ 1 ] { histsrc }
                                    , true
                                    , null
                                    ) );
        }

        public static DenseHistogram ShowHisto(
         this List<double> @this
         , int binsize
         , int min
         , int max )
        {
            int srcLen = @this.Count;
            var histsrc = new Matrix<float>(1, srcLen);


            for ( int i = 0 ; i < srcLen ; i++ )
            {
                histsrc [ 0 , i ] = ( float )@this [ i ];
            }

            return new DenseHistogram( binsize , new RangeF( min , max ) )
                        .Act( densehist =>
                                densehist.Calculate(
                                    new Matrix<float> [ 1 ] { histsrc }
                                    , true
                                    , null
                                    ) );
        }
    }
}
