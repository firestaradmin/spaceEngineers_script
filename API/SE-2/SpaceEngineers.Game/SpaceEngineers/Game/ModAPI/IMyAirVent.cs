using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Air Vent block (mods interface)
	/// </summary>
	public interface IMyAirVent : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent
	{
		/// <summary>
		/// Gets value of gas that can be pushed out per second
		/// </summary>
		float GasOutputPerSecond { get; }

		/// <summary>
		/// Gets value of gas can be pulled in per second
		/// </summary>
		float GasInputPerSecond { get; }

		/// <summary>
		/// Gets resource sink component for gas
		/// </summary>
		MyResourceSinkInfo OxygenSinkInfo { get; set; }

		/// <summary>
		/// Gets resource source component
		/// </summary>
		MyResourceSourceComponent SourceComp { get; set; }
	}
}
