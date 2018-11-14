using Daramee.Mint.Entities;
using Daramee.Mint.Scenes;
using Psychic.Components;
using Psychic.Static;
using System;
using System.Collections.Generic;
using System.Text;

namespace Psychic.Scenes
{
	public class GameScene : Scene
	{
		public override string Name => "GameScene";

		protected override void Enter ()
		{
			var tileEntity = EntityManager.SharedManager.CreateEntity ();
			tileEntity.AddComponent<Tile> ().TileData = StageTileInfo.Stage01;
		}

		protected override void Exit ()
		{

		}
	}
}
