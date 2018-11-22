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
	public class SaveSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Save> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				var saveState = GameSceneParameter.SaveParameter ();
				var text = saveState ? Resources.Message_Saved : Resources.Message_FailedSave;
				Message message = new Message
				{
					Font = Engine.SharedEngine.Content.Load<SpriteFont> ( "Fonts/Gulim8" ),
					Name = null,
					Text = text
				};
				
				( SceneManager.SharedManager.CurrentScene as GameScene ).MessageQueue.Enqueue ( message );
				EntityManager.SharedManager.DestroyEntity ( entity );
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
