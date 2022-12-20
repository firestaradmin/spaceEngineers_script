using VRage.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes projectile  that have custom logic on handling bullets (mods interface)
	/// </summary>
	public interface IMyProjectileDetector
	{
		/// <summary>
		/// Gets if detector is enabled
		/// </summary>
		bool IsDetectorEnabled { get; }

		/// <summary>
		/// Gets entity which was hit for this detector. It's not used for any logic with data, just for reporting to projectile system, which entity was hit by the projectile.
		/// </summary>
		/// <returns>Entity that projectile detector represents</returns>
		IMyEntity HitEntity { get; }

		/// <summary>
		/// Gets AABB of the detector
		/// </summary>
		/// <returns>Detector AABB in world coordinates</returns>
		BoundingBoxD DetectorAABB { get; }

		/// <summary>
		/// Gets intersection between line and detector
		/// </summary>
		/// <param name="line">Line of the bullet</param>
		/// <param name="hit">World hit position</param>
		/// <returns>Should return true if line intersects detector</returns>
		bool GetDetectorIntersectionWithLine(ref LineD line, out Vector3D? hit);
	}
}
