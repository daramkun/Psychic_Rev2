using Daramee.Mint;
using Daramee.Mint.Components;
using Daramee.Mint.Entities;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychic.Systems
{
	public class TileRenderSystem : ISystem, IDisposable
	{
		SpriteBatch spriteBatch;

		public bool IsParallelExecution => false;
		public int Order => int.MaxValue - 257;

		public bool IsTarget ( Entity entity ) => entity.HasComponent<Tile> ();

		public TileRenderSystem ()
		{
			spriteBatch = new SpriteBatch ( Engine.SharedEngine.GraphicsDevice );
		}

		public void Dispose ()
		{
			spriteBatch?.Dispose ();
			spriteBatch = null;
		}

		public void PreExecute ()
		{
			var cameraEntity = EntityManager.SharedManager.GetEntitiesByComponent<Camera> ().FirstOrDefault ();
			Matrix? cameraMatrix = null;
			if ( cameraEntity != null )
			{
				var transform = cameraEntity.GetComponent<Transform2D> ();
				cameraMatrix = Matrix.CreateTranslation ( new Vector3 ( -transform.Position, 0 ) );
			}
			spriteBatch.Begin ( SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: cameraMatrix );
		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var tile = entity.GetComponent<Tile> ();
			for ( int y = 0; y < 5; ++y )
			{
				for ( int x = 0; x < tile.TileData.GetLength ( 1 ); ++x )
				{
					spriteBatch.Draw ( GetTileImage ( tile.TileData [ y, x ] ),
						new Vector2 ( 1 + x * 25, y * 25 ), Color.White );
				}
			}
		}

		public void PostExecute ()
		{
			spriteBatch.End ();
		}

		Texture2D GetTileImage ( int i )
		{
			switch ( i )
			{
				case 0: return Engine.SharedEngine.Content.Load<Texture2D> ( "Tiles/Background" );
				case 1: return Engine.SharedEngine.Content.Load<Texture2D> ( "Tiles/Ground" );
				case 2: return Engine.SharedEngine.Content.Load<Texture2D> ( "Tiles/Unreflectable" );
				case 3: return Engine.SharedEngine.Content.Load<Texture2D> ( "Tiles/Reflector" );
				default: return null;
			}
		}
	}
}
