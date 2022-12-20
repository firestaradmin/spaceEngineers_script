using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities
{
	public interface IMyHandheldGunObject<out T> : IMyGunObject<T> where T : MyDeviceBase
	{
		MyObjectBuilder_PhysicalGunObject PhysicalObject { get; }

		MyPhysicalItemDefinition PhysicalItemDefinition { get; }

		bool ForceAnimationInsteadOfIK { get; }

		bool IsBlocking { get; }

		int CurrentAmmunition { get; set; }

		int CurrentMagazineAmmunition { get; set; }

		int CurrentMagazineAmount { get; set; }

		long OwnerId { get; }

		long OwnerIdentityId { get; }

		bool Reloadable { get; }

		bool IsReloading { get; }

		bool NeedsReload { get; }

		bool IsRecoiling { get; }

		bool CanReload();

		bool Reload();

		float GetReloadDuration();

		bool CanDoubleClickToStick(MyShootActionEnum action);

		bool ShouldEndShootOnPause(MyShootActionEnum action);

		void DoubleClicked(MyShootActionEnum action);

		void PlayReloadSound();

		bool GetShakeOnAction(MyShootActionEnum action);
	}
}
