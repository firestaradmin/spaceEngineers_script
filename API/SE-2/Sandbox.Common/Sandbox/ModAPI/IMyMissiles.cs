using System;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Interface for controlling missiles
	/// </summary>
	public interface IMyMissiles
	{
		/// <summary>
		/// Called when missile was created
		/// </summary>
		event Action<IMyMissile> OnMissileAdded;

		/// <summary>
		/// Called when missile was removed
		/// </summary>
		event Action<IMyMissile> OnMissileRemoved;

		/// <summary>
		/// Called each frame after missile was moved
		/// </summary>
		event MissileMoveDelegate OnMissileMoved;

		/// <summary>
		/// Called when missile hits something. May be more than 1 call per missile.
		/// </summary>
		event Action<IMyMissile> OnMissileCollided;

		/// <summary>
		/// Removes missile with EntityId
		/// </summary>
		/// <param name="entityId">Missile with this entityId should be removed</param>
		void Remove(long entityId);

		/// <summary>
		/// Returns all missiles in sphere
		/// </summary>
		/// <param name="sphere">Bounding sphere</param>
		/// <param name="result">List, that were results would be added</param>
		void GetAllMissilesInSphere(ref BoundingSphereD sphere, List<MyEntity> result);
	}
}
