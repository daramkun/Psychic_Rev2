﻿using Daramee.Mint;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Scenes;
using Psychic.Systems;
using System;

namespace Psychic
{
#if WINDOWS || LINUX
	/// <summary>
	/// The main class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			using ( var game = new Engine (
				screenSize: new Vector2 ( 176, 178 ),
				fullscreen: false,
				exclusiveFullscreen: false,
				firstSceneName: "GameScene",
				scenes: new Scene []
				{
					new Psychic.Scenes.LogoScene (),
					new IntroScene (),
					new MenuScene (),
					new OpeningScene (),
					new GameScene (),
					new EndingScene (),
				} ) )
			{
				game.IsFixedTimeStep = true;
				game.TargetElapsedTime = TimeSpan.FromTicks ( 333333 );
				game.FrameBufferClearColor = Color.Black;
				game.Initialized += ( sender, e ) =>
				{
					SystemManager.SharedManager.RegisterSystem ( new SpriteTwinkleSystem () );
					SystemManager.SharedManager.RegisterSystem ( new MessageSystem () );
					SystemManager.SharedManager.RegisterSystem ( new TileRenderSystem () );
					SystemManager.SharedManager.RegisterSystem ( new PsychicAnimationSystem () );
					SystemManager.SharedManager.RegisterSystem ( new DoorSystem () );
					SystemManager.SharedManager.RegisterSystem ( new KeySystem () );
					SystemManager.SharedManager.RegisterSystem ( new HelpSystem () );
					SystemManager.SharedManager.RegisterSystem ( new SaveSystem () );
					SystemManager.SharedManager.RegisterSystem ( new DocumentSystem () );

				};
				game.Run ();
			}
		}
	}
#endif
}
