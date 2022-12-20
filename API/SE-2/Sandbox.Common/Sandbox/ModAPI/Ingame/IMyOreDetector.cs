using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes ore detector block (PB scripting interface)(mods interface)
	/// </summary>
	public interface IMyOreDetector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets ore detection range in meters
		/// </summary>
		float Range { get; }

		/// <summary>
		/// Gets or sets whether block should use antennas to broadcast ore deposits
		/// </summary>
=======
		float Range { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool BroadcastUsingAntennas { get; set; }
	}
}
