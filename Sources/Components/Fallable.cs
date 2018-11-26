using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components
{
	public class Fallable : IComponent
	{
		public bool IsFalling;
		public bool DeadFlag;
		public TimeSpan ElapsedTime;

		public void Initialize ()
		{
			IsFalling = false;
			DeadFlag = false;
			ElapsedTime = new TimeSpan ();
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Fallable )
			{
				IsFalling = ( component as Fallable ).IsFalling;
				DeadFlag = ( component as Fallable ).DeadFlag;
			}
		}
	}
}
