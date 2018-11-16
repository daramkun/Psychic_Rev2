using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Entities
{
	class UserInterface
	{
		Entity hpText, spText;
		Entity hpFrame, hpBar;
		Entity spFrame, spBar;

		Entity skill1Image, skill2Image, skill3Image;

		public bool IsVisible
		{
			set
			{
				hpText.IsActived = spText.IsActived =
					hpFrame.IsActived = hpBar.IsActived =
					spFrame.IsActived = spBar.IsActived =
					skill1Image.IsActived = skill2Image.IsActived = skill3Image.IsActived
					= value;
			}
		}

		public UserInterface ()
		{
			hpText = EntityManager.SharedManager.CreateEntity ();
			hpText.AddComponent<Transform2D> ().Position = new Vector2 ( 2 + 8, 137 + 5 );
			var sprite = hpText.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/HP" );
			sprite.IsCameraIndependency = true;

			spText = EntityManager.SharedManager.CreateEntity ();
			spText.AddComponent<Transform2D> ().Position = new Vector2 ( 2 + 8, 157 + 5 );
			sprite = spText.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/SP" );
			sprite.IsCameraIndependency = true;

			hpFrame = EntityManager.SharedManager.CreateEntity ();
			hpFrame.AddComponent<Transform2D> ().Position = new Vector2 ( 23, 137 ) + ( new Vector2 ( 50, 10 ) / 2 );
			var rect = hpFrame.AddComponent<RectangleRender> ();
			rect.Color = Color.Red;
			rect.Fill = false;
			rect.Size = new Vector2 ( 50, 10 );
			rect.IsCameraIndependency = true;

			hpBar = EntityManager.SharedManager.CreateEntity ();
			hpBar.AddComponent<Transform2D> ().Position = new Vector2 ( 23, 137 ) + ( new Vector2 ( 50, 10 ) / 2 );
			rect = hpBar.AddComponent<RectangleRender> ();
			rect.Color = Color.Red;
			rect.Fill = true;
			rect.Size = new Vector2 ( 50, 10 );
			rect.IsCameraIndependency = true;

			spFrame = EntityManager.SharedManager.CreateEntity ();
			spFrame.AddComponent<Transform2D> ().Position = new Vector2 ( 23, 157 ) + ( new Vector2 ( 50, 10 ) / 2 );
			rect = spFrame.AddComponent<RectangleRender> ();
			rect.Color = Color.Blue;
			rect.Fill = false;
			rect.Size = new Vector2 ( 50, 10 );
			rect.IsCameraIndependency = true;

			spBar = EntityManager.SharedManager.CreateEntity ();
			spBar.AddComponent<Transform2D> ().Position = new Vector2 ( 23, 157 ) + ( new Vector2 ( 50, 10 ) / 2 );
			rect = spBar.AddComponent<RectangleRender> ();
			rect.Color = Color.Blue;
			rect.Fill = true;
			rect.Size = new Vector2 ( 50, 10 );
			rect.IsCameraIndependency = true;

			skill1Image = EntityManager.SharedManager.CreateEntity ();
			skill1Image.AddComponent<Transform2D> ().Position = new Vector2 ( 77 + 15, 137 + 15 );
			sprite = skill1Image.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in" );
			sprite.IsCameraIndependency = true;

			skill2Image = EntityManager.SharedManager.CreateEntity ();
			skill2Image.AddComponent<Transform2D> ().Position = new Vector2 ( 110 + 15, 137 + 15 );
			sprite = skill2Image.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in2" );
			sprite.IsCameraIndependency = true;

			skill3Image = EntityManager.SharedManager.CreateEntity ();
			skill3Image.AddComponent<Transform2D> ().Position = new Vector2 ( 143 + 15, 137 + 15 );
			sprite = skill3Image.AddComponent<SpriteRender> ();
			sprite.Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in3" );
			sprite.IsCameraIndependency = true;
		}

		public void Update ( GameTime gameTime )
		{
			hpBar.GetComponent<Transform2D> ().Position = new Vector2 ( 23, 137 ) + ( hpBar.GetComponent<RectangleRender> ().Size = new Vector2 ( 5 * GameSceneParameter.HitPoint, 10 ) ) / 2;
			spBar.GetComponent<Transform2D> ().Position = new Vector2 ( 23, 157 ) + ( spBar.GetComponent<RectangleRender> ().Size = new Vector2 ( 5 * GameSceneParameter.SkillPoint, 10 ) ) / 2;

			switch ( GameSceneParameter.CurrentSkill )
			{
				case 0:
					skill1Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/ina" );
					skill2Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in2" );
					skill3Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in3" );
					break;

				case 1:
					skill1Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in" );
					skill2Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in2a" );
					skill3Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in3" );
					break;

				case 2:
					skill1Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in" );
					skill2Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in2" );
					skill3Image.GetComponent<SpriteRender> ().Sprite = Engine.SharedEngine.Content.Load<Texture2D> ( "Interface/in3a" );
					break;
			}
		}
	}
}
