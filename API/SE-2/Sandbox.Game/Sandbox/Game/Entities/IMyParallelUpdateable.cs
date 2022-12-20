using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.Game.Entities
{
<<<<<<< HEAD
	/// <summary>
	/// Interface that must be implemented by any entity that supports parallel updates.
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public interface IMyParallelUpdateable : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		MyParallelUpdateFlags UpdateFlags { get; }

		void UpdateBeforeSimulationParallel();

		void UpdateAfterSimulationParallel();
	}
}
