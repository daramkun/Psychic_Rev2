using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Items
{
	public class MachineGun : IComponent
	{
		public bool IsRight;

		public void Initialize ()
		{
			IsRight = false;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is MachineGun )
			{
				IsRight = ( component as MachineGun ).IsRight;
			}
		}
	}
}
