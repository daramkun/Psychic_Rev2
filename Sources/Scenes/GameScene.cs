using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Diagnostics;
using Daramee.Mint.Entities;
using Daramee.Mint.Graphics;
using Daramee.Mint.Input;
using Daramee.Mint.Processors;
using Daramee.Mint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Psychic.Components;
using Psychic.Components.Enemies;
using Psychic.Components.Items;
using Psychic.Components.Traps;
using Psychic.Entities;
using Psychic.Input;
using Psychic.Static;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	public class GameScene : Scene, IProcessor
	{
		byte [,] tileData;
		Entity cameraEntity;
		UserInterface ui;
		Entity lisaEntity;
		Entity teleportEntity;
		Entity mindControlEntity;
		
		bool movingStartLeft, movingStartRight;
		TimeSpan movingElapsed;
		bool usingTeleport, usingMindControl;

		Entity targetEnemyEntity;
		bool mcMovingStartLeft, mcMovingStartRight;
		TimeSpan mcMovingElapsed;

		bool isGameOver = false, isInMenu = false;

		public override string Name => "GameScene";

		readonly PsychicAnimation lisaStandardAnimations, lisaInvisibleAnimations;
		readonly PsychicAnimation enemyAnimations;

		public Queue<Message> MessageQueue = new Queue<Message> ();

		public GameScene ()
		{
			lisaStandardAnimations = new PsychicAnimation
			{
				LeftStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0a" ),
				RightStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0" ),
				LeftWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0a", "Player/hero1a", "Player/hero2a", "Player/hero3a", "Player/hero4a" ),
				RightWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/hero0", "Player/hero1", "Player/hero2", "Player/hero3", "Player/hero4" ),
				Dead = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/herod" )
			};

			lisaInvisibleAnimations = new PsychicAnimation
			{
				LeftStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Skills/Invisible/herov0a" ),
				RightStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Skills/Invisible/herov0" ),
				LeftWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Skills/Invisible/herov0a", "Skills/Invisible/herov1a", "Skills/Invisible/herov2a", "Skills/Invisible/herov3a", "Skills/Invisible/herov4a" ),
				RightWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Skills/Invisible/herov0", "Skills/Invisible/herov1", "Skills/Invisible/herov2", "Skills/Invisible/herov3", "Skills/Invisible/herov4" ),
				Dead = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Player/herod" )
			};

			enemyAnimations = new PsychicAnimation
			{
				LeftStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Enemy/mon0a" ),
				RightStand = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Enemy/mon0" ),
				LeftWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Enemy/mon0a", "Enemy/mon1a", "Enemy/mon2a", "Enemy/mon3a" ),
				RightWalk = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Enemy/mon0", "Enemy/mon1", "Enemy/mon2", "Enemy/mon3" ),
				Dead = new Animation ( TimeSpan.FromSeconds ( 0.1 ), "Enemy/mond" )
			};
		}

		protected override void Enter ()
		{
			isGameOver = isInMenu = false;
			movingStartLeft = movingStartRight = false;
			usingTeleport = usingMindControl = false;
			GameSceneParameter.Initialize ();
			MessageQueue.Clear ();

			var tileEntity = EntityManager.SharedManager.CreateEntity ();
			tileEntity.AddComponent<Tile> ().TileData = tileData = StageTileInfo.GetStageData ( GameSceneParameter.Stage );

			foreach ( var decoInfo in StageTileInfo.GetStageDecorations ( GameSceneParameter.Stage ) )
			{
				var deco = EntityManager.SharedManager.CreateEntity ();
				deco.AddComponent<Transform2D> ().Position = decoInfo.Position * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
				deco.AddComponent<SpriteRender> ().Sprite = DecorationInfo.GetDecorationTexture ( decoInfo.DecorationType );
			}

			foreach ( var objInfo in StageTileInfo.GetStageObjects ( GameSceneParameter.Stage ) )
			{
				var obj = EntityManager.SharedManager.CreateEntity ();
				obj.AddComponent<Transform2D> ().Position = objInfo.Position * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
				switch ( objInfo.ObjectType )
				{
					case ObjectType.Door:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Door" );
						obj.AddComponent<Door> ();
						break;

					case ObjectType.Key:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Key" );
						obj.AddComponent<Key> ();
						break;

					case ObjectType.Save:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Items/Save" );
						obj.AddComponent<Save> ();
						break;

					case ObjectType.Help:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Items/Help" );
						obj.AddComponent<Help> ().Index = objInfo.Argument;
						break;

					case ObjectType.Document:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Items/Text" );
						obj.AddComponent<Document> ().DocumentId = objInfo.Argument;
						break;

					case ObjectType.Sensor:
						obj.AddComponent<SpriteRender> ();
						obj.AddComponent<SpriteAnimation> ().Animation = new Animation ( TimeSpan.FromSeconds ( 0.3 )
							, "Traps/Sensors/Sensor1", "Traps/Sensors/Sensor2" );
						obj.AddComponent<Sensor> ();
						break;

					case ObjectType.MachineGun:
						obj.GetComponent<Transform2D> ().Position += new Vector2 ( 0, 5 );
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( objInfo.ToRight ? "Traps/MachineGuns/MachineGunRight" : "Traps/MachineGuns/MachineGunLeft" );
						obj.AddComponent<MachineGun> ().IsRight = objInfo.ToRight;
						break;

					case ObjectType.Enemy:
						obj.AddComponent<SpriteRender> ();
						obj.AddComponent<SpriteAnimation> ();
						obj.AddComponent<PsychicAnimation> ().CopyFrom ( enemyAnimations );
						obj.AddComponent<Enemy> ().OriginalPosition = obj.GetComponent<Transform2D> ().Position;
						obj.AddComponent<Fallable> ();
						break;

					case ObjectType.Trap:
						obj.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Traps/Trap/Trap_Ready" );
						obj.AddComponent<Trap> ();
						obj.GetComponent<Transform2D> ().Position += new Vector2 ( 0, 8 );
						break;
				}
			}

			int emptyTileY = 0;
			for ( ; emptyTileY < 5; ++emptyTileY )
				if ( tileEntity.GetComponent<Tile> ().TileData [ emptyTileY, 1 ] == 0 )
					break;

			cameraEntity = EntityManager.SharedManager.CreateEntity ();
			cameraEntity.AddComponent<Transform2D> ();
			cameraEntity.AddComponent<Camera> ();

			lisaEntity = EntityManager.SharedManager.CreateEntity ();
			lisaEntity.Name = "Lisa";
			lisaEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 1, emptyTileY ) * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
			lisaEntity.AddComponent<SpriteRender> ();
			lisaEntity.AddComponent<SpriteAnimation> ();
			lisaEntity.AddComponent<Fallable> ();

			lisaEntity.AddComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );

			teleportEntity = EntityManager.SharedManager.CreateEntity ();
			teleportEntity.AddComponent<Transform2D> ();
			var sprite = teleportEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Skills/Laser" );
			teleportEntity.IsActived = false;

			mindControlEntity = EntityManager.SharedManager.CreateEntity ();
			mindControlEntity.AddComponent<Transform2D> ();
			mindControlEntity.AddComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Skills/ControlTarget" );
			mindControlEntity.IsActived = false;

			ui = new UserInterface ();

			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( usingMindControl && targetEnemyEntity != null && targetEnemyEntity.GetComponent<Enemy> ().IsDead )
			{
				usingMindControl = false;
				mindControlEntity.IsActived = false;
			}

			if ( !lisaEntity.HasComponent<Message> () && !isGameOver
				&& ( !usingTeleport && !usingMindControl ) )
			{
				PlayerUpdate ( gameTime );
				ui.Update ( gameTime );
			}
			else if ( lisaEntity.HasComponent<Message> () )
			{
				if ( InputManager.AInputDown )
				{
					if ( MessageQueue.Count == 0 )
					{
						lisaEntity.RemoveComponent<Message> ();
						ui.IsVisible = true;
					}
					else
					{
						lisaEntity.GetComponent<Message> ().CopyFrom ( MessageQueue.Dequeue () );
					}
				}
			}
			else if ( usingTeleport || usingMindControl )
			{
				if ( usingTeleport )
				{
					TeleportUpdate ();
				}
				else if ( usingMindControl )
				{
					MindControlUpdate ( gameTime );
				}
			}
			else if ( isGameOver )
			{
				if ( InputManager.AInputDown )
				{
					SceneManager.SharedManager.Transition ( "MenuScene" );
				}
			}
			else if ( isInMenu )
			{

			}
		}

		private void TeleportUpdate ()
		{
			var transform = teleportEntity.GetComponent<Transform2D> ();
			var laserPosScalar = ( transform.Position - new Vector2 ( 12, 12 ) ) / 25;
			if ( InputManager.UpInputDown )
			{
				if ( laserPosScalar.Y - 1 >= 0 && tileData [ ( int ) laserPosScalar.Y - 1, ( int ) laserPosScalar.X ] == 0 )
					transform.Position.Y -= 25;
			}
			else if ( InputManager.DownInputDown )
			{
				if ( laserPosScalar.Y + 1 <= 4 && tileData [ ( int ) laserPosScalar.Y + 1, ( int ) laserPosScalar.X ] == 0 )
					transform.Position.Y += 25;
			}
			else if ( InputManager.LeftInputDown )
			{
				if ( laserPosScalar.X - 1 >= 0 && tileData [ ( int ) laserPosScalar.Y, ( int ) laserPosScalar.X - 1 ] == 0 )
					transform.Position.X -= 25;
			}
			else if ( InputManager.RightInputDown )
			{
				if ( laserPosScalar.X + 1 < tileData.GetLength ( 1 ) && tileData [ ( int ) laserPosScalar.Y, ( int ) laserPosScalar.X + 1 ] == 0 )
					transform.Position.X += 25;
			}
			else if ( InputManager.AInputDown )
			{
				if ( transform.Position != lisaEntity.GetComponent<Transform2D>().Position )
				{
					if ( tileData [ 0, ( int ) laserPosScalar.X ] == 3 ||
						tileData [ 1, ( int ) laserPosScalar.X ] == 3 ||
						tileData [ 2, ( int ) laserPosScalar.X ] == 3 ||
						tileData [ 3, ( int ) laserPosScalar.X ] == 3 ||
						tileData [ 4, ( int ) laserPosScalar.X ] == 3 )
					{
						transform.Position.X += transform.Position.X - lisaEntity.GetComponent<Transform2D> ().Position.X;
						laserPosScalar = ( transform.Position - new Vector2 ( 12, 12 ) ) / 25;

						if ( laserPosScalar.Y >= 2 )
						{
							for ( int i = 3; i >=0; --i )
							{
								if ( tileData [ i, ( int ) laserPosScalar.X ] == 0 )
								{
									transform.Position.Y = i * 25 + 12;
									break;
								}
							}
						}
						else
						{
							for ( int i = 0; i <= 3; ++i )
							{
								if ( tileData [ i, ( int ) laserPosScalar.X ] == 0 )
								{
									transform.Position.Y = i * 25 + 12;
									break;
								}
							}
						}
					}
					else if ( !( tileData [ 0, ( int ) laserPosScalar.X ] == 2 ||
						tileData [ 1, ( int ) laserPosScalar.X ] == 2 ||
						tileData [ 2, ( int ) laserPosScalar.X ] == 2 ||
						tileData [ 3, ( int ) laserPosScalar.X ] == 2 ||
						tileData [ 4, ( int ) laserPosScalar.X ] == 2 ) )
					{
						lisaEntity.GetComponent<Transform2D> ().Position = teleportEntity.GetComponent<Transform2D> ().Position;
						GameSceneParameter.SkillPoint -= 2;
						usingTeleport = false;
					}
				}
				GameSceneParameter.UsingSkill = false;
				teleportEntity.IsActived = false;
			}
		}

		private void MindControlUpdate ( GameTime gameTime )
		{
			var pa = targetEnemyEntity.GetComponent<PsychicAnimation> ();
			var enemyPosition = ( targetEnemyEntity.GetComponent<Transform2D> ().Position - new Vector2 ( 12, 12 ) ) / 25;
			if ( !targetEnemyEntity.GetComponent<Fallable>().IsFalling && !mcMovingStartLeft && !mcMovingStartRight )
			{
				if ( InputManager.LeftInput )
				{
					pa.CurrentAnimationStatus = CurrentAnimationStatus.LeftWalk;
					if ( tileData [ ( int ) enemyPosition.Y, ( int ) ( enemyPosition.X - 1 ) ] == 0 )
						mcMovingStartLeft = true;
				}
				else if ( InputManager.RightInput )
				{
					pa.CurrentAnimationStatus = CurrentAnimationStatus.RightWalk;
					if ( tileData [ ( int ) enemyPosition.Y, ( int ) ( enemyPosition.X + 1 ) ] == 0 )
						mcMovingStartRight = true;
				}
				else if ( InputManager.AInputDown )
				{
					GameSceneParameter.UsingSkill = false;
					usingMindControl = false;
					mindControlEntity.IsActived = false;
					targetEnemyEntity.GetComponent<Enemy> ().IsControllingByPlayer = false;
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
			else if ( !targetEnemyEntity.GetComponent<Fallable> ().IsFalling )
			{
				mcMovingElapsed += gameTime.ElapsedGameTime;
				if ( mcMovingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
				{
					targetEnemyEntity.GetComponent<Transform2D> ().Position += new Vector2 ( 12.5f * ( mcMovingStartLeft ? -1 : 1 ), 0 );
					enemyPosition = ( targetEnemyEntity.GetComponent<Transform2D> ().Position - new Vector2 ( 12, 12 ) ) / 25;

					if ( enemyPosition.X - ( int ) enemyPosition.X <= float.Epsilon )
					{
						mcMovingElapsed = new TimeSpan ();
						mcMovingStartLeft = mcMovingStartRight = false;
					}
					else mcMovingElapsed -= TimeSpan.FromSeconds ( 0.1 );
				}
			}

			mindControlEntity.GetComponent<Transform2D> ().Position = targetEnemyEntity.GetComponent<Transform2D> ().Position - new Vector2 ( 0, 18 );
			if ( targetEnemyEntity.GetComponent<Fallable> ().DeadFlag )
			{
				pa.CurrentAnimationStatus = CurrentAnimationStatus.Dead;
				targetEnemyEntity.GetComponent<Transform2D> ().Position += new Vector2 ( 0, 8 );
				targetEnemyEntity.GetComponent<Enemy> ().IsDead = true;
				targetEnemyEntity.GetComponent<Enemy> ().IsControllingByPlayer = false;
				mindControlEntity.IsActived = false;
				usingMindControl = false;
				targetEnemyEntity = null;
			}
		}

		private void PlayerUpdate ( GameTime gameTime )
		{
			if ( MessageQueue.Count > 0 )
			{
				ui.IsVisible = false;
				var message = lisaEntity.AddComponent<Message> ();
				message.CopyFrom ( MessageQueue.Dequeue () );
				return;
			}

			var fallable = lisaEntity.GetComponent<Fallable> ();
			if ( fallable.DeadFlag )
			{
				DoGameOver ();
			}

			if ( ( !movingStartLeft && !movingStartRight ) && !fallable.IsFalling )
			{
				PlayerInputUpdate ( gameTime );
			}
			else if ( !fallable.IsFalling )
			{
				PlayerMoving ( gameTime );
			}
			
			CameraUpdate ( gameTime );
		}

		private void PlayerInputUpdate ( GameTime gameTime )
		{
			var pa = lisaEntity.GetComponent<PsychicAnimation> ();
			var playerPosition = ( lisaEntity.GetComponent<Transform2D> ().Position - new Vector2 ( 12, 12 ) ) / 25;
			if ( InputManager.LeftInput )
			{
				pa.CurrentAnimationStatus = CurrentAnimationStatus.LeftWalk;
				if ( tileData [ ( int ) playerPosition.Y, ( int ) ( playerPosition.X - 1 ) ] == 0 )
					movingStartLeft = true;
			}
			else if ( InputManager.RightInput )
			{
				pa.CurrentAnimationStatus = CurrentAnimationStatus.RightWalk;
				if ( tileData [ ( int ) playerPosition.Y, ( int ) ( playerPosition.X + 1 ) ] == 0 )
					movingStartRight = true;
			}
			else if ( InputManager.AInputDown )
			{
				switch ( GameSceneParameter.CurrentSkill )
				{
					case 0:
						if ( GameSceneParameter.SkillPoint - 2 < 0 )
							break;
						GameSceneParameter.UsingSkill = true;
						lisaEntity.GetComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );
						teleportEntity.GetComponent<Transform2D> ().Position = lisaEntity.GetComponent<Transform2D> ().Position;
						teleportEntity.IsActived = true;
						usingTeleport = true;
						break;

					case 1:
						if ( GameSceneParameter.UsingSkill )
						{
							GameSceneParameter.UsingSkill = false;
							lisaEntity.GetComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );
						}
						else
						{
							if ( GameSceneParameter.SkillPoint - 5 < 0 )
								break;
							GameSceneParameter.SkillPoint -= 5;
							GameSceneParameter.UsingSkill = true;
							lisaEntity.GetComponent<PsychicAnimation> ().CopyFrom ( lisaInvisibleAnimations );
						}
						break;

					case 2:
						if ( GameSceneParameter.SkillPoint - 10 < 0 )
							break;
						var enemies = EntityManager.SharedManager.GetEntitiesByComponent<Enemy> ();
						targetEnemyEntity = null;
						foreach ( var enemy in enemies )
						{
							if ( enemy.GetComponent<Transform2D> ().Position == lisaEntity.GetComponent<Transform2D> ().Position )
							{
								targetEnemyEntity = enemy;
								break;
							}
						}
						if ( targetEnemyEntity != null )
						{
							GameSceneParameter.SkillPoint -= 10;
							GameSceneParameter.UsingSkill = true;
							lisaEntity.GetComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );
							usingMindControl = true;
							targetEnemyEntity.GetComponent<Enemy> ().IsControllingByPlayer = true;
							mindControlEntity.GetComponent<Transform2D> ().Position = targetEnemyEntity.GetComponent<Transform2D> ().Position - new Vector2 ( 0, 18 );
							mindControlEntity.IsActived = true;
						}
						break;
				}
			}
			else if ( InputManager.XInputDown )
			{
				--GameSceneParameter.CurrentSkill;
				if ( GameSceneParameter.CurrentSkill < 0 )
					GameSceneParameter.CurrentSkill = 2;
			}
			else if ( InputManager.BInputDown )
			{
				++GameSceneParameter.CurrentSkill;
				if ( GameSceneParameter.CurrentSkill > 2 )
					GameSceneParameter.CurrentSkill = 0;
			}
			else if ( InputService.SharedInputService.IsKeyDown ( Keys.D1 ) )
				GameSceneParameter.CurrentSkill = 0;
			else if ( InputService.SharedInputService.IsKeyDown ( Keys.D2 ) )
				GameSceneParameter.CurrentSkill = 1;
			else if ( InputService.SharedInputService.IsKeyDown ( Keys.D3 ) )
				GameSceneParameter.CurrentSkill = 2;
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

		private void PlayerMoving ( GameTime gameTime )
		{
			movingElapsed += gameTime.ElapsedGameTime;
			if ( movingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
			{
				var transform = lisaEntity.GetComponent<Transform2D> ();
				transform.Position += new Vector2 ( 12.5f * ( movingStartLeft ? -1 : 1 ), 0 );
				if ( transform.Position.X - ( int ) transform.Position.X <= float.Epsilon )
				{
					movingElapsed = new TimeSpan ();
					movingStartLeft = movingStartRight = false;
				}
				else movingElapsed -= TimeSpan.FromSeconds ( 0.1 );
			}
		}

		private void CameraUpdate ( GameTime gameTime )
		{
			var transform = lisaEntity.GetComponent<Transform2D> ();
			if ( transform.Position.X - 12 >= 100 && transform.Position.X <= ( tileData.GetLength ( 1 ) - 4 ) * 25 )
			{
				cameraEntity.GetComponent<Transform2D> ().Position
					= new Vector2 ( transform.Position.X - 11 - 75, 0 );
			}
			else if ( transform.Position.X - 12 < 100 )
			{
				cameraEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 0, 0 );
			}
			else if ( transform.Position.X > ( tileData.GetLength ( 1 ) - 4 ) * 25 )
			{
				cameraEntity.GetComponent<Transform2D> ().Position = new Vector2 ( ( tileData.GetLength ( 1 ) - 7 ) * 25, 0 );
			}
		}

		public void DoGameOver ()
		{
			isGameOver = true;
			lisaEntity.GetComponent<Transform2D>().Position += new Vector2 ( 0, 6f );

			var pa = lisaEntity.GetComponent<PsychicAnimation> ();
			pa.CurrentAnimationStatus = CurrentAnimationStatus.Dead;

			var gameOverEntity = EntityManager.SharedManager.CreateEntity ();
			gameOverEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 176 / 2, 125 / 2 );
			var sprite = gameOverEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Text/GameOver" );
			sprite.IsCameraIndependency = true;
		}
	}
}
