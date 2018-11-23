using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Items
{
	public class Sensor : IComponent
	{
		public bool IsActived;

		public void Initialize ()
		{
			IsActived = false;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Sensor )
			{
				IsActived = ( component as Sensor ).IsActived;
			}
		}
	}
}
