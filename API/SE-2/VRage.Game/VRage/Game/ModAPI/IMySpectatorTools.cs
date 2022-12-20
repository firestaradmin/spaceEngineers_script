using System.Collections.Generic;
using VRage.ModAPI;

namespace VRage.Game.ModAPI
{
	public interface IMySpectatorTools
	{
		IReadOnlyList<MyLockEntityState> TrackedSlots { get; }

		void SetTarget(IMyEntity ent);

		IMyEntity GetTarget();

		void SetMode(MyCameraMode mode);

		MyCameraMode GetMode();

		void LockHitEntity();

		void ClearTrackedSlot(int slotIndex);

		void SaveTrackedSlot(int slotIndex);

		void SelectTrackedSlot(int slotIndex);

		void NextPlayer();

		void PreviousPlayer();
	}
}
