using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes reactor block (PB scripting interface)
	/// </summary>
	public interface IMyReactor : IMyPowerProducer, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets whether this reactor block should pull items from conveyor system
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool UseConveyorSystem { get; set; }
	}
}
