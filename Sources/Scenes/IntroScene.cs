using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Input;
using Daramee.Mint.Processors;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Input;
using Psychic.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	class IntroScene : Scene, IProcessor
	{
		public override string Name => "IntroScene";

		protected override void Enter ()
		{
			var backEntity = EntityManager.SharedManager.CreateEntity ();
			backEntity.Name = "IntroBackground";
			backEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 176, 178 ) / 2;
			var sprite = backEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Intro/Intro" );

			var pakEntity = EntityManager.SharedManager.CreateEntity ();
			pakEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 176 / 2, 150 );
			pakEntity.AddComponent<SpriteTwinkle> ().TwinkleInterval = 0.5;
			sprite = pakEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Intro/PressAnyKey" );

			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( InputManager.AnyKeyInput )
			{
				SceneManager.SharedManager.Transition ( "MenuScene" );
			}
		}
	}
}
