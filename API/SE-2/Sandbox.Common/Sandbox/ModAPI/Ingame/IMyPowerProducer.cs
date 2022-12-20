using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes power producer block (PB scripting interface)
	/// </summary>
	public interface IMyPowerProducer : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets current output in Megawatts
		/// </summary>
		float CurrentOutput { get; }

		/// <summary>
		/// Gets maximum output in Megawatts
		/// </summary>
		float MaxOutput { get; }
	}
}
