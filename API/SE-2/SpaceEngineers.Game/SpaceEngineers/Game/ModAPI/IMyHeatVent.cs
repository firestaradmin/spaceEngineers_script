using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Heat vent block (mods interface)
	/// </summary>
	public interface IMyHeatVent : SpaceEngineers.Game.ModAPI.Ingame.IMyHeatVent, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		/// <summary>
		/// Gets or Sets power dependency
		/// </summary>
		float PowerDependency { get; set; }

		/// <summary>
		/// Gets current power level
		/// </summary>
		float CurrentPowerLevel { get; }

		/// <summary>
		/// Gets or Sets minimal (power) color
		/// </summary>
		Color ColorMinimal { get; set; }

		/// <summary>
		/// Gets or Sets maximal (power) color
		/// </summary>
		Color ColorMaximal { get; set; }

		/// <summary>
		/// Gets or Sets current color
		/// </summary>
		Color ColorCurrent { get; set; }
	}
}
