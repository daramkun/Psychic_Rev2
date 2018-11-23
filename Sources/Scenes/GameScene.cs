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
using Psychic.Components.Items;
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

		Vector2 playerPosition, laserPosition;
		bool movingStartLeft, movingStartRight;
		TimeSpan movingElapsed;
		bool movingFall;

		bool isGameOver = false, isInMenu = false;

		public override string Name => "GameScene";

		PsychicAnimation lisaStandardAnimations, lisaInvisibleAnimations;

		public Queue<Message> MessageQueue = new Queue<Message> ();

		protected override void Enter ()
		{
			isGameOver = isInMenu = false;
			movingStartLeft = movingStartRight = movingFall = false;
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
				}
			}

			int emptyTileY = 0;
			for ( ; emptyTileY < 5; ++emptyTileY )
				if ( tileEntity.GetComponent<Tile> ().TileData [ emptyTileY, 1 ] == 0 )
					break;
			playerPosition = new Vector2 ( 1, emptyTileY );

			cameraEntity = EntityManager.SharedManager.CreateEntity ();
			cameraEntity.AddComponent<Transform2D> ();
			cameraEntity.AddComponent<Camera> ();

			lisaEntity = EntityManager.SharedManager.CreateEntity ();
			lisaEntity.Name = "Lisa";
			lisaEntity.AddComponent<Transform2D> ().Position = playerPosition * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
			lisaEntity.AddComponent<SpriteRender> ();
			lisaEntity.AddComponent<SpriteAnimation> ();

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

			lisaEntity.AddComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );

			teleportEntity = EntityManager.SharedManager.CreateEntity ();
			teleportEntity.AddComponent<Transform2D> ();
			var sprite = teleportEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Skills/Laser" );
			teleportEntity.IsActived = false;

			ui = new UserInterface ();

			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( !lisaEntity.HasComponent<Message> () && !isGameOver
				&& !( ( GameSceneParameter.CurrentSkill == 0 || GameSceneParameter.CurrentSkill == 2 ) && GameSceneParameter.UsingSkill ) )
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
			else if ( ( GameSceneParameter.CurrentSkill == 0 || GameSceneParameter.CurrentSkill == 2 ) && GameSceneParameter.UsingSkill )
			{
				if ( GameSceneParameter.CurrentSkill == 0 )
				{
					TeleportUpdate ();
				}
				else if ( GameSceneParameter.CurrentSkill == 2 )
				{
					MindControlUpdate ();
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
			if ( InputManager.UpInputDown )
			{
				if ( laserPosition.Y - 1 >= 0 && tileData [ ( int ) laserPosition.Y - 1, ( int ) laserPosition.X ] == 0 )
					laserPosition.Y -= 1;
			}
			else if ( InputManager.DownInputDown )
			{
				if ( laserPosition.Y + 1 <= 4 && tileData [ ( int ) laserPosition.Y + 1, ( int ) laserPosition.X ] == 0 )
					laserPosition.Y += 1;
			}
			else if ( InputManager.LeftInputDown )
			{
				if ( laserPosition.X - 1 >= 0 && tileData [ ( int ) laserPosition.Y, ( int ) laserPosition.X - 1 ] == 0 )
					laserPosition.X -= 1;
			}
			else if ( InputManager.RightInputDown )
			{
				if ( laserPosition.X + 1 < tileData.GetLength ( 1 ) && tileData [ ( int ) laserPosition.Y, ( int ) laserPosition.X + 1 ] == 0 )
					laserPosition.X += 1;
			}
			else if ( InputManager.AInputDown )
			{
				if ( laserPosition != playerPosition )
				{
					if ( tileData [ 0, ( int ) laserPosition.X ] == 3 ||
						tileData [ 1, ( int ) laserPosition.X ] == 3 ||
						tileData [ 2, ( int ) laserPosition.X ] == 3 ||
						tileData [ 3, ( int ) laserPosition.X ] == 3 ||
						tileData [ 4, ( int ) laserPosition.X ] == 3 )
					{
						laserPosition.X += laserPosition.X - playerPosition.X;
						if ( laserPosition.Y >= 2 )
						{
							for ( int i = 3; i >=0; --i )
							{
								if ( tileData [ i, ( int ) laserPosition.X ] == 0 )
								{
									laserPosition.Y = i;
									break;
								}
							}
						}
						else
						{
							for ( int i = 0; i <= 3; ++i )
							{
								if ( tileData [ i, ( int ) laserPosition.X ] == 0 )
								{
									laserPosition.Y = i;
									break;
								}
							}
						}
					}
					else if ( !( tileData [ 0, ( int ) laserPosition.X ] == 2 ||
						tileData [ 1, ( int ) laserPosition.X ] == 2 ||
						tileData [ 2, ( int ) laserPosition.X ] == 2 ||
						tileData [ 3, ( int ) laserPosition.X ] == 2 ||
						tileData [ 4, ( int ) laserPosition.X ] == 2 ) )
					{
						playerPosition = laserPosition;
						GameSceneParameter.SkillPoint -= 2;
					}
				}
				GameSceneParameter.UsingSkill = false;
				teleportEntity.IsActived = false;
			}
			teleportEntity.GetComponent<Transform2D> ().Position
				= laserPosition * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
		}

		private void MindControlUpdate ()
		{

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

			if ( ( !movingStartLeft && !movingStartRight ) && !movingFall )
			{
				PlayerInputUpdate ( gameTime );
				if ( playerPosition.Y >= 4 || tileData [ ( int ) playerPosition.Y + 1, ( int ) playerPosition.X ] == 0 )
					movingFall = true;
			}
			else if ( movingFall )
			{
				PlayerFall ( gameTime );
			}
			else
			{
				PlayerMoving ( gameTime );
			}

			lisaEntity.GetComponent<Transform2D> ().Position
				= playerPosition * new Vector2 ( 25, 25 ) + new Vector2 ( 12, 12 );
			CameraUpdate ( gameTime );
		}

		private void PlayerInputUpdate ( GameTime gameTime )
		{
			var pa = lisaEntity.GetComponent<PsychicAnimation> ();
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
						laserPosition = playerPosition;
						teleportEntity.IsActived = true;
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
						GameSceneParameter.SkillPoint -= 10;
						GameSceneParameter.UsingSkill = true;
						lisaEntity.GetComponent<PsychicAnimation> ().CopyFrom ( lisaStandardAnimations );
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

		private void PlayerFall ( GameTime gameTime )
		{
			movingElapsed += gameTime.ElapsedGameTime;
			if ( movingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
			{
				playerPosition += new Vector2 ( 0, 0.5f );
				if ( ( int ) playerPosition.Y >= 4 )
				{
					playerPosition.Y = 4;
					DoGameOver ();
				}
				else
				{
					if ( tileData [ ( int ) playerPosition.Y + 1, ( int ) playerPosition.X ] != 0 )
						movingFall = false;
				}
				movingElapsed -= TimeSpan.FromSeconds ( 0.1 );
			}
		}

		private void PlayerMoving ( GameTime gameTime )
		{
			movingElapsed += gameTime.ElapsedGameTime;
			if ( movingElapsed > TimeSpan.FromSeconds ( 0.1 ) )
			{
				playerPosition += new Vector2 ( 0.5f * ( movingStartLeft ? -1 : 1 ), 0 );
				if ( playerPosition.X - ( int ) playerPosition.X <= float.Epsilon )
				{
					movingElapsed = new TimeSpan ();
					movingStartLeft = movingStartRight = false;
				}
				else movingElapsed -= TimeSpan.FromSeconds ( 0.1 );
			}
		}

		private void CameraUpdate ( GameTime gameTime )
		{
			if ( playerPosition.X >= 4 && playerPosition.X <= ( tileData.GetLength ( 1 ) - 4 ) )
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

		private void DoGameOver ()
		{
			isGameOver = true;
			playerPosition += new Vector2 ( 0, 0.275f );

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
