using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components
{
	public class Tile : IComponent
	{
		public byte [,] TileData;

		public void Initialize ()
		{
			TileData = null;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Tile )
			{
				TileData = ( component as Tile ).TileData;
			}
		}
	}
}
