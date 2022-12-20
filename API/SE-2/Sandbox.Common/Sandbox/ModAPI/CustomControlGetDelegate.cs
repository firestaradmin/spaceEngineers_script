using System.Collections.Generic;
using Sandbox.ModAPI.Interfaces.Terminal;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Allows you to modify the terminal control list before it is displayed to the user.  Modifying controls will change which controls are displayed.
	/// </summary>
	/// <param name="block">The block that was selected</param>
	/// <param name="controls">The list of controls for this block</param>
	public delegate void CustomControlGetDelegate(IMyTerminalBlock block, List<IMyTerminalControl> controls);
}
