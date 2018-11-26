using Daramee.Mint.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Enemies
{
	public class Enemy : IComponent
	{
		public bool IsRightViewing;
		public bool IsDead;
		public Vector2 OriginalPosition;
		public TimeSpan ElapsedTime;
		public bool IsControllingByPlayer;

		public void Initialize ()
		{
			IsRightViewing = true;
			ElapsedTime = new TimeSpan ();
			IsControllingByPlayer = false;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Enemy )
			{
				IsRightViewing = ( component as Enemy ).IsRightViewing;
				OriginalPosition = ( component as Enemy ).OriginalPosition;
			}
		}
	}
}
