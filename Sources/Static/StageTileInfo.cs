using Daramee.Mint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Psychic.Components;
using Psychic.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Static
{
	public enum DecorationType
	{
		Lamp,
		DeadBody,
		Pipe,
		Computer,
	}

	public struct DecorationInfo
	{
		public Vector2 Position;
		public DecorationType DecorationType;

		public static Texture2D GetDecorationTexture ( DecorationType decorationType )
		{
			string filename = null;
			switch ( decorationType )
			{
				case DecorationType.Lamp: filename = "Objects/Decorations/Lamp"; break;
				case DecorationType.DeadBody: filename = "Objects/Decorations/DeadBody"; break;
				case DecorationType.Pipe: filename = "Objects/Decorations/Pipe"; break;
				case DecorationType.Computer: filename = "Objects/Decorations/Computer"; break;
			}
			return Engine.SharedEngine.Content.Load<Texture2D> ( filename );
		}
	}

	public enum ObjectType
	{
		Enemy,
		Key,
		Door,
		Compresser,
		Trap,
		Help,
		Save,
		Document,
		MachineGun,
		Sensor,
	}

	public struct ObjectInfo
	{
		public Vector2 Position;
		public ObjectType ObjectType;
		public bool ToRight;
		public int Argument;
	}

	public static class StageTileInfo
	{
		public static byte [,] Stage01 = {
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 2, 1, 1 },
			{ 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 0, 1, 2, 1, 0, 2, 1, 0, 2, 1, 1 }
		};
		public static byte [,] Stage02 = {
			{ 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 0, 1, 1, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1 },
			{ 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 2, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1 }
		};
		public static byte [,] Stage03 = {
			{ 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
			{ 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 2, 2, 2, 2, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
			{ 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
		};
		public static byte [,] Stage04 = {
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
			{ 1, 0, 0, 2, 2, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 1, 1, 0, 3, 0, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 1, 1, 1, 1, 1, 1 }
		};
		public static byte [,] Stage05 = {
			{ 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 2, 0, 0, 0, 2, 2, 2, 2, 1, 1, 2, 0, 2, 1 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1, 2, 2, 1, 1, 2, 0, 2, 1 },
			{ 1, 0, 0, 1, 0, 1, 1, 1, 0, 2, 1, 2, 0, 3, 0, 2, 2, 2, 2, 2, 2, 2, 0, 2, 2 },
			{ 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }
		};
		public static byte [,] Stage06 = {
			{ 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 3, 0, 2, 2, 2 },
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 2, 2 },
			{ 1, 0, 2, 2, 2, 0, 2, 2, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 2, 2, 0, 3, 2, 2 },
			{ 1, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
			{ 1, 0, 2, 2, 2, 2, 2, 2, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 2, 2 }
		};
		public static byte [,] Stage07 = {
			{ 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0 },
			{ 1, 1, 1, 1, 0, 1, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0 },
			{ 1, 1, 1, 1, 0, 1, 0, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 3, 1, 0, 2, 0, 2, 0 },
			{ 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 0, 1, 2, 1, 2, 2, 2, 1, 1, 1, 1, 1, 2, 0, 0, 0, 2, 2, 2, 2, 2 }
		};
		public static byte [,] Stage08 = {
			{ 0, 0, 0, 0, 0, 1, 1, 1, 3, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 0 },
			{ 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
			{ 1, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 2, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0 },
			{ 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 2 }
		};
		public static byte [,] Stage09 = {
			{ 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 3, 0, 0, 0, 0, 0, 2 },
			{ 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2 },
			{ 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 2, 2, 2, 2, 1, 0, 2 },
			{ 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0, 2, 0, 0, 0, 0, 0, 2 },
			{ 1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 0, 0, 1, 0, 1, 2, 0, 0, 2, 2, 2, 2, 1, 2, 2 }
		};
		public static byte [,] Stage10 = {
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
			{ 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 1, 2, 1, 1, 1, 1, 1, 0, 1 },
			{ 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }
		};
		public static byte [,] Stage11 = {
			{ 2, 2, 2, 2, 2, 3, 0, 0, 2, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 3, 0 },
			{ 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 },
			{ 2, 0, 2, 2, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
			{ 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 1, 0, 2 }
		};
		public static byte [,] Stage12 = {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, 0, 2, 1, 2, 2, 2, 0, 0, 0, 1, 2, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 1, 2, 1, 2, 2, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2 },
			{ 0, 0, 0, 0, 0, 1, 2, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 1, 2, 1, 2, 2, 0, 3, 2, 2, 2, 2, 2, 2, 2, 2 }
		};

		public static byte [,] GetStageData ( int stage )
		{
			switch ( stage )
			{
				case 1: return Stage01;
				case 2: return Stage02;
				case 3: return Stage03;
				case 4: return Stage04;
				case 5: return Stage05;
				case 6: return Stage06;
				case 7: return Stage07;
				case 8: return Stage08;
				case 9: return Stage09;
				case 10: return Stage10;
				case 11: return Stage11;
				case 12: return Stage12;
				default: return null;
			}
		}

		private static IEnumerable<Message> SplitText ( SpriteFont font, string talker, string text )
		{
			foreach ( var txt in Message.CalculateMessageTextArea ( text, font ) )
			{
				Message msg = new Message
				{
					Font = font,
					OverlayImage = null,
					Name = talker,
					Text = txt
				};
				yield return msg;
			}
		}

		public static IEnumerable<DecorationInfo> GetStageDecorations ( int stage )
		{
			switch ( stage )
			{
				case 1:
					{
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 2, 3 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 7, 3 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 12, 3 ) };
					}
					break;

				case 2:
					{
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 2, 3 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.DeadBody, Position = new Vector2 ( 8, 1 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 18, 3 ) };
					}
					break;

				case 3:
					{
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 2, 3 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.DeadBody, Position = new Vector2 ( 13, 1 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 18, 3 ) };
					}
					break;

				case 4:
					{
						yield return new DecorationInfo () { DecorationType = DecorationType.Computer, Position = new Vector2 ( 4, 2 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 2, 1 ) };
						yield return new DecorationInfo () { DecorationType = DecorationType.Lamp, Position = new Vector2 ( 18, 3 ) };
					}
					break;

				case 5: yield break;

				case 6: yield break;

				case 7: yield break;

				case 8: yield break;

				case 9: yield break;

				case 10: yield break;

				case 11: yield break;

				case 12: yield break;

				default:
					break;
			}
		}

		public static IEnumerable<ObjectInfo> GetStageObjects ( int stage )
		{
			switch ( stage )
			{
				case 1:
					{
						yield return new ObjectInfo () { ObjectType = ObjectType.Help, Position = new Vector2 ( 3, 3 ), Argument = 0 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Help, Position = new Vector2 ( 13, 3 ), Argument = 1 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Document, Position = new Vector2 ( 5, 1 ), Argument = 1 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Door, Position = new Vector2 ( 14, 3 ) };
					}
					break;

				case 2:
					{
						yield return new ObjectInfo () { ObjectType = ObjectType.Save, Position = new Vector2 ( 2, 3 ), Argument = 0 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Help, Position = new Vector2 ( 5, 1 ), Argument = 2 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Help, Position = new Vector2 ( 14, 3 ), Argument = 3 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Document, Position = new Vector2 ( 4, 1 ), Argument = 2 };
						yield return new ObjectInfo () { ObjectType = ObjectType.Door, Position = new Vector2 ( 23, 3 ) };
						yield return new ObjectInfo () { ObjectType = ObjectType.Sensor, Position = new Vector2 ( 6, 1 ) };
						yield return new ObjectInfo () { ObjectType = ObjectType.MachineGun, Position = new Vector2 ( 7, 1 ), ToRight = false };
					}
					break;

				case 3:
					{

					}
					break;

				case 4:
					{

					}
					break;

				case 5: yield break;

				case 6: yield break;

				case 7: yield break;

				case 8: yield break;

				case 9: yield break;

				case 10: yield break;

				case 11: yield break;

				case 12: yield break;

				default:
					break;
			}
		}

		public static IEnumerable<Message> GetStageStartEventMessage ( SpriteFont font, int stage )
		{
			switch ( stage )
			{
				case 1:
					{
						foreach ( var msg in SplitText ( font, Resources.Talker_Lisa, Resources.Message_Event_1_1 ) )
							yield return msg;
						foreach ( var msg in SplitText ( font, Resources.Talker_MrE_Unknown, Resources.Message_Event_1_2 ) )
							yield return msg;
					}
					break;

				case 2:
					{

					}
					break;

				case 3:
					{

					}
					break;

				case 4:
					{

					}
					break;

				case 5:
					{

					}
					break;

				case 6:
					{

					}
					break;

				case 7:
					{

					}
					break;

				case 8:
					{

					}
					break;

				case 9:
					{

					}
					break;

				case 10:
					{

					}
					break;

				case 11:
					{

					}
					break;

				case 12:
					{

					}
					break;

				default:
					break;
			}
		}
	}
}
