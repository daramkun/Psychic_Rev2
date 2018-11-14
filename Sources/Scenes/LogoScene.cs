using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	public class LogoScene : Daramee.Mint.Scenes.LogoScene
	{
		public override string Name => "LogoScene";

		public LogoScene ()
			: base ( "Intro/Logo" )
		{

		}

		protected override void Enter ()
		{
			base.Enter ();
		}

		protected override void OnLogoDisplayEnded ()
		{
			SceneManager.SharedManager.Transition ( "IntroScene" );
		}
	}
}
