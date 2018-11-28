using Daramee.Mint.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Psychic.Static
{
	public static class GameSceneParameter
	{
		public static int Stage
#if DEBUG
			= 3;
#else
			= 1;
#endif
		public static bool [] Documents = new bool [ 10 ];

		public static int HitPoint = 10;
		public static int SkillPoint = 50;
		public static int CurrentSkill = 0;
		public static bool UsingSkill = false;

		public static void Initialize ()
		{
			HitPoint = 10;
			SkillPoint = 50;
			CurrentSkill = 0;
			UsingSkill = false;
		}

		public static void InitializeDocuments ()
		{
			for ( int i = 0; i < 10; ++i )
				Documents [ i ] = false;
		}

		public static bool LoadParameter ()
		{
			try
			{
				var file = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly ();
				if ( !file.FileExists ( "Saved.dat" ) ) return false;
				using ( var open = file.OpenFile ( "Saved.dat", FileMode.Open ) )
				{
					using ( var reader = new BinaryReader ( open, Encoding.UTF8, true ) )
					{
						Stage = reader.ReadByte ();
						for ( int i = 0; i < 10; ++i )
							Documents [ i ] = reader.ReadBoolean ();
					}
				}
			}
			catch { return false; }
			return true;
		}

		public static bool SaveParameter ()
		{
			try
			{
				var file = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForAssembly ();
				using ( var open = file.OpenFile ( "Saved.dat", FileMode.Create ) )
				{
					using ( var writer = new BinaryWriter ( open, Encoding.UTF8, true ) )
					{
						writer.Write ( ( byte ) Stage );
						for ( int i = 0; i < 10; ++i )
							writer.Write ( Documents [ i ] );
					}
				}
			}
			catch ( Exception ex )
			{
				Logger.SharedLogger.Log ( ex.ToString () );
				return false;
			}
			return true;
		}
	}
}
