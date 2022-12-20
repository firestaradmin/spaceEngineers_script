using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes cockpit block (PB scripting interface)
	/// </summary>
	public interface IMyCockpit : IMyShipController, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
	{
		/// <summary>
		/// Gets the maximum oxygen capacity of this cockpit.
		/// </summary>
		float OxygenCapacity { get; }

		/// <summary>
		/// Gets the current oxygen level of this cockpit, as a value between 0 (empty) and 1 (full).
		/// </summary>
		/// <returns></returns>
		float OxygenFilledRatio { get; }
	}
}
