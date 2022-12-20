using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes information projectile about projectile hit. (mods interface)
	/// You can change values 
	/// </summary>
	public class MyProjectileHitInfo
	{
		/// <summary>
		/// Damage that would 
		/// </summary>
		public float Damage;

		/// <summary>
		/// Impulse, that would be applied to <see cref="F:Sandbox.ModAPI.MyProjectileHitInfo.HitEntity" />
		/// </summary>
		public Vector3 Impulse;

		/// <summary>
		/// Hit normal
		/// </summary>
		public Vector3 HitNormal;

		/// <summary>
		/// World coordinates of projectile hit
		/// </summary>
		public Vector3D HitPosition;

		/// <summary>
		/// Used in <see cref="M:Sandbox.ModAPI.IMyProjectiles.GetSurfaceAndMaterial(VRage.ModAPI.IMyEntity,VRageMath.LineD@,VRageMath.Vector3D@,System.UInt32,Sandbox.ModAPI.MySurfaceImpactEnum@,VRage.Utils.MyStringHash@)" />
		/// </summary>
		public uint HitShapeKey;

		/// <summary>
		/// Entity that was hitted by projectile
		/// </summary>
		public IMyEntity HitEntity;

		/// <summary>
		/// Velocity of projectile before hit
		/// </summary>
		public Vector3D Velocity;

		/// <summary>
		/// Material that hitted by projectile
		/// </summary>
		public MyStringHash HitMaterial;

		/// <summary>
		/// Voxel Material that hitted by projectile
		/// </summary>
		public MyStringHash HitVoxelMaterial;

		/// <summary>
		/// When it is false, decals wont be added (default = true)
		/// </summary>
		public bool AddDecals;

		/// <summary>
		/// When it is false, player wont see hit indicator
		/// </summary>
		public bool AddHitIndicator;

		/// <summary>
		/// When it is false, player wont see hit particles (default = true)
		/// </summary>
		public bool AddHitParticles;

		/// <summary>
		/// When it is false, player wont hear hit sound (default = true)
		/// </summary>
		public bool PlayHitSound;

		/// <summary>
		/// When it is false, player wont see safezone notification (default = true)
		/// </summary>
		public bool AddSZNotification;

		public override string ToString()
		{
			return $"Damage={Damage} Impulse={Impulse} HitNormal={HitNormal} HitPosition={HitPosition} HitShapeKey={HitShapeKey} " + $"HitEntity={HitEntity} Velocity={Velocity} HitMaterial={HitMaterial} HitVoxelMaterial={HitVoxelMaterial} AddDecals={AddDecals} " + $"AddHitParticles={AddHitParticles} PlayHitSound={PlayHitSound} AddSZNotification={AddSZNotification}";
		}
	}
}
