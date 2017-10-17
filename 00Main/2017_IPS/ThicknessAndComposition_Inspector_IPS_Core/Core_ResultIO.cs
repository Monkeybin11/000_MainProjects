﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineLib.DeviceLib;
using ThicknessAndComposition_Inspector_IPS_Data;
using ApplicationUtilTool.FileIO;
using System.IO;
using ModelLib.TypeClass;
using ModelLib.ClassInstance;
using ApplicationUtilTool.Log;
using System.Windows;
using SpeedyCoding;
using System.Threading;
using ModelLib.Data;
using System.Reflection;
using System.Windows.Input;
using Emgu.CV;
using Emgu.CV.Structure;
using EmguCvExtension;
using System.Diagnostics;

namespace ThicknessAndComposition_Inspector_IPS_Core
{
	public partial class IPSCore
	{
		#region Trans
		public IPSResult ToResult( List<PlrCrd> pos , List<double> thckness , List<double [ ]> intens , List<double [ ]> refelctivityes , List<double> wavelen )
		{
			var res = new IPSResult(wavelen );
			for ( int i = 0 ; i < pos.Count ; i++ )
			{
				res.SpotDataList.Add( new SpotData( pos [ i ] , thckness [ i ] , intens [ i ] , refelctivityes [i]) );
			}
			return res;
		}
		#endregion


		#region Visualize
		public void DrawTest()
		{
			CreateMap( ResultData , 6 )
					.Act( x => ImgScanResult = x.Item1 [ 0 ] )
					.Act( x => Imgscalebar = x.Item1 [ 1 ] )
					.Act( x => EstedThickness = x.Item2 );

		}

		public Image<Bgr , byte> [ ] CreateMap2( IPSResult src , int divide )
		{
			var imgshiftoffset = 5;
			var offset = src.SpotDataList.Select( x => x.PlrPos.Rho/10).Max();
			var thlist = src.SpotDataList.Select( x => x.Thickness);
			var min = thlist.Min();
			var max = thlist.Max();

			var cm = new ColorMap().RB_cm;
			var xyCm1 = src.Result2TRThArr().OrderBy( x => x[1]).Select( x =>x).ToList();
			var xyCm2 = xyCm1            .Interpol_Theta(divide).OrderBy( x => x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();
			var xyCm3 = xyCm2             .Interpol_Rho(divide).OrderBy( x => x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();
			var xyCm4 = xyCm3             .ToCartesianReslt().OrderBy( x=> x[0]).ThenBy( x => x[1]).Select( x =>x).ToList();

			return null;
		}

		public Tuple<Image<Bgr , byte> [ ],int[]> CreateMap( IPSResult src , int divide )
		{

			int dotSize = 4;
			var sizemultiflier = 8;
			var scalebarTask = new Task<Image<Bgr,byte>>(
									() => CreateScalebar())
								.Act( x => x.Start());

			var imgshiftoffset = 10;
			var offset = src.SpotDataList.Select( x => x.PlrPos.Rho/10.0).Max();
			var thlist = src.SpotDataList.Select( x => x.Thickness);
			var min = thlist.Min();
			var max = thlist.Max();

			var cm = new ColorMap().RB_cm;

			var srcdatas = src.Result2TRThArr().Select( x =>x).ToList();

			for ( int i = 0 ; i < divide ; i++ )
			{
				srcdatas = srcdatas.Interpol_Theta( 1 ).Select( x => x ).ToList()
								   .Interpol_Rho( 1 ).Select( x => x ).ToList();
			}

			var xyCm = srcdatas.ToCartesianReslt()
							.OrderBy( x=> x[0])
							.ThenBy( x => x[1])
							.AsParallel()
							.AsOrdered()
							.Select( x => new
										{
											X  = offset + x[0] ,
											Y  = offset + x[1] ,
											Cm = (min - max) == 0 
													? cm[127]  //color double[r,g,b]
													: cm[ (int)(( x[2] -min )/(max - min)*255) ] ,
											Gry = (int)(( x[2] -min )/(max - min)*255 + 1)
							}).ToList();

			var imgsize = Math.Max(
				 xyCm.Select( x => x.X).Max(),
				 xyCm.Select( x => x.Y).Max()
				);

			var imgData = new byte[(int)(imgsize*sizemultiflier + imgshiftoffset*2.0) ,(int)(imgsize*sizemultiflier+imgshiftoffset*2.0),3];
			//var grayimgData = new byte[(int)(imgsize*sizemultiflier + imgshiftoffset*2.0) ,(int)(imgsize*sizemultiflier+imgshiftoffset*2.0),1];
			Image<Bgr,byte> img = new Image<Bgr, byte>(imgData);
			//Image<Gray,byte> grayimg = new Image<Gray, byte>(grayimgData);

			var circleLst = xyCm.Select(x =>
									new
									{
										pos = new System.Drawing.Point(
																			(int)(x.X*sizemultiflier)+imgshiftoffset,
																			(int)(x.Y*sizemultiflier)+imgshiftoffset),

										color = new MCvScalar(x.Cm[2]*255 , x.Cm[1]*255 , x.Cm[0]*255)
									});



			circleLst.ActLoop( x => CvInvoke.Circle( img , x.pos , dotSize , x.color , -1 , Emgu.CV.CvEnum.LineType.EightConnected ) );
			//circleLst.ActLoop( (x,i) => CvInvoke.Circle( grayimg , x.pos , dotSize , new MCvScalar( xyCm[i].Gry) , -1 , Emgu.CV.CvEnum.LineType.EightConnected ) );

			img = img.Median( 5 );
			img = img.SmoothGaussian( 3 );

			//grayimg = grayimg.Median( 5 );
			//grayimg = grayimg.SmoothGaussian( 3 );
			//grayimg.Data.Flatten();




			return Tuple.Create(
					 new Image<Bgr , byte> [ ] { img , scalebarTask.Result } ,
					 new int [ ] { } );
					 //grayimg.Data.Flatten().Select(x => (int)x).ToArray() );
		}

		public System.Windows.Media.Imaging.BitmapSource GetImage()
		{
			return new Image<Bgr , byte>( @"C:\Temp\testImg052538.bmp" ).ToBitmapSource();
		}

		public System.Windows.Media.Imaging.BitmapSource GetImage2()
		{
			return new Image<Bgr , byte>( @"C:\Temp\testImg051743.bmp" ).ToBitmapSource();
		}

		public Image<Bgr , byte> CreateScalebar()
		{

			var scalebar = new byte[255,2,3];
			var cm = new ColorMap().RB_cm;
			for ( int i = 0 ; i < 255 ; i++ )
			{
				scalebar [ i , 0 , 0 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
				scalebar [ i , 0 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 0 , 2 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
				scalebar [ i , 1 , 0 ] = ( byte )( cm [ i ] [ 0 ] * 255 );
				scalebar [ i , 1 , 1 ] = ( byte )( cm [ i ] [ 1 ] * 255 );
				scalebar [ i , 1 , 2 ] = ( byte )( cm [ i ] [ 2 ] * 255 );
			}
			return new Image<Bgr , byte>( scalebar );
		}

		#endregion

		#region IO

		public void SaveResult( string path , IPSResult result )
		{
			StringBuilder stb = new StringBuilder();
			//stb.Append( "Theta" + "," + "Rho" + "," + "Thickness" );
			stb.Append( "X" + "," + "Y" + "," + "Thickness" );
			stb.Append( Environment.NewLine );
			result.SpotDataList.ForEach( x =>
			{
				stb.Append( x.CrtPos.X.ToString("N2") + "," +  x.CrtPos.Y.ToString("N2") );
				//stb.Append( x.PlrPos.Theta.ToString() + "," +  x.PlrPos.Rho.ToString() );
				stb.Append( "," + x.Thickness );
				stb.Append( Environment.NewLine);
			} );
			File.WriteAllText( path , stb.ToString() );
		}

		public void SaveRaw( 
			string path , 
			IPSResult result , 
			List<double> wavelen ,
			List<double> dark ,
			List<double> refer , 
			List<double> absreflect )
		{
			StringBuilder stb = new StringBuilder();
			
			stb.Append( "WaveLength,Dark,Referance,Abs_Reflectivity" );
			
			result.SpotDataList.ForEach( x =>
			{
				stb.Append( "," + x.CrtPos.X.ToString("N2") + " " + x.CrtPos.Y.ToString("N2") );
				
				//stb.Append( "," + x.PlrPos.Theta.ToString() + " " + x.PlrPos.Rho.ToString() + ",Reflectivity" );
			} );
			stb.Append(Environment.NewLine);

			for ( int i = 0 ; i < dark.Count ; i++ )
			{
				stb.Append(
					wavelen[i].ToString() + "," +
					dark[i].ToString() + "," +
					refer[i].ToString() + "," +
					absreflect[i].ToString()  
					);
				result.SpotDataList.ForEach( x =>
				{
					stb.Append( "," + x.IntenList[i] );
					
				} );
				stb.Append( Environment.NewLine );
			}

			File.WriteAllText( path , stb.ToString() );
		}

		public void SaveRawReflectivity(
		string path ,
		IPSResult result,
		List<double> wavelen )
		{
			StringBuilder stb_raw = new StringBuilder();
			stb_raw.Append( "WaveLength" );
			result.SpotDataList.ForEach( x =>
			{
				stb_raw.Append( "," + x.CrtPos.X.ToString( "N2" ) + " " + x.CrtPos.Y.ToString( "N2" ) );
			} );
			stb_raw.Append( Environment.NewLine );

			for ( int i = 0 ; i < wavelen.Count ; i++ )
			{
				stb_raw.Append( wavelen [ i ].ToString() );
				result.SpotDataList.ForEach( x =>
				{
					stb_raw.Append( "," + x.Refelctivityes [ i ] );
				} );
				stb_raw.Append( Environment.NewLine );
			}

			File.WriteAllText( path , stb_raw.ToString() );
		}

		public void SaveImage(string path )
			=> ImgScanResult.Save( path );

		public void LoadConfig( string path )
		{
			Config = XmlTool.ReadXmlClas(
						Config ,
						Path.GetDirectoryName( path ) ,
						Path.GetFileName( path ) );
		}

		public void SaveConfig( string path )
		{
			XmlTool.WriteXmlClass(
			   Config ,
			   Path.GetDirectoryName( path ) ,
			   Path.GetFileName( path ) );
		}

		public void SavePrcConfig( string path )
		{
			XmlTool.WriteXmlClass(
					 Config ,
					 Path.GetDirectoryName( path ) ,
					 Path.GetFileName( path ) );
		}


		#endregion
	}
}