using Daramee.Mint.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Components
{
	public class Message : IComponent
	{
		public SpriteFont Font;
		public Texture2D OverlayImage;
		public string Name;
		public string Text;

		public void Initialize ()
		{
			Font = null;
			OverlayImage = null;
			Name = null;
			Text = "";
		}

		public void CopyFrom ( IComponent component )
		{
			if ( component is Message )
			{
				Font = ( component as Message ).Font;
				OverlayImage = ( component as Message ).OverlayImage;
				Name = ( component as Message ).Name;
				Text = ( component as Message ).Text;
			}
		}

		public static IEnumerable<string> CalculateMessageTextArea ( string text, SpriteFont font )
		{
			Queue<char> q = new Queue<char> ( text );
			StringBuilder builder = new StringBuilder ();

			while ( q.Count > 0 )
			{
				builder.Append ( q.Peek () );
				var measure = font.MeasureString ( builder );
				if ( measure.X > 168 )
				{
					builder.Insert ( builder.Length - 1, '\n' );
					measure = font.MeasureString ( builder );
				}
				if ( measure.Y > 36)
				{
					builder.Remove ( builder.Length - 2, 2 );
					yield return builder.ToString ();
					builder.Clear ();
				}
				q.Dequeue ();
			}

			if ( builder.Length > 0 )
				yield return builder.ToString ();
		}
	}
}
