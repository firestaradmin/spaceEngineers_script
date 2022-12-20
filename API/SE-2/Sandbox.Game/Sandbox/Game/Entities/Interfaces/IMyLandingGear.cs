using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyLandingGear
	{
		bool AutoLock { get; }

		LandingGearMode LockMode { get; }
<<<<<<< HEAD

		bool IsParkingEnabled { get; set; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		event LockModeChangedHandler LockModeChanged;

		void RequestLock(bool enable);

		void ResetAutolock();

		IMyEntity GetAttachedEntity();
	}
}
