using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes missile launcher block (PB scripting interface)
	/// </summary>
	public interface IMySmallMissileLauncher : IMyUserControllableGun, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Getter whether missile launcher can pull items from conveyor system
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool UseConveyorSystem { get; }
	}
}
