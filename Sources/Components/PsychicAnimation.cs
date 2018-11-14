using Daramee.Mint.Components;
using Daramee.Mint.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components
{
	public enum CurrentAnimationStatus
	{
		LeftStand, RightStand, LeftWalk, RightWalk, Dead
	}

	public class PsychicAnimation : IComponent
	{
		public CurrentAnimationStatus CurrentAnimationStatus;
		public Animation LeftStand, RightStand;
		public Animation LeftWalk, RightWalk;
		public Animation Dead;

		public void Initialize ()
		{
			CurrentAnimationStatus = CurrentAnimationStatus.RightStand;
			LeftStand = RightStand = LeftWalk = RightWalk = Dead = null;
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is PsychicAnimation )
			{
				CurrentAnimationStatus = ( component as PsychicAnimation ).CurrentAnimationStatus;
				LeftStand = ( component as PsychicAnimation ).LeftStand;
				RightStand = ( component as PsychicAnimation ).RightStand;
				LeftWalk = ( component as PsychicAnimation ).LeftWalk;
				RightWalk = ( component as PsychicAnimation ).RightWalk;
				Dead = ( component as PsychicAnimation ).Dead;
			}
		}
	}
}
