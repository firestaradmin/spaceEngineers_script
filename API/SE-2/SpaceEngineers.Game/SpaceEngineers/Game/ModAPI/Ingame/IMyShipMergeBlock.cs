using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
	public interface IMyShipMergeBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets if the merge block is locked to another one.
		/// </summary>
		bool IsConnected { get; }
<<<<<<< HEAD

		/// <summary>
		/// State of the merge block
		/// </summary>
		MergeState State { get; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
