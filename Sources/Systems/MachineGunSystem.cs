using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components.Items;
using Psychic.Components.Traps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class MachineGunSystem : ISystem
	{
		public bool IsParallelExecution => true;
		public int Order => 0;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<MachineGun> ();

		bool sensorIsActive = false;
		TimeSpan elapsedTime = TimeSpan.FromSeconds ( 2 );

		public void PreExecute ()
		{
			var sensor = EntityManager.SharedManager.GetEntitiesByComponent<Sensor> ().FirstOrDefault ();
			sensorIsActive = sensor?.GetComponent<Sensor> ().IsActived ?? false;
		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			if ( !sensorIsActive ) return;
			elapsedTime += gameTime.ElapsedGameTime;
			if ( elapsedTime > TimeSpan.FromSeconds ( 2 ) )
			{
				var bullet = EntityManager.SharedManager.CreateEntity ();
				bullet.AddComponent<Transform2D> ().CopyFrom ( entity.GetComponent<Transform2D> () );
				var rect = bullet.AddComponent<RectangleRender> ();
				rect.Size = new Vector2 ( 5, 3 );
				rect.Fill = true;
				rect.Color = new Color ( 1, 1, 0, 1.0f );
				bullet.AddComponent<Bullet> ().IsRight = entity.GetComponent<MachineGun> ().IsRight;

				elapsedTime -= TimeSpan.FromSeconds ( 2 );
			}
		}

		public void PostExecute () { }
	}
}
