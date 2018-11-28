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
	public class TrapSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Trap> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var trap = entity.GetComponent<Trap> ();
			if ( trap.IsTrigged )
				return;

			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 5, 25, 10 );

			var playerTransform = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).FirstOrDefault ()?.GetComponent<Transform2D> ();
			if ( playerTransform != null )
			{
				Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );
				if ( boundingBox.Intersects ( playerBoundingBox ) )
				{
					trap.IsTrigged = true;
					entity.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Traps/Trap/Trap_Activated" );
					( SceneManager.SharedManager.CurrentScene as GameScene ).DoGameOver ();
				}
			}

			foreach ( var enemy in EntityManager.SharedManager.GetEntitiesByComponent<Enemy> () )
			{
				var enemyTransform = enemy.GetComponent<Transform2D> ();
				Rectangle enemyBoundingBox = new Rectangle ( ( int ) enemyTransform.Position.X - 12, ( int ) enemyTransform.Position.Y - 12, 25, 25 );
				if ( boundingBox.Intersects ( enemyBoundingBox ) )
				{
					trap.IsTrigged = true;
					entity.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Traps/Trap/Trap_Activated" );

					var enemyComp = enemy.GetComponent<Enemy> ();
					enemyComp.IsControllingByPlayer = false;
					enemyComp.IsDead = true;

					var enemyAni = enemy.GetComponent<PsychicAnimation> ();
					enemyAni.CurrentAnimationStatus = CurrentAnimationStatus.Dead;

					enemy.GetComponent<Transform2D> ().Position += new Vector2 ( 0, 8 );
				}
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
