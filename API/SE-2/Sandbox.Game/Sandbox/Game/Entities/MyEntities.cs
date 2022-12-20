using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Havok;
using ParallelTasks;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Definitions.SessionComponents;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	[StaticEventOwner]
	public static class MyEntities
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct AsyncUpdateToken : IDisposable
		{
<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void Dispose()
			{
				IsAsyncUpdateInProgress = false;
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Holds data for asynchronous entity init
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public class InitEntityData : WorkData
		{
			private readonly MyObjectBuilder_EntityBase m_objectBuilder;

			private readonly bool m_addToScene;

			private readonly Action<MyEntity> m_completionCallback;

			private MyEntity m_entity;

			private List<IMyEntity> m_resultIDs;

			private readonly MyEntity m_relativeSpawner;

			private Vector3D? m_relativeOffset;

			private readonly bool m_checkPosition;

			private readonly bool m_fadeIn;

			public InitEntityData(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> completionCallback, MyEntity entity, bool fadeIn, MyEntity relativeSpawner = null, Vector3D? relativeOffset = null, bool checkPosition = false)
			{
				m_objectBuilder = objectBuilder;
				m_addToScene = addToScene;
				m_completionCallback = completionCallback;
				m_entity = entity;
				m_fadeIn = fadeIn;
				m_relativeSpawner = relativeSpawner;
				m_relativeOffset = relativeOffset;
				m_checkPosition = checkPosition;
			}

			public (bool Success, MyEntity Entity) CallInitEntity(bool tolerateBlacklistedPlanets = false)
			{
				try
				{
					MyEntityIdentifier.InEntityCreationBlock = true;
					MyEntityIdentifier.LazyInitPerThreadStorage(2048);
					m_entity.Render.FadeIn = m_fadeIn;
					return (InitEntity(m_objectBuilder, ref m_entity, tolerateBlacklistedPlanets), m_entity);
				}
				finally
				{
					m_resultIDs = new List<IMyEntity>();
					MyEntityIdentifier.GetPerThreadEntities(m_resultIDs);
					MyEntityIdentifier.ClearPerThreadEntities();
					MyEntityIdentifier.InEntityCreationBlock = false;
				}
			}

			public void OnEntityInitialized()
			{
				if (m_relativeSpawner != null && m_relativeOffset.HasValue)
				{
					MatrixD worldMatrix = m_entity.WorldMatrix;
					worldMatrix.Translation = m_relativeSpawner.WorldMatrix.Translation + m_relativeOffset.Value;
					m_entity.WorldMatrix = worldMatrix;
				}
				MyCubeGrid myCubeGrid = m_entity as MyCubeGrid;
				if (MyFakes.ENABLE_GRID_PLACEMENT_TEST && m_checkPosition && myCubeGrid != null && myCubeGrid.CubeBlocks.get_Count() == 1)
				{
					MyGridPlacementSettings settings = MyBlockBuilderBase.CubeBuilderDefinition.BuildingSettings.GetGridPlacementSettings(myCubeGrid.GridSizeEnum, myCubeGrid.IsStatic);
					if (!MyCubeGrid.TestPlacementArea(myCubeGrid, myCubeGrid.IsStatic, ref settings, myCubeGrid.PositionComp.LocalAABB, dynamicBuildMode: false))
					{
						MyLog.Default.Info($"OnEntityInitialized removed entity '{myCubeGrid.Name}:{myCubeGrid.DisplayName}' with entity id '{myCubeGrid.EntityId}'");
						m_entity.Close();
						return;
					}
				}
				foreach (IMyEntity resultID in m_resultIDs)
				{
					MyEntityIdentifier.TryGetEntity(resultID.EntityId, out var entity);
					if (entity != null)
					{
						MyLog.Default.WriteLineAndConsole("Dropping entity with duplicated id: " + resultID.EntityId);
						resultID.Close();
					}
					else
					{
						MyEntityIdentifier.AddEntityWithId(resultID);
					}
				}
				if (m_entity != null && m_entity.EntityId != 0L)
				{
					if (m_addToScene)
					{
						bool insertIntoScene = (m_objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
						Add(m_entity, insertIntoScene);
					}
					if (m_completionCallback != null)
					{
						m_completionCallback(m_entity);
					}
				}
			}
		}

		private struct BoundingBoxDrawArgs
		{
			public Color Color;

			public float LineWidth;

			public Vector3 InflateAmount;

			public MyStringId LineMaterial;

			public bool WithAxis;
		}

		protected sealed class OnEntityCloseRequest_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnEntityCloseRequest(entityId);
			}
		}

		protected sealed class ForceCloseEntityOnClients_003C_003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in long entityId, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ForceCloseEntityOnClients(entityId);
			}
		}

		private static MyConcurrentHashSet<MyEntity> m_entities;

		private static ConcurrentCachingList<IMyEntity> m_entitiesForDraw;

		private static readonly List<IMySceneComponent> m_sceneComponents;

		public static IMyUpdateOrchestrator Orchestrator;

		[ThreadStatic]
		private static MyEntityIdRemapHelper m_remapHelper;

		public static bool IsClosingAll;

		public static bool IgnoreMemoryLimits;

		private static MyEntityCreationThread m_creationThread;

		private static Dictionary<uint, IMyEntity> m_renderObjectToEntityMap;

		private static readonly FastResourceLock m_renderObjectToEntityMapLock;

		[ThreadStatic]
		private static List<MyEntity> m_overlapRBElementList;

		private static readonly List<List<MyEntity>> m_overlapRBElementListCollection;

		private static List<HkBodyCollision> m_rigidBodyList;

		private static readonly List<MyLineSegmentOverlapResult<MyEntity>> LineOverlapEntityList;

		private static readonly List<MyPhysics.HitInfo> m_hits;

		[ThreadStatic]
		private static HashSet<IMyEntity> m_entityResultSet;

		private static readonly List<HashSet<IMyEntity>> m_entityResultSetCollection;

		[ThreadStatic]
		private static List<MyEntity> m_entityInputList;

		private static readonly List<List<MyEntity>> m_entityInputListCollection;

		private static HashSet<MyEntity> m_entitiesToDelete;

		private static HashSet<MyEntity> m_entitiesToDeleteNextFrame;

		public static ConcurrentDictionary<string, MyEntity> m_entityNameDictionary;

		private static bool m_isLoaded;

		private static HkShape m_cameraSphere;

		public static FastResourceLock EntityCloseLock;

		public static FastResourceLock EntityMarkForCloseLock;

		public static FastResourceLock UnloadDataLock;

		public static bool UpdateInProgress;

		public static bool CloseAllowed;

		[ThreadStatic]
		private static List<MyEntity> m_allIgnoredEntities;

		private static readonly List<List<MyEntity>> m_allIgnoredEntitiesCollection;

		/// <summary>
		/// Types in this set and their subtypes will be temporarily invisible.
		/// </summary>
		private static readonly HashSet<Type> m_hiddenTypes;

		public static bool SafeAreasHidden;

		public static bool SafeAreasSelectable;

		public static bool DetectorsHidden;

		public static bool DetectorsSelectable;

		public static bool ParticleEffectsHidden;

		public static bool ParticleEffectsSelectable;

		private static readonly Dictionary<string, int> m_typesStats;

		private static List<MyCubeGrid> m_cubeGridList;

		private static readonly HashSet<MyCubeGrid> m_cubeGridHash;

		private static readonly HashSet<IMyEntity> m_entitiesForDebugDraw;

		private static readonly HashSet<object> m_groupDebugHelper;

		private static readonly MyStringId GIZMO_LINE_MATERIAL;

		private static readonly MyStringId GIZMO_LINE_MATERIAL_WHITE;

		private static readonly ConcurrentDictionary<MyEntity, BoundingBoxDrawArgs> m_entitiesForBBoxDraw;

		public static bool IsAsyncUpdateInProgress { get; private set; }

		private static List<MyEntity> OverlapRBElementList
		{
			get
			{
				if (m_overlapRBElementList == null)
				{
					m_overlapRBElementList = new List<MyEntity>(256);
					lock (m_overlapRBElementListCollection)
					{
						m_overlapRBElementListCollection.Add(m_overlapRBElementList);
					}
				}
				return m_overlapRBElementList;
			}
		}

		private static HashSet<IMyEntity> EntityResultSet
		{
			get
			{
				if (m_entityResultSet == null)
				{
					m_entityResultSet = new HashSet<IMyEntity>();
					lock (m_entityResultSetCollection)
					{
						m_entityResultSetCollection.Add(m_entityResultSet);
					}
				}
				return m_entityResultSet;
			}
		}

		private static List<MyEntity> EntityInputList
		{
			get
			{
				if (m_entityInputList == null)
				{
					m_entityInputList = new List<MyEntity>(32);
					lock (m_entityInputListCollection)
					{
						m_entityInputListCollection.Add(m_entityInputList);
					}
				}
				return m_entityInputList;
			}
		}

		public static bool IsLoaded => m_isLoaded;

		private static List<MyEntity> AllIgnoredEntities
		{
			get
			{
				if (m_allIgnoredEntities == null)
				{
					m_allIgnoredEntities = new List<MyEntity>();
					m_allIgnoredEntitiesCollection.Add(m_allIgnoredEntities);
				}
				return m_allIgnoredEntities;
			}
		}

		public static bool MemoryLimitAddFailure { get; private set; }

		public static event Action<MyEntity> OnEntityRemove;

		public static event Action<MyEntity> OnEntityAdd;

		public static event Action<MyEntity> OnEntityCreate;

		public static event Action<MyEntity> OnEntityDelete;

		public static event Action OnCloseAll;

		public static event Action<MyEntity, string, string> OnEntityNameSet;

		static MyEntities()
		{
			m_entities = new MyConcurrentHashSet<MyEntity>();
			m_entitiesForDraw = new ConcurrentCachingList<IMyEntity>();
			m_sceneComponents = new List<IMySceneComponent>();
			IsClosingAll = false;
			IgnoreMemoryLimits = false;
			m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
			m_renderObjectToEntityMapLock = new FastResourceLock();
			m_overlapRBElementListCollection = new List<List<MyEntity>>();
			m_rigidBodyList = new List<HkBodyCollision>();
			LineOverlapEntityList = new List<MyLineSegmentOverlapResult<MyEntity>>();
			m_hits = new List<MyPhysics.HitInfo>();
			m_entityResultSetCollection = new List<HashSet<IMyEntity>>();
			m_entityInputListCollection = new List<List<MyEntity>>();
			m_entitiesToDelete = new HashSet<MyEntity>();
			m_entitiesToDeleteNextFrame = new HashSet<MyEntity>();
			m_entityNameDictionary = new ConcurrentDictionary<string, MyEntity>();
			m_isLoaded = false;
			EntityCloseLock = new FastResourceLock();
			EntityMarkForCloseLock = new FastResourceLock();
			UnloadDataLock = new FastResourceLock();
			UpdateInProgress = false;
			CloseAllowed = false;
			m_allIgnoredEntitiesCollection = new List<List<MyEntity>>();
			m_hiddenTypes = new HashSet<Type>();
			m_typesStats = new Dictionary<string, int>();
			m_cubeGridList = new List<MyCubeGrid>();
			m_cubeGridHash = new HashSet<MyCubeGrid>();
			m_entitiesForDebugDraw = new HashSet<IMyEntity>();
			m_groupDebugHelper = new HashSet<object>();
			GIZMO_LINE_MATERIAL = MyStringId.GetOrCompute("GizmoDrawLine");
			GIZMO_LINE_MATERIAL_WHITE = MyStringId.GetOrCompute("GizmoDrawLineWhite");
			m_entitiesForBBoxDraw = new ConcurrentDictionary<MyEntity, BoundingBoxDrawArgs>();
			Type typeFromHandle = typeof(MyEntity);
			MyEntityFactory.RegisterDescriptor(CustomAttributeExtensions.GetCustomAttribute<MyEntityTypeAttribute>(typeFromHandle, inherit: false), typeFromHandle);
			MyEntityFactory.RegisterDescriptorsFromAssembly(typeof(MyEntities).Assembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.GameAssembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.SandboxAssembly);
			MyEntityFactory.RegisterDescriptorsFromAssembly(MyPlugins.UserAssemblies);
			MyEntityExtensions.SetCallbacks();
			MyEntitiesInterface.RegisterUpdate = RegisterForUpdate;
			MyEntitiesInterface.UnregisterUpdate = UnregisterForUpdate;
			MyEntitiesInterface.RegisterDraw = RegisterForDraw;
			MyEntitiesInterface.UnregisterDraw = UnregisterForDraw;
			MyEntitiesInterface.SetEntityName = SetEntityName;
			MyEntitiesInterface.IsUpdateInProgress = IsUpdateInProgress;
			MyEntitiesInterface.IsCloseAllowed = IsCloseAllowed;
			MyEntitiesInterface.RemoveName = RemoveName;
			MyEntitiesInterface.RemoveFromClosedEntities = RemoveFromClosedEntities;
			MyEntitiesInterface.Remove = Remove;
			MyEntitiesInterface.RaiseEntityRemove = RaiseEntityRemove;
			MyEntitiesInterface.Close = Close;
			Orchestrator = (IMyUpdateOrchestrator)Activator.CreateInstance(MyPerGameSettings.UpdateOrchestratorType);
		}

		public static void AddRenderObjectToMap(uint id, IMyEntity entity)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				using (m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
				{
					m_renderObjectToEntityMap.Add(id, entity);
				}
			}
		}

		public static void RemoveRenderObjectFromMap(uint id)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				using (m_renderObjectToEntityMapLock.AcquireExclusiveUsing())
				{
					m_renderObjectToEntityMap.Remove(id);
				}
			}
		}

		public static bool IsShapePenetrating(HkShape shape, ref Vector3D position, ref Quaternion rotation, int filter = 15, MyEntity ignoreEnt = null)
		{
			using (MyUtils.ReuseCollection(ref m_rigidBodyList))
			{
				MyPhysics.GetPenetrationsShape(shape, ref position, ref rotation, m_rigidBodyList, filter);
				if (ignoreEnt != null)
				{
					for (int num = m_rigidBodyList.Count - 1; num >= 0; num--)
					{
						if (m_rigidBodyList[num].GetCollisionEntity() == ignoreEnt)
						{
							m_rigidBodyList.RemoveAtFast(num);
							break;
						}
					}
				}
				return m_rigidBodyList.Count > 0;
			}
		}

		public static bool IsSpherePenetrating(ref BoundingSphereD bs)
		{
			return IsShapePenetrating(m_cameraSphere, ref bs.Center, ref Quaternion.Identity);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="matrix">Reference frame from which search for a free place</param>
		/// <param name="axis">Axis where to perform a rotation searching for a free place</param>
		/// <param name="radius"></param>
		/// <param name="maxTestCount"></param>
		/// <param name="testsPerDistance"></param>
		/// <param name="stepSize"></param>
		/// <returns></returns>
		public static Vector3D? FindFreePlace(ref MatrixD matrix, Vector3 axis, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f)
		{
			Vector3 vector = matrix.Forward;
			vector.Normalize();
			Vector3D position = matrix.Translation;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation) && FindFreePlaceVoxelMap(position, radius, ref shape, ref position))
				{
					return position;
				}
				int num = (int)Math.Ceiling((float)maxTestCount / (float)testsPerDistance);
				float num2 = (float)Math.PI * 2f / (float)testsPerDistance;
				float num3 = 0f;
				for (int i = 0; i < num; i++)
				{
					num3 += radius * stepSize;
					Vector3D vector3D = vector;
					float num4 = 0f;
					for (int j = 0; j < testsPerDistance; j++)
					{
						if (j != 0)
						{
							num4 += num2;
							Quaternion rotation2 = Quaternion.CreateFromAxisAngle(axis, num4);
							vector3D = Vector3D.Transform(vector, rotation2);
						}
						position = matrix.Translation + vector3D * num3;
						if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation) && FindFreePlaceVoxelMap(position, radius, ref shape, ref position))
						{
							return position;
						}
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		/// <summary>
		/// Finds free place for objects defined by position and radius.
		/// StepSize is how fast to increase radius, 0.5f means by half radius
		/// </summary>
		public static Vector3D? FindFreePlace(Vector3D basePos, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f, MyEntity ignoreEnt = null)
		{
			return FindFreePlaceCustom(basePos, radius, maxTestCount, testsPerDistance, stepSize, 0f, ignoreEnt);
		}

		public static Vector3D? FindFreePlaceCustom(Vector3D basePos, float radius, int maxTestCount = 20, int testsPerDistance = 5, float stepSize = 1f, float radiusIncrement = 0f, MyEntity ignoreEnt = null)
		{
			Vector3D position = basePos;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation, 15, ignoreEnt))
				{
					BoundingSphereD sphere = new BoundingSphereD(position, radius);
					MyVoxelBase overlappingWithSphere = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere);
					if (overlappingWithSphere == null)
					{
						return position;
					}
					if (overlappingWithSphere is MyPlanet)
					{
						(overlappingWithSphere as MyPlanet).CorrectSpawnLocation(ref basePos, radius);
					}
					return basePos;
				}
				int num = (int)Math.Ceiling((float)maxTestCount / (float)testsPerDistance);
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					num2 += radius * stepSize + radiusIncrement;
					for (int j = 0; j < testsPerDistance; j++)
					{
						position = basePos + MyUtils.GetRandomVector3Normalized() * num2;
						if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation, 15, ignoreEnt))
						{
							BoundingSphereD sphere2 = new BoundingSphereD(position, radius);
							MyVoxelBase overlappingWithSphere2 = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere2);
							if (overlappingWithSphere2 == null)
							{
								return position;
							}
							if (overlappingWithSphere2 is MyPlanet)
							{
								(overlappingWithSphere2 as MyPlanet).CorrectSpawnLocation(ref basePos, radius);
							}
						}
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		public static Vector3D? TestPlaceInSpace(Vector3D basePos, float radius)
		{
			List<MyVoxelBase> list = new List<MyVoxelBase>();
			Vector3D position = basePos;
			Quaternion rotation = Quaternion.Identity;
			HkShape shape = new HkSphereShape(radius);
			try
			{
				if (IsInsideWorld(position) && !IsShapePenetrating(shape, ref position, ref rotation))
				{
					BoundingSphereD sphere = new BoundingSphereD(position, radius);
					MySession.Static.VoxelMaps.GetAllOverlappingWithSphere(ref sphere, list);
					if (list.Count == 0)
					{
						return position;
					}
					bool flag = true;
					foreach (MyVoxelBase item in list)
					{
						MyPlanet myPlanet = item as MyPlanet;
						if (myPlanet == null)
						{
							flag = false;
							break;
						}
						if ((position - myPlanet.PositionComp.GetPosition()).Length() < (double)myPlanet.MaximumRadius)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return position;
					}
				}
				return null;
			}
			finally
			{
				shape.RemoveReference();
			}
		}

		/// <returns>True if it a safe position is found</returns>
		private static bool FindFreePlaceVoxelMap(Vector3D currentPos, float radius, ref HkShape shape, ref Vector3D ret)
		{
			BoundingSphereD sphere = new BoundingSphereD(currentPos, radius);
			MyVoxelBase myVoxelBase = MySession.Static.VoxelMaps.GetOverlappingWithSphere(ref sphere)?.RootVoxel;
			if (myVoxelBase == null)
			{
				ret = currentPos;
				return true;
			}
			MyPlanet myPlanet = myVoxelBase as MyPlanet;
			if (myPlanet != null)
			{
				bool num = myPlanet.CorrectSpawnLocation2(ref currentPos, radius);
				Quaternion rotation = Quaternion.Identity;
				if (num)
				{
					if (!IsShapePenetrating(shape, ref currentPos, ref rotation))
					{
						ret = currentPos;
						return true;
					}
					if (myPlanet.CorrectSpawnLocation2(ref currentPos, radius, resumeSearch: true) && !IsShapePenetrating(shape, ref currentPos, ref rotation))
					{
						ret = currentPos;
						return true;
					}
				}
			}
			return false;
		}

		public static void GetInflatedPlayerBoundingBox(ref BoundingBoxD playerBox, float inflation)
		{
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				playerBox.Include(onlinePlayer.GetPosition());
			}
			playerBox.Inflate(inflation);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="pos">Position of object</param>
		/// <param name="hintPosition">Position of object few frames back to test whether object enterred voxel. Usually pos - LinearVelocity * x, where x it time.</param>
		/// <param name="lastOutsidePos"></param>
		public static bool IsInsideVoxel(Vector3D pos, Vector3D hintPosition, out Vector3D lastOutsidePos)
		{
			m_hits.Clear();
			lastOutsidePos = pos;
			MyPhysics.CastRay(hintPosition, pos, m_hits, 15);
			int num = 0;
			foreach (MyPhysics.HitInfo hit in m_hits)
			{
				if (hit.HkHitInfo.GetHitEntity() is MyVoxelMap)
				{
					num++;
					lastOutsidePos = hit.Position;
				}
			}
			m_hits.Clear();
			return num % 2 != 0;
		}

		public static bool IsWorldLimited()
		{
			if (MySession.Static != null)
			{
				return MySession.Static.Settings.WorldSizeKm != 0;
			}
			return false;
		}

		/// <summary>
		/// Returns shortest distance (i.e. axis-aligned) to the world border from the world center.
		/// Will be 0 if world is borderless
		/// </summary>
		public static float WorldHalfExtent()
		{
			return (MySession.Static != null) ? (MySession.Static.Settings.WorldSizeKm * 500) : 0;
		}

		/// <summary>
		/// Returns shortest distance (i.e. axis-aligned) to the world border from the world center, minus 600m to make spawning somewhat safer.
		/// WIll be 0 if world is borderless
		/// </summary>
		public static float WorldSafeHalfExtent()
		{
			float num = WorldHalfExtent();
			if (num != 0f)
			{
				return num - 600f;
			}
			return 0f;
		}

		public static bool IsInsideWorld(Vector3D pos)
		{
			float num = WorldHalfExtent();
			if (num == 0f)
			{
				return true;
			}
			return pos.AbsMax() <= (double)num;
		}

		public static bool IsRaycastBlocked(Vector3D pos, Vector3D target)
		{
			m_hits.Clear();
			MyPhysics.CastRay(pos, target, m_hits);
			return m_hits.Count > 0;
		}

		/// <summary>
		/// Get all rigid body elements touching a bounding box.
		/// Clear() the result list after you're done with it!
		/// </summary>
		/// <returns>The list of results.</returns>
		public static List<MyEntity> GetEntitiesInAABB(ref BoundingBox boundingBox)
		{
			BoundingBoxD box = boundingBox;
			MyGamePruningStructure.GetAllEntitiesInBox(ref box, OverlapRBElementList);
			return OverlapRBElementList;
		}

		/// <summary>
		/// Returns list of entities that intersects with BoundingBox.
		/// If you are modder, you better use IMyEntities method. It is safe to use.
		/// </summary>
		/// <param name="boundingBox">Bounding box in world coordinates</param>
		/// <param name="exact">When true more accurate</param>
		/// <returns>WARNING: Next call of function GetEntitiesInAABB would add elements to that list. <b>Always clean</b> right after use. It may break or slow game, if you won't clean array before next <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBox@)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBoxD@,System.Boolean)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetElementsInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity})" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity},Sandbox.Game.Entities.MyEntityQueryType)" /></returns>
		public static List<MyEntity> GetEntitiesInAABB(ref BoundingBoxD boundingBox, bool exact = false)
		{
			MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, OverlapRBElementList);
			if (exact)
			{
				int num = 0;
				while (num < OverlapRBElementList.Count)
				{
					MyEntity myEntity = OverlapRBElementList[num];
					if (!boundingBox.Intersects(myEntity.PositionComp.WorldAABB))
					{
						OverlapRBElementList.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
			return OverlapRBElementList;
		}

		/// <summary>
		/// Returns list of entities that intersects with boundingSphere.
		/// If you are modder, you better use IMyEntities method. It is safe to use.
		/// </summary>
		/// <param name="boundingSphere">Bounding sphere in world coordinates</param>
		/// <returns>WARNING: Next call of function GetEntitiesInAABB would add elements to that list. <b>Always clean</b> right after use. It may break or slow game, if you won't clean array before next <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBox@)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBoxD@,System.Boolean)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetElementsInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity})" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity},Sandbox.Game.Entities.MyEntityQueryType)" /></returns>
		public static List<MyEntity> GetEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			MyGamePruningStructure.GetAllEntitiesInSphere(ref boundingSphere, OverlapRBElementList);
			return OverlapRBElementList;
		}

		/// <summary>
		/// Returns list of entities that intersects with oriented bounding box.
		/// If you are modder, you better use IMyEntities method. It is safe to use.
		/// </summary>
		/// <param name="obb">Oriented bounding box in world coordinates</param>
		/// <returns>WARNING: Next call of function GetEntitiesInAABB would add elements to that list. <b>Always clean</b> right after use. It may break or slow game, if you won't clean array before next <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBox@)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBoxD@,System.Boolean)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetElementsInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity})" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity},Sandbox.Game.Entities.MyEntityQueryType)" /></returns>
		public static List<MyEntity> GetEntitiesInOBB(ref MyOrientedBoundingBoxD obb)
		{
			MyGamePruningStructure.GetAllEntitiesInOBB(ref obb, OverlapRBElementList);
			return OverlapRBElementList;
		}

		/// <summary>
		/// Returns list of entities that intersects with oriented bounding box.
		/// If you are modder, you better use IMyEntities method. It is safe to use.
		/// </summary>
		/// <param name="boundingSphere">Bounding sphere in world coordinates</param>
		/// <returns>WARNING: Next call of function GetEntitiesInAABB would add elements to that list. <b>Always clean</b> right after use. It may break or slow game, if you won't clean array before next <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBox@)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInAABB(VRageMath.BoundingBoxD@,System.Boolean)" />, <see cref="M:Sandbox.Game.Entities.MyEntities.GetEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetElementsInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity})" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInSphere(VRageMath.BoundingSphereD@)" /> <see cref="M:Sandbox.Game.Entities.MyEntities.GetTopMostEntitiesInBox(VRageMath.BoundingBoxD@,System.Collections.Generic.List{VRage.Game.Entity.MyEntity},Sandbox.Game.Entities.MyEntityQueryType)" /></returns>
		public static List<MyEntity> GetTopMostEntitiesInSphere(ref BoundingSphereD boundingSphere)
		{
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref boundingSphere, OverlapRBElementList);
			return OverlapRBElementList;
		}

		public static void GetElementsInBox(ref BoundingBoxD boundingBox, List<MyEntity> foundElements)
		{
			MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, foundElements);
		}

		public static void GetTopMostEntitiesInBox(ref BoundingBoxD boundingBox, List<MyEntity> foundElements, MyEntityQueryType qtype = MyEntityQueryType.Both)
		{
			MyGamePruningStructure.GetAllTopMostStaticEntitiesInBox(ref boundingBox, foundElements, qtype);
		}

		private static void AddComponents()
		{
			m_sceneComponents.Add(new MyCubeGridGroups());
			m_sceneComponents.Add(new MyWeldingGroups());
			m_sceneComponents.Add(new MyGridPhysicalHierarchy());
			m_sceneComponents.Add(new MySharedTensorsGroups());
			m_sceneComponents.Add(new MyFixedGrids());
		}

		public static void LoadData()
		{
			m_entities.Clear();
			m_entitiesToDelete.Clear();
			m_entitiesToDeleteNextFrame.Clear();
			m_cameraSphere = new HkSphereShape(0.125f);
			AddComponents();
			foreach (IMySceneComponent sceneComponent in m_sceneComponents)
			{
				sceneComponent.Load();
			}
			m_creationThread = new MyEntityCreationThread();
			m_isLoaded = true;
		}

		public static void UnloadData()
		{
			if (m_isLoaded)
			{
				m_cameraSphere.RemoveReference();
			}
			using (UnloadDataLock.AcquireExclusiveUsing())
			{
				m_creationThread.Dispose();
				m_creationThread = null;
				CloseAll();
				m_overlapRBElementList = null;
				m_entityResultSet = null;
				m_isLoaded = false;
				lock (m_entityInputListCollection)
				{
					foreach (List<MyEntity> item in m_entityInputListCollection)
					{
						item.Clear();
					}
				}
				lock (m_overlapRBElementListCollection)
				{
					foreach (List<MyEntity> item2 in m_overlapRBElementListCollection)
					{
						item2.Clear();
					}
				}
				lock (m_entityResultSetCollection)
				{
					foreach (HashSet<IMyEntity> item3 in m_entityResultSetCollection)
					{
						item3.Clear();
					}
				}
				lock (m_allIgnoredEntitiesCollection)
				{
					foreach (List<MyEntity> item4 in m_allIgnoredEntitiesCollection)
					{
						item4.Clear();
					}
				}
			}
			for (int num = m_sceneComponents.Count - 1; num >= 0; num--)
			{
				m_sceneComponents[num].Unload();
			}
			m_sceneComponents.Clear();
			MyEntities.OnEntityRemove = null;
			MyEntities.OnEntityAdd = null;
			MyEntities.OnEntityCreate = null;
			MyEntities.OnEntityDelete = null;
			Orchestrator.Unload();
			m_entitiesToDelete.Clear();
			m_entitiesToDeleteNextFrame.Clear();
			m_entities = new MyConcurrentHashSet<MyEntity>();
			m_entitiesForDraw = new ConcurrentCachingList<IMyEntity>();
			m_remapHelper = new MyEntityIdRemapHelper();
			m_renderObjectToEntityMap = new Dictionary<uint, IMyEntity>();
			m_entityNameDictionary.Clear();
			m_entitiesForBBoxDraw.Clear();
		}

		public static void Add(MyEntity entity, bool insertIntoScene = true)
		{
			if (insertIntoScene)
			{
				entity.OnAddedToScene(entity);
			}
			if (!Exist(entity))
			{
				if (entity is MyVoxelBase)
				{
					MySession.Static.VoxelMaps.Add((MyVoxelBase)entity);
				}
				m_entities.Add(entity);
				if (GetEntityById(entity.EntityId) == null)
				{
					MyEntityIdentifier.AddEntityWithId(entity);
				}
				RaiseEntityAdd(entity);
			}
		}

		public static void SetEntityName(MyEntity myEntity, bool possibleRename = true)
		{
			string arg = null;
			string name = myEntity.Name;
			if (possibleRename)
			{
				foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
				{
					if (item.Value == myEntity)
					{
						m_entityNameDictionary.Remove<string, MyEntity>(item.Key);
						arg = item.Key;
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(myEntity.Name))
			{
<<<<<<< HEAD
				if (m_entityNameDictionary.TryGetValue(myEntity.Name, out var value))
				{
					if (value == myEntity)
=======
				MyEntity myEntity2 = default(MyEntity);
				if (m_entityNameDictionary.TryGetValue(myEntity.Name, ref myEntity2))
				{
					if (myEntity2 == myEntity)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						return;
					}
				}
				else
				{
					m_entityNameDictionary.TryAdd(myEntity.Name, myEntity);
				}
			}
			if (MyEntities.OnEntityNameSet != null)
			{
				MyEntities.OnEntityNameSet(myEntity, arg, name);
			}
		}

		public static bool IsNameExists(MyEntity entity, string name)
		{
			foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
			{
				if (item.Key == name && item.Value != entity)
				{
					return true;
				}
			}
			return false;
		}

		public static bool EntityNameExists(string name)
		{
			return m_entityNameDictionary.ContainsKey(name);
		}

<<<<<<< HEAD
		/// <summary>
		/// Removes the specified entity from scene
		/// </summary>
		/// <param name="entity">The entity.</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool Remove(MyEntity entity)
		{
			if (entity is MyVoxelBase)
			{
				MySession.Static.VoxelMaps.RemoveVoxelMap((MyVoxelBase)entity);
			}
			if (m_entities.Remove(entity))
			{
				entity.OnRemovedFromScene(entity);
				RaiseEntityRemove(entity);
				return true;
			}
			return false;
		}

		public static void DeleteRememberedEntities()
		{
			CloseAllowed = true;
			while (m_entitiesToDelete.get_Count() > 0)
			{
				using (EntityCloseLock.AcquireExclusiveUsing())
				{
					MyEntity myEntity = m_entitiesToDelete.FirstElement<MyEntity>();
					if (!myEntity.Pinned)
					{
						MyEntities.OnEntityDelete?.Invoke(myEntity);
						myEntity.Delete();
					}
					else
					{
						Remove(myEntity);
						m_entitiesToDelete.Remove(myEntity);
						m_entitiesToDeleteNextFrame.Add(myEntity);
					}
				}
			}
			CloseAllowed = false;
			HashSet<MyEntity> entitiesToDelete = m_entitiesToDelete;
			m_entitiesToDelete = m_entitiesToDeleteNextFrame;
			m_entitiesToDeleteNextFrame = entitiesToDelete;
		}

		public static bool HasEntitiesToDelete()
		{
			return m_entitiesToDelete.get_Count() > 0;
		}

		public static void RemoveFromClosedEntities(MyEntity entity)
		{
			if (m_entitiesToDelete.get_Count() > 0)
			{
				m_entitiesToDelete.Remove(entity);
			}
			if (m_entitiesToDeleteNextFrame.get_Count() > 0)
			{
				m_entitiesToDeleteNextFrame.Remove(entity);
			}
		}

		/// <summary>
		/// Remove name of entity from used names
		/// </summary>
		public static void RemoveName(MyEntity entity)
		{
			if (!string.IsNullOrEmpty(entity.Name))
			{
				m_entityNameDictionary.Remove<string, MyEntity>(entity.Name);
			}
		}

		/// <summary>
		/// Checks if entity exists in scene already
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static bool Exist(MyEntity entity)
		{
			if (m_entities == null)
			{
				return false;
			}
			return m_entities.Contains(entity);
		}

		public static void Close(MyEntity entity)
		{
			if (CloseAllowed)
			{
				m_entitiesToDeleteNextFrame.Add(entity);
			}
			else if (!m_entitiesToDelete.Contains(entity))
			{
				using (EntityMarkForCloseLock.AcquireExclusiveUsing())
				{
					m_entitiesToDelete.Add(entity);
				}
			}
		}

		public static void CloseAll()
		{
			IsClosingAll = true;
			if (MyEntities.OnCloseAll != null)
			{
				MyEntities.OnCloseAll();
			}
			CloseAllowed = true;
			List<MyEntity> list = new List<MyEntity>();
			foreach (MyEntity entity in m_entities)
			{
				entity.Close();
				m_entitiesToDelete.Add(entity);
			}
			MyEntity[] array = Enumerable.ToArray<MyEntity>((IEnumerable<MyEntity>)m_entitiesToDelete);
			foreach (MyEntity myEntity in array)
			{
				if (!myEntity.Pinned)
				{
					myEntity.Render.FadeOut = false;
					myEntity.Delete();
				}
				else
				{
					list.Add(myEntity);
				}
			}
			while (list.Count > 0)
			{
				MyEntity myEntity2 = Enumerable.First<MyEntity>((IEnumerable<MyEntity>)list);
				if (!myEntity2.Pinned)
				{
					myEntity2.Render.FadeOut = false;
					myEntity2.Delete();
					list.Remove(myEntity2);
				}
				else
				{
					Thread.Sleep(10);
				}
			}
			CloseAllowed = false;
			m_entitiesToDelete.Clear();
			MyEntityIdentifier.Clear();
			MyGamePruningStructure.Clear();
			MyRadioBroadcasters.Clear();
			m_entitiesForDraw.ApplyChanges();
			IsClosingAll = false;
		}

		public static void InvokeLater(Action action, string callerDebugName = null)
		{
			Orchestrator.InvokeLater(action, callerDebugName);
		}

		public static void RegisterForUpdate(MyEntity entity)
		{
			IMyParallelUpdateable myParallelUpdateable;
			if (entity.NeedsUpdate != 0 || ((myParallelUpdateable = entity as IMyParallelUpdateable) != null && myParallelUpdateable.UpdateFlags != 0))
			{
				Orchestrator.AddEntity(entity);
			}
		}

		public static void RegisterForDraw(IMyEntity entity)
		{
			if (entity.Render.NeedsDraw)
			{
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					m_entitiesForDraw.Add(entity);
				}
				entity.Render.SetVisibilityUpdates(state: true);
			}
		}

		public static void UnregisterForUpdate(MyEntity entity, bool immediate = false)
		{
			Orchestrator.RemoveEntity(entity, immediate);
		}

		public static void UnregisterForDraw(IMyEntity entity)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				m_entitiesForDraw.Remove(entity);
			}
			entity.Render.SetVisibilityUpdates(state: false);
		}

		public static bool IsUpdateInProgress()
		{
			return UpdateInProgress;
		}

		public static bool IsCloseAllowed()
		{
			return CloseAllowed;
		}

		public static void UpdateBeforeSimulation()
		{
			if (MySandboxGame.IsGameReady)
			{
				UpdateInProgress = true;
				UpdateOnceBeforeFrame();
				Orchestrator.DispatchBeforeSimulation();
				UpdateInProgress = false;
			}
		}

		public static void UpdateOnceBeforeFrame()
		{
			Orchestrator.DispatchOnceBeforeFrame();
		}

		public static void Simulate()
		{
			if (MySandboxGame.IsGameReady)
			{
				UpdateInProgress = true;
				Orchestrator.DispatchSimulate();
				UpdateInProgress = false;
			}
		}

		public static void UpdateAfterSimulation()
		{
			if (!MySandboxGame.IsGameReady)
			{
				return;
			}
			UpdateInProgress = true;
			Orchestrator.DispatchAfterSimulation();
			UpdateInProgress = false;
			DeleteRememberedEntities();
			if (MyMultiplayer.Static != null && m_creationThread.AnyResult)
			{
				while (m_creationThread.ConsumeResult(MyMultiplayer.Static.ReplicationLayer.GetSimulationUpdateTime()))
				{
				}
			}
		}

		public static void UpdatingStopped()
		{
			Orchestrator.DispatchUpdatingStopped();
		}

<<<<<<< HEAD
		/// <summary>
		/// Start a asynchronous update block.
		/// </summary>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static AsyncUpdateToken StartAsyncUpdateBlock()
		{
			IsAsyncUpdateInProgress = true;
			return default(AsyncUpdateToken);
		}

		private static bool IsAnyRenderObjectVisible(MyEntity entity)
		{
			if (entity.MarkedForClose)
			{
				MyLog.Default.WriteLine($"Entity {entity} is closed.");
			}
			if (entity.Render == null)
			{
				MyLog.Default.WriteLine($"Entity {entity} nas no render.");
			}
			if (entity.Render.RenderObjectIDs == null)
			{
				MyLog.Default.WriteLine($"Entity {entity} nas no render object ids.");
			}
			if (MyRenderProxy.VisibleObjectsRead == null)
			{
				MyLog.Default.WriteLine("Set of visible objects is null.");
			}
			uint[] renderObjectIDs = entity.Render.RenderObjectIDs;
			foreach (uint num in renderObjectIDs)
			{
				if (MyRenderProxy.VisibleObjectsRead.Contains(num))
				{
					return true;
				}
			}
			return false;
		}

		public static void Draw()
		{
			m_entitiesForDraw.ApplyChanges();
			foreach (MyEntity item in m_entitiesForDraw)
			{
				if (IsAnyRenderObjectVisible(item))
				{
					item.PrepareForDraw();
					item.Render.Draw();
				}
			}
			foreach (KeyValuePair<MyEntity, BoundingBoxDrawArgs> item2 in m_entitiesForBBoxDraw)
			{
				MatrixD worldMatrix = item2.Key.WorldMatrix;
				BoundingBoxD localbox = item2.Key.PositionComp.LocalAABB;
				BoundingBoxDrawArgs value = item2.Value;
				localbox.Min -= value.InflateAmount;
				localbox.Max += value.InflateAmount;
				MatrixD worldToLocal = MatrixD.Invert(worldMatrix);
				MySimpleObjectDraw.DrawAttachedTransparentBox(ref worldMatrix, ref localbox, ref value.Color, item2.Key.Render.GetRenderObjectID(), ref worldToLocal, MySimpleObjectRasterizer.Wireframe, Vector3I.One, value.LineWidth, null, value.LineMaterial, onlyFrontFaces: false, MyBillboard.BlendTypeEnum.LDR);
				if (item2.Value.WithAxis)
				{
					Vector4 color = Color.Green.ToVector4();
					Vector4 color2 = Color.Red.ToVector4();
					Vector4 color3 = Color.Blue.ToVector4();
					MatrixD identity = MatrixD.Identity;
					identity.Forward = worldMatrix.Forward;
					identity.Up = worldMatrix.Up;
					identity.Right = worldMatrix.Right;
					Vector3D vector3D = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
					MySimpleObjectDraw.DrawLine(vector3D, vector3D + worldMatrix.Right * localbox.Size.X / 2.0, GIZMO_LINE_MATERIAL_WHITE, ref color2, 0.25f, MyBillboard.BlendTypeEnum.LDR);
					MySimpleObjectDraw.DrawLine(vector3D, vector3D + worldMatrix.Up * localbox.Size.Y / 2.0, GIZMO_LINE_MATERIAL_WHITE, ref color, 0.25f, MyBillboard.BlendTypeEnum.LDR);
					MySimpleObjectDraw.DrawLine(vector3D, vector3D + worldMatrix.Forward * localbox.Size.Z / 2.0, GIZMO_LINE_MATERIAL_WHITE, ref color3, 0.25f, MyBillboard.BlendTypeEnum.LDR);
				}
			}
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere)
		{
			return GetIntersectionWithSphere(ref sphere, null, null, ignoreVoxelMaps: false, volumetricTest: false);
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1)
		{
			return GetIntersectionWithSphere(ref sphere, ignoreEntity0, ignoreEntity1, ignoreVoxelMaps: false, volumetricTest: true);
		}

		public static void GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, ref List<MyEntity> result)
		{
			BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid().Include(sphere);
			List<MyEntity> entitiesInAABB = GetEntitiesInAABB(ref boundingBox);
			foreach (MyEntity item in entitiesInAABB)
			{
				if ((!ignoreVoxelMaps || !(item is MyVoxelMap)) && item != ignoreEntity0 && item != ignoreEntity1)
				{
					if (item.GetIntersectionWithSphere(ref sphere))
					{
						result.Add(item);
					}
					if (volumetricTest && item is MyVoxelMap && (item as MyVoxelMap).DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
					{
						result.Add(item);
					}
				}
			}
			entitiesInAABB.Clear();
		}

		public static MyEntity GetIntersectionWithSphere(ref BoundingSphereD sphere, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreVoxelMaps, bool volumetricTest, bool excludeEntitiesWithDisabledPhysics = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true)
		{
			BoundingBoxD boundingBox = BoundingBoxD.CreateInvalid().Include(sphere);
			MyEntity result = null;
			List<MyEntity> entitiesInAABB = GetEntitiesInAABB(ref boundingBox);
			foreach (MyEntity item in entitiesInAABB)
			{
				if ((!ignoreVoxelMaps || !(item is MyVoxelMap)) && item != ignoreEntity0 && item != ignoreEntity1 && (!excludeEntitiesWithDisabledPhysics || item.Physics == null || item.Physics.Enabled) && (!ignoreFloatingObjects || (!(item is MyFloatingObject) && !(item is MyDebrisBase))) && (!ignoreHandWeapons || (!(item is IMyHandheldGunObject<MyDeviceBase>) && !(item.Parent is IMyHandheldGunObject<MyDeviceBase>))))
				{
					if (volumetricTest && item.IsVolumetric && item.DoOverlapSphereTest((float)sphere.Radius, sphere.Center))
					{
						result = item;
						break;
					}
					if (item.GetIntersectionWithSphere(ref sphere))
					{
						result = item;
						break;
					}
				}
			}
			entitiesInAABB.Clear();
			return result;
		}

		public static void OverlapAllLineSegment(ref LineD line, List<MyLineSegmentOverlapResult<MyEntity>> resultList)
		{
			MyGamePruningStructure.GetAllEntitiesInRay(ref line, resultList);
		}

<<<<<<< HEAD
		private static void AddIgnoredSubgridsToLists(ref MyEntity ignoreEntity, ref List<long> ignoredSubgridsIds)
=======
		public static MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(ref LineD line, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreChildren = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES, float timeFrame = 0f, bool ignoreObjectsWithoutPhysics = true, bool ignoreCharacters = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			MyCubeGrid node;
			if (ignoreEntity == null || (node = ignoreEntity as MyCubeGrid) == null)
			{
				return;
			}
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node item in (MyCubeGridGroups.Static.Logical.GetGroup(node)?.Nodes).Value)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node2 in item.Group.Nodes)
				{
					MyEntity nodeData;
					if ((nodeData = node2.NodeData) != null && !ignoredSubgridsIds.Contains(nodeData.EntityId))
					{
						ignoredSubgridsIds.Add(nodeData.EntityId);
						EntityResultSet.Add(nodeData);
					}
				}
			}
			ignoreEntity = ignoreEntity.GetBaseEntity();
			ignoreEntity.Hierarchy.GetChildrenRecursive(EntityResultSet);
		}

		public static MyIntersectionResultLineTriangleEx? GetIntersectionWithLine(ref LineD line, MyEntity ignoreEntity0, MyEntity ignoreEntity1, bool ignoreChildren = false, bool ignoreFloatingObjects = true, bool ignoreHandWeapons = true, IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES, float timeFrame = 0f, bool ignoreObjectsWithoutPhysics = true, bool ignoreSubgridsOfIgnoredEntities = false, bool ignoreCharacters = false)
		{
			EntityResultSet.Clear();
			List<long> ignoredSubgridsIds = new List<long>();
			if (ignoreChildren)
			{
				AddIgnoredSubgridsToLists(ref ignoreEntity0, ref ignoredSubgridsIds);
				AddIgnoredSubgridsToLists(ref ignoreEntity1, ref ignoredSubgridsIds);
			}
			LineOverlapEntityList.Clear();
			MyGamePruningStructure.GetAllEntitiesInRay(ref line, LineOverlapEntityList);
			LineOverlapEntityList.Sort(MyLineSegmentOverlapResult<MyEntity>.DistanceComparer);
			MyIntersectionResultLineTriangleEx? a = null;
			RayD ray = new RayD(line.From, line.Direction);
			foreach (MyLineSegmentOverlapResult<MyEntity> lineOverlapEntity in LineOverlapEntityList)
			{
				if (a.HasValue)
				{
					double? num = lineOverlapEntity.Element.PositionComp.WorldAABB.Intersects(ray);
					if (num.HasValue)
					{
						double num2 = Vector3D.DistanceSquared(line.From, a.Value.IntersectionPointInWorldSpace);
						double num3 = num.Value * num.Value;
						if (num2 < num3)
						{
							if (lineOverlapEntity.Distance > 0.0)
							{
								break;
							}
							continue;
						}
					}
				}
				MyEntity element = lineOverlapEntity.Element;
<<<<<<< HEAD
				MyCubeBlock myCubeBlock;
				if (element == ignoreEntity0 || element == ignoreEntity1 || (ignoreChildren && EntityResultSet.Contains(element)) || (ignoredSubgridsIds.Count > 0 && (myCubeBlock = element as MyCubeBlock) != null && ignoredSubgridsIds.Contains(myCubeBlock.CubeGrid.EntityId)) || (ignoreObjectsWithoutPhysics && (element.Physics == null || !element.Physics.Enabled)) || element.MarkedForClose || (ignoreFloatingObjects && (element is MyFloatingObject || element is MyDebrisBase)) || (ignoreHandWeapons && (element is IMyHandheldGunObject<MyDeviceBase> || element.Parent is IMyHandheldGunObject<MyDeviceBase>)) || (ignoreCharacters && element is MyCharacter))
				{
					continue;
				}
				MyIntersectionResultLineTriangleEx? t = null;
				if (timeFrame == 0f || element.Physics == null || element.Physics.LinearVelocity.LengthSquared() < 0.1f || !element.IsCCDForProjectiles)
				{
=======
				if (element == ignoreEntity0 || element == ignoreEntity1 || (ignoreChildren && EntityResultSet.Contains((IMyEntity)element)) || (ignoreObjectsWithoutPhysics && (element.Physics == null || !element.Physics.Enabled)) || element.MarkedForClose || (ignoreFloatingObjects && (element is MyFloatingObject || element is MyDebrisBase)) || (ignoreHandWeapons && (element is IMyHandheldGunObject<MyDeviceBase> || element.Parent is IMyHandheldGunObject<MyDeviceBase>)) || (ignoreCharacters && element is MyCharacter))
				{
					continue;
				}
				MyIntersectionResultLineTriangleEx? t = null;
				if (timeFrame == 0f || element.Physics == null || element.Physics.LinearVelocity.LengthSquared() < 0.1f || !element.IsCCDForProjectiles)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					element.GetIntersectionWithLine(ref line, out t, flags);
				}
				else
				{
					float num4 = element.Physics.LinearVelocity.Length() * timeFrame;
					float radius = element.PositionComp.LocalVolume.Radius;
					float num5 = 0f;
					Vector3D position = element.PositionComp.GetPosition();
					Vector3 vector = Vector3.Normalize(element.Physics.LinearVelocity);
					while (!t.HasValue && num5 < num4)
					{
						element.PositionComp.SetPosition(position + (Vector3D)(num5 * vector));
						element.GetIntersectionWithLine(ref line, out t, flags);
						num5 += radius;
					}
					element.PositionComp.SetPosition(position);
				}
				if (t.HasValue && t.Value.Entity != ignoreEntity0 && t.Value.Entity != ignoreEntity1 && (!ignoreChildren || !EntityResultSet.Contains(t.Value.Entity)))
				{
					a = MyIntersectionResultLineTriangleEx.GetCloserIntersection(ref a, ref t);
				}
			}
			LineOverlapEntityList.Clear();
			return a;
		}

		public static MyConcurrentHashSet<MyEntity> GetEntities()
		{
			return m_entities;
		}

		public static MyEntity GetEntityById(long entityId, bool allowClosed = false)
		{
			return MyEntityIdentifier.GetEntityById(entityId, allowClosed) as MyEntity;
		}

		public static bool IsEntityIdValid(long entityId)
		{
			MyEntity myEntity = MyEntityIdentifier.GetEntityById(entityId, allowClosed: true) as MyEntity;
			if (myEntity != null)
			{
				return !myEntity.GetTopMostParent().MarkedForClose;
			}
			return false;
		}

		public static MyEntity GetEntityByIdOrDefault(long entityId, MyEntity defaultValue = null, bool allowClosed = false)
		{
			MyEntityIdentifier.TryGetEntity(entityId, out var entity, allowClosed);
			return (entity as MyEntity) ?? defaultValue;
		}

		public static T GetEntityByIdOrDefault<T>(long entityId, T defaultValue = null, bool allowClosed = false) where T : MyEntity
		{
			MyEntityIdentifier.TryGetEntity(entityId, out var entity, allowClosed);
			return (entity as T) ?? defaultValue;
		}

		public static bool EntityExists(long entityId)
		{
			return MyEntityIdentifier.ExistsById(entityId);
		}

		public static bool TryGetEntityById(long entityId, out MyEntity entity, bool allowClosed = false)
		{
			return MyEntityIdentifier.TryGetEntity(entityId, out entity, allowClosed);
		}

		public static bool TryGetEntityById<T>(long entityId, out T entity, bool allowClosed = false) where T : MyEntity
		{
			MyEntity entity2;
			bool result = MyEntityIdentifier.TryGetEntity(entityId, out entity2, allowClosed) && entity2 is T;
			entity = entity2 as T;
			return result;
		}

		public static MyEntity GetEntityByName(string name)
		{
			return m_entityNameDictionary.get_Item(name);
		}

		public static bool TryGetEntityByName(string name, out MyEntity entity)
		{
			return m_entityNameDictionary.TryGetValue(name, ref entity);
		}

		public static bool TryGetEntityByName<T>(string name, out T entity) where T : MyEntity
		{
			MyEntity myEntity = default(MyEntity);
			if (m_entityNameDictionary.TryGetValue(name, ref myEntity) && myEntity is T)
			{
				entity = (T)myEntity;
				return true;
			}
			entity = null;
			return false;
		}

		public static bool TryGetEntityByName<T>(string name, out T entity) where T : MyEntity
		{
			if (m_entityNameDictionary.TryGetValue(name, out var value) && value is T)
			{
				entity = (T)value;
				return true;
			}
			entity = null;
			return false;
		}

		public static bool EntityExists(string name)
		{
			return m_entityNameDictionary.ContainsKey(name);
		}

		public static void RaiseEntityRemove(MyEntity entity)
		{
			if (MyEntities.OnEntityRemove != null)
			{
				MyEntities.OnEntityRemove(entity);
			}
		}

		public static void RaiseEntityAdd(MyEntity entity)
		{
			if (MyEntities.OnEntityAdd != null)
			{
				MyEntities.OnEntityAdd(entity);
			}
		}

		public static void SetTypeHidden(Type type, bool hidden)
		{
			if (hidden != m_hiddenTypes.Contains(type))
			{
				if (hidden)
				{
					m_hiddenTypes.Add(type);
				}
				else
				{
					m_hiddenTypes.Remove(type);
				}
			}
		}

		public static bool IsTypeHidden(Type type)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<Type> enumerator = m_hiddenTypes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().IsAssignableFrom(type))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public static bool IsVisible(IMyEntity entity)
		{
			return !IsTypeHidden(entity.GetType());
		}

		public static void UnhideAllTypes()
		{
			foreach (Type item in Enumerable.ToList<Type>((IEnumerable<Type>)m_hiddenTypes))
			{
				SetTypeHidden(item, hidden: false);
			}
		}

		public static void DebugDrawGridStatistics()
		{
			m_cubeGridList.Clear();
			m_cubeGridHash.Clear();
			int num = 0;
			int num2 = 0;
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed grid statistics", Color.Yellow, 1f);
			foreach (MyEntity entity in GetEntities())
			{
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = entity as MyCubeGrid) != null)
				{
					m_cubeGridList.Add(entity as MyCubeGrid);
					m_cubeGridHash.Add(MyGridPhysicalHierarchy.Static.GetRoot(entity as MyCubeGrid));
					if ((myCubeGrid.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) != 0)
					{
						MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(myCubeGrid.PositionComp.LocalAABB, myCubeGrid.PositionComp.WorldMatrixRef), Color.Red, 0.1f, depthRead: true, smooth: true);
						num++;
					}
					if (myCubeGrid.NeedsPerFrameDraw)
					{
						num2++;
					}
				}
			}
			m_cubeGridList = Enumerable.ToList<MyCubeGrid>((IEnumerable<MyCubeGrid>)Enumerable.OrderByDescending<MyCubeGrid, int>((IEnumerable<MyCubeGrid>)m_cubeGridList, (Func<MyCubeGrid, int>)((MyCubeGrid x) => x.BlocksCount)));
			float scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids by blocks (" + m_cubeGridList.Count + "):", Color.Yellow, scale);
			screenCoord.Y += 30f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing update: " + num, Color.Yellow, scale);
			screenCoord.Y += 30f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Grids needing draw: " + num2, Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (MyCubeGrid cubeGrid in m_cubeGridList)
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid.DisplayName + ": " + cubeGrid.BlocksCount + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			screenCoord.Y = 0f;
			screenCoord.X += 800f;
			screenCoord.Y += 50f;
			m_cubeGridList = Enumerable.ToList<MyCubeGrid>((IEnumerable<MyCubeGrid>)Enumerable.OrderByDescending<MyCubeGrid, int>((IEnumerable<MyCubeGrid>)m_cubeGridHash, (Func<MyCubeGrid, int>)((MyCubeGrid x) => (MyGridPhysicalHierarchy.Static.GetNode(x) != null) ? MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count : 0)));
			m_cubeGridList.RemoveAll((MyCubeGrid x) => MyGridPhysicalHierarchy.Static.GetNode(x) == null || MyGridPhysicalHierarchy.Static.GetNode(x).Children.Count == 0);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Root grids (" + m_cubeGridList.Count + "):", Color.Yellow, scale);
			screenCoord.Y += 30f;
			foreach (MyCubeGrid cubeGrid2 in m_cubeGridList)
			{
				int num3 = ((MyGridPhysicalHierarchy.Static.GetNode(cubeGrid2) != null) ? MyGridPhysicalHierarchy.Static.GetNode(cubeGrid2).Children.Count : 0);
				MyRenderProxy.DebugDrawText2D(screenCoord, cubeGrid2.DisplayName + ": " + num3 + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
		}

		public static void DebugDrawStatistics()
		{
			Orchestrator.DebugDraw();
			m_typesStats.Clear();
			Vector2 zero = Vector2.Zero;
			float num = 1f;
			foreach (MyEntity entity in m_entities)
			{
				string key = entity.GetType().Name.ToString();
				if (!m_typesStats.ContainsKey(key))
				{
					m_typesStats.Add(key, 0);
				}
				m_typesStats[key]++;
			}
			zero.X += 300f;
			zero.Y += 50f;
			num = 0.7f;
			zero.Y += 50f;
			MyRenderProxy.DebugDrawText2D(zero, "All entities:", Color.Yellow, num);
			zero.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
=======
			foreach (KeyValuePair<string, int> item in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyRenderProxy.DebugDrawText2D(zero, item.Key + ": " + item.Value + "x", Color.Yellow, num);
				zero.Y += 20f;
			}
		}

		public static IMyEntity GetEntityFromRenderObjectID(uint renderObjectID)
		{
			using (m_renderObjectToEntityMapLock.AcquireSharedUsing())
			{
				IMyEntity value = null;
				m_renderObjectToEntityMap.TryGetValue(renderObjectID, out value);
				return value;
			}
		}

		private static void DebugDrawGroups<TNode, TGroupData>(MyGroups<TNode, TGroupData> groups) where TNode : MyCubeGrid where TGroupData : IGroupData<TNode>, new()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			Enumerator<MyGroups<TNode, TGroupData>.Group> enumerator = groups.Groups.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<TNode, TGroupData>.Group current = enumerator.get_Current();
					Color color = new Vector3((float)(num++ % 15) / 15f, 1f, 1f).HSVtoColor();
					Enumerator<MyGroups<TNode, TGroupData>.Node> enumerator2 = current.Nodes.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyGroups<TNode, TGroupData>.Node current2 = enumerator2.get_Current();
							try
							{
								Enumerator<long, MyGroups<TNode, TGroupData>.Node> enumerator3 = current2.Children.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										MyGroups<TNode, TGroupData>.Node current3 = enumerator3.get_Current();
										m_groupDebugHelper.Add((object)current3);
									}
								}
								finally
								{
									((IDisposable)enumerator3).Dispose();
								}
								Enumerator<object> enumerator4 = m_groupDebugHelper.GetEnumerator();
								try
								{
									while (enumerator4.MoveNext())
									{
										object current4 = enumerator4.get_Current();
										MyGroups<TNode, TGroupData>.Node node = null;
										int num2 = 0;
										enumerator3 = current2.Children.GetEnumerator();
										try
										{
											while (enumerator3.MoveNext())
											{
												MyGroups<TNode, TGroupData>.Node current5 = enumerator3.get_Current();
												if (current4 == current5)
												{
													node = current5;
													num2++;
												}
											}
										}
										finally
										{
											((IDisposable)enumerator3).Dispose();
										}
										MyRenderProxy.DebugDrawLine3D(current2.NodeData.PositionComp.WorldAABB.Center, node.NodeData.PositionComp.WorldAABB.Center, color, color, depthRead: false);
										MyRenderProxy.DebugDrawText3D((current2.NodeData.PositionComp.WorldAABB.Center + node.NodeData.PositionComp.WorldAABB.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
									}
								}
								finally
								{
									((IDisposable)enumerator4).Dispose();
								}
								Color color2 = new Color(color.ToVector3() + 0.25f);
								MyRenderProxy.DebugDrawSphere(current2.NodeData.PositionComp.WorldAABB.Center, 0.2f, color2.ToVector3(), 0.5f, depthRead: false, smooth: true);
								MyRenderProxy.DebugDrawText3D(current2.NodeData.PositionComp.WorldAABB.Center, current2.LinkCount.ToString(), color2, 1f, depthRead: false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
							}
							finally
							{
								m_groupDebugHelper.Clear();
							}
<<<<<<< HEAD
							if (node != null)
							{
								MyRenderProxy.DebugDrawLine3D(node2.NodeData.PositionComp.WorldAABB.Center, node.NodeData.PositionComp.WorldAABB.Center, color, color, depthRead: false);
								MyRenderProxy.DebugDrawText3D((node2.NodeData.PositionComp.WorldAABB.Center + node.NodeData.PositionComp.WorldAABB.Center) * 0.5, num2.ToString(), color, 1f, depthRead: false);
							}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public static void DebugDraw()
		{
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			MyEntityComponentsDebugDraw.DebugDraw();
			if (MyCubeGridGroups.Static != null)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_PHYSICAL)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.Physical);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_GROUPS_LOGICAL)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.Logical);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_SMALL_TO_LARGE_BLOCK_GROUPS)
				{
					MyCubeGridGroups.DebugDrawBlockGroups(MyCubeGridGroups.Static.SmallToLargeBlockConnections);
				}
				if (MyDebugDrawSettings.DEBUG_DRAW_DYNAMIC_PHYSICAL_GROUPS)
				{
					DebugDrawGroups(MyCubeGridGroups.Static.PhysicalDynamic);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS || MyDebugDrawSettings.ENABLE_DEBUG_DRAW || MyFakes.SHOW_INVALID_TRIANGLES)
			{
				using (m_renderObjectToEntityMapLock.AcquireSharedUsing())
				{
					m_entitiesForDebugDraw.Clear();
					Enumerator<uint> enumerator = MyRenderProxy.VisibleObjectsRead.GetEnumerator();
					try
					{
<<<<<<< HEAD
						m_renderObjectToEntityMap.TryGetValue(item, out var value);
						if (value != null)
=======
						while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							uint current = enumerator.get_Current();
							m_renderObjectToEntityMap.TryGetValue(current, out var value);
							if (value != null)
							{
								IMyEntity topMostParent = value.GetTopMostParent();
								if (!m_entitiesForDebugDraw.Contains(topMostParent))
								{
									m_entitiesForDebugDraw.Add(topMostParent);
								}
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_GRID_COUNTER)
					{
						MyRenderProxy.DebugDrawText2D(new Vector2(700f, 0f), "Grid number: " + MyCubeGrid.GridCounter, Color.Red, 1f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP);
					}
					foreach (MyEntity entity in m_entities)
					{
						m_entitiesForDebugDraw.Add((IMyEntity)entity);
					}
					Enumerator<IMyEntity> enumerator2 = m_entitiesForDebugDraw.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							IMyEntity current3 = enumerator2.get_Current();
							if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
							{
								current3.DebugDraw();
							}
							if (MyFakes.SHOW_INVALID_TRIANGLES)
							{
								current3.DebugDrawInvalidTriangles();
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES | MyDebugDrawSettings.DEBUG_DRAW_RIGID_BODY_ACTIONS)
					{
						enumerator2 = m_entitiesForDebugDraw.GetEnumerator();
						try
						{
<<<<<<< HEAD
							if (item3.Physics == null || !(Vector3D.Distance(MySector.MainCamera.Position, item3.WorldAABB.Center) < 500.0))
							{
								continue;
							}
							MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(item3.LocalAABB, item3.WorldMatrix);
							if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES)
							{
								Color color = Color.Yellow;
								if (item3.Physics.IsStatic)
								{
									color = Color.RoyalBlue;
=======
							while (enumerator2.MoveNext())
							{
								IMyEntity current4 = enumerator2.get_Current();
								if (current4.Physics == null || !(Vector3D.Distance(MySector.MainCamera.Position, current4.WorldAABB.Center) < 500.0))
								{
									continue;
								}
								MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(current4.LocalAABB, current4.WorldMatrix);
								if (MyDebugDrawSettings.DEBUG_DRAW_VELOCITIES)
								{
									Color color = Color.Yellow;
									if (current4.Physics.IsStatic)
									{
										color = Color.RoyalBlue;
									}
									else if (!current4.Physics.IsActive)
									{
										color = Color.Red;
									}
									MyRenderProxy.DebugDrawOBB(obb, color, 1f, depthRead: false, smooth: false);
									MyRenderProxy.DebugDrawLine3D(current4.WorldAABB.Center, current4.WorldAABB.Center + current4.Physics.LinearVelocity * 100f, Color.Green, Color.White, depthRead: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								}
								else if (!item3.Physics.IsActive)
								{
<<<<<<< HEAD
									color = Color.Red;
								}
								MyRenderProxy.DebugDrawOBB(obb, color, 1f, depthRead: false, smooth: false);
								MyRenderProxy.DebugDrawLine3D(item3.WorldAABB.Center, item3.WorldAABB.Center + item3.Physics.LinearVelocity * 100f, Color.Green, Color.White, depthRead: false);
							}
							if (MyDebugDrawSettings.DEBUG_DRAW_INTERPOLATED_VELOCITIES)
							{
								HkRigidBody rigidBody = item3.Physics.RigidBody;
								if (rigidBody != null && rigidBody.GetCustomVelocity(out var velocity))
								{
									MyRenderProxy.DebugDrawOBB(obb, Color.RoyalBlue, 1f, depthRead: false, smooth: false);
									MyRenderProxy.DebugDrawLine3D(item3.WorldAABB.Center, item3.WorldAABB.Center + velocity * 100f, Color.Green, Color.White, depthRead: false);
=======
									HkRigidBody rigidBody = current4.Physics.RigidBody;
									if (rigidBody != null && rigidBody.GetCustomVelocity(out var velocity))
									{
										MyRenderProxy.DebugDrawOBB(obb, Color.RoyalBlue, 1f, depthRead: false, smooth: false);
										MyRenderProxy.DebugDrawLine3D(current4.WorldAABB.Center, current4.WorldAABB.Center + velocity * 100f, Color.Green, Color.White, depthRead: false);
									}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					m_entitiesForDebugDraw.Clear();
					if (MyDebugDrawSettings.DEBUG_DRAW_GAME_PRUNNING)
					{
						MyGamePruningStructure.DebugDraw();
					}
					if (MyDebugDrawSettings.DEBUG_DRAW_RADIO_BROADCASTERS)
					{
						MyRadioBroadcasters.DebugDraw();
					}
				}
				m_entitiesForDebugDraw.Clear();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS != MyPhysics.DebugDrawClustersEnable && MySector.MainCamera != null)
				{
					MyPhysics.DebugDrawClustersMatrix = MySector.MainCamera.WorldMatrix;
				}
				MyPhysics.DebugDrawClusters();
			}
			MyPhysics.DebugDrawClustersEnable = MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_CLUSTERS;
			if (MyDebugDrawSettings.DEBUG_DRAW_ENTITY_STATISTICS)
			{
				DebugDrawStatistics();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_STATISTICS)
			{
				DebugDrawGridStatistics();
			}
		}

		public static MyEntity CreateFromObjectBuilderAndAdd(MyObjectBuilder_EntityBase objectBuilder, bool fadeIn)
		{
			bool insertIntoScene = (objectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) > MyPersistentEntityFlags2.None;
			if (MyFakes.ENABLE_LARGE_OFFSET && objectBuilder.PositionAndOrientation.Value.Position.X < 10000.0)
			{
				objectBuilder.PositionAndOrientation = new MyPositionAndOrientation
				{
					Forward = objectBuilder.PositionAndOrientation.Value.Forward,
					Up = objectBuilder.PositionAndOrientation.Value.Up,
					Position = new SerializableVector3D(objectBuilder.PositionAndOrientation.Value.Position + new Vector3D(1000000000.0))
				};
			}
			MyEntity myEntity = CreateFromObjectBuilder(objectBuilder, fadeIn);
			if (myEntity != null)
			{
				if (myEntity.EntityId == 0L)
				{
					myEntity = null;
				}
				else
				{
					Add(myEntity, insertIntoScene);
				}
			}
			return myEntity;
		}

		/// <summary>
		/// Creates object asynchronously and adds it into scene.
		/// DoneHandler is invoked from update thread when the object is added into scene.
		/// </summary>
		public static void CreateAsync(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> doneHandler = null)
		{
			if (m_creationThread != null)
			{
				m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler, null, 0);
			}
		}

		public static void InitAsync(MyEntity entity, MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> doneHandler = null, double serializationTimestamp = 0.0, bool fadeIn = false)
		{
			if (m_creationThread != null)
			{
				m_creationThread.SubmitWork(objectBuilder, addToScene, doneHandler, entity, 0, serializationTimestamp, fadeIn);
			}
		}

		public static void ReleaseWaitingAsync(byte index, Dictionary<long, MatrixD> matrices)
		{
			m_creationThread.ReleaseWaiting(index, matrices);
		}

		public static void CallAsync(MyEntity entity, Action<MyEntity> doneHandler)
		{
			InitAsync(entity, null, addToScene: false, doneHandler);
		}

		public static void CallAsync(Action doneHandler)
		{
			InitAsync(null, null, addToScene: false, delegate
			{
				doneHandler();
			});
		}

		public static void MemoryLimitAddFailureReset()
		{
			MemoryLimitAddFailure = false;
		}

		public static void RemapObjectBuilderCollection(IEnumerable<MyObjectBuilder_EntityBase> objectBuilders)
		{
<<<<<<< HEAD
			string[] array = objectBuilders.Select((MyObjectBuilder_EntityBase x) => x.Name).ToArray();
=======
			string[] array = Enumerable.ToArray<string>(Enumerable.Select<MyObjectBuilder_EntityBase, string>(objectBuilders, (Func<MyObjectBuilder_EntityBase, string>)((MyObjectBuilder_EntityBase x) => x.Name)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (m_remapHelper == null)
			{
				m_remapHelper = new MyEntityIdRemapHelper();
			}
			foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
			{
				objectBuilder.Remap(m_remapHelper);
			}
			m_remapHelper.Clear();
			int num = 0;
			foreach (MyObjectBuilder_EntityBase objectBuilder2 in objectBuilders)
			{
				if (!string.IsNullOrEmpty(array[num]) && !EntityNameExists(array[num]))
				{
					objectBuilder2.Name = array[num];
				}
				num++;
			}
		}

		public static void RemapObjectBuilder(MyObjectBuilder_EntityBase objectBuilder)
		{
			if (m_remapHelper == null)
			{
				m_remapHelper = new MyEntityIdRemapHelper();
			}
			objectBuilder.Remap(m_remapHelper);
			m_remapHelper.Clear();
		}

		public static MyEntity CreateFromObjectBuilderNoinit(MyObjectBuilder_EntityBase objectBuilder)
		{
			return MyEntityFactory.CreateEntity(objectBuilder);
		}

		/// <summary>
		/// Create and asynchronously initialize and entity.
		/// </summary>        
		/// <param name="objectBuilder"></param>
		/// <param name="addToScene"></param>
		/// <param name="completionCallback">Callback when the entity is initialized</param>
		/// <param name="entity">Already created entity you only want to init</param>
		/// <param name="relativeSpawner"></param>
		/// <param name="relativeOffset"></param>
		/// <param name="checkPosition"></param>
		/// <param name="fadeIn"></param>
		/// <returns></returns>
		public static MyEntity CreateFromObjectBuilderParallel(MyObjectBuilder_EntityBase objectBuilder, bool addToScene = false, Action<MyEntity> completionCallback = null, MyEntity entity = null, MyEntity relativeSpawner = null, Vector3D? relativeOffset = null, bool checkPosition = false, bool fadeIn = false)
		{
			if (entity == null)
			{
				entity = CreateFromObjectBuilderNoinit(objectBuilder);
				if (entity == null)
				{
					return null;
				}
			}
			InitEntityData initData = new InitEntityData(objectBuilder, addToScene, completionCallback, entity, fadeIn, relativeSpawner, relativeOffset, checkPosition);
			Parallel.Start(delegate
			{
				if (CallInitEntity(initData))
				{
					MySandboxGame.Static.Invoke(delegate
					{
						OnEntityInitialized(initData);
					}, "CreateFromObjectBuilderParallel(alreadyParallel: true)");
				}
			});
			return entity;
		}

		private static bool CallInitEntity(WorkData workData)
		{
			InitEntityData initEntityData = workData as InitEntityData;
			if (initEntityData == null)
			{
				workData.FlagAsFailed();
				return false;
			}
			return initEntityData.CallInitEntity().Success;
		}

		private static void OnEntityInitialized(WorkData workData)
		{
			InitEntityData initEntityData = workData as InitEntityData;
			if (initEntityData == null)
			{
				workData.FlagAsFailed();
			}
			else
			{
				initEntityData.OnEntityInitialized();
			}
		}

		public static MyEntity CreateFromObjectBuilder(MyObjectBuilder_EntityBase objectBuilder, bool fadeIn)
		{
			MyEntity entity = CreateFromObjectBuilderNoinit(objectBuilder);
			entity.Render.FadeIn = fadeIn;
			InitEntity(objectBuilder, ref entity);
			return entity;
		}

		public static bool InitEntity(MyObjectBuilder_EntityBase objectBuilder, ref MyEntity entity, bool tolerateBlacklistedPlanets = false)
		{
			if (entity != null)
			{
				try
				{
					entity.Init(objectBuilder);
				}
				catch (MyPlanetWhitelistException ex) when (tolerateBlacklistedPlanets)
				{
					MySandboxGame.Log.WriteLine("Planet skipped " + ex);
					entity.EntityId = 0L;
					entity = null;
					throw;
				}
				catch (Exception ex2) when (!(ex2 is OutOfMemoryException))
				{
					MySandboxGame.Log.WriteLine("ERROR Entity init!: " + ex2);
					entity.EntityId = 0L;
					entity = null;
					return false;
				}
			}
			return true;
		}

<<<<<<< HEAD
		/// <summary>
		/// Returns false when not all entities were loaded
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool Load(List<MyObjectBuilder_EntityBase> objectBuilders, out MyStringId? errorMessage)
		{
			MyGuiScreenLoading loading = MyScreenManager.GetFirstScreenOfType<MyGuiScreenLoading>();
			MyEntityIdentifier.AllocationSuspended = true;
			bool result = true;
			try
			{
				if (objectBuilders != null)
				{
					ConcurrentQueue<InitEntityData> results = new ConcurrentQueue<InitEntityData>();
<<<<<<< HEAD
					MyEntityContainerEventExtensions.SkipProcessingEvents(state: true);
					int entitiesLoaded = 0;
					Task task = default(Task);
					if (MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING)
=======
					if (MySandboxGame.Config.SyncRendering)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						task = Parallel.Start(delegate
						{
							foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
							{
								if (objectBuilder is MyObjectBuilder_Planet)
								{
									bool success2;
									InitEntityData initEntityData2 = LoadEntity(objectBuilder, out success2);
									if (success2)
									{
										Interlocked.Increment(ref entitiesLoaded);
										if (initEntityData2 != null)
										{
											results.Enqueue(initEntityData2);
										}
									}
								}
							}
						}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "LoadPlanets"), WorkPriority.VeryHigh);
					}
					MyEntityContainerEventExtensions.SkipProcessingEvents(state: true);
					int entitiesLoaded = 0;
					Task task = default(Task);
					if (MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING)
					{
						task = Parallel.Start(delegate
						{
							foreach (MyObjectBuilder_EntityBase objectBuilder in objectBuilders)
							{
								if (objectBuilder is MyObjectBuilder_Planet)
								{
									bool success2;
									InitEntityData initEntityData4 = LoadEntity(objectBuilder, out success2);
									if (success2)
									{
										Interlocked.Increment(ref entitiesLoaded);
										if (initEntityData4 != null)
										{
											results.Enqueue(initEntityData4);
										}
									}
								}
							}
						}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "LoadPlanets"), WorkPriority.VeryHigh);
					}
					Parallel.For(0, objectBuilders.Count, delegate(int i)
					{
						MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = objectBuilders[i];
						if (!MyPlatformGameSettings.SYNCHRONIZED_PLANET_LOADING || !(myObjectBuilder_EntityBase is MyObjectBuilder_Planet))
						{
							bool success;
<<<<<<< HEAD
							InitEntityData initEntityData = LoadEntity(myObjectBuilder_EntityBase, out success);
							if (success)
							{
								Interlocked.Increment(ref entitiesLoaded);
								if (initEntityData != null)
								{
									results.Enqueue(initEntityData);
								}
							}
						}
						if (MyUtils.MainThread == Thread.CurrentThread)
						{
							InitEntityData result3;
							while (results.TryDequeue(out result3))
							{
								result3?.OnEntityInitialized();
=======
							InitEntityData initEntityData2 = LoadEntity(myObjectBuilder_EntityBase, out success);
							if (success)
							{
								Interlocked.Increment(ref entitiesLoaded);
								if (initEntityData2 != null)
								{
									results.Enqueue(initEntityData2);
								}
							}
						}
						if (MyUtils.MainThread == Thread.get_CurrentThread())
						{
							InitEntityData initEntityData3 = default(InitEntityData);
							while (results.TryDequeue(ref initEntityData3))
							{
								initEntityData3?.OnEntityInitialized();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							loading?.DrawLoading();
						}
					}, WorkPriority.Normal, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Loading, "LoadEntities"));
					if (task.valid)
<<<<<<< HEAD
=======
					{
						task.WaitOrExecute();
					}
					MyEntityContainerEventExtensions.SkipProcessingEvents(state: false);
					InitEntityData initEntityData = default(InitEntityData);
					while (results.TryDequeue(ref initEntityData))
					{
						initEntityData?.OnEntityInitialized();
					}
					result = entitiesLoaded == objectBuilders.Count;
					if (MySandboxGame.Config.SyncRendering)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						task.WaitOrExecute();
					}
					MyEntityContainerEventExtensions.SkipProcessingEvents(state: false);
					InitEntityData result2;
					while (results.TryDequeue(out result2))
					{
						result2?.OnEntityInitialized();
					}
					result = entitiesLoaded == objectBuilders.Count;
				}
				loading?.DrawLoading();
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Exceptions during entities load:");
				MyLog.Default.WriteLine(ex);
				TaskException ex2;
<<<<<<< HEAD
				if ((ex2 = ex as TaskException) != null && ex2.InnerExceptions.All((Exception x) => x is MyPlanetWhitelistException))
=======
				if ((ex2 = ex as TaskException) != null && Enumerable.All<Exception>((IEnumerable<Exception>)ex2.InnerExceptions, (Func<Exception, bool>)((Exception x) => x is MyPlanetWhitelistException)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					errorMessage = MySpaceTexts.Notification_TooManyPlanets;
				}
				else
				{
					errorMessage = null;
				}
				return false;
			}
			finally
			{
				MyEntityIdentifier.InEntityCreationBlock = false;
				MyEntityIdentifier.AllocationSuspended = false;
			}
			MyLog.Default.WriteLine("Entities loaded & initialized");
			errorMessage = null;
			return result;
		}

		private static InitEntityData LoadEntity(MyObjectBuilder_EntityBase objectBuilder, out bool success)
		{
			success = true;
			MyObjectBuilder_Character myObjectBuilder_Character = objectBuilder as MyObjectBuilder_Character;
			if (myObjectBuilder_Character != null && MyMultiplayer.Static != null && Sync.IsServer && !myObjectBuilder_Character.IsPersistenceCharacter && (!myObjectBuilder_Character.IsStartingCharacterForLobby || Sandbox.Engine.Platform.Game.IsDedicated))
			{
				return null;
			}
			if (MyFakes.SKIP_VOXELS_DURING_LOAD && objectBuilder.TypeId == typeof(MyObjectBuilder_VoxelMap) && (objectBuilder as MyObjectBuilder_VoxelMap).StorageName != "BaseAsteroid")
			{
				return null;
			}
			try
			{
				MyEntity myEntity = CreateFromObjectBuilderNoinit(objectBuilder);
				if (myEntity != null)
				{
					InitEntityData initEntityData = new InitEntityData(objectBuilder, addToScene: true, null, myEntity, fadeIn: false);
					(success, myEntity) = initEntityData.CallInitEntity(tolerateBlacklistedPlanets: true);
					if (myEntity != null)
					{
						return initEntityData;
					}
				}
				success = false;
				return null;
			}
			finally
			{
			}
		}

		internal static List<MyObjectBuilder_EntityBase> Save()
		{
			List<MyObjectBuilder_EntityBase> list = new List<MyObjectBuilder_EntityBase>();
			foreach (MyEntity entity in m_entities)
			{
				if (entity.Save && !m_entitiesToDelete.Contains(entity) && !entity.MarkedForClose)
				{
					entity.BeforeSave();
					MyObjectBuilder_EntityBase objectBuilder = entity.GetObjectBuilder();
					list.Add(objectBuilder);
				}
			}
			return list;
		}

		public static void EnableEntityBoundingBoxDraw(MyEntity entity, bool enable, Vector4? color = null, float lineWidth = 0.01f, Vector3? inflateAmount = null, MyStringId? lineMaterial = null, bool withAxis = false)
		{
			if (enable)
			{
				if (!m_entitiesForBBoxDraw.ContainsKey(entity))
				{
					entity.OnClose += entityForBBoxDraw_OnClose;
				}
				m_entitiesForBBoxDraw.set_Item(entity, new BoundingBoxDrawArgs
				{
					Color = (color ?? Vector4.One),
					LineWidth = lineWidth,
					InflateAmount = (inflateAmount ?? Vector3.Zero),
					LineMaterial = (lineMaterial ?? GIZMO_LINE_MATERIAL),
					WithAxis = withAxis
<<<<<<< HEAD
				};
=======
				});
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				m_entitiesForBBoxDraw.Remove<MyEntity, BoundingBoxDrawArgs>(entity);
				entity.OnClose -= entityForBBoxDraw_OnClose;
			}
		}

		private static void entityForBBoxDraw_OnClose(MyEntity entity)
		{
			m_entitiesForBBoxDraw.Remove<MyEntity, BoundingBoxDrawArgs>(entity);
		}

		public static MyEntity CreateFromComponentContainerDefinitionAndAdd(MyDefinitionId entityContainerDefinitionId, bool fadeIn, bool insertIntoScene = true)
		{
			if (!typeof(MyObjectBuilder_EntityBase).IsAssignableFrom(entityContainerDefinitionId.TypeId))
			{
				return null;
			}
			if (!MyComponentContainerExtension.TryGetContainerDefinition(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeId, out var _))
			{
				MySandboxGame.Log.WriteLine("Entity container definition not found: " + entityContainerDefinitionId);
				return null;
			}
			MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerDefinitionId.TypeId, entityContainerDefinitionId.SubtypeName) as MyObjectBuilder_EntityBase;
			if (myObjectBuilder_EntityBase == null)
			{
				MySandboxGame.Log.WriteLine("Entity builder was not created: " + entityContainerDefinitionId);
				return null;
			}
			if (insertIntoScene)
			{
				myObjectBuilder_EntityBase.PersistentFlags |= MyPersistentEntityFlags2.InScene;
			}
			return CreateFromObjectBuilderAndAdd(myObjectBuilder_EntityBase, fadeIn);
		}

		public static void RaiseEntityCreated(MyEntity entity)
		{
			MyEntities.OnEntityCreate?.Invoke(entity);
		}

		/// <summary>
		/// This method will try to retrieve a definition of components container of the entity and create the type of the entity.        
		/// </summary>
		/// <param name="entityContainerId">This is the id of container definition</param>        
		/// <param name="fadeIn"></param>
		/// <param name="setPosAndRot">Set true if want to set entity position, orientation</param>
		/// <param name="position"></param>
		/// <param name="up"></param>
		/// <param name="forward"></param>
		/// <returns></returns>
		public static MyEntity CreateEntityAndAdd(MyDefinitionId entityContainerId, bool fadeIn, bool setPosAndRot = false, Vector3? position = null, Vector3? up = null, Vector3? forward = null)
		{
			if (MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out var _))
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerId) as MyObjectBuilder_EntityBase;
				if (myObjectBuilder_EntityBase != null)
				{
					if (setPosAndRot)
					{
						myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation(position.HasValue ? position.Value : Vector3.Zero, forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up);
					}
					return CreateFromObjectBuilderAndAdd(myObjectBuilder_EntityBase, fadeIn);
				}
				return null;
			}
			return null;
		}

		public static MyEntity CreateEntity(MyDefinitionId entityContainerId, bool fadeIn, bool setPosAndRot = false, Vector3? position = null, Vector3? up = null, Vector3? forward = null)
		{
			if (MyDefinitionManager.Static.TryGetContainerDefinition(entityContainerId, out var _))
			{
				MyObjectBuilder_EntityBase myObjectBuilder_EntityBase = MyObjectBuilderSerializer.CreateNewObject(entityContainerId) as MyObjectBuilder_EntityBase;
				if (myObjectBuilder_EntityBase != null)
				{
					if (setPosAndRot)
					{
						myObjectBuilder_EntityBase.PositionAndOrientation = new MyPositionAndOrientation(position.HasValue ? position.Value : Vector3.Zero, forward.HasValue ? forward.Value : Vector3.Forward, up.HasValue ? up.Value : Vector3.Up);
					}
					return CreateFromObjectBuilder(myObjectBuilder_EntityBase, fadeIn);
				}
				return null;
			}
			return null;
		}

		public static void SendCloseRequest(IMyEntity entity)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnEntityCloseRequest, entity.EntityId);
		}

<<<<<<< HEAD
		[Event(null, 2783)]
=======
		[Event(null, 2764)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void OnEntityCloseRequest(long entityId)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsCopyPastingEnabledForUser(MyEventContext.Current.Sender.Value) && !MySession.Static.CreativeMode && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			TryGetEntityById(entityId, out var entity);
			if (entity == null)
			{
				return;
			}
			MyLog.Default.Info($"OnEntityCloseRequest removed entity '{entity.Name}:{entity.DisplayName}' with entity id '{entity.EntityId}'");
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (MyMultiplayer.Static != null && myVoxelBase != null && !myVoxelBase.Save && !myVoxelBase.ContentChanged && !myVoxelBase.BeforeContentChanged)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => ForceCloseEntityOnClients, entityId);
			}
			entity.OnEntityCloseRequest.InvokeIfNotNull(entity);
			if (!entity.MarkedForClose)
			{
				entity.Close();
			}
		}

<<<<<<< HEAD
		[Event(null, 2810)]
=======
		[Event(null, 2791)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		public static void ForceCloseEntityOnClients(long entityId)
		{
			TryGetEntityById(entityId, out var entity);
			if (entity != null)
			{
				entity.OnEntityCloseRequest.InvokeIfNotNull(entity);
				if (!entity.MarkedForClose)
				{
					entity.Close();
				}
			}
		}

		[Conditional("DEBUG")]
		private static void AssertNoLeakingEntitiesInByNameDictionary()
		{
			MySession @static = MySession.Static;
			if (@static == null || @static.GameplayFrameCounter % 100 != 0)
			{
				return;
			}
<<<<<<< HEAD
			Dictionary<string, int> dictionary = null;
			foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
			{
				if (item.Value.Closed)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<string, int>();
					}
					string name = item.Value.GetType().Name;
					if (!dictionary.TryGetValue(name, out var value))
					{
						value = 0;
					}
					value = (dictionary[name] = value + 1);
				}
=======
			foreach (KeyValuePair<string, MyEntity> item in m_entityNameDictionary)
			{
				_ = item;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
