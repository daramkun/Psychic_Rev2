using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Coroutines;
using Daramee.Mint.Entities;
using Daramee.Mint.Processors;
using Daramee.Mint.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Input;
using Psychic.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	public class OpeningScene : Scene, IProcessor
	{
		public override string Name => "OpeningScene";
		
		Entity messageEntity;

		Queue<Message> messages;

		protected override void Enter ()
		{
			messageEntity = EntityManager.SharedManager.CreateEntity ();
			var msg = messageEntity.AddComponent<Message> ();
			msg.Font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );

			messages = new Queue<Message> ();
			foreach ( var txt in Message.CalculateMessageTextArea ( Resources.Message_Opening_Scene_1_1, msg.Font ) )
			{
				Message message = new Message ();
				message.Font = msg.Font;
				message.OverlayImage = Engine.SharedEngine.Content.Load<Texture2D> ( "Scenes/Opening/Opening1" );
				message.Name = Resources.Talker_Lisa;
				message.Text = txt;
				messages.Enqueue ( message );
			}
			foreach ( var txt in Message.CalculateMessageTextArea ( Resources.Message_Opening_Scene_1_2, msg.Font ) )
			{
				Message message = new Message ();
				message.Font = msg.Font;
				message.OverlayImage = Engine.SharedEngine.Content.Load<Texture2D> ( "Scenes/Opening/Opening1" );
				message.Name = Resources.Talker_Lisa;
				message.Text = txt;
				messages.Enqueue ( message );
			}
			foreach ( var txt in Message.CalculateMessageTextArea ( Resources.Message_Opening_Scene_2_1, msg.Font ) )
			{
				Message message = new Message ();
				message.Font = msg.Font;
				message.OverlayImage = Engine.SharedEngine.Content.Load<Texture2D> ( "Scenes/Opening/Opening2" );
				message.Name = Resources.Talker_Lisa;
				message.Text = txt;
				messages.Enqueue ( message );
			}
			foreach ( var txt in Message.CalculateMessageTextArea ( Resources.Message_Opening_Scene_2_2, msg.Font ) )
			{
				Message message = new Message ();
				message.Font = msg.Font;
				message.OverlayImage = Engine.SharedEngine.Content.Load<Texture2D> ( "Scenes/Opening/Opening2" );
				message.Name = Resources.Talker_Lisa;
				message.Text = txt;
				messages.Enqueue ( message );
			}
			foreach ( var txt in Message.CalculateMessageTextArea ( Resources.Message_Opening_Scene_3_1, msg.Font ) )
			{
				Message message = new Message ();
				message.Font = msg.Font;
				message.OverlayImage = Engine.SharedEngine.Content.Load<Texture2D> ( "Scenes/Opening/Opening3" );
				message.Name = Resources.Talker_Lisa;
				message.Text = txt;
				messages.Enqueue ( message );
			}

			msg.CopyFrom ( messages.Dequeue () );

			ProcessorManager.SharedManager.RegisterProcessor ( this );
		}

		protected override void Exit ()
		{
			ProcessorManager.SharedManager.UnregisterProcessor ( this );
		}

		public void Process ( GameTime gameTime )
		{
			if ( InputManager.AInputDown )
			{
				if ( messages.Count > 0 )
				{
					var msg = messageEntity.GetComponent<Message> ();
					msg.CopyFrom ( messages.Dequeue () );
				}
				else
					Coroutine.SharedCoroutine.RegisterCoroutine ( TransitionToGameScene () );
			}
		}

		private IEnumerator TransitionToGameScene ()
		{
			SceneManager.SharedManager.Transition ( "GameScene" );
			yield return null;
		}
	}
}
