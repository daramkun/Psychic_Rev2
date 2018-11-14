using Daramee.Mint.Components;
using Daramee.Mint.Diagnostics;
using Daramee.Mint.Entities;
using Daramee.Mint.Graphics;
using Daramee.Mint.Processors;
using Daramee.Mint.Scenes;
using Microsoft.Xna.Framework;
using Psychic.Components;
using Psychic.Input;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	public class GameScene : Scene, IProcessor
	{
		byte [,] tileData;
		Entity cameraEntity;
		Entity lisaEntity;

		Vector2 playerPosition;
		bool movingStartLeft, movingStartRight;
		TimeSpan movingElapsed;
		bool movingFall;

		bool isGameOver = false;

		public override string Name => "GameScene";

		protected override void Enter ()
		{
			var tileEntity = EntityManager.SharedManager.CreateEntity ();
			tileEntity.AddComponent<Tile> ().TileData = tileData = StageTileInfo.GetStageData ( GameSceneParameter.Stage );

			int emptyTileY = 0;
			for ( ; emptyTileY < 5; ++emptyTileY )
				if ( tileEntity.GetComponent<Tile> ().TileData [ emptyTileY, 1 ] == 0 )
					break;
			playerPosition = new Vector2 ( 1, emptyTileY );

			cameraEntity = EntityManager.SharedManager.CreateEntity ();
			cameraEntity.AddComponent<Transform2D> ();
			cameraEntity.AddComponent<Camera> ();

			lisaEntity = EntityManager.SharedManager.CreateEntity ();
			lisaEntity.AddComponent<Transform2D> ().Position = playerPosition * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
			lisaEntity.AddComponent<SpriteRender> ();
			lisaEntity.AddComponent<SpriteAnimation> ();
			var lisaAnimation = lisaEntity.AddComponent<PsychicAnimation> ();
			lisaAnimation.LeftStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0a" );
			lisaAnimation.RightStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0" );
			lisaAnimation.LeftWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0a", "Player/hero1a", "Player/hero2a", "Player/hero3a", "Player/hero4a" );
			lisaAnimation.RightWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0", "Player/hero1", "Player/hero2", "Player/hero3", "Player/hero4" );
			lisaAnimation.Dead = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/herod" );

			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( !lisaEntity.HasComponent<Message> () && !isGameOver )
			{
				var pa = lisaEntity.GetComponent<PsychicAnimation> ();
				if ( ( !movingStartLeft && !movingStartRight ) && !movingFall )
				{
					if ( InputManager.LeftInput )
					{
						pa.CurrentAnimationStatus = CurrentAnimationStatus.LeftWalk;
						if ( tileData [ ( int ) playerPosition.Y, ( int ) ( playerPosition.X - 1 ) ] == 0 )
						{
							movingStartLeft = true;
						}
					}
					else if ( InputManager.RightInput )
					{
						pa.CurrentAnimationStatus = CurrentAnimationStatus.RightWalk;
						if ( tileData [ ( int ) playerPosition.Y, ( int ) ( playerPosition.X + 1 ) ] == 0 )
						{
							movingStartRight = true;
						}
					}
					else
					{
						if ( pa.CurrentAnimationStatus == CurrentAnimationStatus.LeftWalk
							|| pa.CurrentAnimationStatus == CurrentAnimationStatus.LeftStand )
							pa.CurrentAnimationStatus = CurrentAnimationStatus.LeftStand;
						else if ( pa.CurrentAnimationStatus == CurrentAnimationStatus.RightWalk
							|| pa.CurrentAnimationStatus == CurrentAnimationStatus.RightStand )
							pa.CurrentAnimationStatus = CurrentAnimationStatus.RightStand;
						movingElapsed = new TimeSpan ();
						movingStartLeft = movingStartRight = false;
					}
				}
				else if ( movingFall )
				{
					movingElapsed += gameTime.ElapsedGameTime;
					if ( movingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
					{
						playerPosition += new Vector2 ( 0, 0.5f );
						if ( ( int ) playerPosition.Y >= 4 )
						{
							isGameOver = true;
							pa.CurrentAnimationStatus = CurrentAnimationStatus.Dead;
						}
						else
						{
							if ( tileData [ ( int ) playerPosition.Y - 1, ( int ) playerPosition.X ] != 0 )
								movingFall = false;
						}
						movingElapsed -= TimeSpan.FromSeconds ( 0.1 );
					}
				}
				else
				{
					movingElapsed += gameTime.ElapsedGameTime;
					if ( movingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
					{
						playerPosition += new Vector2 ( 0.5f * ( movingStartLeft ? -1 : 1 ), 0 );
						if ( playerPosition.X - ( int ) playerPosition.X <= float.Epsilon )
						{
							movingElapsed = new TimeSpan ();
							movingStartLeft = movingStartRight = false;
							if ( tileData [ ( int ) playerPosition.Y - 1, ( int ) playerPosition.X ] == 0 )
								movingFall = true;
						}
						else movingElapsed -= TimeSpan.FromSeconds ( 0.1 );
					}
				}
				lisaEntity.GetComponent<Transform2D> ().Position
					= playerPosition * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
				if ( playerPosition.X >= 4
					&& playerPosition.X <= ( tileData.GetLength ( 1 ) - 4 ) )
				{
					cameraEntity.GetComponent<Transform2D> ().Position
						= new Vector2 ( ( playerPosition.X - 3 ) * 25, 0 );
				}
				else if ( playerPosition.X < 4 )
				{
					cameraEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 0, 0 );
				}
				else if ( playerPosition.X > ( tileData.GetLength ( 1 ) - 4 ) )
				{
					cameraEntity.GetComponent<Transform2D> ().Position = new Vector2 ( ( tileData.GetLength ( 1 ) - 7 ) * 25, 0 );
				}
			}
			else if ( lisaEntity.HasComponent<Message> () )
			{

			}
			else if ( isGameOver )
			{

			}
		}
	}
}
