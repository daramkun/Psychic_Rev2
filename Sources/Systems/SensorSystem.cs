using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Psychic.Components.Items;
using Psychic.Components.Traps;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class SensorSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;
		public bool IsTarget ( Entity entity ) => entity.HasComponent<Sensor> ();

		public void Execute ( Entity entity, GameTime gameTime )
		{
			if ( GameSceneParameter.CurrentSkill == 1 && GameSceneParameter.UsingSkill )
				return;

			var transform = entity.GetComponent<Transform2D> ();
			Rectangle boundingBox = new Rectangle ( ( int ) transform.Position.X - 12, ( int ) transform.Position.Y - 12, 25, 25 );

			var player = EntityManager.SharedManager.GetEntitiesByName ( "Lisa" ).First ();
			var playerTransform = player.GetComponent<Transform2D> ();
			Rectangle playerBoundingBox = new Rectangle ( ( int ) playerTransform.Position.X - 12, ( int ) playerTransform.Position.Y - 12, 25, 25 );

			if ( boundingBox.Intersects ( playerBoundingBox ) )
			{
				entity.GetComponent<Sensor> ().IsActived = true;
				entity.GetComponent<SpriteAnimation> ().Speed = 4;
			}
		}

		public void PreExecute () { }
		public void PostExecute () { }
	}
}
