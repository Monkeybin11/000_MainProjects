using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeedyCoding;

namespace VisionTool
{
    public static class ChipUtil
    {
        #region Position
        public static Func<double , double , double [ , , ]> FnEstChipPos_2Point( double [ ] pos_LT , double [ ] pos_RB )
        {
            var createEsted = new Func<double , double , double[,,]>( (double hChipN,double wChipN) => {
                double[,,] output = new double[(int)hChipN , (int)wChipN,2];
                double realImgROIH = Math.Abs(pos_RB[0] - pos_LT[0]);
                double realImgROIW = Math.Abs(pos_RB[1] - pos_LT[1]);

                for (int j = 0; j < hChipN; j++)
                {
                    for (int i = 0; i < wChipN; i++)
                    {
                        output[j,i,0] = realImgROIH / (hChipN-1) * j + pos_LT[0];
                        output[j,i,1] = realImgROIW / (wChipN-1) * i + pos_LT[1];
                    }
                }
                return output;
            } );
            return createEsted;
        }

        public static Func<double , double , double [ , , ]> FnEstChipPos_4Point( double [ ] realLT , double [ ] realLB , double [ ] realRT , double [ ] realRB )
        {
            var createEsted = new Func<double , double , double[,,]>( (double hChipN,double wChipN) => {
                double[,,] output = new double[(int)hChipN , (int)wChipN,2];

                /* Avg of Gradient */
                /* Recalculate Bias with first chip position */

                /* X est model, Y fixed */
                double[] model_FH = Calc_YXAxis(realLT,realLB);
                double[] model_SH = Calc_YXAxis(realRT,realRB);

                /* Y est model, X fixed */
                double[] model_FW = Calc_XYAxis(realLT,realRT);
                double[] model_SW = Calc_XYAxis(realLB,realRB);

                /* Avg of Gradient */
                double[] model_H = new double[2] { (model_FH[0]+model_SH[0])/2 , 0};
                double[] model_W = new double[2] { (model_FW[0]+model_SW[0])/2 , 0};

                /* Recalculate Bias */
                model_H[1] = realLT[1] - model_H[0] * realLT[0];
                model_W[1] = realLT[0] - model_W[0] * realLT[1];

                double height_left  = realLB[0] - realLT[0] ;
                double height_right = realRB[0] - realRT[0] ;
                double width_top    = realRT[1] - realLT[1] ;
                double width_bot    = realRB[1] - realLB[1] ;

                double height = (height_left+height_right)/2;
                double width  = (width_top + width_bot)   /2;

                double hStep = height/(hChipN-1);
                double wStep = width_bot/(wChipN-1);

                for (int j = 0; j < hChipN; j++)
                {
                    for (int i = 0; i < wChipN; i++)
                    {
                        double xW = realLT[1] + i*wStep; // fixed X
                        double ested_Y  = xW *model_W[0] + model_W[1]; // Ested Y
                        output[j,i,0] = ested_Y;
                        output[j,i,1] = xW;
                    }
                    /*Update Bias*/
                    model_W[1] += hStep;
                }
                for (int i = 0; i < wChipN; i++)
                {
                    for (int j = 0; j < hChipN; j++)
                    {
                        double yH = realLT[0] + j*hStep; // fixed Y
                        double ested_X  = yH *model_H[0] + model_H[1]; // Ested X
                        output[j,i,0] = (output[j,i,0]+yH     )/2;
                        output[j,i,1] = (output[j,i,1]+ested_X)/2;
                    }
                    /*Update Bias*/
                    model_H[1] += wStep;
                }
                return output;
            } );
            return createEsted;
        }

		public static Func<double , double , double [ , , ]> FnEstChipPos_4PointP_rhombus( double [ ] realLT , double [ ] realLB , double [ ] realRT , double [ ] realRB )
		{
			var createEsted = new Func<double , double , double[,,]>( (double hChipN,double wChipN) =>
			{
				double[,,] output = new double[(int)hChipN , (int)wChipN,2];



				// wsplit num , hsplit num
				var leftSplitedY = realLT[0].xRange(
											(int)hChipN ,
											( realLB[0] - realLT[0])/hChipN);

				var rghtSplitedY = realRT[0].xRange(
											(int)hChipN ,
											( realRB[0] - realRT[0])/hChipN);

				var lEq = Calc_YXAxis(realLT , realLB);
				var rEq = Calc_YXAxis(realRT , realRB);

				// (y,x)
				var leftXY = leftSplitedY.Select( y => new double[] { y , lEq[0]*y + lEq[1] } ).ToList();
				var rghtXY = rghtSplitedY.Select( y => new double[] { y , rEq[0]*y + rEq[1] } ).ToList();



				// [ List(yl,xl) , List(yr,xr) ]
				var zippedSplited = leftXY.Zip(rghtXY , (f,s) => new { L = f , R = s } ).ToArray(); 

				// List of gradient of each singleline 
				var gradientList = zippedSplited.Select( x => Calc_XYAxis( x.L , x.R )).ToArray();


				int count = zippedSplited.Count();

				var res = zippedSplited.Select((crd,i) =>
				{
					double step = (crd.R[1] - crd.L[1])/count;

					var xlist = crd.L[1].xRange(count , step ).ToList();

					var ylist = xlist.Select( x => gradientList[i][0]* (i*step + zippedSplited[i].L[1]) + gradientList[i][1]).ToList();

					var singleLineZiped = ylist.Zip(xlist , (y,x) => new { Y = y , X = x } ).ToArray();

					return singleLineZiped;
				} ).ToList();
				//(int)hChipN , (int)wChipN,2];
				for (int j = 0; j < res.Count; j++)
				{
					for (int i = 0; i < res[j].Length; i++)
					{
						var x = res[j][i].X;
						var y = res[j][i].Y;
						output[i,j,0] = y;
						output[i,j,1] = x;
					}
				}
				return output;
			} );
			return createEsted;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="realLT"></param>
		/// <param name="realLB"></param>
		/// <param name="realRT"></param>
		/// <param name="realRB"></param>
		/// <param name="distanceFS">Distance of First and Second Chip Height</param>
		/// <returns></returns>
		public static Func<double , double , double [ , , ]> FnEstChipPos_4Point_Advanced( double [ ] realLT , double [ ] realLB , double [ ] realRT , double [ ] realRB , double [ ] distanceFS )
        {
            var createEsted = new Func<double , double , double[,,]>( (double hChipN,double wChipN) =>
            {
                double[,,] output = new double[(int)hChipN , (int)wChipN,2];
                #region FirstWidth

                var topwidth = realRT[1] - realLT[1];
                var botwidth = realRB[1] - realLB[1];

                var topstep = topwidth/(wChipN-1);
                var botstep = botwidth/(wChipN-1);

                double[][] topXY = new double[(int)wChipN][];
                double[][] botXY = new double[(int)wChipN][];

                for (int i = 0; i < (int)wChipN; i++)
                {
                    topXY[i] = new double[2];
                    botXY[i] = new double[2];
                }

                var modeltop = Calc_XYAxis(realLT , realRT);
                var modelbot = Calc_XYAxis(realLB , realRB);

                for (int i = 0; i < wChipN; i++)
                {
                    var xtop = realLT[1] + i*topstep;
                    var xbot = realLB[1] + i*botstep;
                    topXY[i][0] = Poly_1(modeltop,xtop);
                    botXY[i][0] = Poly_1(modelbot,xbot);
                    topXY[i][1] = xtop;
                    botXY[i][1] = xbot;
                }

                for (int i = 0; i < wChipN; i++)
                {
                    var modelH = Calc_YXAxis(topXY[i] , botXY[i]);

                    var height = (botXY[i][0] - topXY[i][0]);

                    var distancesum  =distanceFS.Sum();
                    var residual = height % distancesum;
                    var stepHNum = Math.Truncate(height / distancesum) * 2;

                    if (residual > distanceFS[0]) stepHNum ++;
                    
                    //test
                    if( hChipN != stepHNum + 1)
                    {
                        Console.WriteLine("Chip number and ested H number is not same");
                        return null;
                    }

                    double[] steplist = new double[(int)hChipN]; 
                    
                    // precalculate chip distance, it will be list of distance 
                    for (int j = 0; j < hChipN; j++)
                    {
                        var currentStep = j%2 == 0 ? distanceFS[1] : distanceFS[0];
                        if(j == 0 ) steplist[j] = 0;
                        else
                        {
                            steplist[j] = steplist[j-1] + currentStep;
                        }
                    }

                    for (int j = 0; j < hChipN; j++)
                    {
                        var y = topXY[i][0] + steplist[j];
                        output[j,i,0]= y ;
                        output[j,i,1]=Poly_1(modelH , y );
                    }
                }
            
                #endregion 

                // Y X 
                return output;
            } );
            return createEsted;
        }


        static double [ ] Calc_YXAxis( double [ ] first , double [ ] second )
        {
            double gradient = (second[1] - first[1])/(second[0] - first[0]);
            double biasf = first[1] - gradient * first[0];
            double biass = second[1] - gradient * second[0];
            return new double [ ] { gradient , ( biasf + biass ) / 2.0 };
        }
        static double [ ] Calc_XYAxis( double [ ] first , double [ ] second )
        {
            double gradient = (second[0] - first[0])/(second[1] - first[1]);
            double bias = first[0] - gradient * first[1];
            return new double [ ] { gradient , bias };
        }

        static double Poly_1( double [ ] model , double point )
        {
            return model [ 0 ] * point + model [ 1 ];
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="this">chip position data [ row number , column number , [Ypos , Xpos] ]</param>
        /// <param name="hSize">chip h number</param>
        /// <param name="wSize">chip w number</param>
        /// <param name="height">Image height</param>
        /// <param name="width">Image width</param>
        /// <returns></returns>
        public static List<System.Drawing.Rectangle> GetRectList(
          this double [ , , ] @this ,
          int hSize
          , int wSize
          , int height
          , int width
          )
        {
            int hlimit = @this.GetLength(0), wlimit = @this.GetLength(1);

            return Enumerable.Range( 0 , @this.GetLength( 0 ) )
                        .SelectMany( j => Enumerable.Range( 0 , @this.GetLength( 1 ) )
                                     , ( j , i ) => new System.Drawing.Rectangle(
                                         ( int )( @this [ j , i , 1 ] - wSize / 2 > 0 ? @this [ j , i , 1 ] - wSize / 2 : 0 )
                                         , ( int )( @this [ j , i , 0 ] - hSize / 2 > 0 ? @this [ j , i , 0 ] - hSize / 2 : 0 )
                                         , @this [ j , i , 1 ] + wSize / 2 <= width ? wSize : ( int )( width - @this [ j , i , 1 ] )
                                         , @this [ j , i , 0 ] + hSize / 2 <= height ? hSize : ( int )( height - @this [ j , i , 0 ] ) ) )
                        .ToList();
        }


        public static System.Drawing.Point [ ] GetMomnetList(
           this double [ , , ] @this )
        {
            return Enumerable.Range( 0 , @this.GetLength( 0 ) )
                        .SelectMany( j => Enumerable.Range( 0 , @this.GetLength( 1 ) )
                                     , ( j , i ) => new System.Drawing.Point( i , j ) )
                        .ToArray();
        }



        public static System.Drawing.Rectangle ExpendRect(
            this System.Drawing.Rectangle @this ,
            int margin
            )
        {
            return new System.Drawing.Rectangle(
                @this.X - margin
                , @this.Y - margin
                , @this.Width + margin * 2
                , @this.Height + margin * 2 );
        }

        public static System.Drawing.Rectangle ShurinkRect(
            this System.Drawing.Rectangle @this ,
            int margin
            )
        {
            return new System.Drawing.Rectangle(
                @this.X + margin
                , @this.Y + margin
                , @this.Width - margin * 2
                , @this.Height - margin * 2 );
        }

    }
}
