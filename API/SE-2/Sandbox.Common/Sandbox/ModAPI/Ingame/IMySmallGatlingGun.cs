using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes gatling gun (PB scripting interface)
	/// </summary>
	public interface IMySmallGatlingGun : IMyUserControllableGun, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Getter whether gatling gun can pull items from conveyor system
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool UseConveyorSystem { get; }
	}
}
