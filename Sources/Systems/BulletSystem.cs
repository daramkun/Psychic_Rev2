using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components.Items;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class BulletSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Bullet> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var bullet = entity.GetComponent<Bullet> ();
			bullet.Elapsed += gameTime.ElapsedGameTime;
			if ( bullet.Elapsed >= TimeSpan.FromSeconds ( 0.3 ) )
			{
				entity.GetComponent<Transform2D> ().Position += new Vector2 ( 12 * ( bullet.IsRight ? 1 : -1 ), 0 );
				++bullet.Movement;
				if ( bullet.Movement > 14 )
				{
					EntityManager.SharedManager.DestroyEntity ( entity );
					return;
				}
				bullet.Elapsed -= TimeSpan.FromSeconds ( 0.3 );
			}

			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				GameSceneParameter.HitPoint -= 3;
				if ( GameSceneParameter.HitPoint < 0 )
					GameSceneParameter.HitPoint = 0;
				EntityManager.SharedManager.DestroyEntity ( entity );
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
