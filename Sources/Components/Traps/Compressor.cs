using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Traps
{
	public class Compressor : IComponent
	{
		public int Frame;
		public bool IsCompressing;
		public TimeSpan ElapsedTime;

		public void Initialize ()
		{
			Frame = 0;
			IsCompressing = true;
			ElapsedTime = new TimeSpan ();
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Compressor )
			{
				Frame = ( component as Compressor ).Frame;
				IsCompressing = ( component as Compressor ).IsCompressing;
				ElapsedTime = ( component as Compressor ).ElapsedTime;
			}
		}
	}
}
