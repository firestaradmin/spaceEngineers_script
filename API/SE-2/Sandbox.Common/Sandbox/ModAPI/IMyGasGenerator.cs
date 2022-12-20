using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Gas generator interface (mods interface)
	/// </summary>
	public interface IMyGasGenerator : Sandbox.ModAPI.Ingame.IMyGasGenerator, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		/// <summary>
		/// Gets or sets multiplier of gas produced and amount of gas item took 
		/// </summary>
		float ProductionCapacityMultiplier { get; set; }

		/// <summary>
		/// Gets or sets power consumption multiplier
		/// </summary>
		float PowerConsumptionMultiplier { get; set; }
	}
}
