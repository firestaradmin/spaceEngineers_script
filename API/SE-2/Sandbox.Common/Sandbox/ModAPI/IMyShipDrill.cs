using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes ship drill block (mods interface)
	/// </summary>
	public interface IMyShipDrill : Sandbox.ModAPI.Ingame.IMyShipDrill, Sandbox.ModAPI.Ingame.IMyShipToolBase, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyShipToolBase, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets/sets multiplier affecting amount of ore mined from voxels
		/// </summary>
		float DrillHarvestMultiplier { get; set; }

		/// <summary>
		/// Gets/sets multiplier affecting drill power consumption
		/// </summary>
=======
		float DrillHarvestMultiplier { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float PowerConsumptionMultiplier { get; set; }
	}
}
