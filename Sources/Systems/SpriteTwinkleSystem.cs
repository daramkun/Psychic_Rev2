using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Systems
{
	class SpriteTwinkleSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public void PreExecute () { }
		public void PostExecute () { }

		public bool IsTarget ( Entity entity ) => entity.HasComponent<SpriteTwinkle> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var twinkler = entity.GetComponent<SpriteTwinkle> ();
			twinkler.ElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
			if ( twinkler.TwinkleInterval <= twinkler.ElapsedTime )
			{
				var spriteRender = entity.GetComponent<SpriteRender> ();
				spriteRender.IsVisible = !spriteRender.IsVisible;
				twinkler.ElapsedTime -= twinkler.TwinkleInterval;
			}
		}
	}
}
