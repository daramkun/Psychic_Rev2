using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Items
{
	public class Help : IComponent
	{
		public int Index;

		public void Initialize ()
		{
			Index = 0;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Help )
			{
				Index = ( component as Help ).Index;
			}
		}
	}
}
