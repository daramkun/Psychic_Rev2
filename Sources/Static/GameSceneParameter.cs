using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Static
{
	public static class GameSceneParameter
	{
		public static int Stage = 1;
		public static int HitPoint = 10;
		public static int SkillPoint = 10;
		public static int CurrentSkill = 0;
		public static bool UsingSkill = false;

		public static void Initialize ()
		{
			HitPoint = 10;
			SkillPoint = 10;
			CurrentSkill = 0;
			UsingSkill = false;
		}
	}
}
