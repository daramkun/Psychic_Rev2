using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Coroutines;
using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Components.Items;
using Psychic.Input;
using Psychic.Properties;
using Psychic.Scenes;
using Psychic.Static;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	class DoorSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;
		public bool IsTarget ( Entity entity ) => entity.IsActived && entity.HasComponent<Door> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			if ( InputManager.UpInputDown )
			{
				var transform = entity.GetComponent<Transform2D> ();
				Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

				var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
				var playerTransform = player.GetComponent<Transform2D> ();
				Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

				if ( boundingBox.Intersects ( playerBoundingBox ) )
				{
					if ( EntityManager.SharedManager.GetEntitiesByComponent<Key> ().Count () > 0 )
					{
						var font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );
						Message message = new Message
						{
							Font = font,
							Name = Resources.Talker_Lisa,
							Text = Message.CalculateMessageTextArea ( Resources.Message_DoorLocked, font ).First ()
						};

						( SceneManager.SharedManager.CurrentScene as GameScene ).MessageQueue.Enqueue ( message );
					}
					else
					{
						++GameSceneParameter.Stage;
						Coroutine.SharedCoroutine.RegisterCoroutine ( DoTransition () );
					}
				}
			}
		}

		private IEnumerator DoTransition ()
		{
			yield return SceneManager.SharedManager.Transition ( "GameScene" );
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
