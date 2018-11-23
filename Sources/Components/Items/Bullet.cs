using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Items
{
	public class Bullet : IComponent
	{
		public int Movement;
		public TimeSpan Elapsed;
		public bool IsRight;

		public void Initialize ()
		{
			Movement = 0;
			Elapsed = new TimeSpan ();
		}

		public void CopyFrom ( IComponent component )
		{

		}
	}
}
