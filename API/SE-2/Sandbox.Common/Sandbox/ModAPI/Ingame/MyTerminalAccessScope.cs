namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Determines how <see cref="M:Sandbox.ModAPI.Ingame.IMyGridTerminalSystem.CanAccess(Sandbox.ModAPI.Ingame.IMyTerminalBlock,Sandbox.ModAPI.Ingame.MyTerminalAccessScope)" /> limits its
	/// access check.
	/// </summary>
	public enum MyTerminalAccessScope
	{
		/// <summary>
		/// Checks for access over the entire grid terminal system, no matter how the block is connected.
		/// </summary>
		All,
		/// <summary>
		/// Checks for access only within the current construct. This is any block connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		Construct,
		/// <summary>
		/// Checks for access only for blocks on the same grid as the programmable block.
		/// </summary>
		Grid
	}
}
