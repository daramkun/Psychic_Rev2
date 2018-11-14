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
	public class PsychicAnimationSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => -1;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<PsychicAnimation> () && entity.HasComponent<SpriteAnimation> ();

		public void PreExecute ()
		{

		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var pa = entity.GetComponent<PsychicAnimation> ();
			var sa = entity.GetComponent<SpriteAnimation> ();
			switch ( pa.CurrentAnimationStatus )
			{
				case CurrentAnimationStatus.LeftStand: sa.Animation = pa.LeftStand; break;
				case CurrentAnimationStatus.RightStand: sa.Animation = pa.RightStand; break;
				case CurrentAnimationStatus.LeftWalk: sa.Animation = pa.LeftWalk; break;
				case CurrentAnimationStatus.RightWalk: sa.Animation = pa.RightWalk; break;
				case CurrentAnimationStatus.Dead: sa.Animation = pa.Dead; break;
				default: sa.Animation = null; break;
			}
		}

		public void PostExecute ()
		{

		}
	}
}
