using VRage.Game.ModAPI.Interfaces;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// This can be hand held weapon (including welders and drills) as well as 
	/// weapons on ship (including ship drills).
	/// </summary>
	public interface IMyGunObject<out T> where T : MyDeviceBase
	{
		/// <summary>
		/// Gets force in Newtons.
		/// </summary>
		/// <remarks>Not related to modders. Modders should return zero</remarks>
		float BackkickForcePerSecond { get; }

		/// <summary>
		/// Gets shake amount of grid, when 
		/// </summary>
		/// <remarks>Not related to modders. Modders should return zero</remarks>
		float ShakeAmount { get; }

		/// <summary>
		/// Gets block definition id
		/// </summary>
		MyDefinitionId DefinitionId { get; }

		/// <summary>
		/// Gets if enabled by world rules (<see cref="P:VRage.Game.ModAPI.IMySession.WeaponsEnabled" />)
		/// </summary>
		bool EnabledInWorldRules { get; }

		/// <summary>
		/// Gets class that extends from <see cref="T:VRage.Game.ModAPI.MyDeviceBase" />. It could be MyToolBase, MyGunBase or even custom logic.
		/// Keep in mind, that some functions works differently for tools, gun or custom logic. 
		/// </summary>
		T GunBase { get; }

		/// <summary>
		/// Gets if character weapon/tool is skinnable
		/// </summary>
		bool IsSkinnable { get; }

		/// <summary>
		/// Gets if device can lock targets. (Warfare 2 feature)
		/// </summary>
		bool IsTargetLockingCapable { get; }

		/// <summary>
		/// Should return true when the weapon is shooting projectiles and other classes should react accordingly (i.e.apply backkick force etc.)
		/// </summary>
		bool IsShooting { get; }

		/// <summary>
		/// Zero means that the gun should not update shoot direction at all
		/// </summary>
		/// <returns>Minimal time interval in milliseconds between two direction updates</returns>
		int ShootDirectionUpdateTime { get; }

		/// <summary>
		/// Whether this gun needs the shoot direction at all times. Guns that do not will have their direction 
		/// </summary>
		bool NeedsShootDirectionWhileAiming { get; }

		/// <summary>
		/// Zero means that the gun should not update shoot direction at all
		/// </summary>
		/// <returns>Minimal time interval in milliseconds between two direction updates</returns>
		float MaximumShotLength { get; }

		/// <summary>
		/// Gets direction vector (normalized) between device and provided target vector. Used for character devices
		/// </summary>
		/// <param name="target">Target vector in world coordinates</param>
		/// <returns>Normalized direction between device position and provided vector</returns>
		Vector3 DirectionToTarget(Vector3D target);

		/// <summary>
		/// Should return true if and only if the gun would be able to shoot using the given shoot action.
		/// This method should not do any side-effects such as play sounds or create particle FX.
		/// </summary>
		/// <param name="action">The shooting action to test</param>
		/// <param name="shooter">Id of shooting player</param>
		/// <param name="status">Detailed status of the gun, telling why the gun couldn't perform the given shoot action</param>
		/// <returns></returns>
		bool CanShoot(MyShootActionEnum action, long shooter, out MyGunStatusEnum status);

		/// <summary>
		/// Perform the shoot action according to the action parameter.
		/// This method should only be called when CanShoot returns true for the given action!
		/// </summary>
		/// <param name="action">The shooting action to perform</param>
		/// <param name="overrideWeaponPos">Changes weapon position, world space.</param>
		/// <param name="direction">The prefered direction of shooting</param>
		/// <param name="gunAction">Always null</param>
		void Shoot(MyShootActionEnum action, Vector3 direction, Vector3D? overrideWeaponPos, string gunAction = null);

		/// <summary>
		/// Direction where gun is shooting
		/// </summary>
		/// <returns></returns>
		Vector3 GetShootDirection();

		/// <summary>
		/// Called when device start shooting
		/// </summary>
		/// <param name="action">Type if shooting</param>
		void BeginShoot(MyShootActionEnum action);

		/// <summary>
		/// Called when device stop shooting
		/// </summary>
		/// <param name="action">Type if shooting</param>
		void EndShoot(MyShootActionEnum action);

		/// <summary>
		/// Perform a fail reaction to begin shoot that is shown on all clients (e.g. fail sound, etc.)
		/// </summary>
		/// <param name="action">The shooting action, whose begin shoot failed</param>
		/// <param name="status">Why the begin shoot failed</param>
		void BeginFailReaction(MyShootActionEnum action, MyGunStatusEnum status);

		/// <summary>
		/// Perform a fail reaction to begin shoot that is shown only on client that controls character or ship, that has this device
		/// </summary>
		/// <param name="action">The shooting action, whose begin shoot failed</param>
		/// <param name="status">Why the begin shoot failed</param>
		void BeginFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status);

		/// <summary>
		/// Perform a fail reaction to during shooting that is shown only on client that controls character or ship, that has this device
		/// </summary>
		/// <param name="action">The shooting action, whose shooting failed</param>
		/// <param name="status">Why the shooting failed</param>
		void ShootFailReactionLocal(MyShootActionEnum action, MyGunStatusEnum status);

		/// <summary>
		/// Gets total ammunition count: <see cref="M:VRage.Game.ModAPI.IMyGunObject`1.GetAmmunitionAmount" /> + <see cref="M:VRage.Game.ModAPI.IMyGunObject`1.GetMagazineAmount" /> * Magazine.Capacity
		/// </summary>
		/// <returns>Total ammunition amount</returns>
		int GetTotalAmmunitionAmount();

		/// <summary>
		/// Gets current ammunition left before reloading
		/// </summary>
		/// <returns>Ammunition amount</returns>
		int GetAmmunitionAmount();

		/// <summary>
		/// Gets magazines amount
		/// </summary>
		/// <returns>Magazines amount</returns>
		int GetMagazineAmount();

		/// <summary>
		/// Called when control over device acquired
		/// </summary>
		/// <param name="owner">Controlling character</param>
		void OnControlAcquired(IMyCharacter owner);

		/// <summary>
		/// Called when control over device lost
		/// </summary>
		void OnControlReleased();

		/// <summary>
		/// When device is selected, this method is called in Draw thread. Example: welder shows info about what block it welds.
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="playerId">Player that controls device</param>
		void DrawHud(IMyCameraController camera, long playerId);

		/// <summary>
		/// When device is selected, this method is called in Draw thread. 
		/// </summary>
		/// <param name="camera">Current camera</param>
		/// <param name="playerId">Player that controls device</param>
		/// <param name="fullUpdate">True when should update internal cache</param>
		void DrawHud(IMyCameraController camera, long playerId, bool fullUpdate);

		/// <summary>
		/// If device has sound emmiter(s), it(they) should be updated
		/// </summary>
		void UpdateSoundEmitter();

		/// <summary>
		///  When too close to object and hands shouldn't be extended
		/// </summary>
		/// <returns>True if too close</returns>
		bool SupressShootAnimation();

		/// <summary>
		/// Gets muzzle world position
		/// </summary>
		/// <returns>Position of muzzle in world coordinates</returns>
		Vector3D GetMuzzlePosition();

		/// <summary>
		/// Returns true if can be used with LMB/RMB like drills
		/// </summary>
		/// <returns>True if can be used with LMB/RMB like drills</returns>
		bool IsToolbarUsable();
	}
}
