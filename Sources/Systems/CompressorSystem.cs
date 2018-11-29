using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Components.Enemies;
using Psychic.Components.Traps;
using Psychic.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
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
			if ( entity.GetComponent<SpriteAnimation> ().Animation.GetCurrentImage
				== Engine.SharedEngine.Content.Load<Texture2D> ( "Traps/Compressor/Compressor5" ) )
			{
				var transform = entity.GetComponent<Transform2D> ();
				var compressorPosition = transform.Position - new Vector2 ( 12, 12 ) + new Vector2 ( 0, 12.5f );

				var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
				if ( player != null )
				{
					var playerPosition = player.GetComponent<Transform2D> ().Position - new Vector2 ( 12, 12 );
					if ( playerPosition == compressorPosition )
					{
						( SceneManager.SharedManager.CurrentScene as GameScene ).DoGameOver ();
					}
				}

				foreach ( var enemy in EntityManager.SharedManager.GetEntitiesByComponent<Enemy> () )
				{
					var enemyPosition = enemy.GetComponent<Transform2D> ().Position - new Vector2 ( 12, 12 );
					if ( enemyPosition == compressorPosition )
					{
						var enemyComp = enemy.GetComponent<Enemy> ();
						enemyComp.IsControllingByPlayer = false;
						enemyComp.IsDead = true;

						var enemyAni = enemy.GetComponent<PsychicAnimation> ();
						enemyAni.CurrentAnimationStatus = CurrentAnimationStatus.Dead;

						enemy.GetComponent<Transform2D> ().Position += new Vector2 ( 0, 8 );
					}
				}
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
