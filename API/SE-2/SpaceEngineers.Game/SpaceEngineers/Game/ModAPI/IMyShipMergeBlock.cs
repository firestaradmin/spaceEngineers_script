using System;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Merge block (mods interface)
	/// </summary>
	public interface IMyShipMergeBlock : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyShipMergeBlock
	{
		/// <summary>
		/// Gets the other merge block this is connected to.
		/// </summary>
		IMyShipMergeBlock Other { get; }

		/// <summary>
		/// Gets the number of grids connected.
		/// </summary>
		int GridCount { get; }

		/// <summary>
		/// Event before merge is started
		/// </summary>
		event Action BeforeMerge;
	}
}
