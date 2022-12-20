using System;
using System.Collections.Generic;
using VRage.ObjectBuilders;
using VRageMath;

namespace VRage.ModAPI
{
	/// <summary>
	/// Provides API, that granting access to enitities (mods interface)
	/// </summary>
	/// <seealso cref="T:VRage.ModAPI.IMyEntity" />
	public interface IMyEntities
	{
		/// <summary>
		/// Called when <see cref="M:VRage.ModAPI.IMyEntities.RemoveEntity(VRage.ModAPI.IMyEntity)" /> called on entity
		/// </summary>
		event Action<IMyEntity> OnEntityRemove;

		/// <summary>
		/// Called when <see cref="M:VRage.ModAPI.IMyEntities.AddEntity(VRage.ModAPI.IMyEntity,System.Boolean)" /> called on entity
		/// </summary>
		event Action<IMyEntity> OnEntityAdd;

		/// <summary>
		/// Called when session unloads, before grids are closed
		/// </summary>
		event Action OnCloseAll;

		/// <summary>
		/// Called when entity gets <see cref="P:VRage.ModAPI.IMyEntity.Name" />. First string - old name, Second - new name
		/// </summary>
		event Action<IMyEntity, string, string> OnEntityNameSet;

		/// <summary>
		/// Returns entity with provided id
		/// </summary>
		/// <param name="id">EntityId</param>
		/// <param name="entity">Found entity</param>
		/// <returns>True if entity is found</returns>
		bool TryGetEntityById(long id, out IMyEntity entity);

		/// <summary>
		/// Returns entity with provided id
		/// </summary>
		/// <param name="id"><see cref="P:VRage.ModAPI.IMyEntity.EntityId" /></param>
		/// <param name="entity">Found entity</param>
		/// <returns>True if entity is found</returns>
		bool TryGetEntityById(long? id, out IMyEntity entity);

		/// <summary>
		/// Returns entity with provided name
		/// </summary>
		/// <param name="name"><see cref="P:VRage.ModAPI.IMyEntity.Name" /></param>
		/// <param name="entity">Found entity</param>
		/// <returns>True if entity is found</returns>
		bool TryGetEntityByName(string name, out IMyEntity entity);

		/// <summary>
		/// Returns if entity with provided name exists
		/// </summary>
		/// <param name="name"><see cref="P:VRage.ModAPI.IMyEntity.Name" /></param>
		/// <returns>True if entity exists</returns>
		bool EntityExists(string name);

		/// <summary>
		/// Registers entity
		/// </summary>
		/// <param name="entity">Entity that should be registered</param>
		/// <param name="insertIntoScene">When true <see cref="M:VRage.ModAPI.IMyEntity.OnAddedToScene(System.Object)" /> is called</param>
		void AddEntity(IMyEntity entity, bool insertIntoScene = true);

		/// <summary>
		/// Create entity from object builder
		/// </summary>
		/// <param name="objectBuilder">Object builder of entity</param>
		/// <returns>Created entity</returns>
		IMyEntity CreateFromObjectBuilder(MyObjectBuilder_EntityBase objectBuilder);

		/// <summary>
		/// Create entity from object builder, and then call <see cref="M:VRage.ModAPI.IMyEntities.AddEntity(VRage.ModAPI.IMyEntity,System.Boolean)" />
		/// </summary>
		/// <param name="objectBuilder">Object builder of entity</param>
		/// <returns>Created entity</returns>
		IMyEntity CreateFromObjectBuilderAndAdd(MyObjectBuilder_EntityBase objectBuilder);

		/// <summary>
		/// Unregisters entity
		/// </summary>
		/// <param name="entity">Entity that should be unregistered</param>
		void RemoveEntity(IMyEntity entity);

		/// <summary>
		/// Checks if sphere hits any <see cref="T:Havok.HkRigidBody" />
		/// </summary>
		/// <param name="bs">Sphere that used for intersection check</param>
		/// <returns>True if sphere hits any body</returns>
		bool IsSpherePenetrating(ref BoundingSphereD bs);

		/// <summary>
		/// Use to find place that doesn't have any voxels, grids, or physical bodies.
		/// If original position can't fill check sphere, new position in some distance is picked.
		/// Distance grows each testsPerDistance attempts. 
		/// Maximum distance from BasePos that can be used is calculated by formula: maxTestCount / testsPerDistance * radius * stepSize
		/// </summary>
		/// <param name="basePos">Base position</param>
		/// <param name="radius">Radius in which there should be nothing</param>
		/// <param name="maxTestCount">How many tries should be done, to find free space</param>
		/// <param name="testsPerDistance">Depends how often distance from original position grows</param>
		/// <param name="stepSize">How distance grows</param>
		/// <returns>Position that can doesn't have voxels, grids and other HkBodies in provided radius</returns>
		Vector3D? FindFreePlace(Vector3D basePos, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f);

		[Obsolete]
		void GetInflatedPlayerBoundingBox(ref BoundingBox playerBox, float inflation);

		/// <summary>
		/// Making playerBox include all connected players
		/// </summary>
		/// <param name="playerBox">Box, that would contain all players</param>
		/// <param name="inflation">Minimal distance between player, and border</param>
		void GetInflatedPlayerBoundingBox(ref BoundingBoxD playerBox, float inflation);

		[Obsolete]
		bool IsInsideVoxel(Vector3 pos, Vector3 hintPosition, out Vector3 lastOutsidePos);

		/// <summary>
		/// Return true if line between pos and hintPosition doesn't intersect any voxel, or intersects it even number of times
		/// Does inside physical ray casting inside
		/// </summary>
		/// <param name="pos">Position of object</param>
		/// <param name="hintPosition">Position of object few frames back to test whether object entered voxel. Usually pos - LinearVelocity * x, where x it time.</param>
		/// <param name="lastOutsidePos">Last position that was outside of voxels</param>
		bool IsInsideVoxel(Vector3D pos, Vector3D hintPosition, out Vector3D lastOutsidePos);

		/// <summary>
		/// Return whether world has limited size in kilometers
		/// </summary>
		/// <returns>True if limited</returns>
		bool IsWorldLimited();

		/// <summary>
		/// Returns shortest distance (i.e. axis-aligned) to the world border from the world center.
		/// Will be 0 if world is borderless
		/// </summary>
		float WorldHalfExtent();

		/// <summary>
		/// Returns shortest distance (i.e. axis-aligned) to the world border from the world center, minus 600m to make spawning somewhat safer.
		/// Will be 0 if world is borderless
		/// </summary>
		float WorldSafeHalfExtent();

		/// <summary>
		/// Returns true if distance from 0,0,0 to provided position is less than <see cref="M:VRage.ModAPI.IMyEntities.WorldHalfExtent" />
		/// </summary>
		/// <param name="pos">Checked position in world coordinates</param>
		/// <returns>True if distance is less than <see cref="M:VRage.ModAPI.IMyEntities.WorldHalfExtent" /></returns>
		bool IsInsideWorld(Vector3D pos);

		/// <summary>
		/// Returns true if raycast hits anything (with raycast layer=0)
		/// </summary>
		/// <param name="pos">From</param>
		/// <param name="target">To</param>
		/// <returns></returns>
		bool IsRaycastBlocked(Vector3D pos, Vector3D target);

		/// <summary>
		/// Apply <see cref="P:VRage.ModAPI.IMyEntity.Name" /> for entity
		/// </summary>
		/// <param name="IMyEntity">Entity that should be named</param>
		/// <param name="possibleRename">Allows renaming</param>
		void SetEntityName(IMyEntity IMyEntity, bool possibleRename = true);

		/// <summary>
		/// Checks if registered name belongs to this entity
		/// </summary>
		/// <param name="entity">Entity to test</param>
		/// <param name="name">Name to test</param>
		/// <returns>True if registered name belongs to this entity</returns>
		bool IsNameExists(IMyEntity entity, string name);

		/// <summary>
		/// Remove entity from lists of closed entities
		/// </summary>
		/// <param name="entity">Entity that should be removed</param>
		void RemoveFromClosedEntities(IMyEntity entity);

		/// <summary>
		/// Removes registered name from entity. 
		/// </summary>
		/// <param name="entity">Entity, that has name</param>
		void RemoveName(IMyEntity entity);

		/// <summary>
		/// Checks if entity is registered entity 
		/// </summary>
		/// <param name="entity">Entity to test</param>
		/// <returns>True if entity is registered</returns>
		bool Exist(IMyEntity entity);

		/// <summary>
		/// Mark entity as closed. Soon it would be deleted. Doesn't call <see cref="M:VRage.ModAPI.IMyEntity.Close" /> 
		/// </summary>
		/// <param name="entity">Entity to close</param>
		void MarkForClose(IMyEntity entity);

		/// <summary>
		/// Make entity receive UpdateBeforeSimulation, UpdateBefore10Simulation, UpdateBefore100Simulation, UpdateAfterSimulation, UpdateAfter10Simulation, UpdateAfter100Simulation, Simulate, UpdateBeforeNextFrame depending on it's <see cref="P:VRage.ModAPI.IMyEntity.NeedsUpdate" /> flags.
		/// Physics are still simulated
		/// </summary>
		/// <param name="entity">That should have updates</param>
		void RegisterForUpdate(IMyEntity entity);

		/// <summary>
		/// Make entity receive PrepareForDraw and sending to it's Render Draw call
		/// </summary>
		/// <param name="entity">That should be drawn</param>
		void RegisterForDraw(IMyEntity entity);

		/// <summary>
		/// Unregistering entity from following updates: UpdateBeforeSimulation, UpdateBefore10Simulation, UpdateBefore100Simulation, UpdateAfterSimulation, UpdateAfter10Simulation, UpdateAfter100Simulation, Simulate, UpdateBeforeNextFrame
		/// Physics are still simulated
		/// </summary>
		/// <param name="entity">Entity that should not receive updates anymore</param>
		/// <param name="immediate">When true, updates removed immediately, but may cause crashes</param>
		void UnregisterForUpdate(IMyEntity entity, bool immediate = false);

		/// <summary>
		/// Unregistering entity from PrepareForDraw events and it Render from Draw calls.
		/// Entity may still be rendered
		/// </summary>
		/// <param name="entity">Entity that should stop receive draw calls</param>
		void UnregisterForDraw(IMyEntity entity);

		/// <summary>
		/// Returns first found (not closest) entity that intersects with sphere
		/// </summary>
		/// <param name="sphere">Sphere to test (in world coordinates)</param>
		/// <returns>First found entity</returns>
		IMyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere);

		/// <summary>
		/// Returns first found (not closest) entity that intersects with sphere
		/// </summary>
		/// <param name="sphere">Sphere to test (in world coordinates)</param>
		/// <param name="ignoreEntity0">Return value can't be this entity</param>
		/// <param name="ignoreEntity1">Return value can't be this entity</param>
		/// <returns>First found entity, or null</returns>
		IMyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1);

		/// <summary>
		/// Returns first found (not closest) entity that intersects with sphere
		/// </summary>
		/// <param name="sphere">Sphere to test (in world coordinates)</param>
		/// <param name="ignoreEntity0">Return value can't be this entity</param>
		/// <param name="ignoreEntity1">Return value can't be this entity</param>
		/// <param name="ignoreVoxelMaps">When true, voxels won't checked</param>
		/// <param name="volumetricTest">When false physical shape checking used. It is much more accurate, but slower</param>
		/// <param name="excludeEntitiesWithDisabledPhysics">When true, entities with disabled physics won't checked</param>
		/// <param name="ignoreFloatingObjects">When true, floating objects won't checked</param>
		/// <param name="ignoreHandWeapons">When true, hand weapons (tools) won't checked</param>
		/// <returns>Found entity matching conditions</returns>
		IMyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, bool excludeEntitiesWithDisabledPhysics = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true);

		/// <summary>
		/// Returns entity with provided entityId
		/// </summary>
		/// <param name="entityId">EntityId</param>
		/// <returns>Entity with provided id, or null</returns>
		IMyEntity GetEntityById(long entityId);

		/// <summary>
		/// Returns entity with provided entityId
		/// </summary>
		/// <param name="entityId"><see cref="P:VRage.ModAPI.IMyEntity.EntityId" /></param>
		/// <returns>Entity with provided id, or null</returns>
		IMyEntity GetEntityById(long? entityId);

		/// <summary>
		/// Returns if entity with provided name exists
		/// </summary>
		/// <param name="entityId"><see cref="P:VRage.ModAPI.IMyEntity.EntityId" /></param>
		/// <returns>True if entity exists</returns>
		bool EntityExists(long entityId);

		/// <summary>
		/// Returns if entity with provided name exists
		/// </summary>
		/// <param name="entityId"><see cref="P:VRage.ModAPI.IMyEntity.EntityId" /></param>
		/// <returns>True if entity exists</returns>
		bool EntityExists(long? entityId);

		/// <summary>
		/// Returns entity with provided name
		/// </summary>
		/// <param name="name"><see cref="P:VRage.ModAPI.IMyEntity.Name" /></param>
		/// <returns>Entity with registered Name or null</returns>
		IMyEntity GetEntityByName(string name);

		/// <summary>
		/// Entities that inherit that type would be visible/invisible. 
		/// </summary>
		/// <param name="type">Type that class should inherit to be invisible, ex: IMyCubeGrid</param>
		/// <param name="hidden">Sets if inherited entities should be visible visible or not</param>
		void SetTypeHidden(Type type, bool hidden);

		/// <summary>
		/// Gets whether entities that inherit type is visible or not. Example: <code>IsTypeHidden(typeof (IMyCubeGrid))</code>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		bool IsTypeHidden(Type type);

		/// <summary>
		/// Gets whether entity is visible or not because of <see cref="M:VRage.ModAPI.IMyEntities.SetTypeHidden(System.Type,System.Boolean)" />
		/// </summary>
		/// <returns>True when visible</returns>
		bool IsVisible(IMyEntity entity);

		/// <summary>
		/// Revert all changes to default. Make all entities visible, that were hidden because of <see cref="M:VRage.ModAPI.IMyEntities.SetTypeHidden(System.Type,System.Boolean)" />  
		/// </summary>
		void UnhideAllTypes();

		/// <summary>
		/// Remaps this entity's <see cref="P:VRage.ModAPI.IMyEntity.EntityId" /> and <see cref="P:VRage.ModAPI.IMyEntity.Name" /> to a new values.
		/// </summary>
		/// <param name="objectBuilders">ObjectBuilders that should be remapped</param>
		void RemapObjectBuilderCollection(IEnumerable<MyObjectBuilder_EntityBase> objectBuilders);

		/// <summary>
		/// Remaps this entity's <see cref="P:VRage.ModAPI.IMyEntity.EntityId" /> and <see cref="P:VRage.ModAPI.IMyEntity.Name" /> to a new values.
		/// </summary>
		/// <param name="objectBuilder">ObjectBuilder that should be remapped</param>
		void RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder);

		/// <summary>
		/// Create new entity from objectBuilder, but doesn't call <b>Init(MyObjectBuilder_EntityBase objectBuilder)</b>
		/// </summary>
		/// <param name="objectBuilder"></param>
		/// <returns></returns>
		IMyEntity CreateFromObjectBuilderNoinit(MyObjectBuilder_EntityBase objectBuilder);

		/// <summary>
		/// Draw bounding box around entity
		/// </summary>
		/// <param name="entity">That should have visible bounding box</param>
		/// <param name="enable">When true, bounding box start draw around entity</param>
		/// <param name="color">Color of lines</param>
		/// <param name="lineWidth">With of lines</param>
		/// <param name="inflateAmount">Distance from original bounding box, from each side in meters</param>
		void EnableEntityBoundingBoxDraw(IMyEntity entity, bool enable, Vector4? color = null, float lineWidth = 0.01f, Vector3? inflateAmount = null);

		/// <summary>
		/// Get first entity that matching condition
		/// </summary>
		/// <param name="match">When return true, this entity would be used as a return value</param>
		/// <returns>First matching condition entity</returns>
		IMyEntity GetEntity(Func<IMyEntity, bool> match);

		/// <summary>
		/// Get all entities matching condition
		/// </summary>
		/// <param name="entities">This set would receive results. Can be null, but then collect function should always return false</param>
		/// <param name="collect">When it is null or returns true, provided hashset adds entity</param>
		void GetEntities(HashSet<IMyEntity> entities, Func<IMyEntity, bool> collect = null);

		/// <summary>
		/// Returns list of entities that intersects with sphere
		/// </summary>
		/// <param name="sphere">Sphere to test (in world coordinates)</param>
		/// <param name="ignoreEntity0">Returned list can't contain this entity</param>
		/// <param name="ignoreEntity1">Returned list can't contain this entity</param>
		/// <param name="ignoreVoxelMaps">When true, voxels won't checked</param>
		/// <param name="volumetricTest">When false physical shape checking used. It is much more accurate, but slower</param>
		/// <returns>List of entities inside of sphere</returns>
		/// <remarks>Returned list may be used by system, next call if this or other similar function will clear list, so if you need to store data for long time, copy data from it. Also clean list, after you don't need it anymore</remarks>
		List<IMyEntity> GetIntersectionWithSphere(ref BoundingSphereD sphere, IMyEntity ignoreEntity0, IMyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest);

		/// <summary>
		/// Returns list of entities that intersects with BoundingBox.
		/// This function will return CubeBlocks. This function works slower than <see cref="M:VRage.ModAPI.IMyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@)" />
		/// </summary>
		/// <param name="boundingBox">Bounding box in world coordinates</param>
		/// <returns>New list of entities</returns>
		/// <remarks>Same as <see cref="M:VRage.ModAPI.IMyEntities.GetElementsInBox(VRageMath.BoundingBoxD@)" /></remarks>
		List<IMyEntity> GetEntitiesInAABB(ref BoundingBoxD boundingBox);

		/// <summary>
		/// Returns list of entities that intersects with sphere.
		/// This function will return CubeBlocks. This function works slower than <see cref="M:VRage.ModAPI.IMyEntities.GetTopMostEntitiesInSphere(VRageMath.BoundingSphereD@)" />
		/// </summary>
		/// <param name="boundingSphere">Bounding sphere in world coordinates</param>
		/// <returns>New list of entities</returns>
		List<IMyEntity> GetEntitiesInSphere(ref BoundingSphereD boundingSphere);

		/// <summary>
		/// Returns list of entities that intersects with BoundingBox.
		/// This function will return CubeBlocks. This function works slower than <see cref="M:VRage.ModAPI.IMyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@)" />
		/// </summary>
		/// <param name="boundingBox">Bounding box in world coordinates</param>
		/// <returns>New list of entities</returns>
		/// <remarks>Same as <see cref="M:VRage.ModAPI.IMyEntities.GetEntitiesInAABB(VRageMath.BoundingBoxD@)" /></remarks>
		List<IMyEntity> GetElementsInBox(ref BoundingBoxD boundingBox);

		/// <summary>
		/// Returns list of `TopMost` entities that intersects with sphere.
		/// This function won't return CubeBlocks. Use <see cref="M:VRage.ModAPI.IMyEntities.GetEntitiesInSphere(VRageMath.BoundingSphereD@)" /> to retrieve CubeBlocks also.  
		/// </summary>
		/// <param name="boundingSphere">Bounding sphere in world coordinates</param>
		/// <returns>New list of entities</returns>
		List<IMyEntity> GetTopMostEntitiesInSphere(ref BoundingSphereD boundingSphere);

		/// <summary>
		/// Returns list of `TopMost` entities that intersects with bounding box.
		/// This function won't return CubeBlocks. Use <see cref="M:VRage.ModAPI.IMyEntities.GetElementsInBox(VRageMath.BoundingBoxD@)" /> to retrieve CubeBlocks also.  
		/// </summary>
		/// <param name="boundingBox">Bounding box in world coordinates</param>
		/// <returns>New list of entities</returns>
		List<IMyEntity> GetTopMostEntitiesInBox(ref BoundingBoxD boundingBox);

		/// <summary>
		/// Creates and asynchronously initializes and entity.
		/// </summary>
		/// <param name="objectBuilder">Object builder of grid</param>
		/// <param name="addToScene">Call <see cref="M:VRage.ModAPI.IMyEntities.AddEntity(VRage.ModAPI.IMyEntity,System.Boolean)" /> and call OnAddedToScene</param>
		/// <param name="completionCallback">Callback called in main thread. </param>
		/// <returns>Create <b>but not inited yet</b> entity. Entity would be inited correctly after callback trigger</returns>
		IMyEntity CreateFromObjectBuilderParallel(MyObjectBuilder_EntityBase objectBuilder, bool addToScene = false, Action<IMyEntity> completionCallback = null);
	}
}
