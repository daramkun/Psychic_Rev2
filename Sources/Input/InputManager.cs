using Daramee.Mint.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static Daramee.Mint.Input.InputService;

namespace Psychic.Input
{
	public static class InputManager
	{
		public static bool AnyKeyInput => SharedInputService.IsAnyKeyPress ( Keys.Kanji, Keys.Kana ) || InputService.SharedInputService.IsAnyGamePadButtonPress ();

		public static bool LeftInputDown => SharedInputService.IsKeyDown ( Keys.Left )
			|| ( SharedInputService.IsGamePadButtonDown ( Buttons.DPadLeft ) || ( SharedInputService.LastGamePadState.ThumbSticks.Left.X > -0.5 && InputService.SharedInputService.CurrentGamePadState.ThumbSticks.Left.X < -0.5 ) );
		public static bool LeftInput => SharedInputService.IsKeyPress ( Keys.Left )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.DPadLeft ) || SharedInputService.CurrentGamePadState.ThumbSticks.Left.X < -0.5f;

		public static bool RightInputDown => SharedInputService.IsKeyDown ( Keys.Right )
			|| ( SharedInputService.IsGamePadButtonDown ( Buttons.DPadRight ) || ( SharedInputService.LastGamePadState.ThumbSticks.Left.X < 0.5 && InputService.SharedInputService.CurrentGamePadState.ThumbSticks.Left.X > 0.5 ) );
		public static bool RightInput => SharedInputService.IsKeyPress ( Keys.Right )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.DPadRight ) || SharedInputService.CurrentGamePadState.ThumbSticks.Left.X > 0.5f;

		public static bool UpInputDown => SharedInputService.IsKeyDown ( Keys.Up )
			|| ( SharedInputService.IsGamePadButtonDown ( Buttons.DPadUp ) || ( SharedInputService.LastGamePadState.ThumbSticks.Left.Y > -0.5 && InputService.SharedInputService.CurrentGamePadState.ThumbSticks.Left.Y < -0.5 ) );
		public static bool UpInput => SharedInputService.IsKeyPress ( Keys.Up )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.DPadUp ) || SharedInputService.CurrentGamePadState.ThumbSticks.Left.Y < -0.5f;

		public static bool DownInputDown => SharedInputService.IsKeyDown ( Keys.Down )
			|| ( SharedInputService.IsGamePadButtonDown ( Buttons.DPadDown ) || ( SharedInputService.LastGamePadState.ThumbSticks.Left.Y < 0.5 && InputService.SharedInputService.CurrentGamePadState.ThumbSticks.Left.Y > 0.5 ) );
		public static bool DownInput => SharedInputService.IsKeyPress ( Keys.Down )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.DPadDown ) || SharedInputService.CurrentGamePadState.ThumbSticks.Left.Y > 0.5f;

		public static bool AInputDown => SharedInputService.IsKeyDown ( Keys.S ) || SharedInputService.IsKeyDown ( Keys.Space )
			|| SharedInputService.IsGamePadButtonDown ( Buttons.A );
		public static bool AInput => SharedInputService.IsKeyPress ( Keys.S ) || SharedInputService.IsKeyPress ( Keys.Space )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.A );

		public static bool BInputDown => SharedInputService.IsKeyDown ( Keys.D )
			|| SharedInputService.IsGamePadButtonDown ( Buttons.B );
		public static bool BInput => SharedInputService.IsKeyPress ( Keys.D )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.B );

		public static bool BackInputDown => SharedInputService.IsKeyDown ( Keys.Back ) || SharedInputService.IsKeyDown ( Keys.Escape )
			|| SharedInputService.IsGamePadButtonDown ( Buttons.Back );
		public static bool BackInput => SharedInputService.IsKeyPress ( Keys.Back ) || SharedInputService.IsKeyPress ( Keys.Escape )
			|| SharedInputService.IsGamePadButtonPress ( Buttons.Back );
	}
}
