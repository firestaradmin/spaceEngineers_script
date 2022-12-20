using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes Gas generator interface (H2/O2 block) (PB scripting interface)
	/// </summary>
	public interface IMyGasGenerator : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets or sets bottles auto refill enabled
		/// </summary>
		bool AutoRefill { get; set; }

		/// <summary>
		/// Gets or sets whether block can use conveyor system (pull ice and hydrogen/oxygen bottles)
		/// </summary>
		bool UseConveyorSystem { get; set; }
	}
}
