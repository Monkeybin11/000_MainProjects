using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIP_InspectLib.DataType
{
	using Gradient = Double;
	using Bias = Double;
	using IndexPos = Double;
	using i = Int32;
	using j = Int32;
	using ModelLib.AmplifiedType;

	using static ModelLib.AmplifiedType.Handler;

	class DataFormatLIb
	{
	}

	public static class Handler
	{
		public static LineEQ GB( double g , double b ) => new LineEQ( g , b );

		public static Maybe<Indexji> Indexji( int j , int i ) => Just( new Indexji( j , i ) );

	}
	#region For Line Equation 
	public class LineEQ
	{
		public Gradient Gradient;
		public Bias Bias;
		public bool IsHorizontal;

		public LineEQ( double g , double b , bool ishorizontal = true )
		{
			Gradient = g;
			Bias = b;
			IsHorizontal = ishorizontal;
		}
	}

	public class PosLineEq
	{
		public IndexPos[,,] IndexPos;
		public List<LineEQ> HLineEQs;
		public List<LineEQ> VLineEQs;

		public PosLineEq( IndexPos [ , , ] indexPos , List<LineEQ> hLineEQs , List<LineEQ> vLineEQs )
		{
			IndexPos = indexPos;
			HLineEQs = hLineEQs;
			VLineEQs = vLineEQs;
		}
	}
	#endregion

	public class Indexji
	{
		public j j;
		public i i;

		public Indexji( int j , int i )
		{
			this.j = j;
			this.i = i;
		}
	}

}
