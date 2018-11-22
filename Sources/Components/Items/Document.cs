using Daramee.Mint.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components.Items
{
	public class Document : IComponent
	{
		public int DocumentId;

		public void Initialize ()
		{
			DocumentId = 0;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Document )
			{
				DocumentId = ( component as Document ).DocumentId;
			}
		}
	}
}
