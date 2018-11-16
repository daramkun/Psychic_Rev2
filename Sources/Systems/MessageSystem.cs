using Daramee.Mint;
using Daramee.Mint.Entities;
using Daramee.Mint.Graphics;
using Daramee.Mint.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Systems
{
	public class MessageSystem : ISystem, IDisposable
	{
		Texture2D blackPanel;
		SpriteBatch spriteBatch;

		public bool IsParallelExecution => false;
		public int Order => int.MaxValue;

		public bool IsTarget ( Entity entity ) => entity.IsActived && entity.HasComponent<Message> ();

		public MessageSystem ()
		{
			blackPanel = new Texture2D ( Engine.SharedEngine.GraphicsDevice, 1, 1 );
			blackPanel.SetData<Color> ( new Color [] { Color.Black } );
			spriteBatch = new SpriteBatch ( Engine.SharedEngine.GraphicsDevice );
		}

		public void Dispose ()
		{
			blackPanel?.Dispose ();
			blackPanel = null;

			spriteBatch?.Dispose ();
			spriteBatch = null;
		}

		public void PreExecute ()
		{
			spriteBatch.Begin ( SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp );
		}

		public void Execute ( Entity entity, GameTime gameTime )
		{
			var msg = entity.GetComponent<Message> ();

			if ( msg.OverlayImage != null )
			{
				spriteBatch.Draw ( msg.OverlayImage, new Vector2 (), Color.White );
			}

			spriteBatch.Draw ( blackPanel, new Rectangle ( 0, 120, 176, 58 ), Color.Black );

			spriteBatch.DrawString ( msg.Font, msg.Name ?? "SYSTEM",
				new Vector2 ( 4, 126 ), new Color ( 0, 255, 0, 255 ) );

			spriteBatch.DrawString ( msg.Font, msg.Text,
				new Vector2 ( 4, 142 ), Color.White );
		}

		public void PostExecute ()
		{
			spriteBatch.End ();
		}
	}
}
