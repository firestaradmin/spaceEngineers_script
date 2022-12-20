using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.Models;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender.Messages;

namespace VRage.ModAPI
{
	public interface IMyEntity : VRage.Game.ModAPI.Ingame.IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets class that coordinates other components like <see cref="P:VRage.ModAPI.IMyEntity.Physics" />, <see cref="P:VRage.ModAPI.IMyEntity.Render" />, <see cref="P:VRage.ModAPI.IMyEntity.Hierarchy" />
		/// </summary>
		new MyEntityComponentContainer Components { get; }

		/// <summary>
		/// Gets or sets physics for object
		/// </summary>
		MyPhysicsComponentBase Physics { get; set; }

		/// <summary>
		/// Gets or sets position provider logic
		/// </summary>
		MyPositionComponentBase PositionComp { get; set; }

		/// <summary>
		/// Gets or sets render logic
		/// </summary>
		MyRenderComponentBase Render { get; set; }

		/// <summary>
		/// Gets or sets game logic for object.
		/// If there is more than 1 game logic attached it should be wrapped into <b>MyCompositeGameLogicComponent</b>.
		/// </summary>
		/// <seealso cref="T:VRage.Game.Components.MyGameLogicComponent" />
		MyEntityComponentBase GameLogic { get; set; }

		/// <summary>
		/// Gets or sets Hierarchy component
		/// </summary>
		MyHierarchyComponentBase Hierarchy { get; set; }

		/// <summary>
		/// Gets SyncObject used for synchronizing data over network with <b>VRage.Sync.Sync</b>
		/// </summary>
=======
		new MyEntityComponentContainer Components { get; }

		MyPhysicsComponentBase Physics { get; set; }

		MyPositionComponentBase PositionComp { get; set; }

		MyRenderComponentBase Render { get; set; }

		MyEntityComponentBase GameLogic { get; set; }

		MyHierarchyComponentBase Hierarchy { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		MySyncComponentBase SyncObject { get; }

		/// <summary>
		/// Custom storage for mods. Shared with all mods.
		/// </summary>
		/// <remarks>Not synced, but saved with blueprints.
		/// Only use set accessor if value is null.
		/// <code> Entity.Storage = new MyModStorageComponent(); </code>
		/// </remarks>
		MyModStorageComponentBase Storage { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or set some behavior of entity. <see cref="T:VRage.ModAPI.EntityFlags" />
		/// </summary>
		EntityFlags Flags { get; set; }

		/// <summary>
		/// Uniq id of entity. 
		/// </summary>
		/// <seealso cref="T:VRage.ModAPI.IMyEntities" />
		new long EntityId { get; set; }

		/// <summary>
		/// Uniq name of entity. Can be used to find entity by name
		/// </summary>
		new string Name { get; set; }

		/// <summary>
		/// Checked if <see cref="M:VRage.ModAPI.IMyEntity.Close" /> was called
		/// </summary>
		bool MarkedForClose { get; }

		/// <summary>
		/// Used for internal usage. Modders should not use it. Will be eventually removed
		/// </summary>
		bool DebugAsyncLoading { get; }

		/// <summary>
		/// Gets or sets <see cref="F:VRage.ModAPI.EntityFlags.Save" />. Entity won't be saved if <see cref="P:VRage.ModAPI.IMyEntity.Save" /> is false
		/// </summary>
		bool Save { get; set; }

		/// <summary>
		/// Gets or sets persistent flags that are used in rendering.
		/// </summary>
		MyPersistentEntityFlags2 PersistentFlags { get; set; }

		/// <summary>
		/// Gets model of block
		/// </summary>
		IMyModel Model { get; }

		/// <summary>
		/// Gets collision model of block
		/// </summary>
		IMyModel ModelCollision { get; }
=======
		EntityFlags Flags { get; set; }

		new long EntityId { get; set; }

		new string Name { get; set; }

		bool MarkedForClose { get; }

		bool Closed { get; }

		bool DebugAsyncLoading { get; }

		bool Save { get; set; }

		MyPersistentEntityFlags2 PersistentFlags { get; set; }

		IMyModel Model { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets or sets if the entity should be synced.
		/// </summary>
		bool Synchronized { get; set; }

		/// <summary>
		/// Gets or sets how often the entity should be updated.
		/// </summary>
		MyEntityUpdateEnum NeedsUpdate { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Gets parent of entity or null, if this entity doesn't have parent.
		/// Ex: character sitting in cockpit, has parent - cockpit, cockpit has parent - cube grid, connected grids also has main grid, which would be parent for other grids.
		/// </summary>
		IMyEntity Parent { get; }

		/// <summary>
		/// Gets or sets local matrix.
		/// When entity, has parent, it's world coordinates are calculated from localMatrix and parent world matrix
		/// </summary>
		Matrix LocalMatrix { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ModAPI.EntityFlags.Near" />
		/// </summary>
		bool NearFlag { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ObjectBuilders.MyPersistentEntityFlags2.CastShadows" />
		/// </summary>
		bool CastShadows { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ObjectBuilders.MyPersistentEntityFlags2.CastShadows" />
		/// </summary>
		bool FastCastShadowResolve { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ModAPI.EntityFlags.NeedsResolveCastShadow" />
		/// </summary>
		bool NeedsResolveCastShadow { get; set; }

		/// <summary>
		/// Not used in game anymore
		/// </summary>
		float MaxGlassDistSq { get; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ModAPI.EntityFlags.NeedsDraw" />
		/// </summary>
		bool NeedsDraw { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ModAPI.EntityFlags.NeedsDrawFromParent" />
		/// </summary>
		bool NeedsDrawFromParent { get; set; }

		/// <summary>
		/// Gets or sets <see cref="F:VRage.Game.Components.MyRenderComponentBase.Transparency" />. When setting true entity would be 25% transparent   
		/// </summary>
		bool Transparent { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="P:VRage.Game.Components.MyRenderComponentBase.ShadowBoxLod" />
		/// </summary>
		bool ShadowBoxLod { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="F:VRage.ModAPI.EntityFlags.SkipIfTooSmall" />
		/// </summary>
		bool SkipIfTooSmall { get; set; }

		/// <summary>
		/// Gets or sets flag <see cref="P:VRage.Game.Components.MyRenderComponentBase.Visible" />
		/// </summary>
		bool Visible { get; set; }

		/// <summary>
		/// Gets or sets if <see cref="P:VRage.ModAPI.IMyEntity.WorldMatrix" /> should be calculated when parent WorldMatrix is changed.
		/// </summary>
		/// <remarks>Enabling it on big amount entities may lower simulation speed</remarks>
		bool NeedsWorldMatrix { get; set; }

		/// <summary>
		/// Gets or set if grid is InScene. Objects that are not in scene are not updated and drawn.
		/// </summary>
		bool InScene { get; set; }

		/// <summary>
		/// Gets if entity is invalidated on move
		/// When visual look of entity depends on position - then InvalidateOnMove should be true
		/// </summary>
		bool InvalidateOnMove { get; }

		/// <summary>
		/// Gets or sets world matrix. 
		/// </summary>
		/// <seealso cref="T:VRageMath.MatrixD" />
		new MatrixD WorldMatrix { get; set; }

		/// <summary>
		/// Get scaled, inverted world matrix. Same as <see cref="M:VRage.ModAPI.IMyEntity.GetViewMatrix" />, <see cref="M:VRage.ModAPI.IMyEntity.GetWorldMatrixNormalizedInv" />, <see cref="P:VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv" />, but not normalized
		/// </summary>
		/// <returns>Matrix</returns>
		MatrixD WorldMatrixInvScaled { get; }

		/// <summary>
		/// Get normalized, inverted world matrix. Same as <see cref="M:VRage.ModAPI.IMyEntity.GetViewMatrix" />, <see cref="P:VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv" />
		/// </summary>
		/// <returns>Matrix</returns>
		MatrixD WorldMatrixNormalizedInv { get; }

		/// <summary>
		/// Always returns false
		/// </summary>
		bool IsVolumetric { get; }

		/// <summary>
		/// Gets or sets local axis aligned bounding box. Same as <see cref="P:VRage.ModAPI.IMyEntity.LocalAABBHr" />, <see cref="P:VRage.Game.Components.MyPositionComponentBase.LocalAABB" />
		/// </summary>
		BoundingBox LocalAABB { get; set; }

		/// <summary>
		/// Gets local axis aligned bounding box. Same as <see cref="P:VRage.ModAPI.IMyEntity.LocalAABB" />, <see cref="P:VRage.Game.Components.MyPositionComponentBase.LocalAABB" />
		/// </summary>
		BoundingBox LocalAABBHr { get; }

		/// <summary>
		/// Gets or sets local volume. Same as <see cref="P:VRage.Game.Components.MyPositionComponentBase.LocalVolume" />
		/// </summary>
		BoundingSphere LocalVolume { get; set; }

		/// <summary>
		/// Gets or sets local volume offset. Same as <see cref="P:VRage.Game.Components.MyPositionComponentBase.LocalVolumeOffset" />
		/// </summary>
=======
		IMyEntity Parent { get; }

		Matrix LocalMatrix { get; set; }

		bool NearFlag { get; set; }

		bool CastShadows { get; set; }

		bool FastCastShadowResolve { get; set; }

		bool NeedsResolveCastShadow { get; set; }

		float MaxGlassDistSq { get; }

		bool NeedsDraw { get; set; }

		bool NeedsDrawFromParent { get; set; }

		bool Transparent { get; set; }

		bool ShadowBoxLod { get; set; }

		bool SkipIfTooSmall { get; set; }

		bool Visible { get; set; }

		bool NeedsWorldMatrix { get; set; }

		bool InScene { get; set; }

		bool InvalidateOnMove { get; }

		new MatrixD WorldMatrix { get; set; }

		MatrixD WorldMatrixInvScaled { get; }

		MatrixD WorldMatrixNormalizedInv { get; }

		bool IsVolumetric { get; }

		BoundingBox LocalAABB { get; set; }

		BoundingBox LocalAABBHr { get; }

		BoundingSphere LocalVolume { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		Vector3 LocalVolumeOffset { get; set; }

		[Obsolete]
		Vector3D LocationForHudMarker { get; }

		[Obsolete]
		bool IsCCDForProjectiles { get; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets user friendly name of entity
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		new string DisplayName { get; set; }

		/// <summary>
		/// Called when <see cref="M:VRage.ModAPI.IMyEntity.Close" /> is called. Order 1) <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> 2) <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" /> 3)  <see cref="E:VRage.ModAPI.IMyEntity.OnClose" />. 
		/// </summary>
		/// <remarks>Modders should prefer <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> or <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" /></remarks>
		event Action<IMyEntity> OnClose;

		/// <summary>
		/// Called when <see cref="M:VRage.ModAPI.IMyEntity.Close" /> is called. Order 1) <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> 2) <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" /> 3)  <see cref="E:VRage.ModAPI.IMyEntity.OnClose" />. 
		/// </summary>
		/// <remarks>Modders should prefer <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> or <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" /></remarks>
		event Action<IMyEntity> OnClosing;

		/// <summary>
		/// Called when <see cref="M:VRage.ModAPI.IMyEntity.Close" /> is called. Order 1) <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> 2) <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" /> 3)  <see cref="E:VRage.ModAPI.IMyEntity.OnClose" />. 
		/// </summary>
		/// <remarks>Modders should prefer <see cref="E:VRage.ModAPI.IMyEntity.OnMarkForClose" /> or <see cref="E:VRage.ModAPI.IMyEntity.OnClosing" />. This event may not be invoked at all, when calling MyEntities.CloseAll, marking is bypassed</remarks>
		event Action<IMyEntity> OnMarkForClose;

		/// <summary>
		/// Called when havok rigid body physics are changed: inited, closed, modified.
		/// </summary>
		event Action<IMyEntity> OnPhysicsChanged;

		/// <summary>
		/// Not used. Actually not a friendly name
		/// </summary>
		/// <returns>String.Empty or "MyVoxelMap"</returns>
		string GetFriendlyName();

		/// <summary>
		/// This method marks this entity for close which means, that Close
		/// will be called after all entities are updated
		/// </summary>
		void Close();

		/// <summary>
		/// Performs real cleaning of entity. Should be called by game. Modders should prefer <see cref="M:VRage.ModAPI.IMyEntity.Close" /> method.  
		/// </summary>
		void Delete();

		/// <summary>
		/// Returns object builder - object representing block state, and allows restore it. Used in game save, or syncing over network.
		/// </summary>
		/// <param name="copy">When true, <see cref="P:VRage.ModAPI.IMyEntity.Name" /> won't be saved. Copy true comes only from MyGridClipboard/MyFloatingObjectClipboard/MyVoxelClipboard</param>
		/// <returns>Object builder</returns>
		MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false);

		/// <summary>
		/// Called before method GetObjectBuilder, when saving sector
		/// </summary>
		void BeforeSave();

		/// <summary>
		/// Gets top most <see cref="P:VRage.ModAPI.IMyEntity.Parent" /> of specified type.
		/// Top most is entity that doesn't have parent (of specified type)
		/// </summary>
		/// <param name="type">Type of parent. When type is null, type check disabled</param>
		/// <returns>Entity</returns>
		IMyEntity GetTopMostParent(Type type = null);

		/// <summary>
		/// Sets local matrix.
		/// When entity, has parent, it's world coordinates are calculated from localMatrix and parent world matrix
		/// </summary>
		/// <param name="localMatrix">New matrix</param>
		/// <param name="source">Object that caused this event. Not used in anyway</param>
		void SetLocalMatrix(Matrix localMatrix, object source = null);

		/// <summary>
		/// Gets children of entity. Child - entity, who's <see cref="P:VRage.ModAPI.IMyEntity.Parent" /> is this entity
		/// </summary>
		/// <param name="children">List, that would receive results</param>
		/// <param name="collect">When returns true - child added to list</param>
		void GetChildren(List<IMyEntity> children, Func<IMyEntity, bool> collect = null);

		/// <summary>
		/// Gets subpart by subpart name
		/// </summary>
		/// <param name="name">Name of subpart. Keep in mind that subpart names, should not start with `subpart_`</param>
		/// <returns>Subpart if it is found, or crashes if subpart not found</returns>
		/// <remarks>If you press Alt+11, enable `Debug draw` and `Model dummies` then you'll see all subpart names. </remarks>
		MyEntitySubpart GetSubpart(string name);

		/// <summary>
		/// Gets subpart by subpart name
		/// </summary>
		/// <param name="name">Name of subpart. Keep in mind that subpart names, should not start with `subpart_`</param>
		/// <param name="subpart">Subpart if it is found</param>
		/// <returns>True if subpart is found</returns>
		/// <remarks>If you press Alt+11, enable `Debug draw` and `Model dummies` then you'll see all subpart names. </remarks>
		bool TryGetSubpart(string name, out MyEntitySubpart subpart);

		/// <summary>
		/// Gets render diffuse color
		/// </summary>
		/// <returns>Diffuse color</returns>
		Vector3 GetDiffuseColor();

		/// <summary>
		/// Gets or result of function <see cref="M:VRage.Game.Components.MyRenderComponentBase.IsVisible" />. Function inside check for <see cref="M:VRage.ModAPI.IMyEntities.IsVisible(VRage.ModAPI.IMyEntity)" />
		/// </summary>
		/// <returns>True if entity should be drawn</returns>
		bool IsVisible();

		/// <summary>
		/// Calls debug draw of entity
		/// </summary>
		void DebugDraw();

		/// <summary>
		/// Calls special debug draw, that highlighting invalid triangles in model
		/// </summary>
		void DebugDrawInvalidTriangles();

		/// <summary>
		/// Allows subparts have different color than their parent
		/// </summary>
		/// <param name="enable"></param>
		void EnableColorMaskForSubparts(bool enable);

		/// <summary>
		/// Sets subparts custom color
		/// </summary>
		/// <param name="colorMaskHsv">Color</param>
		void SetColorMaskForSubparts(Vector3 colorMaskHsv);

		/// <summary>
		/// Sets subparts custom skinning. Copy values from <see cref="P:VRage.Game.Components.MyRenderComponentBase.TextureChanges" />, then change needed keys.
		/// You can retrieve values for exact skin with following code: <code>MyDefinitionManager.Static.GetAssetModifierDefinitionForRender(skinId);</code>
		/// </summary>
		/// <param name="value">Key - name of texture, value - path to texture files</param>
		void SetTextureChangesForSubparts(Dictionary<string, MyTextureChange> value);

		/// <summary>
		/// Sets the emissive value of a specific emissive material on entity.
		/// </summary>
		/// <param name="emissiveName">The name of the emissive material (ie. "Emissive0")</param>
		/// <param name="emissivity">Level of emissivity (0 is off, 1 is full brightness)</param>
		/// <param name="emissivePartColor">Color to emit</param>
		void SetEmissiveParts(string emissiveName, Color emissivePartColor, float emissivity);

		/// <summary>
		/// Sets the emissive value of a specific emissive material on all entity subparts.
		/// </summary>
		/// <param name="emissiveName">The name of the emissive material (ie. "Emissive0")</param>
		/// <param name="emissivity">Level of emissivity (0 is off, 1 is full brightness).</param>
		/// <param name="emissivePartColor">Color to emit</param>
		void SetEmissivePartsForSubparts(string emissiveName, Color emissivePartColor, float emissivity);

		/// <summary>
		/// Distance from camera to bounding sphere of this phys object. Result is always positive, even if camera is inside the sphere. (in meters)
		/// </summary>
		/// <returns>Distance in meters</returns>
		float GetDistanceBetweenCameraAndBoundingSphere();

		/// <summary>
		/// Distance from camera to position of entity.
		/// </summary>
		/// <returns>Distance in meters</returns>
		float GetDistanceBetweenCameraAndPosition();

		/// <summary>
		/// Largest distance from camera to bounding sphere of this phys object. Result is always positive, even if camera is inside the sphere.
		/// It's actually distance between camera and opposite side of the sphere
		/// </summary>
		/// <returns>Distance in meters</returns>
		float GetLargestDistanceBetweenCameraAndBoundingSphere();

		/// <summary>
		/// Smallest distance between camera and bounding sphere of this phys object. Result is always positive, even if camera is inside the sphere.
		/// </summary>
		/// <returns>Distance in meters</returns>
		float GetSmallestDistanceBetweenCameraAndBoundingSphere();

		/// <summary>
		/// Remove entity and it's children from scene: stops updates and render, deactivates physics. Usually called when entity deleted 
		/// </summary>
		/// <param name="source">Object that triggered removing from scene</param>
		void OnRemovedFromScene(object source);

		/// <summary>
		/// Adds entity to scene: init updates, render physics
		/// </summary>
		/// <param name="source">Object that triggered adding from scene</param>
		void OnAddedToScene(object source);

		/// <summary>
		/// Get normalized, inverted world matrix. Same as <see cref="M:VRage.ModAPI.IMyEntity.GetWorldMatrixNormalizedInv" />, <see cref="P:VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv" />
		/// </summary>
		/// <returns>Matrix</returns>
		MatrixD GetViewMatrix();

		/// <summary>
		/// Get normalized, inverted world matrix. Same as <see cref="M:VRage.ModAPI.IMyEntity.GetViewMatrix" />, <see cref="P:VRage.ModAPI.IMyEntity.WorldMatrixNormalizedInv" />
		/// </summary>
		/// <returns>Matrix</returns>
		MatrixD GetWorldMatrixNormalizedInv();

		/// <summary>
		/// Sets world matrix of entity.
		/// </summary>
		/// <param name="worldMatrix">New world matrix</param>
		/// <param name="source">Object that triggered set of new matrix</param>
		void SetWorldMatrix(MatrixD worldMatrix, object source = null);

		/// <summary>
		/// Set WorldMatrix's <see cref="P:VRageMath.MatrixD.Translation" />. Moves entity.
		/// </summary>
		/// <param name="pos">New position of entity</param>
		/// <seeaslo cref="M:VRage.ModAPI.IMyEntity.Teleport(VRageMath.MatrixD,System.Object,System.Boolean)" />
		void SetPosition(Vector3D pos);

		/// <summary>
		/// When moving entity over large distances or when entity has children, using this method preferred over <see cref="M:VRage.ModAPI.IMyEntity.SetPosition(VRageMath.Vector3D)" />
		/// </summary>
		/// <param name="pos">Teleport destination</param>
		/// <param name="source">Object that triggered </param>
		/// <param name="ignoreAssert">Do extra validation</param>
		void Teleport(MatrixD pos, object source = null, bool ignoreAssert = false);

		/// <summary>
		/// Get intersection of model with provided line
		/// </summary>
		/// <param name="line">Line that should intersect model</param>
		/// <param name="tri">Returns model first triangle that intersects</param>
		/// <param name="flags">Mode of work</param>
		/// <returns>True when line intersects models</returns>
		bool GetIntersectionWithLine(ref LineD line, out MyIntersectionResultLineTriangleEx? tri, IntersectionFlags flags);

		/// <summary>
		/// Calculates intersection of line with any bounding sphere in this model instance. Center of the bounding sphere will be returned.
		/// It takes boundingSphereRadiusMultiplier argument which serves for extending the influence (radius) for interaction with line. 
		/// </summary>
		/// <param name="line">Line to check</param>
		/// <param name="boundingSphereRadiusMultiplier">Bounding sphere radius would be multiplied by this value</param>
		/// <returns>Position of intersection if of line and bounding sphere</returns>
		Vector3D? GetIntersectionWithLineAndBoundingSphere(ref LineD line, float boundingSphereRadiusMultiplier);

		/// <summary>
		/// Return true if object intersects specified sphere.
		/// </summary>
		/// <param name="sphere">Sphere to check</param>
		/// <returns>True if intersects</returns>
		bool GetIntersectionWithSphere(ref BoundingSphereD sphere);

		/// <summary>
		/// Return true if object intersects specified bounding box.
		/// </summary>
		/// <param name="aabb">Bounding box to check</param>
		/// <returns>True if intersects</returns>
		bool GetIntersectionWithAABB(ref BoundingBoxD aabb);

		/// <summary>
		/// Return list of triangles intersecting specified sphere. Angle between every triangleVertexes normal vector and 'referenceNormalVector'
		/// is calculated, and if more than 'maxAngle', we ignore such triangleVertexes.
		/// Triangles are returned in 'retTriangles', and this list must be preallocated!
		/// IMPORTANT: Sphere must be in model space, so don't transform it!
		/// </summary>
		/// <param name="sphere">Sphere to check</param>
		/// <param name="referenceNormalVector">Used in filtering triangles</param>
		/// <param name="maxAngle">Max angle between referenceNormalVector and every triangleVertex of model</param>
		/// <param name="retTriangles">Triangles would be added here</param>
		/// <param name="maxNeighbourTriangles">Limit of added triangles</param>
		void GetTrianglesIntersectingSphere(ref BoundingSphere sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles);

		/// <summary>
		/// Checks if intersects Works only with <see cref="T:VRage.ModAPI.IMyVoxelBase" />
		/// </summary>
		/// <param name="sphereRadius">Radius of sphere</param>
		/// <param name="spherePos">World position</param>
		/// <returns>True if intersects</returns>
		bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos);

		/// <summary>
		/// Simply get the MyInventoryBase component stored in this entity.
		/// </summary>
		/// <returns>Null, or first inventory</returns>
		new VRage.Game.ModAPI.IMyInventory GetInventory();

		/// <summary>
		/// Search for inventory component with matching index.
		/// </summary>
		/// <param name="index">Index of inventory, starting from 0</param>
		/// <returns>Null, or inventory at matching index</returns>
		new VRage.Game.ModAPI.IMyInventory GetInventory(int index);

		[Obsolete("Only used during Sandbox removal.")]
		void AddToGamePruningStructure();

		[Obsolete("Only used during Sandbox removal.")]
		void RemoveFromGamePruningStructure();

		/// <summary>
		/// Update position of entity in MyGamePruningStructure. Calls: <code>MyGamePruningStructure.Move(this)</code>
		/// </summary>
		void UpdateGamePruningStructure();
	}
}
