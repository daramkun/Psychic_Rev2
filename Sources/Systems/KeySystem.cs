using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	class KeySystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.IsActived && entity.HasComponent<Key> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				EntityManager.SharedManager.DestroyEntity ( entity );
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
