using VRageMath;

namespace Sandbox.Game.Entities
{
	public interface IMyGravityProvider
	{
		bool IsWorking { get; }

		Vector3 GetWorldGravity(Vector3D worldPoint);

		/// <summary>
		/// Tests if the specified point is within the gravity of this entity.
		/// </summary>
		/// <param name="worldPoint">Point to test</param>
		/// <returns><b>true</b> if in range; <b>false</b> if not</returns>
		bool IsPositionInRange(Vector3D worldPoint);

		float GetGravityMultiplier(Vector3D worldPoint);

		void GetProxyAABB(out BoundingBoxD aabb);
	}
}
