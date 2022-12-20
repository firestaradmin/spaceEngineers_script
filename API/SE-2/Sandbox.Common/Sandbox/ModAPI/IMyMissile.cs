using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes missile entity (mods interface)
	/// </summary>
	public interface IMyMissile : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, IMyDestroyableObject
	{
		/// <summary>
		/// Gets or sets max travel trajectory
		/// </summary>
		float MaxTrajectory { get; set; }

		/// <summary>
		/// Character EntityId or block EntityId that shoots
		/// </summary>
		long Owner { get; set; }

		/// <summary>
		/// Character EntityId or block EntityId that shoots
		/// </summary>
		long LauncherId { get; set; }

		/// <summary>
		/// Gets or sets velocity of missile
		/// </summary>
		Vector3 LinearVelocity { get; set; }

		/// <summary>
		/// Get or sets initial position of missile 
		/// </summary>
		Vector3D Origin { get; set; }

		/// <summary>
		/// Gets MyWeaponDefinition of missile
		/// </summary>
		MyDefinitionBase WeaponDefinition { get; }

		/// <summary>
		/// Gets MyAmmoDefinition of missile
		/// </summary>
		MyDefinitionBase AmmoDefinition { get; }

		/// <summary>
		/// Gets MyAmmoMagazineDefinition of missile
		/// </summary>
		MyDefinitionBase AmmoMagazineDefinition { get; }

		/// <summary>
		/// Gets or sets missile trail particle effect
		/// </summary>
		MyParticleEffect ParticleEffect { get; set; }

		/// <summary>
		/// Gets or sets explosion type of missile (changes effect of explosion)
		/// </summary>
		MyExplosionTypeEnum ExplosionType { get; set; }

		/// <summary>
		/// Gets collided entity, on missile hit. You should get this value only on <see cref="E:Sandbox.ModAPI.IMyMissiles.OnMissileCollided" /> event
		/// </summary>
		MyEntity CollidedEntity { get; }

		/// <summary>
		/// Gets collision point, on missile hit. You should get this value only on <see cref="E:Sandbox.ModAPI.IMyMissiles.OnMissileCollided" /> event
		/// </summary>
		Vector3D? CollisionPoint { get; }

		/// <summary>
		/// Gets collision normal, on missile hit. You should get this value only on <see cref="E:Sandbox.ModAPI.IMyMissiles.OnMissileCollided" /> event
		/// </summary>
		Vector3 CollisionNormal { get; }

		/// <summary>
		/// Gets or sets explosion damage for missile
		/// </summary>
		float ExplosionDamage { get; set; }

		/// <summary>
		/// Gets or sets health of missile.
		/// While it is more than 0, it is damaging blocks that it collide with, each time subtracting amount of damage dealt.
		/// When left health is less or equal 0, missile explodes (if it hitted anything and <see cref="P:Sandbox.ModAPI.IMyMissile.ShouldExplode" /> is true). 
		/// </summary>
		float HealthPool { get; set; }

		/// <summary>
		/// Gets or sets if missile should explode on hit
		/// </summary>
		bool ShouldExplode { get; set; }

		/// <summary>
		/// Returns whether player is friendly to missile
		/// </summary>
		/// <param name="charId">Player identityId</param>
		/// <returns>If player is friendly</returns>
		bool IsCharacterIdFriendly(long charId);

		/// <summary>
		/// Can't be called in Missiles.OnMissileMoved, MissileCollided
		/// </summary>
		void Destroy();
	}
}
