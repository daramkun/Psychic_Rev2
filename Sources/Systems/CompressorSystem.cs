using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components.Traps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Systems
{
	public class CompressorSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Compressor> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{

		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
