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
				.Lift( CalcTagPos )
				.ForEach( DrawBtnTag );
			var poslist = CalcTagPos(result);

		}


		private void DrawBtnTag( List<ValPosCrt> tagPos ) // done
		{
			cvsMap.Children.Clear();
			int posNum = tagPos.Count;

			StackPanel[] temp = new StackPanel[ posNum ];
			Button[] btn = new Button[posNum];

			for ( int i = 0 ; i < posNum ; i++ )
			{
				var btntemp = CheckButton(i);
				Canvas.SetLeft( btntemp , tagPos [ i ].X - btntemp.Width/2 );
				Canvas.SetTop( btntemp , tagPos [ i ].Y - btntemp.Height/2 );
				cvsMap.Children.Add( btntemp );
				btn [ i ] = btntemp;
			}
		}

		private Button CheckButton( int i ) // done 
		{
			var btn = new Button();
			btn.Name = "btn" + i.ToString();
			btn.Width = 10;
			btn.Height = 10;
			btn.Opacity = 0.9;
			btn.Background = Brushes.LawnGreen; 
			btn.Click += ClickIdx;
			return btn;
		}

		public void ClickIdx( object sender , RoutedEventArgs e ) // done
		{
			try
			{
				var self = sender as Button;
				if ( Keyboard.IsKeyDown( Key.LeftCtrl ) ) // Remove with ctrl
				{
					self.Background = Brushes.LawnGreen;
					evtClickedIndex( ( sender as Button ).Name.Replace( "btn" , "" ) , MsgType.Remove );
				}
				else
				{
					self.Background = Brushes.OrangeRed;
					evtClickedIndex( ( sender as Button ).Name.Replace( "btn" , "" ) , MsgType.Add );
				}
					
			}
			catch ( Exception )
			{ }
		
		}


		private List<ValPosCrt> CalcTagPos( IPSResult result ) // done
		{
			var w0 = 300;
			var h0 = 300;
			var w1 = this.ActualWidth - 60;
			var h1 = this.ActualHeight - 60;

			var w2 = this.Width;
			var h2 = this.Height;



			var RealToCanvas = FnReScale( w0 , h0 , w1 , h1, w1/2+10 , h1/2+10);

			Func<CrtnCrd , ValPosCrt> toValPos
				= pos => RealToCanvas( ValPosCrt(pos.X, pos.Y) );

			var scaledPosList = result.SpotDataList.Map(x => x.CrtPos)
												   .Map(toValPos)
												   .ToList();
			return scaledPosList;
		}
	}
}
