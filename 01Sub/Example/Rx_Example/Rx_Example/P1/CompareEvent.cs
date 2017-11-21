using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace Rx_Example.P1
{
	class CompareEvent 
	{
	}

	public class JetEventArgs : EventArgs
	{
		public JetFighter_Ev JetFight { get; set; }

		public JetEventArgs( JetFighter_Ev jet )
		{
			JetFight = jet;
		}

	}

	public class JetFighter_Ev
	{
		public string Name;

		public event EventHandler<JetEventArgs> PlaneSpotted; 

		public void SpotPlane( JetFighter_Ev jetfighter )
		{
			EventHandler<JetEventArgs> eventHandler = this.PlaneSpotted; // PlaneSptted라는 이벤트를 넣어준다 .
			if ( eventHandler != null )
			{
				eventHandler( this , new JetEventArgs( jetfighter ) ); //start event
			}
		}
	}

	public class JetFight_Ob
	{
		public string Name;

		private Subject<JetFight_Ob> planeSpotted = new Subject<JetFight_Ob>();

		public IObservable<JetFight_Ob> PlaneSpotted
		{
			get { return this.planeSpotted.AsObservable();  }
		}

		//public void SplotPlane( JetFight_Ob jetFighter )
		//{
		//	this.planeSpotted.OnNext( jetFighter );
		//}

		public void SpotPlane( JetFight_Ob jetFighter )
		{
			try
			{
				if ( string.Equals( jetFighter.Name , "UFO" ) )
				{
					throw new Exception( "UFO Found" );
		
		}
				this.planeSpotted.OnNext( jetFighter );
			}
			catch ( Exception exception )
			{
				this.planeSpotted.OnError( exception );
			}
		}

		public void AllPlanesSpotted()
		{
			this.planeSpotted.OnCompleted();
		}


	}

	public class BomberControl : IDisposable
	{
		private JetFighter_Ev jetfighter;

		public BomberControl( JetFighter_Ev jetfighter )
		{
			jetfighter.PlaneSpotted += this.OnPlaneSpotted();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		private void OnPlaneSpotted( object sender , JetEventArgs e )
		{
			JetFighter_Ev spottedPlane = e.JetFight;
		}

	}



}
