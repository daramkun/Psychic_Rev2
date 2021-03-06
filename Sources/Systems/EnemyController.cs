﻿using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components;
using Psychic.Components.Enemies;
using Psychic.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class EnemyController : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Enemy> ();

		Tile tile;

		public void PreExecute ()
		{
			tile = EntityManager.SharedManager.GetEntitiesByComponent<Tile> ()?.FirstOrDefault ()?.GetComponent<Tile> ();
		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var enemy = entity.GetComponent<Enemy> ();
			if ( enemy.IsControllingByPlayer ) return;

			var ani = entity.GetComponent<PsychicAnimation> ();
			if ( enemy.IsDead )
			{
				ani.CurrentAnimationStatus = CurrentAnimationStatus.Dead;
				return;
			}
			else
				ani.CurrentAnimationStatus = enemy.IsRightViewing
					? CurrentAnimationStatus.RightWalk
					: CurrentAnimationStatus.LeftWalk;

			var transform = entity.GetComponent<Transform2D> ();

			enemy.ElapsedTime += gameTime.ElapsedGameTime;
			if ( enemy.ElapsedTime > TimeSpan.FromSeconds ( 0.2 ) )
			{
				var offset = Math.Abs ( enemy.OriginalPosition.X - transform.Position.X ) / 25;
				if ( offset >= 3 )
					enemy.IsRightViewing = !enemy.IsRightViewing;

				var posScalar = ( transform.Position - new Vector2 ( 12 ) ) / 25;
				
				if ( enemy.IsRightViewing )
				{
					if ( tile.TileData [ ( int ) posScalar.Y + 1, ( int ) posScalar.X + 1 ] == 0 )
						enemy.IsRightViewing = !enemy.IsRightViewing;
					else if ( tile.TileData [ ( int ) posScalar.Y, ( int ) posScalar.X + 1 ] != 0 )
						enemy.IsRightViewing = !enemy.IsRightViewing;
					transform.Position.X += 12.5f;
				}
				else
				{
					if ( tile.TileData [ ( int ) posScalar.Y + 1, ( int ) posScalar.X - 1 ] == 0 )
						enemy.IsRightViewing = !enemy.IsRightViewing;
					else if ( tile.TileData [ ( int ) posScalar.Y, ( int ) posScalar.X - 1 ] != 0 )
						enemy.IsRightViewing = !enemy.IsRightViewing;
					transform.Position.X -= 12.5f;
				}

				enemy.ElapsedTime -= TimeSpan.FromSeconds ( 0.2 );
			}
		}

		public void PostExecute () { tile = null; }
	}
}
