using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
	public interface IMyControllableEntity : VRage.Game.ModAPI.Interfaces.IMyControllableEntity
	{
		new MyControllerInfo ControllerInfo { get; }
<<<<<<< HEAD
=======

		new MyEntity Entity { get; }

		float HeadLocalXAngle { get; set; }

		float HeadLocalYAngle { get; set; }

		bool EnabledBroadcasting { get; }

		MyToolbarType ToolbarType { get; }

		MyStringId ControlContext { get; }

		MyStringId AuxiliaryContext { get; }

		MyToolbar Toolbar { get; }

		MyEntity RelativeDampeningEntity { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		new MyEntity Entity { get; }

		float HeadLocalXAngle { get; set; }

		float HeadLocalYAngle { get; set; }

		bool EnabledBroadcasting { get; }

		MyToolbarType ToolbarType { get; }

		MyStringId ControlContext { get; }

		MyStringId AuxiliaryContext { get; }

		MyToolbar Toolbar { get; }

		MyEntity RelativeDampeningEntity { get; set; }

		/// <summary>
		/// This will be called locally to start shooting with the given action
		/// </summary>
		void BeginShoot(MyShootActionEnum action);

		/// <summary>
		/// This will be called locally to start shooting with the given action
		/// </summary>
		void EndShoot(MyShootActionEnum action);

		bool ShouldEndShootingOnPause(MyShootActionEnum action);

		/// <summary>
		/// This will be called back from the sync object both on local and remote clients
		/// </summary>
		void OnBeginShoot(MyShootActionEnum action);

		/// <summary>
		/// This will be called back from the sync object both on local and remote clients
		/// </summary>
		void OnEndShoot(MyShootActionEnum action);

		void UseFinished();

		void PickUpFinished();

		void Sprint(bool enabled);

		void SwitchToWeapon(MyDefinitionId weaponDefinition);

		void SwitchToWeapon(MyToolbarItemWeapon weapon);

		bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition);

		void SwitchAmmoMagazine();

		bool CanSwitchAmmoMagazine();

		void SwitchBroadcasting();

		MyEntityCameraSettings GetCameraEntitySettings();
	}
}
