using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Components.Items;
using Psychic.Input;
using Psychic.Properties;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Entities
{
	public class InGameMenu
	{
		bool isVisible;

		Entity background, frame, menuSide, cursor;
		int selectedMenuIndex = 0;

		Entity docuSide, docuTopBackground, docuTitleBackground, docuTitleText, docuDescription, docuCursor;
		Entity [] documents;
		int selectedDocumentIndex = 0, lineScroll = 0;
		bool isInDocument;

		public bool IsVisible
		{
			get => isVisible;
			set
			{
				if ( value )
				{
					background.IsActived = IsMenuVisible = isVisible = true;
					isInDocument = false;
				}
				else
				{
					background.IsActived
						= IsMenuVisible
						= IsDocumentVisible
						= isVisible = false;
				}
			}
		}

		bool IsMenuVisible
		{
			set
			{
				frame.IsActived = menuSide.IsActived = cursor.IsActived = value;
			}
		}

		bool IsDocumentVisible
		{
			set
			{
				docuSide.IsActived = docuTopBackground.IsActived = docuTitleBackground.IsActived
					= docuTitleText.IsActived = docuDescription.IsActived
					= docuCursor.IsActived
					= value;
				for ( int i = 0; i < 10; ++i )
					documents [ i ].IsActived = value;
				selectedDocumentIndex = 0;
				isInDocument = value;
			}
		}

		public InGameMenu ()
		{
			background = EntityManager.SharedManager.CreateEntity ();
			background.AddComponent<Transform2D> ().Position = new Vector2 ( 176, 178 ) / 2;
			var rect = background.AddComponent<RectangleRender> ();
			rect.Size = new Vector2 ( 176, 178 );
			rect.Fill = true;
			rect.Color = Color.Black;
			rect.IsCameraIndependency = true;

			#region Initialize Menu
			frame = EntityManager.SharedManager.CreateEntity ();
			frame.AddComponent<Transform2D> ().Position = new Vector2 ( 176, 168 ) / 2 + new Vector2 ( 35, 0 );
			var sprite = frame.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/InGameMenu/Frame" );
			sprite.IsCameraIndependency = true;

			menuSide = EntityManager.SharedManager.CreateEntity ();
			menuSide.AddComponent<Transform2D> ().Position = new Vector2 ( 35, 178 ) / 2;
			sprite = menuSide.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/InGameMenu/menu1" );
			sprite.IsCameraIndependency = true;

			cursor = EntityManager.SharedManager.CreateEntity ();
			cursor.AddComponent<Transform2D> ().Position = new Vector2 ( 70, 41 ) + new Vector2 ( 0, 17 ) / 2;
			sprite = cursor.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/InGameMenu/Cursor" );
			sprite.IsCameraIndependency = true;
			#endregion

			#region Initialize Documents
			docuDescription = EntityManager.SharedManager.CreateEntity ();
			docuDescription.AddComponent<Transform2D> ().Position = new Vector2 ( 40, 70 );
			var text = docuDescription.AddComponent<TextRender> ();
			text.ForegroundColor = new Color ( 240, 240, 240 );
			text.Font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );
			text.IsCameraIndependency = true;

			docuTopBackground = EntityManager.SharedManager.CreateEntity ();
			docuTopBackground.AddComponent<Transform2D> ().Position = new Vector2 ( 35, 0 ) + new Vector2( 176 - 35, 70 ) / 2;
			rect = docuTopBackground.AddComponent<RectangleRender> ();
			rect.Fill = true;
			rect.Size = new Vector2 ( 176 - 35, 70 );
			rect.Color = Color.Black;
			rect.IsCameraIndependency = true;

			docuSide = EntityManager.SharedManager.CreateEntity ();
			docuSide.AddComponent<Transform2D> ().Position = new Vector2 ( 35, 178 ) / 2;
			sprite = docuSide.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/InGameMenu/menu2" );
			sprite.IsCameraIndependency = true;

			docuTitleBackground = EntityManager.SharedManager.CreateEntity ();
			docuTitleBackground.AddComponent<Transform2D> ().Position = new Vector2 ( 44, 5 ) + new Vector2 ( 120, 15 ) / 2;
			rect = docuTitleBackground.AddComponent<RectangleRender> ();
			rect.Fill = true;
			rect.Size = new Vector2 ( 120, 15 );
			rect.Color = new Color ( 240, 240, 240 );
			rect.IsCameraIndependency = true;

			docuTitleText = EntityManager.SharedManager.CreateEntity ();
			docuTitleText.AddComponent<Transform2D> ().Position = new Vector2 ( 46, 7 );
			text = docuTitleText.AddComponent<TextRender> ();
			text.ForegroundColor = Color.Black;
			text.Font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );
			text.IsCameraIndependency = true;

			docuCursor = EntityManager.SharedManager.CreateEntity ();
			docuCursor.AddComponent<Transform2D> ().Position = new Vector2 ( 38, 22 ) + new Vector2 ( 25, 25 ) / 2;
			sprite = docuCursor.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Menu/InGameMenu/DocumentCursor" );
			sprite.IsCameraIndependency = true;

			documents = new Entity [ 10 ];
			for ( int y = 0; y < 2; ++y )
			{
				for ( int x = 0; x < 5; ++x )
				{
					var entity = documents [ y * 5 + x ] = EntityManager.SharedManager.CreateEntity ();
					entity.AddComponent<Transform2D> ().Position = new Vector2 ( 44, 26 ) + new Vector2 ( 16, 16 ) / 2 + new Vector2 ( x * 28, y * 24 );
					sprite = entity.AddComponent<SpriteRender> ();
					sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Objects/Items/Text" );
					sprite.IsCameraIndependency = true;
				}
			}
			#endregion

			IsVisible = false;
		}

		public void Update ( GameTime gameTime )
		{
			if ( !isInDocument )
			{
				cursor.GetComponent<Transform2D> ().Position = new Vector2 ( 70, 41 + 8.5f ) + new Vector2 ( 0, 27 * selectedMenuIndex );

				if ( InputManager.UpInputDown )
				{
					--selectedMenuIndex;
					if ( selectedMenuIndex < 0 )
						selectedMenuIndex = 4;
				}
				else if ( InputManager.DownInputDown )
				{
					++selectedMenuIndex;
					if ( selectedMenuIndex > 4 )
						selectedMenuIndex = 0;
				}
				else if ( InputManager.BackInputDown )
				{
					IsVisible = false;
				}
				else if ( InputManager.AInputDown )
				{
					switch ( selectedMenuIndex )
					{
						case 0: IsVisible = false; break;

						case 1:
							DoDisplayDocuments ();
							break;

						case 2:
							DoDisplayHelp ();
							break;

						case 3:
							DoDisplayOptions ();
							break;

						case 4:
							SceneManager.SharedManager.Transition ( "MenuScene" );
							break;
					}
				}
			}
			else if ( isInDocument )
			{
				var descText = docuDescription.GetComponent<TextRender> ();

				if ( InputManager.LeftInputDown )
				{
					--selectedDocumentIndex;
					if ( selectedDocumentIndex < 0 )
						selectedDocumentIndex = 9;
					lineScroll = 0;
					UpdateDocument ();
				}
				else if ( InputManager.RightInputDown )
				{
					++selectedDocumentIndex;
					if ( selectedDocumentIndex > 9 )
						selectedDocumentIndex = 0;
					lineScroll = 0;
					UpdateDocument ();
				}
				else if ( InputManager.UpInputDown )
				{
					++lineScroll;
					if ( lineScroll > 0 )
						lineScroll = 0;
				}
				else if ( InputManager.DownInputDown )
				{
					--lineScroll;
					var maxCount = -descText.Text.Count ( ( ch ) => ch == '\n' );
					if ( lineScroll < maxCount )
						lineScroll = maxCount;
				}
				else if ( InputManager.BackInputDown )
				{
					IsDocumentVisible = false;
					IsMenuVisible = true;
				}

				docuCursor.GetComponent<Transform2D> ().Position = new Vector2 ( 38, 22 ) + new Vector2 ( 25, 25 ) / 2
					+ new Vector2 ( ( selectedDocumentIndex % 5 ) * 28, ( selectedDocumentIndex / 5 ) * 24 );
				docuDescription.GetComponent<Transform2D> ().Position = new Vector2 ( 40, 70 ) + descText.Font.MeasureString ( descText.Text ) / 2
					+ new Vector2 ( 0, 13 * lineScroll );
			}
		}

		private void UpdateDocument ()
		{
			var titleText = docuTitleText.GetComponent<TextRender> ();
			titleText.Text = GameSceneParameter.Documents [ selectedDocumentIndex ]
				? Resources.ResourceManager.GetString ( string.Format ( "Document_{0:00}_Title", selectedDocumentIndex + 1 ) )
				: Resources.Document_DidntTakeDoc;
			docuTitleText.GetComponent<Transform2D> ().Position = new Vector2 ( 46, 7 ) + titleText.Font.MeasureString ( titleText.Text ) / 2;

			var descText = docuDescription.GetComponent<TextRender> ();
			descText.Text = GameSceneParameter.Documents [ selectedDocumentIndex ]
				? Resources.ResourceManager.GetString ( string.Format ( "Document_{0:00}_Desc", selectedDocumentIndex + 1 ) )
				: Resources.Document_DidntTakeDoc;
			descText.Text = Message.CalculateTextArea ( descText.Text, descText.Font, new Vector2 ( 136, 2000 ) ).FirstOrDefault ();
		}

		private void DoDisplayDocuments ()
		{
			UpdateDocument ();
			IsMenuVisible = false;
			IsDocumentVisible = true;
		}

		private void DoDisplayHelp ()
		{
			IsMenuVisible = false;

		}

		private void DoDisplayOptions ()
		{
			IsMenuVisible = false;

		}
	}
}
