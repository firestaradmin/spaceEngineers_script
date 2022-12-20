using System;
using System.Collections.Generic;
using System.Threading;
using Havok;
using ParallelTasks;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GUI.DebugInputComponents;
using VRage.Definitions.Components;
using VRage.Entities.Components;
using VRage.Factory;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Voxels;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.ObjectBuilders.Definitions.Components;
using VRage.Profiler;
using VRage.Utils;
using VRage.Voxels;
using VRage.Voxels.DualContouring;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Voxels
{
	[MyDependency(typeof(MyVoxelMesherComponent), Critical = true)]
	public class MyVoxelPhysicsBody : MyPhysicsBody
	{
		private class Sandbox_Engine_Voxels_MyVoxelPhysicsBody_003C_003EActor
		{
		}

		public static int ActiveVoxelPhysicsBodies = 0;

		public static int ActiveVoxelPhysicsBodiesWithExtendedCache = 0;

		public static bool EnableShapeDiscard = true;

		private const int ShapeDiscardThreshold = 0;

		private const int ShapeDiscardCheckInterval = 18;

		private readonly int PhysicsUpdatesPerSecond = 6;

		private static Vector3I[] m_cellsToGenerateBuffer = new Vector3I[128];

		internal readonly HashSet<Vector3I>[] InvalidCells;

		internal readonly MyPrecalcJobPhysicsBatch[] RunningBatchTask = new MyPrecalcJobPhysicsBatch[2];

		public readonly MyVoxelBase m_voxelMap;

		private bool m_needsShapeUpdate;

		private bool m_bodiesInitialized;

		private readonly List<MyEntity> m_nearbyEntities = new List<MyEntity>();

		private int m_nearbyEntitiesUpdateCounter;

		private MyVoxelMesherComponent m_mesher;

		private readonly MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch> m_workTracker = new MyWorkTracker<MyCellCoord, MyPrecalcJobPhysicsPrefetch>(MyCellCoord.Comparer);

		private readonly Vector3I m_cellsOffset = new Vector3I(0, 0, 0);

		private bool m_staticForCluster = true;

		private float m_phantomExtend;

		private float m_predictionSize = 3f;

		private int m_lastDiscardCheck;

		private BoundingBoxI m_queuedRange = new BoundingBoxI(-1, -1);

		private bool m_queueInvalidation;

		private int m_hasExtendedCache;

		private int m_voxelQueriesInLastFrames;

		private const int EXTENDED_CACHE_QUERY_THRESHOLD = 20000;

		private const int EXTENDED_CACHE_QUERIES_THRESHOLD = 10000;

		private static List<MyCellCoord> m_toBeCancelledCache;

		[ThreadStatic]
		private static MyList<int> m_indexListCached;

		[ThreadStatic]
		private static MyList<byte> m_materialListCached;

		[ThreadStatic]
		private static MyList<Vector3> m_vertexListCached;

		public static bool UseLod1VoxelPhysics = false;

		public override bool IsStatic => true;

		public bool QueueInvalidate
		{
			get
			{
				return m_queueInvalidation;
			}
			set
			{
				m_queueInvalidation = value;
				if (!value && m_queuedRange.Max.X >= 0)
				{
					InvalidateRange(m_queuedRange.Min, m_queuedRange.Max);
					m_queuedRange = new BoundingBoxI(-1, -1);
				}
			}
		}

		public override bool IsStaticForCluster
		{
			get
			{
				return m_staticForCluster;
			}
			set
			{
				m_staticForCluster = value;
			}
		}

		internal MyVoxelPhysicsBody(MyVoxelBase voxelMap, float phantomExtend, float predictionSize = 3f, bool lazyPhysics = false)
			: base(voxelMap, RigidBodyFlag.RBF_STATIC)
		{
			InvalidCells = new HashSet<Vector3I>[2];
			InvalidCells[0] = new HashSet<Vector3I>();
			InvalidCells[1] = new HashSet<Vector3I>();
			m_predictionSize = predictionSize;
			m_phantomExtend = phantomExtend;
			m_voxelMap = voxelMap;
			_ = m_voxelMap.Size >> 3;
			m_cellsOffset = m_voxelMap.StorageMin >> 3;
			if (!MyFakes.ENABLE_LAZY_VOXEL_PHYSICS || !lazyPhysics)
			{
				CreateRigidBodies();
			}
			base.MaterialType = MyMaterialType.ROCK;
			m_nearbyEntitiesUpdateCounter = MyUtils.GetRandomInt(10);
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			if (m_mesher == null)
			{
				m_mesher = new MyVoxelMesherComponent();
				MyPlanet myPlanet = m_voxelMap.RootVoxel as MyPlanet;
				if (myPlanet != null)
				{
					MyObjectBuilder_VoxelMesherComponentDefinition mesherPostprocessing = myPlanet.Generator.MesherPostprocessing;
					if (mesherPostprocessing != null)
					{
						MyVoxelMesherComponentDefinition myVoxelMesherComponentDefinition = new MyVoxelMesherComponentDefinition();
						myVoxelMesherComponentDefinition.Init(mesherPostprocessing, MyModContext.BaseGame);
						m_mesher.Init(myVoxelMesherComponentDefinition);
					}
				}
				m_mesher.SetContainer(base.Entity.Components);
				m_mesher.OnAddedToScene();
			}
			if (!m_bodiesInitialized)
			{
				CreateRigidBodies();
			}
			ActiveVoxelPhysicsBodies++;
		}

		public override void OnRemovedFromScene()
		{
			base.OnRemovedFromScene();
			ActiveVoxelPhysicsBodies--;
			if (m_hasExtendedCache != 0)
			{
				Interlocked.Decrement(ref ActiveVoxelPhysicsBodiesWithExtendedCache);
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Get the per lod rigid bodies, will assert if LOD1 is not available but is requested.
		/// </summary>
		/// <param name="lod"></param>
		/// <returns></returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public HkRigidBody GetRigidBody(int lod)
		{
			if (UseLod1VoxelPhysics && lod == 1)
			{
				return RigidBody2;
			}
			return RigidBody;
		}

		public bool GetShape(int lod, out HkUniformGridShape gridShape)
		{
			HkRigidBody rigidBody = GetRigidBody(lod);
			if (rigidBody == null)
			{
				gridShape = default(HkUniformGridShape);
				return false;
			}
			gridShape = (HkUniformGridShape)rigidBody.GetShape();
			return true;
		}

		/// <summary>
		/// Get a reference to the mesh at a given position.
		/// </summary>
		/// <param name="lod">The lod of the mesh.</param>
		/// <param name="localPos">the position relative to the body.</param>
		/// <param name="mesh">The mesh at the position, if null and true was returned it means the position was set empty.</param>
		/// <returns>Returns weather the mesh was already calculated or not.</returns>
		public bool GetShape(int lod, Vector3D localPos, out HkBvCompressedMeshShape mesh)
		{
			HkUniformGridShape hkUniformGridShape = (HkUniformGridShape)GetRigidBody(lod).GetShape();
			localPos += m_voxelMap.SizeInMetresHalf;
			Vector3I vector3I = new Vector3I(localPos / (8f * (float)(1 << lod)));
			return hkUniformGridShape.GetChild(vector3I.X, vector3I.Y, vector3I.Z, out mesh);
		}

		private void CreateRigidBodies()
		{
			if (base.Entity.MarkedForClose || m_mesher == null || m_world == null)
			{
				return;
			}
			try
			{
				if (m_bodiesInitialized)
				{
					return;
				}
				Vector3I vector3I = m_voxelMap.Size >> 3;
				HkRigidBody rigidBody = null;
				HkUniformGridShapeArgs args;
				HkUniformGridShape hkUniformGridShape;
				if (UseLod1VoxelPhysics)
				{
					args = new HkUniformGridShapeArgs
					{
						CellsCount = vector3I >> 1,
						CellSize = 16f,
						CellOffset = 0.5f,
						CellExpand = 1f
					};
					hkUniformGridShape = new HkUniformGridShape(args);
					hkUniformGridShape.SetShapeRequestHandler(RequestShapeBlockingLod1);
					CreateFromCollisionObject(hkUniformGridShape, -m_voxelMap.SizeInMetresHalf, m_voxelMap.WorldMatrix, null, 11);
					hkUniformGridShape.Base.RemoveReference();
					rigidBody = RigidBody;
					RigidBody = null;
				}
				args = new HkUniformGridShapeArgs
<<<<<<< HEAD
				{
					CellsCount = vector3I,
					CellSize = 8f,
					CellOffset = 0.5f,
					CellExpand = 1f
				};
				hkUniformGridShape = new HkUniformGridShape(args);
				hkUniformGridShape.SetShapeRequestHandler(RequestShapeBlocking);
				CreateFromCollisionObject(hkUniformGridShape, -m_voxelMap.SizeInMetresHalf, m_voxelMap.WorldMatrix, null, 28);
				hkUniformGridShape.Base.RemoveReference();
				RigidBody.IsEnvironment = true;
				if (UseLod1VoxelPhysics)
				{
					RigidBody2 = rigidBody;
				}
				if (MyFakes.ENABLE_PHYSICS_HIGH_FRICTION)
				{
=======
				{
					CellsCount = vector3I,
					CellSize = 8f,
					CellOffset = 0.5f,
					CellExpand = 1f
				};
				hkUniformGridShape = new HkUniformGridShape(args);
				hkUniformGridShape.SetShapeRequestHandler(RequestShapeBlocking);
				CreateFromCollisionObject(hkUniformGridShape, -m_voxelMap.SizeInMetresHalf, m_voxelMap.WorldMatrix, null, 28);
				hkUniformGridShape.Base.RemoveReference();
				RigidBody.IsEnvironment = true;
				if (UseLod1VoxelPhysics)
				{
					RigidBody2 = rigidBody;
				}
				if (MyFakes.ENABLE_PHYSICS_HIGH_FRICTION)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Friction = 0.65f;
				}
				m_bodiesInitialized = true;
				if (Enabled)
				{
					Matrix rigidBodyMatrix = GetRigidBodyMatrix();
					RigidBody.SetWorldMatrix(rigidBodyMatrix);
					m_world.AddRigidBody(RigidBody);
					if (UseLod1VoxelPhysics)
					{
						RigidBody2.SetWorldMatrix(rigidBodyMatrix);
						m_world.AddRigidBody(RigidBody2);
					}
				}
			}
			finally
			{
			}
		}

		private void MarkForShapeUpdate()
		{
			if (!m_needsShapeUpdate)
			{
				MyEntities.InvokeLater(UpdateRigidBodyShape);
				m_needsShapeUpdate = true;
			}
		}

		private void UpdateRigidBodyShape()
		{
			if (m_needsShapeUpdate && base.Entity != null)
			{
				if (!m_bodiesInitialized)
				{
					CreateRigidBodies();
				}
				m_needsShapeUpdate = false;
				if (RigidBody != null)
				{
					RigidBody.UpdateShape();
				}
				if (RigidBody2 != null)
				{
					RigidBody2.UpdateShape();
				}
			}
		}

		private bool QueryEmptyOrFull(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			BoundingBoxI box = new BoundingBoxI(new Vector3I(minX, minY, minZ), new Vector3I(maxX, maxY, maxZ));
			if (box.Volume() < 100f)
			{
				return false;
			}
			bool flag = m_voxelMap.Storage.Intersect(ref box, 0) != ContainmentType.Intersects;
			BoundingBoxD boundingBoxD = new BoundingBoxD(new Vector3(minX, minY, minZ) * 8f, new Vector3(maxX, maxY, maxZ) * 8f);
			boundingBoxD.TransformFast(base.Entity.WorldMatrix);
			new MyOrientedBoundingBoxD(boundingBoxD, base.Entity.WorldMatrix);
			MyRenderProxy.DebugDrawAABB(boundingBoxD, flag ? Color.Green : Color.Red, 1f, 1f, depthRead: false);
			return flag;
		}

		private void RequestShapeBlockingLod1(HkShapeBatch batch)
		{
			RequestShapeBatchBlockingInternal(batch, lod1physics: true);
		}

		private void RequestShapeBlocking(HkShapeBatch batch)
		{
			RequestShapeBatchBlockingInternal(batch, lod1physics: false);
		}

		private void RequestShapeBatchBlockingInternal(HkShapeBatch info, bool lod1physics)
		{
			if (m_voxelMap.MarkedForClose)
			{
				return;
			}
			if (!m_bodiesInitialized)
			{
				CreateRigidBodies();
			}
			MarkForShapeUpdate();
			int count = info.Count;
			if ((count > 20000 || Interlocked.Add(ref m_voxelQueriesInLastFrames, count) >= 10000) && Interlocked.Exchange(ref m_hasExtendedCache, 1) == 0)
			{
				Interlocked.Increment(ref ActiveVoxelPhysicsBodiesWithExtendedCache);
				if (GetShape(lod1physics ? 1 : 0, out var gridShape))
				{
					gridShape.EnableExtendedCache();
				}
			}
			int lod = (lod1physics ? 1 : 0);
			Parallel.For(0, count, delegate(int i)
			{
				info.GetInfo(i, out var cell);
				MyCellCoord geometryCellCoord = new MyCellCoord(lod, cell);
				if (MyDebugDrawSettings.DEBUG_DRAW_REQUEST_SHAPE_BLOCKING)
				{
					MyVoxelCoordSystems.GeometryCellCoordToWorldAABB(m_voxelMap.PositionLeftBottomCorner, ref geometryCellCoord, out var worldAABB);
					MyRenderProxy.DebugDrawAABB(worldAABB, lod1physics ? Color.Yellow : Color.Red, 1f, 1f, depthRead: false);
				}
				HkBvCompressedMeshShape shape = CreateShapeBlocking(geometryCellCoord);
				info.SetResult(i, shape);
			}, 1, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Voxels, "Physics.RequestVoxels"), blocking: true);
		}

		private HkBvCompressedMeshShape CreateShapeBlocking(MyCellCoord cellCoord)
		{
			bool flag = false;
			HkBvCompressedMeshShape result = (HkBvCompressedMeshShape)HkShape.Empty;
			MyPrecalcJobPhysicsPrefetch myPrecalcJobPhysicsPrefetch = m_workTracker.Cancel(cellCoord);
			if (myPrecalcJobPhysicsPrefetch != null && myPrecalcJobPhysicsPrefetch.ResultComplete && Interlocked.Exchange(ref myPrecalcJobPhysicsPrefetch.Taken, 1) == 0)
			{
				flag = true;
				result = myPrecalcJobPhysicsPrefetch.Result;
			}
			if (!flag)
			{
				VrVoxelMesh vrVoxelMesh = CreateMesh(cellCoord);
				result = CreateShape(vrVoxelMesh, cellCoord.Lod);
				vrVoxelMesh?.Dispose();
			}
			return result;
		}

		/// <param name="minVoxelChanged">Inclusive min.</param>
		/// <param name="maxVoxelChanged">Inclusive max.</param>
		internal void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged)
		{
			InvalidateRange(minVoxelChanged, maxVoxelChanged, 0);
			if (UseLod1VoxelPhysics)
			{
				InvalidateRange(minVoxelChanged, maxVoxelChanged, 1);
			}
		}

		private void GetPrediction(IMyEntity entity, out BoundingBoxD box)
		{
			Vector3 vector = ComputePredictionOffset(entity);
			box = entity.WorldAABB;
			if (entity.Physics.AngularVelocity.Sum > 0.03f)
			{
				float num = entity.LocalAABB.HalfExtents.Length();
				box = new BoundingBoxD(box.Center - num, box.Center + num);
			}
			if (box.Extents.Max() > 8.0)
			{
				box.Inflate(8.0);
			}
			else
			{
				box.InflateToMinimum(new Vector3(8f));
			}
<<<<<<< HEAD
			float num2 = vector.Length() / (float)PhysicsUpdatesPerSecond / 2f;
			box.Inflate(num2);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			box.Translate(vector);
		}

		internal void InvalidateRange(Vector3I minVoxelChanged, Vector3I maxVoxelChanged, int lod)
		{
			if (!m_bodiesInitialized)
			{
				return;
			}
			if (m_queueInvalidation)
			{
				if (m_queuedRange.Max.X < 0)
				{
					m_queuedRange = new BoundingBoxI(minVoxelChanged, maxVoxelChanged);
					return;
				}
				BoundingBoxI box = new BoundingBoxI(minVoxelChanged, maxVoxelChanged);
				m_queuedRange.Include(ref box);
				return;
			}
			minVoxelChanged -= 2;
			maxVoxelChanged += 1;
			m_voxelMap.Storage.ClampVoxelCoord(ref minVoxelChanged);
			m_voxelMap.Storage.ClampVoxelCoord(ref maxVoxelChanged);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref minVoxelChanged, out var geometryCellCoord);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref maxVoxelChanged, out var geometryCellCoord2);
			Vector3I minChanged = geometryCellCoord - m_cellsOffset >> lod;
			Vector3I value = geometryCellCoord2 - m_cellsOffset >> lod;
			Vector3I voxelCoord = m_voxelMap.Size - 1;
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref voxelCoord, out voxelCoord);
			voxelCoord >>= lod;
			Vector3I.Min(ref value, ref voxelCoord, out value);
			HkRigidBody rigidBody = GetRigidBody(lod);
			if (minChanged == Vector3I.Zero && value == voxelCoord)
			{
				m_workTracker.CancelAll();
			}
			else
			{
				using (MyUtils.ReuseCollection(ref m_toBeCancelledCache))
				{
					BoundingBoxI boundingBoxI = new BoundingBoxI(geometryCellCoord, geometryCellCoord2);
					foreach (KeyValuePair<MyCellCoord, MyPrecalcJobPhysicsPrefetch> item in m_workTracker)
					{
						if (boundingBoxI.Contains(item.Key.CoordInLod) != 0)
						{
							m_toBeCancelledCache.Add(item.Key);
						}
					}
					foreach (MyCellCoord item2 in m_toBeCancelledCache)
					{
						m_workTracker.CancelIfStarted(item2);
					}
				}
			}
			if (rigidBody != null)
			{
				HkUniformGridShape hkUniformGridShape = (HkUniformGridShape)rigidBody.GetShape();
				int size = (value - minChanged + 1).Size;
				if (size >= m_cellsToGenerateBuffer.Length)
				{
					m_cellsToGenerateBuffer = new Vector3I[MathHelper.GetNearestBiggerPowerOfTwo(size)];
				}
				Vector3I[] cellsToGenerateBuffer = m_cellsToGenerateBuffer;
				int num = hkUniformGridShape.InvalidateRange(ref minChanged, ref value, cellsToGenerateBuffer);
				for (int i = 0; i < num; i++)
				{
					StartPrecalcJobPhysicsIfNeeded(new MyCellCoord(lod, m_cellsToGenerateBuffer[i]));
				}
			}
			m_voxelMap.RaisePhysicsChanged();
		}

		internal void UpdateBeforeSimulation10()
		{
			if (!m_bodiesInitialized)
			{
				CreateRigidBodies();
			}
			m_voxelQueriesInLastFrames = 0;
		}

		internal void UpdateAfterSimulation10()
		{
<<<<<<< HEAD
			if (m_voxelMap.Storage == null || !m_bodiesInitialized)
			{
				return;
			}
			if (--m_nearbyEntitiesUpdateCounter < 0)
			{
				m_nearbyEntitiesUpdateCounter = 10;
				UpdateNearbyEntities();
			}
			foreach (MyEntity nearbyEntity in m_nearbyEntities)
			{
=======
			if (m_voxelMap.Storage == null)
			{
				return;
			}
			if (--m_nearbyEntitiesUpdateCounter < 0)
			{
				m_nearbyEntitiesUpdateCounter = 10;
				UpdateNearbyEntities();
			}
			foreach (MyEntity nearbyEntity in m_nearbyEntities)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (nearbyEntity == null)
				{
					continue;
				}
				bool flag = false;
				MyPhysicsBody myPhysicsBody = nearbyEntity.Physics as MyPhysicsBody;
				if (myPhysicsBody != null && myPhysicsBody.RigidBody != null && (myPhysicsBody.RigidBody.Layer == 23 || myPhysicsBody.RigidBody.Layer == 10))
				{
					flag = true;
				}
				if ((!(nearbyEntity is MyCubeGrid) && !flag) || nearbyEntity.MarkedForClose || nearbyEntity.Physics == null)
				{
					continue;
				}
				GetPrediction(nearbyEntity, out var box);
				if (!box.Intersects(m_voxelMap.PositionComp.WorldAABB))
				{
					continue;
				}
				int num = ((!flag && UseLod1VoxelPhysics) ? 1 : 0);
				int num2 = 1 << num;
				BoundingBoxD boundingBoxD = box.TransformFast(m_voxelMap.PositionComp.WorldMatrixInvScaled);
				boundingBoxD.Translate(m_voxelMap.SizeInMetresHalf);
				Vector3 localPositionMax = boundingBoxD.Max;
				Vector3 localPositionMin = boundingBoxD.Min;
				ClampVoxelCoords(ref localPositionMin, ref localPositionMax, out var min, out var max);
				min >>= num;
				max >>= num;
				int size = (max - min + 1).Size;
				if (size >= m_cellsToGenerateBuffer.Length)
				{
					m_cellsToGenerateBuffer = new Vector3I[MathHelper.GetNearestBiggerPowerOfTwo(size)];
				}
				if (!GetShape(num, out var gridShape) || gridShape.Base.IsZero)
				{
					continue;
				}
				int missingCellsInRange = gridShape.GetMissingCellsInRange(ref min, ref max, m_cellsToGenerateBuffer);
				if (missingCellsInRange == 0)
<<<<<<< HEAD
				{
					continue;
				}
				BoundingBoxI box2 = new BoundingBoxI(min * 8 * num2, (max + 1) * 8 * num2);
				box2.Translate(m_voxelMap.StorageMin);
				if (missingCellsInRange > 0 && m_voxelMap.Storage.Intersect(ref box2, num) != ContainmentType.Intersects)
				{
					SetEmptyShapes(num, ref gridShape, missingCellsInRange);
					continue;
				}
				for (int i = 0; i < missingCellsInRange; i++)
				{
					StartPrecalcJobPhysicsIfNeeded(new MyCellCoord(num, m_cellsToGenerateBuffer[i]));
=======
				{
					continue;
				}
				BoundingBoxI box2 = new BoundingBoxI(min * 8 * num2, (max + 1) * 8 * num2);
				box2.Translate(m_voxelMap.StorageMin);
				if (missingCellsInRange > 0 && m_voxelMap.Storage.Intersect(ref box2, num) != ContainmentType.Intersects)
				{
					SetEmptyShapes(num, ref gridShape, missingCellsInRange);
					continue;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				for (int i = 0; i < missingCellsInRange; i++)
				{
					StartPrecalcJobPhysicsIfNeeded(new MyCellCoord(num, m_cellsToGenerateBuffer[i]));
				}
			}
			ScheduleBatchJobs();
			if (m_bodiesInitialized)
			{
				CheckAndDiscardShapes();
			}
			ScheduleBatchJobs();
			if (m_bodiesInitialized)
			{
				CheckAndDiscardShapes();
			}
		}

		private void ClampVoxelCoords(ref Vector3 localPositionMin, ref Vector3 localPositionMax, out Vector3I min, out Vector3I max)
		{
			MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPositionMin, out min);
			MyVoxelCoordSystems.LocalPositionToVoxelCoord(ref localPositionMax, out max);
			m_voxelMap.Storage.ClampVoxelCoord(ref min);
			m_voxelMap.Storage.ClampVoxelCoord(ref max);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref min, out min);
			MyVoxelCoordSystems.VoxelCoordToGeometryCellCoord(ref max, out max);
		}

		private void ScheduleBatchJobs()
		{
			for (int i = 0; i < 2; i++)
			{
				if (InvalidCells[i].get_Count() > 0 && RunningBatchTask[i] == null)
				{
					MyPrecalcJobPhysicsBatch.Start(this, ref InvalidCells[i], i);
				}
			}
		}

		private bool StartPrecalcJobPhysicsIfNeeded(MyCellCoord coord)
		{
			if (!m_workTracker.Exists(coord))
			{
				try
				{
					MyPrecalcJobPhysicsPrefetch.Args args = default(MyPrecalcJobPhysicsPrefetch.Args);
					args.TargetPhysics = this;
					args.Tracker = m_workTracker;
					args.GeometryCell = coord;
					args.Storage = m_voxelMap.Storage;
					return MyPrecalcJobPhysicsPrefetch.Start(args);
				}
				finally
				{
				}
			}
			return false;
		}

		private void SetEmptyShapes(int lod, ref HkUniformGridShape shape, int requiredCellsCount)
		{
			for (int i = 0; i < requiredCellsCount; i++)
			{
				Vector3I coordInLod = m_cellsToGenerateBuffer[i];
				m_workTracker.Cancel(new MyCellCoord(lod, coordInLod));
				shape.SetChild(coordInLod.X, coordInLod.Y, coordInLod.Z, (HkBvCompressedMeshShape)HkShape.Empty, HkReferencePolicy.TakeOwnership);
			}
		}

		private void CheckAndDiscardShapes()
		{
			m_lastDiscardCheck++;
			if (m_lastDiscardCheck <= 18 || m_nearbyEntities.Count != 0 || !(RigidBody != null) || !EnableShapeDiscard)
			{
				return;
			}
			m_lastDiscardCheck = 0;
			HkUniformGridShape hkUniformGridShape = (HkUniformGridShape)GetShape();
			int hitsAndClear = hkUniformGridShape.GetHitsAndClear();
			if (hkUniformGridShape.ShapeCount <= 0 || hitsAndClear > 0)
			{
				return;
			}
			hkUniformGridShape.DiscardLargeData();
			if (RigidBody2 != null)
			{
				hkUniformGridShape = (HkUniformGridShape)RigidBody2.GetShape();
				hitsAndClear = hkUniformGridShape.GetHitsAndClear();
				if (hitsAndClear <= 0)
				{
					hkUniformGridShape.DiscardLargeData();
				}
			}
		}

		private Vector3 ComputePredictionOffset(IMyEntity entity)
		{
			return entity.Physics.LinearVelocity;
		}

		public override void DebugDraw()
		{
			base.DebugDraw();
			if (!MyDebugDrawSettings.DEBUG_DRAW_VOXEL_PHYSICS_PREDICTION)
			{
				return;
			}
			foreach (MyEntity nearbyEntity in m_nearbyEntities)
			{
				if (!nearbyEntity.MarkedForClose)
				{
					BoundingBoxD worldAABB = nearbyEntity.PositionComp.WorldAABB;
					MyRenderProxy.DebugDrawAABB(worldAABB, Color.Bisque);
					MyRenderProxy.DebugDrawLine3D(GetWorldMatrix().Translation, worldAABB.Center, Color.Bisque, Color.BlanchedAlmond, depthRead: true);
					GetPrediction(nearbyEntity, out var box);
					MyRenderProxy.DebugDrawAABB(box, Color.Crimson);
				}
			}
<<<<<<< HEAD
			using (IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(GetWorldMatrix(), new Color(Color.Cyan, 0.2f), depthRead: true, shaded: false))
			{
				int num = 0;
				BoundingBoxD aabb = default(BoundingBoxD);
				foreach (KeyValuePair<MyCellCoord, MyPrecalcJobPhysicsPrefetch> item in m_workTracker)
				{
					num++;
					MyCellCoord key = item.Key;
					aabb.Min = key.CoordInLod << key.Lod;
					aabb.Min *= 8.0;
					aabb.Min -= m_voxelMap.SizeInMetresHalf;
					aabb.Max = aabb.Min + 8f;
					myDebugDrawBatchAabb.Add(ref aabb);
					if (num > 250)
					{
						break;
					}
=======
			using IMyDebugDrawBatchAabb myDebugDrawBatchAabb = MyRenderProxy.DebugDrawBatchAABB(GetWorldMatrix(), new Color(Color.Cyan, 0.2f), depthRead: true, shaded: false);
			int num = 0;
			BoundingBoxD aabb = default(BoundingBoxD);
			foreach (KeyValuePair<MyCellCoord, MyPrecalcJobPhysicsPrefetch> item in m_workTracker)
			{
				num++;
				MyCellCoord key = item.Key;
				aabb.Min = key.CoordInLod << key.Lod;
				aabb.Min *= 8.0;
				aabb.Min -= m_voxelMap.SizeInMetresHalf;
				aabb.Max = aabb.Min + 8f;
				myDebugDrawBatchAabb.Add(ref aabb);
				if (num > 250)
				{
					break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		internal void OnTaskComplete(MyCellCoord coord, HkBvCompressedMeshShape childShape)
		{
			if (RigidBody != null)
			{
				GetShape(coord.Lod, out var gridShape);
				gridShape.SetChild(coord.CoordInLod.X, coord.CoordInLod.Y, coord.CoordInLod.Z, childShape, HkReferencePolicy.None);
				if (!childShape.IsZero)
				{
					MarkForShapeUpdate();
				}
			}
		}

		internal void OnBatchTaskComplete(Dictionary<Vector3I, HkBvCompressedMeshShape> newShapes, int lod)
		{
			if (!(RigidBody != null))
			{
				return;
			}
			GetShape(lod, out var gridShape);
			bool flag = false;
			foreach (KeyValuePair<Vector3I, HkBvCompressedMeshShape> newShape in newShapes)
			{
				Vector3I key = newShape.Key;
				HkBvCompressedMeshShape value = newShape.Value;
				gridShape.SetChild(key.X, key.Y, key.Z, value, HkReferencePolicy.None);
				flag |= !value.IsZero;
			}
			if (flag)
			{
				MarkForShapeUpdate();
			}
		}

		internal VrVoxelMesh CreateMesh(MyCellCoord coord)
		{
			coord.CoordInLod += m_cellsOffset >> coord.Lod;
			Vector3I vector3I = coord.CoordInLod << 3;
			Vector3I lodVoxelMax = vector3I + 8;
			vector3I -= 1;
			lodVoxelMax += 2;
			MyMesherResult myMesherResult = m_mesher.CalculateMesh(coord.Lod, vector3I, lodVoxelMax, MyStorageDataTypeFlags.ContentAndMaterial, MyVoxelRequestFlags.SurfaceMaterial | MyVoxelRequestFlags.ForPhysics);
			if (!myMesherResult.MeshProduced)
			{
				_ = MyVoxelDebugInputComponent.PhysicsComponent.Static;
			}
			return myMesherResult.Mesh;
		}

		internal unsafe HkBvCompressedMeshShape CreateShape(VrVoxelMesh mesh, int lod)
		{
			if (mesh == null || mesh.TriangleCount == 0 || mesh.VertexCount == 0)
			{
				return (HkBvCompressedMeshShape)HkShape.Empty;
			}
			using (MyUtils.ReuseCollection(ref m_indexListCached))
			{
				using (MyUtils.ReuseCollection(ref m_vertexListCached))
				{
					using (MyUtils.ReuseCollection(ref m_materialListCached))
					{
						MyList<int> indexListCached = m_indexListCached;
						MyList<Vector3> vertexListCached = m_vertexListCached;
						MyList<byte> materialListCached = m_materialListCached;
						vertexListCached.EnsureCapacity(mesh.VertexCount);
						indexListCached.EnsureCapacity(mesh.TriangleCount * 3);
						materialListCached.EnsureCapacity(mesh.TriangleCount);
						for (int i = 0; i < mesh.TriangleCount; i++)
						{
							indexListCached.Add(mesh.Triangles[i].V0);
							indexListCached.Add(mesh.Triangles[i].V2);
							indexListCached.Add(mesh.Triangles[i].V1);
						}
						float scale = mesh.Scale;
						VrVoxelVertex* vertices = mesh.Vertices;
						Vector3 vector = mesh.Start * scale - m_voxelMap.StorageMin * 1f;
						for (int j = 0; j < mesh.VertexCount; j++)
						{
							vertexListCached.Add(vertices[j].Position * scale + vector);
						}
						uint num = 4294967294u;
						for (int k = 0; k < mesh.TriangleCount; k++)
						{
							VrVoxelTriangle vrVoxelTriangle = mesh.Triangles[k];
							byte material = vertices[(int)vrVoxelTriangle.V0].Material;
							if (num == 4294967294u)
							{
								num = material;
							}
							else if (num != material)
							{
								num = uint.MaxValue;
							}
							materialListCached.Add(material);
						}
						fixed (int* indices = indexListCached.GetInternalArray())
						{
							fixed (byte* materials = materialListCached.GetInternalArray())
							{
								fixed (Vector3* vertices2 = vertexListCached.GetInternalArray())
								{
									float physicsConvexRadius = MyPerGameSettings.PhysicsConvexRadius;
									HkBvCompressedMeshShape hkBvCompressedMeshShape = new HkBvCompressedMeshShape(vertices2, vertexListCached.Count, indices, indexListCached.Count, materials, materialListCached.Count, HkWeldingType.None, physicsConvexRadius);
									if (num == 4294967294u)
									{
										num = uint.MaxValue;
									}
									HkShape.SetUserData(hkBvCompressedMeshShape, num);
									return hkBvCompressedMeshShape;
								}
							}
						}
					}
				}
			}
		}

		public override void Close()
		{
			base.Close();
			m_workTracker.CancelAll();
			for (int i = 0; i < RunningBatchTask.Length; i++)
			{
				if (RunningBatchTask[i] != null)
				{
					RunningBatchTask[i].Cancel();
					RunningBatchTask[i] = null;
				}
			}
		}

		private void UpdateNearbyEntities()
		{
			m_nearbyEntities.Clear();
			Vector3 sizeInMetres = m_voxelMap.SizeInMetres;
			sizeInMetres *= m_phantomExtend;
			BoundingBoxD boundingBox = new BoundingBoxD(m_voxelMap.PositionComp.WorldAABB.Center - 0.5f * sizeInMetres, m_voxelMap.PositionComp.WorldAABB.Center + 0.5f * sizeInMetres);
			MyEntities.GetTopMostEntitiesInBox(ref boundingBox, m_nearbyEntities, MyEntityQueryType.Dynamic);
			for (int num = m_nearbyEntities.Count - 1; num >= 0; num--)
			{
				MyEntity myEntity = m_nearbyEntities[num];
				if (!(myEntity is MyCharacter))
				{
<<<<<<< HEAD
					MyCubeGrid myCubeGrid;
					if ((myCubeGrid = myEntity as MyCubeGrid) != null && MyGridPhysicalHierarchy.Static.GetRoot(myCubeGrid) != myCubeGrid)
					{
						m_nearbyEntities.RemoveAt(num);
					}
					else
					{
						HkRigidBody hkRigidBody = myEntity.Physics?.RigidBody;
						if (hkRigidBody == null || hkRigidBody.IsFixedOrKeyframed)
						{
							m_nearbyEntities.RemoveAt(num);
						}
=======
					HkRigidBody hkRigidBody = myEntity.Physics?.RigidBody;
					if (hkRigidBody == null || hkRigidBody.IsFixedOrKeyframed)
					{
						m_nearbyEntities.RemoveAt(num);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
		}

		internal void GenerateAllShapes()
		{
			if (m_mesher != null)
			{
				if (!m_bodiesInitialized)
				{
					CreateRigidBodies();
				}
				Vector3I start = Vector3I.Zero;
				Vector3I size = m_voxelMap.Size;
				Vector3I end = new Vector3I(0, 0, 0);
				end.X = size.X >> 3;
				end.Y = size.Y >> 3;
				end.Z = size.Z >> 3;
				end += start;
				MyPrecalcJobPhysicsPrefetch.Args args = default(MyPrecalcJobPhysicsPrefetch.Args);
				args.GeometryCell = new MyCellCoord(1, start);
				args.Storage = m_voxelMap.Storage;
				args.TargetPhysics = this;
				args.Tracker = m_workTracker;
				MyPrecalcJobPhysicsPrefetch.Args args2 = args;
				Vector3I_RangeIterator vector3I_RangeIterator = new Vector3I_RangeIterator(ref start, ref end);
				while (vector3I_RangeIterator.IsValid())
				{
					MyPrecalcJobPhysicsPrefetch.Start(args2);
					vector3I_RangeIterator.GetNext(out args2.GeometryCell.CoordInLod);
				}
			}
		}

		public override MyStringHash GetMaterialAt(Vector3D worldPos)
		{
			return ((m_voxelMap != null) ? m_voxelMap.GetMaterialAt(ref worldPos) : null)?.MaterialTypeNameHash ?? MyStringHash.NullOrEmpty;
		}

		/// <summary>
		/// Prefetching physical voxels
		/// </summary>
		/// <param name="ray">line</param>
		/// <returns>true if task was added</returns>
		public bool PrefetchShapeOnRay(ref LineD ray)
		{
			if (m_mesher == null)
			{
				return false;
			}
			int lod = (UseLod1VoxelPhysics ? 1 : 0);
			MyVoxelCoordSystems.WorldPositionToLocalPosition(m_voxelMap.PositionLeftBottomCorner, ref ray.From, out var localPosition);
			MyVoxelCoordSystems.WorldPositionToLocalPosition(m_voxelMap.PositionLeftBottomCorner, ref ray.To, out var localPosition2);
			if (!GetShape(lod, out var gridShape))
			{
				return false;
			}
			if (m_cellsToGenerateBuffer.Length < 64)
			{
				m_cellsToGenerateBuffer = new Vector3I[64];
			}
			int hitCellsInRange = gridShape.GetHitCellsInRange(localPosition, localPosition2, m_cellsToGenerateBuffer);
			if (hitCellsInRange == 0)
			{
				return false;
			}
			for (int i = 0; i < hitCellsInRange; i++)
			{
				MyCellCoord myCellCoord = new MyCellCoord(lod, m_cellsToGenerateBuffer[i]);
				if (!m_workTracker.Exists(myCellCoord))
				{
					MyPrecalcJobPhysicsPrefetch.Args args = default(MyPrecalcJobPhysicsPrefetch.Args);
					args.TargetPhysics = this;
					args.Tracker = m_workTracker;
					args.GeometryCell = myCellCoord;
					args.Storage = m_voxelMap.Storage;
					MyPrecalcJobPhysicsPrefetch.Start(args);
				}
			}
			return true;
		}

		public Vector3 ComputeCellCenterOffset(Vector3 bodyLocal)
		{
			return (Vector3I)(bodyLocal / 8f) * 8f;
		}
	}
}
