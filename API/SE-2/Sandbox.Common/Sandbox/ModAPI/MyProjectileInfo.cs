using VRage.Game;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Implements read-only info about existing projectile
	/// </summary>
	public struct MyProjectileInfo
	{
		/// <summary>
		/// Index of position in array. May be changed
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// World position
		/// </summary>
		public Vector3D Position { get; private set; }

		/// <summary>
		/// Shoot position (World coordinates)
		/// </summary>
		public Vector3D Origin { get; private set; }

		/// <summary>
		/// Current velocity
		/// </summary>
		public Vector3D Velocity { get; private set; }

		/// <summary>
		/// Cached gravity, in position of bullet. (May be inaccurate)
		/// </summary>
		public Vector3D CachedGravity { get; private set; }

		/// <summary>
		/// Max travel distance
		/// </summary>
		public float MaxTrajectory { get; private set; }

		/// <summary>
		/// Gets Projectile Ammo Definition of type MyProjectileAmmoDefinition
		/// </summary>        
		public MyDefinitionBase ProjectileAmmoDefinition { get; private set; }

		/// <summary>
		/// Gets Weapon Definition of type MyWeaponDefinition
		/// </summary>
		public MyDefinitionBase WeaponDefinition { get; private set; }

		/// <summary>
		/// Rifle, block ...
		/// </summary>
		public IMyEntity OwnerEntity { get; private set; }

		/// <summary>
		/// character, main ship cockpit,
		/// </summary>
		public IMyEntity OwnerEntityAbsolute { get; private set; }

		/// <summary>
		/// Player SteamId or zero
		/// </summary>
		public ulong OwningPlayer { get; private set; }

		public MyProjectileInfo(int index, Vector3D position, Vector3D origin, Vector3D velocity, Vector3D cachedGravity, float maxTrajectory, MyDefinitionBase projectileAmmoDefinition, MyDefinitionBase weaponDefinition, IMyEntity ownerEntity, IMyEntity ownerEntityAbsolute, ulong owningPlayer)
		{
			Index = index;
			Position = position;
			Origin = origin;
			Velocity = velocity;
			CachedGravity = cachedGravity;
			MaxTrajectory = maxTrajectory;
			ProjectileAmmoDefinition = projectileAmmoDefinition;
			WeaponDefinition = weaponDefinition;
			OwnerEntity = ownerEntity;
			OwnerEntityAbsolute = ownerEntityAbsolute;
			OwningPlayer = owningPlayer;
		}
	}
}
