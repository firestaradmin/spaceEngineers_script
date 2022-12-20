using VRage.Game;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Interface for controlling projectile behavior (mods interface)
	/// </summary>
	public interface IMyProjectiles
	{
		/// <summary>
		/// Called when new projectile was added
		/// </summary>
		event OnProjectileAddedRemoved OnProjectileAdded;

		/// <summary>
		/// Called when projectile was removed
		/// </summary>
		event OnProjectileAddedRemoved OnProjectileRemoving;

		/// <summary>
		/// Call function when projectile hits something
		/// </summary>
		/// <param name="priority">Calls are ordered by priority. Functions with lower priority are called earlier</param>
		/// <param name="interceptor">Function that should be called on projectile hit</param>
		void AddOnHitInterceptor(int priority, HitInterceptor interceptor);

		/// <summary>
		/// Removed function from call when projectile hits something
		/// </summary>
		/// <param name="interceptor">Function that should not be called on projectile hit</param>
		void RemoveOnHitInterceptor(HitInterceptor interceptor);

		MyProjectileInfo GetProjectile(int index);

		/// <summary>
		/// Marks projectiles for destroy, doesn't decrease projectiles count. Shifting projectiles id, on next frame
		/// </summary>
		/// <param name="index">index of projectile</param>
		void MarkProjectileForDestroy(int index);

		/// <summary>
		/// Gets amount of projectiles currently existing
		/// </summary>
		/// <returns></returns>
		int GetAllProjectileCount();

		/// <summary>
		/// Adds new projectile
		/// </summary>
		/// <param name="weaponDefinition">Definition of weapon. Should be MyWeaponDefinition</param>
		/// <param name="ammoDefinition">Definition of ammo. Should be MyAmmoDefinition</param>
		/// <param name="origin">Spawn position</param>
		/// <param name="initialVelocity">Speed of object that fired projectile</param>
		/// <param name="directionNormalized">Direction of bullet</param>
		/// <param name="owningEntity">Rifle, block, ...</param>
		/// <param name="owningEntityAbsolute">Character, main ship cockpit. Used only to record damage statistics in single player</param>
		/// <param name="weapon">Shooter entity (rifle, block)</param>
		/// <param name="ignoredEntities"></param>
		/// <param name="supressHitIndicator">When true, hit indicator won't show</param>
		/// <param name="owningPlayer">Player that owns this projectile. Adds hit indication for that player on hit, and using that id in safezone access checks</param>
		void Add(MyDefinitionBase weaponDefinition, MyDefinitionBase ammoDefinition, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, MyEntity owningEntity, MyEntity owningEntityAbsolute, MyEntity weapon, MyEntity[] ignoredEntities, bool supressHitIndicator = false, ulong owningPlayer = 0uL);

		/// <summary>
		/// Adds projectile detector. It allows to detect projectiles flying though it, and can cause them hit it
		/// Example: Safezone, ship shields 
		/// </summary>
		/// <param name="detector">Detector logic</param>
		void AddHitDetector(IMyProjectileDetector detector);

		/// <summary>
		/// Removes projectile detector
		/// </summary>
		/// <param name="detector">Detector logic</param>
		void RemoveHitDetector(IMyProjectileDetector detector);

		/// <summary>
		/// Gets information about material and surface, that bullet hitted
		/// Arguments should be taken from <see cref="T:Sandbox.ModAPI.MyProjectileHitInfo" /> on projectile hit (subscribe on event with <see cref="M:Sandbox.ModAPI.IMyProjectiles.AddOnHitInterceptor(System.Int32,Sandbox.ModAPI.HitInterceptor)" />).
		/// </summary>
		/// <param name="entity">Entity that was hitted</param>
		/// <param name="line">Part of bullet trajectory</param>
		/// <param name="hitPosition">World position of hit</param>
		/// <param name="shapeKey">Should be taken from <see cref="F:Sandbox.ModAPI.MyProjectileHitInfo.HitShapeKey" /></param>
		/// <param name="surfaceImpact">Returns surface, that bullet hitted</param>
		/// <param name="materialType">Returns material, that bullet hitted</param>
		void GetSurfaceAndMaterial(IMyEntity entity, ref LineD line, ref Vector3D hitPosition, uint shapeKey, out MySurfaceImpactEnum surfaceImpact, out MyStringHash materialType);
	}
}
