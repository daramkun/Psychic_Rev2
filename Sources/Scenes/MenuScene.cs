using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Coroutines;
using Daramee.Mint.Entities;
using Daramee.Mint.Input;
using Daramee.Mint.Processors;
using Daramee.Mint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Input;
using Psychic.Properties;
using Psychic.Static;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	class MenuScene : Scene, IProcessor
	{
		Entity menuGameStartEntity = null, menuInfoEntity = null, menuOptionEntity = null, menuCreditEntity = null, menuExitEntity = null;
		Entity menuNewGameEntity = null, menuContinueEntity = null;
		Entity menuInfoImageEntity = null;
		Entity menuOptionTextEntity = null, menuOptionCursorEntity = null;
		Entity menuCreditTextEntity = null;
		Entity cursorEntity;
		Entity sideEntity;

		int selectedMenuItem = 0, selectedGameStartItem = 0, selectedInfoItem = 0;
		readonly Texture2D [] sideImages = new Texture2D [ 5 ];
		readonly Texture2D [] infoImages = new Texture2D [ 5 ];
		bool isInStart = false, isInInfo = false, isInOption = false, isInCredit = false;

		public override string Name => "MenuScene";

		protected override void Enter ()
		{
			// 로고 이미지
			var logoEntity = EntityManager.SharedManager.CreateEntity ();
			logoEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 92 + 42, 5 + 7.5f );
			var sprite = logoEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/PsychicLogo" );

			// 저작권 이미지
			var copyrightEntity = EntityManager.SharedManager.CreateEntity ();
			copyrightEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 103 + 36.5f, 178 - 25 / 2f );
			sprite = copyrightEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Copyright" );

			// 메인 메뉴 - 게임 시작 항목
			menuGameStartEntity = EntityManager.SharedManager.CreateEntity ();
			menuGameStartEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 31.5f, 62 + 3.5f );
			sprite = menuGameStartEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Start" );

			// 메인 메뉴 - 정보 항목
			menuInfoEntity = EntityManager.SharedManager.CreateEntity ();
			menuInfoEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 32, 80 + 3.5f );
			sprite = menuInfoEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Info" );

			// 메인 메뉴 - 설정 항목
			menuOptionEntity = EntityManager.SharedManager.CreateEntity ();
			menuOptionEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 20.5f, 98 + 3.5f );
			sprite = menuOptionEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Option" );

			// 메인 메뉴 - 제작자 항목
			menuCreditEntity = EntityManager.SharedManager.CreateEntity ();
			menuCreditEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 19, 116 + 3.5f );
			sprite = menuCreditEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Credit" );

			// 메인 메뉴 - 종료 항목
			menuExitEntity = EntityManager.SharedManager.CreateEntity ();
			menuExitEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 14, 134 + 3.5f );
			sprite = menuExitEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Exit" );

			// 메뉴 커서
			cursorEntity = EntityManager.SharedManager.CreateEntity ();
			cursorEntity.Name = "cursor";
			cursorEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 58 - 2, 62 + 6 / 2f );
			sprite = cursorEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_Cursor" );

			// 왼쪽 사이드 이미지
			sideEntity = EntityManager.SharedManager.CreateEntity ();
			sideEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 30, 178 / 2f );
			sprite = sideEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Start" );

			sideImages [ 0 ] = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Start" );
			sideImages [ 1 ] = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Info" );
			sideImages [ 2 ] = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Option" );
			sideImages [ 3 ] = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Credit" );
			sideImages [ 4 ] = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Side_Exit" );

			for ( int i = 1; i <= 5; ++i )
				infoImages [ i - 1 ] = Engine.SharedEngine.Content.Load<Texture2D> ( $"Menu/Information/{i}" );

			// 게임 시작 메뉴 - 새 게임 항목
			menuNewGameEntity = EntityManager.SharedManager.CreateEntity ();
			menuNewGameEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 32, 62 + 3.5f );
			sprite = menuNewGameEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_NewGame" );
			menuNewGameEntity.IsActived = false;

			// 게임 시작 메뉴 - 불러오기 항목
			menuContinueEntity = EntityManager.SharedManager.CreateEntity ();
			menuContinueEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 62 + 32, 98 + 3.5f );
			sprite = menuContinueEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/MainMenu/Button_LoadGame" );
			menuContinueEntity.IsActived = false;

			// 정보 메뉴 - 정보 이미지
			menuInfoImageEntity = EntityManager.SharedManager.CreateEntity ();
			menuInfoImageEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 70 + 98 / 2, 32 + 116 / 2 );
			sprite = menuInfoImageEntity.AddComponent<SpriteRender> ();
			sprite.Sprite = infoImages [ 0 ];
			menuInfoImageEntity.IsActived = false;

			SpriteFont font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );
			var optionTextSize = font.MeasureString ( Resources.Option_Text );

			// 설정 메뉴 - 설정 텍스트
			menuOptionTextEntity = EntityManager.SharedManager.CreateEntity ();
			menuOptionTextEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 70, 64 ) + optionTextSize / 2;
			var text = menuOptionTextEntity.AddComponent<TextRender> ();
			text.Font = font;
			text.Text = Resources.Option_Text;
			menuOptionTextEntity.IsActived = false;

			// 설정 메뉴 - 설정 커서
			menuOptionCursorEntity = EntityManager.SharedManager.CreateEntity ();
			menuOptionCursorEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 112, 90 );
			var rect = menuOptionCursorEntity.AddComponent<RectangleRender> ();
			rect.Size = new Vector2 ( 20, 2 );
			rect.Fill = true;
			rect.Color = Color.Red;
			menuOptionCursorEntity.IsActived = false;

			// 크레딧 메뉴 - 크레딧 텍스트
			menuCreditTextEntity = EntityManager.SharedManager.CreateEntity ();
			menuCreditTextEntity.AddComponent<Transform2D> ().Position = new Vector2 ( 78, 72 ) + optionTextSize / 2;
			text = menuCreditTextEntity.AddComponent<TextRender> ();
			text.Font = font;
			text.Text = Resources.Credit_Description;
			menuCreditTextEntity.IsActived = false;

			// 시스템 등록
			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( !isInStart && !isInInfo && !isInOption && !isInCredit )
			{
				if ( InputManager.UpInputDown )
				{
					--selectedMenuItem;
					if ( selectedMenuItem < 0 )
						selectedMenuItem = 4;
				}
				if ( InputManager.DownInputDown )
				{
					++selectedMenuItem;
					if ( selectedMenuItem > 4 )
						selectedMenuItem = 0;
				}
				if ( InputManager.BackInputDown )
				{
					if ( selectedMenuItem == 4 )
						Engine.SharedEngine.Exit ();
					else
						selectedMenuItem = 4;
				}

				sideEntity.GetComponent<SpriteRender> ().Sprite = sideImages [ selectedMenuItem ];
				cursorEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 62 + 58 - 2, 62 + 6 / 2f + ( selectedMenuItem * 18 ) );

				if ( InputManager.AInputDown )
				{
					switch ( selectedMenuItem )
					{
						// Game Start
						case 0:
							SetActiveMenuItems ( false );
							menuNewGameEntity.IsActived = menuContinueEntity.IsActived = true;
							selectedGameStartItem = 0;
							isInStart = true;
							break;

						// Information
						case 1:
							SetActiveMenuItems ( false );
							cursorEntity.IsActived = false;
							menuInfoImageEntity.IsActived = true;
							selectedInfoItem = 0;
							menuInfoImageEntity.GetComponent<SpriteRender> ().Sprite = infoImages [ 0 ];
							isInInfo = true;
							break;

						// Option
						case 2:
							SetActiveMenuItems ( false );
							cursorEntity.IsActived = false;
							menuOptionTextEntity.IsActived = true;
							menuOptionCursorEntity.IsActived = true;
							menuOptionCursorEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 112 + ( Options.IsAudioEnabled ? 0 : 27 ), 90 );
							isInOption = true;
							break;

						// Credit
						case 3:
							SetActiveMenuItems ( false );
							cursorEntity.IsActived = false;
							menuCreditTextEntity.IsActived = true;
							isInCredit = true;
							break;

						// Exit
						case 4:
							Engine.SharedEngine.Exit ();
							break;
					}
				}
			}
			else if ( isInStart )
			{
				if ( InputManager.BackInputDown )
				{
					SetActiveMenuItems ( true );
					menuNewGameEntity.IsActived = menuContinueEntity.IsActived = false;
					isInStart = false;
				}
				if ( InputManager.UpInputDown )
				{
					--selectedGameStartItem;
					if ( selectedGameStartItem < 0 )
						selectedGameStartItem = 1;
				}
				if ( InputManager.DownInputDown )
				{
					++selectedGameStartItem;
					if ( selectedGameStartItem > 1 )
						selectedGameStartItem = 0;
				}
				if ( InputManager.AInputDown )
				{
					// TODO
					if ( selectedGameStartItem == 1 )
					{

					}
					else
					{
						Coroutine.SharedCoroutine.RegisterCoroutine ( TransitionToOpeningScene () );
					}
				}
				cursorEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 62 + 58 - 2, 62 + 6 / 2f + ( selectedGameStartItem * 36 ) );
			}
			else if ( isInInfo )
			{
				if ( InputManager.BackInputDown )
				{
					SetActiveMenuItems ( true );
					cursorEntity.IsActived = true;
					menuInfoImageEntity.IsActived = false;
					isInInfo = false;
				}
				if ( InputManager.LeftInputDown )
				{
					--selectedInfoItem;
					if ( selectedInfoItem < 0 )
						selectedInfoItem = 4;
				}
				if ( InputManager.RightInputDown )
				{
					++selectedInfoItem;
					if ( selectedInfoItem > 4 )
						selectedInfoItem = 0;
				}
				menuInfoImageEntity.GetComponent<SpriteRender> ().Sprite = infoImages [ selectedInfoItem ];
			}
			else if ( isInOption )
			{
				if ( InputManager.BackInputDown )
				{
					SetActiveMenuItems ( true );
					cursorEntity.IsActived = true;
					menuOptionTextEntity.IsActived = false;
					menuOptionCursorEntity.IsActived = false;
					isInOption = false;
				}
				if ( InputManager.LeftInputDown || InputManager.RightInputDown )
				{
					Options.IsAudioEnabled = !Options.IsAudioEnabled;
					menuOptionCursorEntity.GetComponent<Transform2D> ().Position = new Vector2 ( 112 + ( Options.IsAudioEnabled ? 0 : 27 ), 90 );
				}
			}
			else if ( isInCredit )
			{
				if ( InputManager.BackInputDown )
				{
					SetActiveMenuItems ( true );
					cursorEntity.IsActived = true;
					menuCreditTextEntity.IsActived = false;
					isInCredit = false;
				}
			}
		}

		private void SetActiveMenuItems ( bool active )
		{
			menuGameStartEntity.IsActived = menuInfoEntity.IsActived
				= menuOptionEntity.IsActived = menuCreditEntity.IsActived
				= menuExitEntity.IsActived = active;
		}

		private IEnumerator TransitionToOpeningScene ()
		{
			SceneManager.SharedManager.Transition ( "OpeningScene" );
			yield return null;
		}
	}
}
