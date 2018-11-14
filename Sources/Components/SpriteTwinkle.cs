using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components
{
	class SpriteTwinkle : IComponent
	{
		public double TwinkleInterval;
		public double ElapsedTime;

		public void Initialize ()
		{
			TwinkleInterval = 0.5;
			ElapsedTime = 0;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is SpriteTwinkle )
			{
				( component as SpriteTwinkle ).TwinkleInterval = TwinkleInterval;
				( component as SpriteTwinkle ).ElapsedTime = 0;
			}
		}
	}
}
