using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using Emgu.CV;
using Emgu.CV.Structure;
using IPSAnalysis;
using static IPSAnalysis.Handler;
using SpeedyCoding;
namespace ThicknessAndComposition_Inspector_IPS
{
	using static ThicknessAndComposition_Inspector_IPS_Core.Core_Helper;
	using static ModelLib.AmplifiedType.Handler;
	using static IPSDataHandler.Handler;
	using ModelLib.AmplifiedType;
	using ModelLib.Data;

	public enum MsgType { Add , Remove , ChangeWav }

	/// <summary>
	/// Interaction logic for UC_AnalysisMap.xaml
	/// Draw Button Automatically Create Button Tag and Event.
	/// When Button is Clicked. Clicked event with index number go to Parent Window.
	/// If parent Window recieve event, All state updated automatically 
	/// and Ui update automatically ooccur
	/// </summary>
	public partial class UC_AnalysisMap : UserControl
	{
		public event Action<string,MsgType> evtClickedIndex;

		public UC_AnalysisMap()
		{
			InitializeComponent();
		}

		public void SetImage(BitmapSource src) // done 
		{
			imgMap.ImageSource = src;
		}

		public void SetBtnTag( IPSResult result ) // done
		{

			Just( result )
				.Map( CalcTagPos )
				.ForEach( DrawBtnTag );
			var poslist = CalcTagPos(result);

		}

		private void DrawBtnTag( List<ValPosCrt> tagPos) // done
		{
			int posNum = tagPos.Count;

			StackPanel[] temp = new StackPanel[ posNum ];
			Button[] btn = new Button[posNum];

			for ( int i = 0 ; i < posNum ; i++ )
			{
				var btntemp = CheckButton(i);
				Canvas.SetLeft( btntemp , tagPos [ i ].X );
				Canvas.SetTop( btntemp , tagPos [ i ].Y );
				cvsMap.Children.Add( btntemp );
				btn [ i ] = btntemp;
			}
		}

		private Button CheckButton( int i ) // done 
		{
			var btn = new Button();
			btn.Name = i.ToString();
			btn.Width = 50;
			btn.Height = 50;
			btn.Click += ClickIdx;
			return btn;
		}

		public void ClickIdx( object sender , RoutedEventArgs e ) // done
		{
			if ( Keyboard.IsKeyDown( Key.LeftCtrl ) )
				evtClickedIndex( ( sender as Button ).Name , MsgType.Remove);
			else
				evtClickedIndex( ( sender as Button ).Name , MsgType.Add);
		}


		private List<ValPosCrt> CalcTagPos( IPSResult result ) // done
		{
			var w0 = 30;
			var h0 = 30;
			var w1 = this.ActualWidth;
			var h1 = this.ActualHeight;

			var RealToCanvas = FnReScale( w0 , h0 , w1 , h1, w1/2 , h1/2);

			Func<CrtnCrd , ValPosCrt> toValPos
				= pos => RealToCanvas( ValPosCrt(pos.X, pos.Y) );

			var scaledPosList = result.SpotDataList.Map(x => x.CrtPos)
												   .Map(toValPos)
												   .ToList();
			return scaledPosList;
		}
	}
}
