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
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class DocumentSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;
		public bool IsTarget ( Entity entity ) => entity.HasComponent<Document> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				Document doc = entity.GetComponent<Document> ();
				Message message = new Message
				{
					Font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" ),
					Name = null,
					Text = string.Format ( Resources.Message_GotDocument, doc.DocumentId )
				};

				GameSceneParameter.Documents [ doc.DocumentId - 1 ] = true;
				( SceneManager.SharedManager.CurrentScene as GameScene ).MessageQueue.Enqueue ( message );
				EntityManager.SharedManager.DestroyEntity ( entity );
			}
		}
		
		public void PreExecute () { }
		public void PostExecute () { }
	}
}
