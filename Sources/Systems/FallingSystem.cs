using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class FallingSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Fallable> ();

		Tile tile;

		public void PreExecute ()
		{
			tile = EntityManager.SharedManager.GetEntitiesByComponent<Tile> ().First ()?.GetComponent<Tile> ();
		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			if ( tile == null ) return;

			var fallable = entity.GetComponent<Fallable> ();
			var transform = entity.GetComponent<Transform2D> ();

			if ( fallable.DeadFlag )
				return;

			if ( !fallable.IsFalling )
			{
				var positionScalar = ( transform.Position - new Vector2 ( 12 ) ) / 25;
				if ( tile.TileData [ ( int ) positionScalar.Y + 1, ( int ) positionScalar.X ] == 0 )
					fallable.IsFalling = true;
				else
					return;
			}

			fallable.ElapsedTime += gameTime.ElapsedGameTime;
			if ( fallable.ElapsedTime > TimeSpan.FromSeconds ( 0.1 ) )
			{
				transform.Position += new Vector2 ( 0, 12.5f );
				if ( ( int ) transform.Position.Y >= 100 )
				{
					transform.Position.Y = 100;
					fallable.DeadFlag = true;
				}
				else
				{
					var positionScalar = ( transform.Position - new Vector2 ( 12 ) ) / 25;
					if ( tile.TileData [ ( int ) positionScalar.Y + 1, ( int ) positionScalar.X ] != 0 )
						fallable.IsFalling = false;
				}
				fallable.ElapsedTime -= TimeSpan.FromSeconds ( 0.1 );
			}
		}

		public void PostExecute ()
		{
			tile = null;
		}
	}
}
