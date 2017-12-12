﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGBoost;

namespace Fitting_Core
{
	using static XGBoost.BaseXgbModel;
	public enum LabelType { IPS, KLA }

	public static class Core_Fitting
	{
		public static XGBRegressor Regr;
		static List<IpsDataSet> LoadedDatas;


		public static void Reset()
		{
			Regr = new XGBRegressor();
			LoadedDatas = new List<IpsDataSet>();
		}
		//  나중에는 세팅 , 데이터 , 로드 여부 순으로 모델 만들기
		public static XGBRegressor CreateModel( List<IpsDataSet> datas )
		{
			LoadedDatas = datas;
			Regr = new XGBRegressor();
			Regr.Fit( GetReflectivity( datas ) , GetKlaThickness( datas ) );
			return Regr;
		}

		public static XGBRegressor LoadModel( string path )
		{
			Regr = LoadRegressorFromFile( path );
			return Regr;
		}


		public static Action<string> SaveModel
			=> ( path )
			=> Regr.SaveModelToFile( path + ".model" );
		
		public static double CalcMSE( XGBRegressor regr)
		{
			var target = GetKlaThickness(LoadedDatas);
			var pred = regr.Predict( GetReflectivity(LoadedDatas) );
			return MSE( pred , target );
		}
		
		public static XGBRegressor UpdateModel( List<IpsDataSet> datas)
		{
			Regr.Fit( GetReflectivity(datas) , GetKlaThickness(datas) );
			return Regr;
		}

		private static Func<List<IpsDataSet> , float [ ]> GetKlaThickness
			=> src
			=> src.Select( x => x.KlaThickness.AsEnumerable() )
				  .Aggregate( ( f , s ) => f.Concat( s ) )
				  .ToArray();

		private static Func<List<IpsDataSet> , float [ ] [ ]> GetReflectivity
			=> src
			=> src.Select( x => x.RfltList.AsEnumerable() )
				  .Aggregate( ( f , s ) => f.Concat( s ) )
				  .ToArray();


		private static Func<float [ ] , float [ ] , double> MSE
			=> ( target , pred )
			=> Math.Sqrt( target.Select( ( x , i ) => ( double )Math.Pow( ( x - pred [ i ] ) , 2 ) ).Sum() / target.Length );
	}
}
