using System;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes assembler block (mods interface)
	/// </summary>
	public interface IMyTerminalBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock
	{
		/// <summary>
		/// Called when custom data of block changed
		/// </summary>
		event Action<IMyTerminalBlock> CustomDataChanged;

		/// <summary>
		/// Called when custom name of block changed
		/// </summary>
		event Action<IMyTerminalBlock> CustomNameChanged;

		/// <summary>
		/// Called when block ownership changed
		/// </summary>
		event Action<IMyTerminalBlock> OwnershipChanged;

		/// <summary>
		/// Called when any of block properties changed.
		/// It could be anything, starting from CustomName ending with Inventory items
		/// </summary>
		event Action<IMyTerminalBlock> PropertiesChanged;

		/// <summary>
		/// Called when ShowOnHUD Changed
		/// </summary>
		event Action<IMyTerminalBlock> ShowOnHUDChanged;

		/// <summary>
		/// Called properties that modify the visibility of this block's controls, are changed
		/// </summary>
		event Action<IMyTerminalBlock> VisibilityChanged;

		/// <summary>
		/// Event to append custom info.
		/// </summary>
		event Action<IMyTerminalBlock, StringBuilder> AppendingCustomInfo;

		/// <summary>
		/// Raises AppendingCustomInfo so every subscriber can append custom info.
		/// </summary>
		void RefreshCustomInfo();

		/// <summary>
		/// Determines whether this block is in the same logical group as the other, meaning they're connected
		/// either mechanically or via blocks like connectors. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other">Block</param>
		/// <returns>True if blocks belongs to same grids, or their grids connected with <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Logical" /></returns>
		bool IsInSameLogicalGroupAs(IMyTerminalBlock other);

		/// <summary>
		/// Determines whether this block is mechanically connected to the other. This is any block connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other"></param>
		/// <returns>True if blocks belongs to same grids, or their grids connected with <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Logical" /></returns>
		bool IsSameConstructAs(IMyTerminalBlock other);
	}
}
