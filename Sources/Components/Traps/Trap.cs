using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Traps
{
	public class Trap : IComponent
	{
		public bool IsTrigged;

		public void Initialize ()
		{
			IsTrigged = false;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Trap )
			{
				IsTrigged = ( component as Trap ).IsTrigged;
			}
		}
	}
}
