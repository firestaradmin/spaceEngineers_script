using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyPhysics
	{
		/// <summary>
		/// Number of physics steps done in last second
		/// </summary>
		int StepsLastSecond { get; }

		/// <summary>
		/// Simulation ratio, when physics cannot keep up, this is smaller than 1
		/// </summary>
		float SimulationRatio { get; }

		/// <summary>
		/// The server's simulation ratio.
		/// When physics cannot keep up server side this is smaller than 1.
		/// </summary>
		float ServerSimulationRatio { get; }

		/// <summary>
		/// Finds closest or any object on the path of the ray from-&gt;to. Uses Storage for voxels for faster 
		/// search but only good for long rays (more or less more than 50m). Use it only in such cases.
		/// </summary>
		/// <param name="from">Start of the ray.</param>
		/// <param name="to">End of the ray.</param>
		/// <param name="hitInfo">Hit info</param>
		/// <param name="any">Indicates if method should return any object found (May not be closest)</param>
		/// <returns>true if hit, false if no hit</returns>
		bool CastLongRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, bool any);

		/// <summary>
		/// Cast a ray and returns all matching entities.
		/// Must not be called from parallel thread!!!
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>
		/// <param name="toList">List of hits</param>
		/// <param name="raycastFilterLayer">Collision filter.</param>
		void CastRay(Vector3D from, Vector3D to, List<IHitInfo> toList, int raycastFilterLayer = 0);

		/// <summary>
		/// Cast a ray and return first entity.
		/// Must not be called from parallel thread!!!
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>
		/// <param name="hitInfo">Hit info</param>
		/// <param name="raycastFilterLayer">Collision filter.</param>
		/// <returns>true if hit; false if no hit</returns>
		bool CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, int raycastFilterLayer = 0);

		/// <summary>
		/// Cast a ray and return first entity.
		/// Must not be called from parallel thread!!!
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>
		/// <param name="hitInfo">Hit info</param>
		/// <param name="raycastCollisionFilter">Collision filter.</param>
		/// <param name="ignoreConvexShape"></param>
		/// <returns>true if hit; false if no hit</returns>
		bool CastRay(Vector3D from, Vector3D to, out IHitInfo hitInfo, uint raycastCollisionFilter, bool ignoreConvexShape);

		/// <summary>
		/// Ensure aabb is inside only one subspace. If no, reorder.
		/// Must not be called from parallel thread!!!
		/// </summary>
		/// <param name="aabb"></param>
		void EnsurePhysicsSpace(BoundingBoxD aabb);

		/// <summary>
		/// Given a string, gets the numeric value for the collision layer. Default: 0.
		/// </summary>
		/// <param name="strLayer">Name of collision layer. See MyPhysics.CollisionLayers for valid names.</param>
		/// <returns>Numeric value from MyPhysics.CollisionLayers</returns>
		/// <remarks>Default string used if not valid: DefaultCollisionLayer</remarks>
		int GetCollisionLayer(string strLayer);

		/// <summary>
		/// Cast a ray and return first entity.
		/// May be called from parallel thread.
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>        
		/// <param name="raycastCollisionFilter">Collision filter.</param>
		/// <param name="callback">Callback were results are returned to when query is done</param>
		void CastRayParallel(ref Vector3D from, ref Vector3D to, int raycastCollisionFilter, Action<IHitInfo> callback);

		/// <summary>
		/// Cast a ray and returns all matching entities.
		/// May be called from parallel thread.
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>
		/// <param name="toList">List of hits</param>
		/// <param name="raycastCollisionFilter">Collision filter.</param>
		/// <param name="callback">Callback were results are returned to when query is done</param>
		void CastRayParallel(ref Vector3D from, ref Vector3D to, List<IHitInfo> toList, int raycastCollisionFilter, Action<List<IHitInfo>> callback);

		/// <summary>
		/// Returns current natural gravity at world position.
		/// </summary>
		/// <param name="worldPosition">Target position</param>
		/// <param name="naturalGravityInterference">Natural gravity affects artificial gravity strength.
		/// Use this value when calling <see cref="M:VRage.Game.ModAPI.IMyPhysics.CalculateArtificialGravityAt(VRageMath.Vector3D,System.Single)" /> to get effective (physical) strength of artificial gravity at point.</param>
		/// <returns></returns>
		Vector3 CalculateNaturalGravityAt(Vector3D worldPosition, out float naturalGravityInterference);

		/// <summary>
		/// Returns current artificial gravity at world position.
		/// </summary>
		/// <param name="worldPosition">Target position</param>
		/// <param name="naturalGravityInterference">Artificial gravity strength is affected by presence of natural gravity.
		/// Use <see cref="M:VRage.Game.ModAPI.IMyPhysics.CalculateNaturalGravityAt(VRageMath.Vector3D,System.Single@)" /> to get correct value for given point in space.
		/// Value of 1 indicates no natural gravity effect.</param>
		Vector3 CalculateArtificialGravityAt(Vector3D worldPosition, float naturalGravityInterference = 1f);
<<<<<<< HEAD

		/// <summary>
		/// WARNING:
		/// Entity must have ModelCollision.HavokCollisionShapes. You can init collision with following code:
		///
		/// var entity = new MyEntity();
		/// entity.Init(new StringBuilder("Test"), "Models\\Cubes\\Large\\GeneratorLarge.mwm", null, null, "Models\\Cubes\\Large\\GeneratorLarge.mwm");
		///
		/// </summary>
		/// <param name="settings"></param>
		void CreateModelPhysics(PhysicsSettings settings);

		void CreateBoxPhysics(PhysicsSettings settings, Vector3 halfExtends, float convexRadius);

		void CreateSpherePhysics(PhysicsSettings settings, float radius);

		void CreateCapsulePhysics(PhysicsSettings settings, Vector3 vertexA, Vector3 vertexB, float radius);

		ModAPIMass CreateMassCombined(ICollection<ModAPIMassElement> massElements);

		ModAPIMass CreateMassForBox(Vector3 halfExtents, float mass);

		ModAPIMass CreateMassForCapsule(Vector3 startAxis, Vector3 endAxis, float radius, float mass);

		ModAPIMass CreateMassForCylinder(Vector3 startAxis, Vector3 endAxis, float radius, float mass);

		ModAPIMass CreateMassForSphere(float radius, float mass);

		/// <summary>
		/// Used for create physics with collisions
		/// </summary>
		PhysicsSettings CreateSettingsForPhysics(IMyEntity entity, MatrixD worldMatrix, Vector3 localCenter, float linearDamping = 0f, float angularDamping = 0f, ushort collisionLayer = 15, RigidBodyFlag rigidBodyFlags = RigidBodyFlag.RBF_DEFAULT, bool isPhantom = false, ModAPIMass? mass = null);

		/// <summary>
		/// Used to create physical detectors. They don't have physical collisions, instead they provide trigger callbacks when Entities collide with their shape 
		/// </summary>
		PhysicsSettings CreateSettingsForDetector(IMyEntity entity, Action<IMyEntity, bool> detectorColliderCallback, MatrixD worldMatrix, Vector3 localCenter, RigidBodyFlag rigidBodyFlags = RigidBodyFlag.RBF_DEFAULT, ushort collisionLayer = 15, bool isPhantom = true);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
