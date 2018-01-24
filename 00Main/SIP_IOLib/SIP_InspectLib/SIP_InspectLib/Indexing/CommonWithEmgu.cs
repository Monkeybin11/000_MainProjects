using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_InspectLib.Indexing
{
	public static partial class Common
	{
		public static Func<Image<Gray , byte> , VectorOfVectorOfPoint> FnFindContour( double areaUP , double areaDW )
		{
			var findpasscntr = new Func<Image<Gray , byte> , VectorOfVectorOfPoint>((imgori) => {

                imgori.Save(@"D:\temp\test2.png");

				VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
				VectorOfVectorOfPoint passcontours = new VectorOfVectorOfPoint();
				CvInvoke.FindContours( imgori , contours , null , RetrType.External , ChainApproxMethod.ChainApproxNone );
                var c = contours.Size;
				for ( int i = 0 ; i < contours.Size ; i++ )
				{
					double areaSize = CvInvoke.ContourArea(contours[i]);
					if ( areaSize >= areaDW && areaSize <= areaUP )
					{
						passcontours.Push( contours[i] );
					}
				}
                var temp = passcontours.Size;
                return passcontours;
			} );
			return findpasscntr;
		}


		public static Func<VectorOfVectorOfPoint , VectorOfVectorOfPoint> SortContour 
			=> (inputContours)
			=>  new VectorOfVectorOfPoint( inputContours
												.ToArrayOfArray()
												.OrderBy( p => p [ 0 ].Y )
												.ThenBy( p => p [ 0 ].X )
												.ToArray());


		public static Func<VectorOfVectorOfPoint , List<Rectangle>> ApplyBox
			=> contrs
			=>
			{
				List<System.Drawing.Rectangle> PassBoxArr = new List<System.Drawing.Rectangle>();
				for ( int i = 0 ; i < contrs.Size ; i++ )
				{
					System.Drawing.Rectangle rc = CvInvoke.BoundingRectangle(contrs[i]);
					PassBoxArr.Add( rc );
				}
				return PassBoxArr;
			}; 
	}
}
