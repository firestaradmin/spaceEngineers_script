using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
	public interface ITerminalControl
	{
		string Id { get; }

<<<<<<< HEAD
		/// <summary>
		/// If control supports multiple blocks
		/// The only control which does not is Name editor control
		/// </summary>
		bool SupportsMultipleBlocks { get; }

		/// <summary>
		/// Sets blocks which are controlled now
		/// </summary>
		MyTerminalBlock[] TargetBlocks { get; set; }

		/// <summary>
		/// Returns terminal actions
		/// </summary>
=======
		bool SupportsMultipleBlocks { get; }

		MyTerminalBlock[] TargetBlocks { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		ITerminalAction[] Actions { get; }

		/// <summary>
		/// Returns control to show in terminal.
		/// When control does not exists yet, it creates it
		/// </summary>
		MyGuiControlBase GetGuiControl();

		/// <summary>
		/// Updates gui controls
		/// </summary>
		void UpdateVisual();

		/// <summary>
		/// Returns true when control is visible for given block
		/// </summary>
		bool IsVisible(MyTerminalBlock block);
	}
}
