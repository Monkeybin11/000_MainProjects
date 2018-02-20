using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_InspectLib.DataType
{
	class DataofPLMapping
	{
	}

	public class ImgPResult
	{
		public int ChipTotalCount { get { return ChipPassCount + ChipLowCount + ChipOverCount + ChipNOPLCount; } }
		public int ChipPassCount = 0;
		public int ChipLowCount = 0;
		public int ChipOverCount = 0;
		public int ChipNOPLCount = 0;
		public int ChipTotalNgCount { get { return ChipLowCount + ChipOverCount + ChipNOPLCount; } }

		public int AreaUpLimit;
		public int AreaDwLimit;
		public int IntenUpLimit;
		public int IntenDwLimit;

		public List<ExResult> OutData = new System.Collections.Generic.List<ExResult>();
		public List<int> SizeHist = new System.Collections.Generic.List<int>();
		public List<int> ChipIntensityHist = new System.Collections.Generic.List<int>();

		public ImgPResult(
			int areaup ,
			int areadw ,
			int intenup ,
			int intendw )
		{
			AreaUpLimit = areaup;
			AreaDwLimit = areadw;
			IntenUpLimit = intenup;
			IntenDwLimit = intendw;
		}
	}
	public class ExResult
	{
        public static double PixelResolution;
		public int Hindex;
		public int Windex;

		//public int HidxPN => Hindex.TOPNPosY();
		//public int WidxPN => Windex.ToPNPosX();

		public int HindexError;
		public int WindexError;
		public string OKNG;
		public double Intensity;
		public double ContourSize;
		public System.Drawing.Rectangle BoxData;
		public System.Drawing.Point PositionError;

		public ExResult( int hindex , int windex )
		{
			Hindex = hindex;
			Windex = windex;
			HindexError = 0;
			WindexError = 0;
			OKNG = "NOPL";
			Intensity = 0;
			ContourSize = 0;
			BoxData = new Rectangle();
		}

		public ExResult(
			int hindex
			, int windex
			, int hindexError
			, int windexError
			, string passfail
			, double inten
			, double contsize
			, Rectangle boxData = new Rectangle() )
		{
			Hindex = hindex;
			Windex = windex;
			HindexError = hindexError;
			WindexError = windexError;
			OKNG = passfail;
			Intensity = inten;
			ContourSize = contsize;
			BoxData = boxData;
		}
	}

	public class ConstrainInfo_Playnitride
	{
		//public readonly double BoundaryLen = 8127; // B1
		//public readonly double BoundaryLen = 8141; // B2
		public readonly double BoundaryLen = 8500;  // G1

		public readonly Tuple<double,double> Center;

		public ConstrainInfo_Playnitride( double xoff , double yoff )
		{
			Center = Tuple.Create( xoff , yoff );
		}
	}

	public static class Playnitride_Ext
	{
		// B1
		//public static int XIDxOffset = 403;
		//public static int YIDxOffset = 700;

		// B2
		//public static int XIDxOffset = 402;
		//public static int YIDxOffset = 736;

		//G1
		public static int XIDxOffset = 495;
		public static int YIDxOffset = 700;

		public static int ToPNPosX( this int x )
			=> x - XIDxOffset;


		public static int TOPNPosY( this int y )
			=> YIDxOffset - y;
	}


}
