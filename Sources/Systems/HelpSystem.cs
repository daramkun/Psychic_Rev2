using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Components.Items;
using Psychic.Properties;
using Psychic.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	class HelpSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;
		public bool IsTarget ( Entity entity ) => entity.HasComponent<Help> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				var helpIndex = entity.GetComponent<Help> ().Index;
				string text = "";
				switch ( helpIndex )
				{
					case 0: text = Resources.Message_Help_Teleport_Cliff; break;
					case 1: text = Resources.Message_Help_Door; break;
					case 2: text = Resources.Message_Help_Alert; break;
					case 3: text = Resources.Message_Help_Invisible; break;
					case 4: text = Resources.Message_Help_Compressor; break;
					case 5: text = Resources.Message_Help_Trap; break;
					case 6: text = Resources.Message_Help_Teleport_Mirror; break;
				}

				var font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" );
				Message message = new Message
				{
					Font = font,
					Name = Resources.Talker_Lisa,
					Text = Message.CalculateMessageTextArea ( text, font ).First ()
				};

				( SceneManager.SharedManager.CurrentScene as GameScene ).MessageQueue.Enqueue ( message );
				EntityManager.SharedManager.DestroyEntity ( entity );
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
