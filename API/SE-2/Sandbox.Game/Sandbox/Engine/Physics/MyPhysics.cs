using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Havok;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Engine.Voxels;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Library.Memory;
using VRage.Library.Threading;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace Sandbox.Engine.Physics
{
	[MySessionComponentDescriptor(MyUpdateOrder.Simulation, 1000)]
	[StaticEventOwner]
	public class MyPhysics : MySessionComponentBase
	{
		public struct HitInfo : IHitInfo
		{
			public HkHitInfo HkHitInfo;

			public Vector3D Position;

			Vector3D IHitInfo.Position => Position;

			IMyEntity IHitInfo.HitEntity => HkHitInfo.GetHitEntity();

			Vector3 IHitInfo.Normal => HkHitInfo.Normal;

			float IHitInfo.Fraction => HkHitInfo.HitFraction;

			public HitInfo(HkHitInfo hi, Vector3D worldPosition)
			{
				HkHitInfo = hi;
				Position = worldPosition;
			}

			public override string ToString()
			{
				IMyEntity hitEntity = HkHitInfo.GetHitEntity();
				if (hitEntity != null)
				{
					return hitEntity.ToString();
				}
				return base.ToString();
			}
		}

		public struct MyContactPointEvent
		{
			public HkContactPointEvent ContactPointEvent;

			public Vector3D Position;

			public Vector3 Normal => ContactPointEvent.ContactPoint.Normal;
		}

		public struct FractureImpactDetails
		{
			public HkdFractureImpactDetails Details;

			public HkWorld World;

			public Vector3D ContactInWorld;

			public MyEntity Entity;
		}

		/// <summary>
		/// Collision layers that can be used to filter what collision should be found for casting methods.
		/// </summary>
		/// <remarks>!!** If new layer is added then also add conversion to "GetCollisionLayer" function **!! 
		/// Also max layer number is 31!!</remarks>
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct CollisionLayers
		{
			public const int BlockPlacementTestCollisionLayer = 7;

			public const int MissileLayer = 8;

			public const int NoVoxelCollisionLayer = 9;

			public const int LightFloatingObjectCollisionLayer = 10;

			/// <summary>
			/// Layer that doesn't collide with static grids and voxels
			/// </summary>
			public const int VoxelLod1CollisionLayer = 11;

			public const int NotCollideWithStaticLayer = 12;

			/// <summary>
			/// Static grids
			/// </summary>
			public const int StaticCollisionLayer = 13;

			public const int CollideWithStaticLayer = 14;

			public const int DefaultCollisionLayer = 15;

			public const int DynamicDoubledCollisionLayer = 16;

			public const int KinematicDoubledCollisionLayer = 17;

			public const int CharacterCollisionLayer = 18;

			public const int NoCollisionLayer = 19;

			public const int DebrisCollisionLayer = 20;

			public const int GravityPhantomLayer = 21;

			public const int CharacterNetworkCollisionLayer = 22;

			public const int FloatingObjectCollisionLayer = 23;

			public const int ObjectDetectionCollisionLayer = 24;

			public const int VirtualMassLayer = 25;

			public const int CollectorCollisionLayer = 26;

			public const int AmmoLayer = 27;

			public const int VoxelCollisionLayer = 28;

			public const int ExplosionRaycastLayer = 29;

			public const int CollisionLayerWithoutCharacter = 30;

			public const int RagdollCollisionLayer = 31;

			public const int TargetDummyLayer = 6;
		}

		public struct ForceInfo
		{
			public readonly bool ActiveOnly;

			public readonly float? MaxSpeed;

			public readonly Vector3? Force;

			public readonly Vector3? Torque;

			public readonly Vector3D? Position;

			public readonly MyPhysicsBody Body;

			public readonly MyPhysicsForceType Type;

			public ForceInfo(MyPhysicsBody body, bool activeOnly, float? maxSpeed, Vector3? force, Vector3? torque, Vector3D? position, MyPhysicsForceType type)
			{
				Body = body;
				Type = type;
				Force = force;
				Torque = torque;
				Position = position;
				MaxSpeed = maxSpeed;
				ActiveOnly = activeOnly;
			}
		}

		private struct RayCastData
		{
			public object Callback;

			public Vector3D To;

			public Vector3D From;

			public HitInfo? HitInfo;

			public int RayCastFilterLayer;

			public List<HitInfo> Collector;
		}

		private struct QueryBoxData
		{
			public int Filter;

			public Quaternion Rotation;

			public Vector3 HalfExtents;

			public Vector3D Translation;

			public List<HkBodyCollision> Results;

			public Action<List<HkBodyCollision>> Callback;
		}

		private class ParallelRayCastQuery
		{
			public enum TKind
			{
				RaycastSingle,
				RaycastAll,
				GetPenetrationsBox
			}

			private static MyConcurrentPool<ParallelRayCastQuery> m_pool = new MyConcurrentPool<ParallelRayCastQuery>(0, null, 100000);

			public ParallelRayCastQuery Next;

			public TKind Kind;

			public RayCastData RayCastData;

			public QueryBoxData QueryBoxData;

			public void ExecuteRayCast()
			{
				switch (Kind)
				{
				case TKind.RaycastSingle:
					RayCastData.HitInfo = CastRayInternal(ref RayCastData.From, ref RayCastData.To, RayCastData.RayCastFilterLayer);
					break;
				case TKind.RaycastAll:
					CastRayInternal(ref RayCastData.From, ref RayCastData.To, RayCastData.Collector, RayCastData.RayCastFilterLayer);
					break;
				case TKind.GetPenetrationsBox:
					GetPenetrationsBox(ref QueryBoxData.HalfExtents, ref QueryBoxData.Translation, ref QueryBoxData.Rotation, QueryBoxData.Results, QueryBoxData.Filter);
					break;
				}
			}

			public void DeliverResults()
			{
				try
				{
					switch (Kind)
					{
					case TKind.RaycastSingle:
						((Action<HitInfo?>)RayCastData.Callback).InvokeIfNotNull(RayCastData.HitInfo);
						break;
					case TKind.RaycastAll:
						((Action<List<HitInfo>>)RayCastData.Callback).InvokeIfNotNull(RayCastData.Collector);
						break;
					case TKind.GetPenetrationsBox:
						QueryBoxData.Callback.InvokeIfNotNull(QueryBoxData.Results);
						break;
					}
				}
				finally
				{
					Return();
				}
			}

			public static ParallelRayCastQuery Allocate()
			{
				return m_pool.Get();
			}

			public void Return()
			{
				Next = null;
				RayCastData = default(RayCastData);
				QueryBoxData = default(QueryBoxData);
				m_pool.Return(this);
			}

			public void Cancel()
			{
				Return();
			}
		}

		private class DeliverData : AbstractWork
		{
			private class Sandbox_Engine_Physics_MyPhysics_003C_003EDeliverData_003C_003EActor : IActivator, IActivator<DeliverData>
			{
				private sealed override object CreateInstance()
				{
					return new DeliverData();
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override DeliverData CreateInstance()
				{
					return new DeliverData();
				}

				DeliverData IActivator<DeliverData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			public readonly Action<int> ExecuteJob;

			public readonly List<ParallelRayCastQuery> Jobs = new List<ParallelRayCastQuery>();

			public DeliverData()
			{
				ExecuteJob = ExecuteJobImpl;
				Options = Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.HK_JOB_TYPE_RAYCAST_QUERY, "RayCastResults");
			}

			private void ExecuteJobImpl(int i)
			{
				Jobs[i].ExecuteRayCast();
			}

			public override void DoWork(WorkData unused)
			{
				DeliverResults();
			}

			private void DeliverResults()
			{
				foreach (ParallelRayCastQuery job in Jobs)
				{
					job.DeliverResults();
				}
				Jobs.Clear();
				m_pendingRayCastsParallelPool.Return(this);
			}
		}

		protected sealed class OnClustersReordered_003C_003ESystem_Collections_Generic_List_00601_003CVRageMath_BoundingBoxD_003E : ICallSite<IMyEventOwner, List<BoundingBoxD>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in List<BoundingBoxD> list, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				OnClustersReordered(list);
			}
		}

		protected sealed class UpdateServerDebugCamera_003C_003EVRageMath_MatrixD : ICallSite<IMyEventOwner, MatrixD, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MatrixD wm, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateServerDebugCamera(wm);
			}
		}

		protected sealed class ControlVDBRecording_003C_003ESystem_String : ICallSite<IMyEventOwner, string, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in string fileName, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				ControlVDBRecording(fileName);
			}
		}

		protected sealed class SetScheduling_003C_003ESystem_Boolean_0023System_Boolean : ICallSite<IMyEventOwner, bool, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in bool multithread, in bool parallel, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				SetScheduling(multithread, parallel);
			}
		}

		private static int? m_currentUserThreadId;

		public static int ThreadId;

		public static MyClusterTree Clusters;

		private static bool ClustersNeedSync = false;

		private static HkJobThreadPool m_threadPool;

		private static HkJobQueue m_jobQueue;

		private static MyProfiler[] m_havokThreadProfilers;

		private bool m_updateKinematicBodies = MyFakes.ENABLE_ANIMATED_KINEMATIC_UPDATE && !Sync.IsServer;

		private List<MyPhysicsBody> m_iterationBodies = new List<MyPhysicsBody>();

		private List<HkCharacterRigidBody> m_characterIterationBodies = new List<HkCharacterRigidBody>();

		[ThreadStatic]
<<<<<<< HEAD
		private static List<MyEntity> m_tmpEntityResults;
=======
		private static List<MyEntity> m_tmpEntityResults = new List<MyEntity>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private int m_nextMemoryUpdate;

		private MyMemorySystem.AllocationRecord? m_currentAllocations;

		private static MyMemorySystem m_physicsMemorySystem = Singleton<MyMemoryTracker>.Instance.ProcessMemorySystem.RegisterSubsystem("Physics");

		private static Queue<long> m_timestamps = new Queue<long>(120);

		private MyWorldObserver m_worldObserver;

		private static SpinLockRef m_raycastLock = new SpinLockRef();

		private const string HkProfilerSymbol = "HkProfiler";

		private List<HkSimulationIslandInfo> m_simulationIslandInfos;

		private static Queue<FractureImpactDetails> m_destructionQueue = new Queue<FractureImpactDetails>();

		public static bool DebugDrawClustersEnable = false;

		public static MatrixD DebugDrawClustersMatrix = MatrixD.Identity;

		private static List<BoundingBoxD> m_clusterStaticObjects = new List<BoundingBoxD>();

		/// <summary>
		/// Used for saving result of search in CastLongRay. (For optimalisation rules)
		/// </summary>
		private static List<MyLineSegmentOverlapResult<MyVoxelBase>> m_foundEntities = new List<MyLineSegmentOverlapResult<MyVoxelBase>>();

		private static List<HkHitInfo?> m_resultShapeCasts;

		private static readonly HashSet<MyPhysicsBody> m_pendingCollisionFilterRefreshes = new HashSet<MyPhysicsBody>();

		public static bool SyncVDBCamera = false;

		private static MatrixD? ClientCameraWM;

		private static bool IsVDBRecording = false;

		private static string VDBRecordFile = null;

		[ThreadStatic]
		private static List<HkHitInfo> m_resultHits;

		[ThreadStatic]
		private static List<MyClusterTree.MyClusterQueryResult> m_resultWorlds;

		private static List<HkShapeCollision> m_shapeCollisionResultsCache;

		private static ParallelRayCastQuery m_pendingRayCasts;

		private static MyConcurrentPool<DeliverData> m_pendingRayCastsParallelPool = new MyConcurrentPool<DeliverData>(0, null, 100);

		private const int SWITCH_HISTERESIS_FRAMES = 10;

		private const float SLOW_FRAME_DEVIATION_FACTOR = 3f;

		private const float FAST_FRAME_DEVIATION_FACTOR = 1.5f;

		private const float SLOW_FRAME_ABSOLUTE_THRESHOLD = 30f;

		private int m_slowFrames;

		private int m_fastFrames;

		private bool m_optimizeNextFrame;

		private bool m_optimizationsEnabled;

		private float m_averageFrameTime;

		private const int NUM_FRAMES_TO_CONSIDER = 100;

		private const int NUM_SLOW_FRAMES_TO_CONSIDER = 1000;

		private List<IPhysicsStepOptimizer> m_optimizers;

		private List<MyTuple<HkWorld, MyTimeSpan>> m_timings = new List<MyTuple<HkWorld, MyTimeSpan>>();

<<<<<<< HEAD
		/// <summary>
		/// Number of physics steps done in last second
		/// </summary>
		public static int StepsLastSecond => m_timestamps.Count;
=======
		private bool ParallelSteppingInitialized;

		public static int StepsLastSecond => m_timestamps.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Simulation ratio, when physics cannot keep up, this is smaller than 1
		/// </summary>
		public static float SimulationRatio
		{
			get
			{
				if (MyFakes.ENABLE_SIMSPEED_LOCKING || MyFakes.PRECISE_SIM_SPEED)
				{
					return Sandbox.Engine.Platform.Game.SimulationRatio;
				}
				return (float)Math.Round(Math.Max(0.5f, StepsLastSecond) / 60f, 2);
			}
		}

		public static float RestingVelocity
		{
			get
			{
				if (!MyPerGameSettings.BallFriendlyPhysics)
				{
					return float.MaxValue;
				}
				return 3f;
			}
		}

		public static Queue<ForceInfo> QueuedForces { get; private set; }

		public static SpinLockRef RaycastLock => m_raycastLock;

		public static HkWorld SingleWorld => Clusters.GetList()[0] as HkWorld;

		public static bool InsideSimulation { get; private set; }

		private static void InitCollisionFilters(HkWorld world)
		{
			world.DisableCollisionsBetween(16, 17);
			world.DisableCollisionsBetween(17, 18);
			world.DisableCollisionsBetween(16, 22);
			world.DisableCollisionsBetween(12, 13);
			world.DisableCollisionsBetween(12, 28);
			world.DisableCollisionsBetween(17, 15);
			world.DisableCollisionsBetween(17, 13);
			world.DisableCollisionsBetween(17, 28);
			world.DisableCollisionsBetween(17, 8);
			world.DisableCollisionsBetween(21, 13);
			world.DisableCollisionsBetween(21, 28);
			world.DisableCollisionsBetween(21, 15);
			world.DisableCollisionsBetween(21, 16);
			world.DisableCollisionsBetween(21, 17);
			world.DisableCollisionsBetween(21, 18);
			world.DisableCollisionsBetween(21, 22);
			world.DisableCollisionsBetween(21, 24);
			world.DisableCollisionsBetween(21, 8);
			world.DisableCollisionsBetween(25, 13);
			world.DisableCollisionsBetween(25, 28);
			world.DisableCollisionsBetween(25, 15);
			world.DisableCollisionsBetween(25, 18);
			world.DisableCollisionsBetween(25, 22);
			world.DisableCollisionsBetween(25, 16);
			world.DisableCollisionsBetween(25, 17);
			world.DisableCollisionsBetween(25, 20);
			world.DisableCollisionsBetween(25, 23);
			world.DisableCollisionsBetween(25, 10);
			world.DisableCollisionsBetween(25, 24);
			world.DisableCollisionsBetween(25, 25);
			world.DisableCollisionsBetween(25, 8);
			world.DisableCollisionsBetween(19, 13);
			world.DisableCollisionsBetween(19, 28);
			world.DisableCollisionsBetween(19, 15);
			world.DisableCollisionsBetween(19, 18);
			world.DisableCollisionsBetween(19, 22);
			world.DisableCollisionsBetween(19, 16);
			world.DisableCollisionsBetween(19, 17);
			world.DisableCollisionsBetween(19, 20);
			world.DisableCollisionsBetween(19, 23);
			world.DisableCollisionsBetween(19, 10);
			world.DisableCollisionsBetween(19, 21);
			world.DisableCollisionsBetween(19, 24);
			world.DisableCollisionsBetween(19, 25);
			world.DisableCollisionsBetween(19, 19);
			world.DisableCollisionsBetween(19, 8);
			world.DisableCollisionsBetween(6, 13);
			world.DisableCollisionsBetween(6, 28);
			world.DisableCollisionsBetween(6, 22);
			world.DisableCollisionsBetween(6, 16);
			world.DisableCollisionsBetween(6, 17);
			world.DisableCollisionsBetween(6, 20);
			world.DisableCollisionsBetween(6, 23);
			world.DisableCollisionsBetween(6, 10);
			world.DisableCollisionsBetween(6, 21);
			world.DisableCollisionsBetween(6, 24);
			world.DisableCollisionsBetween(6, 25);
			world.DisableCollisionsBetween(6, 6);
			if (MyPerGameSettings.PhysicsNoCollisionLayerWithDefault)
			{
				world.DisableCollisionsBetween(19, 0);
				world.DisableCollisionsBetween(6, 0);
			}
			world.DisableCollisionsBetween(24, 24);
			world.DisableCollisionsBetween(26, 13);
			world.DisableCollisionsBetween(26, 28);
			world.DisableCollisionsBetween(26, 15);
			world.DisableCollisionsBetween(26, 18);
			world.DisableCollisionsBetween(26, 22);
			world.DisableCollisionsBetween(26, 16);
			world.DisableCollisionsBetween(26, 17);
			world.DisableCollisionsBetween(26, 20);
			world.DisableCollisionsBetween(26, 21);
			world.DisableCollisionsBetween(26, 24);
			world.DisableCollisionsBetween(26, 25);
			world.DisableCollisionsBetween(26, 8);
			_ = Sync.IsServer;
			if (!MyFakes.ENABLE_CHARACTER_AND_DEBRIS_COLLISIONS)
			{
				world.DisableCollisionsBetween(20, 18);
				world.DisableCollisionsBetween(20, 22);
				world.DisableCollisionsBetween(20, 8);
				world.DisableCollisionsBetween(10, 18);
				world.DisableCollisionsBetween(10, 22);
				world.DisableCollisionsBetween(10, 8);
			}
			world.DisableCollisionsBetween(29, 28);
			world.DisableCollisionsBetween(29, 18);
			world.DisableCollisionsBetween(29, 19);
			world.DisableCollisionsBetween(29, 6);
			world.DisableCollisionsBetween(29, 20);
			world.DisableCollisionsBetween(29, 21);
			world.DisableCollisionsBetween(29, 22);
			world.DisableCollisionsBetween(29, 23);
			world.DisableCollisionsBetween(29, 10);
			world.DisableCollisionsBetween(29, 24);
			world.DisableCollisionsBetween(29, 25);
			world.DisableCollisionsBetween(29, 26);
			world.DisableCollisionsBetween(29, 27);
			world.DisableCollisionsBetween(30, 18);
			world.DisableCollisionsBetween(30, 19);
			world.DisableCollisionsBetween(30, 6);
			world.DisableCollisionsBetween(14, 14);
			world.DisableCollisionsBetween(14, 16);
			world.DisableCollisionsBetween(14, 15);
			world.DisableCollisionsBetween(14, 18);
			world.DisableCollisionsBetween(14, 19);
			world.DisableCollisionsBetween(14, 6);
			world.DisableCollisionsBetween(14, 20);
			world.DisableCollisionsBetween(14, 21);
			world.DisableCollisionsBetween(14, 22);
			world.DisableCollisionsBetween(14, 23);
			world.DisableCollisionsBetween(14, 10);
			world.DisableCollisionsBetween(14, 24);
			world.DisableCollisionsBetween(14, 25);
			world.DisableCollisionsBetween(14, 26);
			world.DisableCollisionsBetween(14, 27);
			world.DisableCollisionsBetween(14, 8);
			world.DisableCollisionsBetween(31, 13);
			world.DisableCollisionsBetween(31, 28);
			world.DisableCollisionsBetween(31, 15);
			world.DisableCollisionsBetween(31, 18);
			world.DisableCollisionsBetween(31, 22);
			world.DisableCollisionsBetween(31, 16);
			world.DisableCollisionsBetween(31, 17);
			world.DisableCollisionsBetween(31, 20);
			world.DisableCollisionsBetween(31, 23);
			world.DisableCollisionsBetween(31, 10);
			world.DisableCollisionsBetween(31, 21);
			world.DisableCollisionsBetween(31, 24);
			world.DisableCollisionsBetween(31, 25);
			world.DisableCollisionsBetween(31, 19);
			world.DisableCollisionsBetween(31, 6);
			world.DisableCollisionsBetween(31, 29);
			world.DisableCollisionsBetween(31, 30);
			world.DisableCollisionsBetween(31, 14);
			world.DisableCollisionsBetween(31, 26);
			world.DisableCollisionsBetween(31, 27);
			world.DisableCollisionsBetween(31, 8);
			if (!MyFakes.ENABLE_JETPACK_RAGDOLL_COLLISIONS)
			{
				world.DisableCollisionsBetween(31, 31);
			}
			if (MyVoxelPhysicsBody.UseLod1VoxelPhysics)
			{
				world.DisableCollisionsBetween(16, 28);
				world.DisableCollisionsBetween(17, 28);
				world.DisableCollisionsBetween(15, 28);
				world.DisableCollisionsBetween(14, 28);
				world.DisableCollisionsBetween(20, 28);
				world.DisableCollisionsBetween(23, 28);
				world.DisableCollisionsBetween(8, 28);
				world.DisableCollisionsBetween(24, 11);
				world.DisableCollisionsBetween(18, 11);
				world.DisableCollisionsBetween(22, 11);
				world.DisableCollisionsBetween(10, 11);
				world.DisableCollisionsBetween(31, 11);
				world.DisableCollisionsBetween(29, 11);
				world.DisableCollisionsBetween(26, 11);
				world.DisableCollisionsBetween(21, 11);
				world.DisableCollisionsBetween(19, 11);
				world.DisableCollisionsBetween(6, 11);
				world.DisableCollisionsBetween(25, 11);
				world.DisableCollisionsBetween(17, 11);
				world.DisableCollisionsBetween(12, 11);
			}
			world.DisableCollisionsBetween(9, 17);
			world.DisableCollisionsBetween(9, 21);
			world.DisableCollisionsBetween(9, 25);
			world.DisableCollisionsBetween(9, 19);
			world.DisableCollisionsBetween(9, 6);
			world.DisableCollisionsBetween(9, 26);
			world.DisableCollisionsBetween(9, 14);
			world.DisableCollisionsBetween(9, 31);
			world.DisableCollisionsBetween(9, 28);
			world.DisableCollisionsBetween(9, 11);
			if (!Sync.IsServer)
			{
				world.DisableCollisionsBetween(9, 22);
			}
			world.DisableCollisionsBetween(7, 17);
			world.DisableCollisionsBetween(7, 21);
			world.DisableCollisionsBetween(7, 25);
			world.DisableCollisionsBetween(7, 19);
			world.DisableCollisionsBetween(7, 6);
			world.DisableCollisionsBetween(7, 26);
			world.DisableCollisionsBetween(7, 14);
			world.DisableCollisionsBetween(7, 31);
			world.DisableCollisionsBetween(7, 28);
			world.DisableCollisionsBetween(7, 11);
		}

		public static bool CheckThread()
		{
			int num = m_currentUserThreadId ?? ThreadId;
<<<<<<< HEAD
			return Thread.CurrentThread.ManagedThreadId == num;
=======
			return Thread.get_CurrentThread().get_ManagedThreadId() == num;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void AssertThread()
		{
		}

		public override void LoadData()
		{
			HkVDB.Port = (Sync.IsServer ? 25001 : 25002);
			HkBaseSystem.EnableAssert(-668493307, enable: false);
			HkBaseSystem.EnableAssert(952495168, enable: false);
			HkBaseSystem.EnableAssert(1501626980, enable: false);
			HkBaseSystem.EnableAssert(-258736554, enable: false);
			HkBaseSystem.EnableAssert(524771844, enable: false);
			HkBaseSystem.EnableAssert(1081361407, enable: false);
			HkBaseSystem.EnableAssert(-1383504214, enable: false);
			HkBaseSystem.EnableAssert(-265005969, enable: false);
			HkBaseSystem.EnableAssert(1976984315, enable: false);
			HkBaseSystem.EnableAssert(-252450131, enable: false);
			HkBaseSystem.EnableAssert(-1400416854, enable: false);
			ThreadId = Thread.get_CurrentThread().get_ManagedThreadId();
			Clusters = new MyClusterTree(MySession.Static.WorldBoundaries, MyFakes.MP_SYNC_CLUSTERTREE && !Sync.IsServer);
			MyClusterTree clusters = Clusters;
			clusters.OnClusterCreated = (Func<int, BoundingBoxD, object>)Delegate.Combine(clusters.OnClusterCreated, new Func<int, BoundingBoxD, object>(OnClusterCreated));
			MyClusterTree clusters2 = Clusters;
			clusters2.OnClusterRemoved = (Action<object, int>)Delegate.Combine(clusters2.OnClusterRemoved, new Action<object, int>(OnClusterRemoved));
			MyClusterTree clusters3 = Clusters;
			clusters3.OnFinishBatch = (Action<object>)Delegate.Combine(clusters3.OnFinishBatch, new Action<object>(OnFinishBatch));
			MyClusterTree clusters4 = Clusters;
			clusters4.OnClustersReordered = (Action)Delegate.Combine(clusters4.OnClustersReordered, new Action(Tree_OnClustersReordered));
			MyClusterTree clusters5 = Clusters;
			clusters5.GetEntityReplicableExistsById = (Func<long, bool>)Delegate.Combine(clusters5.GetEntityReplicableExistsById, new Func<long, bool>(GetEntityReplicableExistsById));
			if (Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.Settings.EnableSelectivePhysicsUpdates)
			{
				m_worldObserver = new MyWorldObserver(Clusters);
			}
			QueuedForces = new Queue<ForceInfo>();
			if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
			{
				m_threadPool = new HkJobThreadPool();
				m_jobQueue = new HkJobQueue(m_threadPool.ThreadCount + 1);
			}
			HkCylinderShape.SetNumberOfVirtualSideSegments(32);
			InitStepOptimizer();
		}

		private HkWorld OnClusterCreated(int clusterId, BoundingBoxD bbox)
		{
			return CreateHkWorld((float)bbox.Size.Max());
		}

		private void OnClusterRemoved(object world, int clusterId)
		{
			HkWorld hkWorld = (HkWorld)world;
			if (hkWorld.DestructionWorld != null)
			{
				hkWorld.DestructionWorld.Dispose();
				hkWorld.DestructionWorld = null;
			}
			hkWorld.Dispose();
			if (m_worldObserver != null)
			{
				m_worldObserver.RemoveCluster(clusterId);
			}
		}

		private void OnFinishBatch(object world)
		{
			((HkWorld)world).FinishBatch();
		}

		public static HkWorld CreateHkWorld(float broadphaseSize = 100000f)
		{
			HkWorld.CInfo cInfo = CreateWorldCInfo(MyPerGameSettings.EnableGlobalGravity, broadphaseSize, MyFakes.WHEEL_SOFTNESS ? float.MaxValue : RestingVelocity, MyFakes.ENABLE_HAVOK_MULTITHREADING, MySession.Static.Settings.PhysicsIterations);
			HkWorld hkWorld = new HkWorld(ref cInfo);
			hkWorld.MarkForWrite();
			if (MySession.Static.Settings.WorldSizeKm > 0)
			{
				hkWorld.EntityLeftWorld += HavokWorld_EntityLeftWorld;
			}
			if (MyPerGameSettings.Destruction && Sync.IsServer)
			{
				hkWorld.DestructionWorld = new HkdWorld(hkWorld);
			}
			if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
			{
				hkWorld.InitMultithreading(m_threadPool, m_jobQueue);
			}
			hkWorld.DeactivationRotationSqrdA /= 3f;
			hkWorld.DeactivationRotationSqrdB /= 3f;
			InitCollisionFilters(hkWorld);
			return hkWorld;
		}

		private static HkWorld.CInfo CreateWorldCInfo(bool enableGlobalGravity, float broadphaseCubeSideLength, float contactRestingVelocity, bool enableMultithreading, int solverIterations)
		{
			HkWorld.CInfo result = HkWorld.CInfo.Create();
			result.Gravity = (enableGlobalGravity ? new Vector3(0.0, -9.8, 0.0) : Vector3.Zero);
			result.BroadPhaseWorldAabb = new BoundingBox(new Vector3(broadphaseCubeSideLength * -0.5f), new Vector3(broadphaseCubeSideLength * 0.5f));
			result.ContactPointGeneration = HkWorld.ContactPointGeneration.CONTACT_POINT_REJECT_DUBIOUS;
			result.SolverTau = 0.6f;
			result.SolverDamp = 1f;
			result.SolverIterations = ((solverIterations < 8) ? 8 : solverIterations);
			result.SimulationType = (enableMultithreading ? HkWorld.SimulationType.SIMULATION_TYPE_MULTITHREADED : HkWorld.SimulationType.SIMULATION_TYPE_CONTINUOUS);
			result.BroadPhaseNumMarkers = 0;
			result.BroadPhaseBorderBehaviour = HkWorld.BroadPhaseBorderBehaviour.BROADPHASE_BORDER_REMOVE_ENTITY;
			result.CollisionTolerance = 0.1f;
			result.FireCollisionCallbacks = true;
			result.ContactRestingVelocity = ((contactRestingVelocity >= 3.40282E+38f) ? 3.40282E+38f : contactRestingVelocity);
			result.ExpectedMinPsiDeltaTime = 0.0166666675f;
			result.SolverMicrosteps = 2;
			result.MinDesiredIslandSize = 2u;
			return result;
		}

		private static void HavokWorld_EntityLeftWorld(HkEntity hkEntity)
		{
			List<IMyEntity> allEntities = hkEntity.GetAllEntities();
			foreach (IMyEntity item in allEntities)
			{
				if (!Sync.IsServer || item == null)
<<<<<<< HEAD
				{
					continue;
				}
				if (item is MyCharacter)
				{
=======
				{
					continue;
				}
				if (item is MyCharacter)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					((MyCharacter)item).DoDamage(1000f, MyDamageType.Suicide, updateSync: true, 0L);
				}
				else if (!(item is MyVoxelMap) && !(item is MyCubeBlock))
				{
					if (item is MyCubeGrid)
					{
						MyCubeGrid.KillAllCharacters(item as MyCubeGrid);
						MyLog.Default.Info($"HavokWorld_EntityLeftWorld removed entity '{item.Name}:{item.DisplayName}' with entity id '{item.EntityId}'");
						item.Close();
					}
					else if (item is MyFloatingObject)
					{
						MyFloatingObjects.RemoveFloatingObject((MyFloatingObject)item);
					}
					else if (item is MyFracturedPiece)
					{
						MyFracturedPiecesManager.Static.RemoveFracturePiece((MyFracturedPiece)item, 0f);
					}
					else
					{
						item.Close();
					}
				}
			}
			allEntities.Clear();
		}

		protected override void UnloadData()
		{
			if (m_worldObserver != null)
			{
				m_worldObserver.CleanUp(Clusters);
			}
			Clusters.Dispose();
			MyClusterTree clusters = Clusters;
			clusters.OnClusterCreated = (Func<int, BoundingBoxD, object>)Delegate.Remove(clusters.OnClusterCreated, new Func<int, BoundingBoxD, object>(OnClusterCreated));
			MyClusterTree clusters2 = Clusters;
			clusters2.OnClusterRemoved = (Action<object, int>)Delegate.Remove(clusters2.OnClusterRemoved, new Action<object, int>(OnClusterRemoved));
			Clusters = null;
			QueuedForces = null;
			if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
			{
				m_threadPool.Dispose();
				m_threadPool = null;
				m_jobQueue.Dispose();
				m_jobQueue = null;
			}
			m_destructionQueue.Clear();
			if (MyPerGameSettings.Destruction)
			{
				HkdBreakableShape.DisposeSharedMaterial();
			}
			UnloadStepOptimizer();
			CancelParallelRayCasts();
			m_currentAllocations?.Dispose();
			m_currentAllocations = null;
		}

		private void AddTimestamp()
		{
			long timestamp = Stopwatch.GetTimestamp();
			m_timestamps.Enqueue(timestamp);
			long num = timestamp - Stopwatch.Frequency;
			while (m_timestamps.Peek() < num)
			{
				m_timestamps.Dequeue();
			}
		}

		private void UpdateMemoryStats()
		{
			if (m_nextMemoryUpdate-- <= 0)
			{
				m_nextMemoryUpdate = 100;
				long num = HkBaseSystem.GetCurrentMemoryConsumption();
				if (num > int.MaxValue)
				{
					num = 2147483647L;
				}
				m_currentAllocations?.Dispose();
				m_currentAllocations = m_physicsMemorySystem.RegisterAllocation("Pooled memory", num);
			}
		}

		private static void ProfilerBegin(string block)
		{
		}

		private static void ProfilerEnd(long elapsedTicks)
		{
			MyTimeSpan.FromTicks(elapsedTicks);
		}

		private void SimulateInternal()
		{
			ExecuteParallelRayCasts();
			InsideSimulation = true;
			HkBaseSystem.OnSimulationFrameStarted((long)MySandboxGame.Static.SimulationFrameCounter);
			StepVDB();
			ProcessDestructions();
			StepWorlds();
			HkBaseSystem.OnSimulationFrameFinished();
			InsideSimulation = false;
			ReplayHavokTimers();
			DrawIslands();
			UpdateActiveRigidBodies();
			UpdateCharacters();
			EnsureClusterSpace();
		}

		private void UpdateCharacters()
		{
			foreach (HkWorld item in Clusters.GetList())
			{
				IterateCharacters(item);
			}
			Clusters.SuppressClusterReorder = true;
			foreach (HkCharacterRigidBody characterIterationBody in m_characterIterationBodies)
			{
				MyPhysicsBody myPhysicsBody = (MyPhysicsBody)characterIterationBody.GetHitRigidBody().UserObject;
				bool num = myPhysicsBody.Entity.Parent == null && Vector3D.DistanceSquared(myPhysicsBody.Entity.WorldMatrix.Translation, myPhysicsBody.GetWorldMatrix().Translation) > 9.9999997473787516E-05;
				(myPhysicsBody.Entity as MyCharacter)?.UpdatePhysicalMovement();
				if (num)
				{
					myPhysicsBody.UpdateCluster();
				}
			}
			Clusters.SuppressClusterReorder = false;
		}

		private void UpdateActiveRigidBodies()
		{
			long num = 0L;
			foreach (HkWorld item in Clusters.GetList())
			{
				IterateBodies(item);
				num += item.ActiveRigidBodies.Count;
			}
			MyPerformanceCounter.PerCameraDrawWrite["Active rigid bodies"] = num;
			Clusters.SuppressClusterReorder = true;
			foreach (MyPhysicsBody iterationBody in m_iterationBodies)
			{
				if (m_updateKinematicBodies && iterationBody.IsKinematic)
				{
					iterationBody.OnMotionKinematic();
				}
				else
				{
					iterationBody.OnMotionDynamic();
				}
			}
			Clusters.SuppressClusterReorder = false;
		}

		private void DrawIslands()
		{
			if (!MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SIMULATION_ISLANDS)
			{
				return;
			}
			foreach (MyClusterTree.MyCluster cluster in Clusters.GetClusters())
			{
				Vector3D center = cluster.AABB.Center;
				HkWorld hkWorld = (HkWorld)cluster.UserData;
				using (MyUtils.ReuseCollection(ref m_simulationIslandInfos))
				{
					hkWorld.ReadSimulationIslandInfos(m_simulationIslandInfos);
					foreach (HkSimulationIslandInfo simulationIslandInfo in m_simulationIslandInfos)
					{
						BoundingBoxD aabb = new BoundingBoxD(simulationIslandInfo.AABB.Min + center, simulationIslandInfo.AABB.Max + center);
						if (aabb.Distance(MySector.MainCamera.Position) < 500.0 && simulationIslandInfo.IsActive)
						{
							MyRenderProxy.DebugDrawAABB(aabb, simulationIslandInfo.IsActive ? Color.Red : Color.RoyalBlue);
						}
					}
				}
			}
		}

		private static void ReplayHavokTimers()
		{
		}

		public override void Simulate()
		{
			if (!MySandboxGame.IsGameReady)
			{
				return;
			}
			AddTimestamp();
			UpdateMemoryStats();
			if (MyFakes.PAUSE_PHYSICS && !MyFakes.STEP_PHYSICS)
			{
				return;
			}
			MyFakes.STEP_PHYSICS = false;
			MySimpleProfiler.Begin("Physics", MySimpleProfiler.ProfilingBlockType.OTHER, "Simulate");
<<<<<<< HEAD
			while (QueuedForces.Count > 0)
			{
				ForceInfo forceInfo = QueuedForces.Dequeue();
				forceInfo.Body.AddForce(forceInfo.Type, forceInfo.Force, forceInfo.Position, forceInfo.Torque, forceInfo.MaxSpeed, applyImmediately: true, forceInfo.ActiveOnly);
			}
			ProcessCollisionFilterRefreshes();
			using (m_raycastLock.Acquire())
			{
				SimulateInternal();
			}
=======
			while (QueuedForces.get_Count() > 0)
			{
				ForceInfo forceInfo = QueuedForces.Dequeue();
				forceInfo.Body.AddForce(forceInfo.Type, forceInfo.Force, forceInfo.Position, forceInfo.Torque, forceInfo.MaxSpeed, applyImmediately: true, forceInfo.ActiveOnly);
			}
			ProcessCollisionFilterRefreshes();
			using (m_raycastLock.Acquire())
			{
				SimulateInternal();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_iterationBodies.Clear();
			m_characterIterationBodies.Clear();
			if (Sync.IsServer && MyFakes.MP_SYNC_CLUSTERTREE && ClustersNeedSync)
			{
				List<BoundingBoxD> list = new List<BoundingBoxD>();
				SerializeClusters(list);
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => OnClustersReordered, list);
				ClustersNeedSync = false;
			}
			MySimpleProfiler.End("Simulate");
		}

		private static void StepVDB()
		{
			if (!ClientCameraWM.HasValue && !SyncVDBCamera)
			{
				return;
			}
			MatrixD arg = MatrixD.Identity;
			if (MySector.MainCamera != null)
			{
				arg = MySector.MainCamera.WorldMatrix;
			}
			HkWorld hkWorld = null;
			Vector3D vector3D = Vector3D.Zero;
			if (Sync.IsDedicated && ClientCameraWM.HasValue)
			{
				MyClusterTree.MyCluster clusterForPosition = Clusters.GetClusterForPosition(ClientCameraWM.Value.Translation);
				if (clusterForPosition != null)
				{
					vector3D = -clusterForPosition.AABB.Center;
					hkWorld = (HkWorld)clusterForPosition.UserData;
				}
			}
			if (hkWorld == null)
			{
				if (MyFakes.VDB_ENTITY != null && MyFakes.VDB_ENTITY.GetTopMostParent().GetPhysicsBody() != null)
				{
					MyPhysicsBody physicsBody = MyFakes.VDB_ENTITY.GetTopMostParent().GetPhysicsBody();
					vector3D = physicsBody.WorldToCluster(Vector3D.Zero);
					hkWorld = physicsBody.HavokWorld;
				}
				else if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity.GetTopMostParent().GetPhysicsBody() != null)
				{
					MyPhysicsBody physicsBody2 = MySession.Static.ControlledEntity.Entity.GetTopMostParent().GetPhysicsBody();
					vector3D = physicsBody2.WorldToCluster(Vector3D.Zero);
					hkWorld = physicsBody2.HavokWorld;
				}
				else if (Clusters.GetList().Count > 0)
				{
					MyClusterTree.MyCluster myCluster = Clusters.GetClusters()[0];
					vector3D = -myCluster.AABB.Center;
					hkWorld = (HkWorld)myCluster.UserData;
				}
			}
			if (hkWorld == null)
			{
				return;
			}
			HkVDB.SyncTimers(m_threadPool);
			HkVDB.StepVDB(hkWorld, 0.0166666675f);
			if (Sync.IsDedicated)
			{
				if (ClientCameraWM.HasValue)
				{
					arg = ClientCameraWM.Value;
				}
			}
			else if (!Sync.IsServer && SyncVDBCamera)
			{
				MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateServerDebugCamera, arg);
			}
			Vector3 up = arg.Up;
			Vector3 from = arg.Translation + vector3D;
			Vector3 to = from + arg.Forward;
			HkVDB.UpdateCamera(ref from, ref to, ref up);
			bool flag = VDBRecordFile != null;
			if (IsVDBRecording != flag)
			{
				IsVDBRecording = flag;
				if (flag)
				{
					HkVDB.Capture(VDBRecordFile);
				}
				else
				{
					HkVDB.EndCapture();
				}
			}
		}

		private static void ProcessDestructions()
		{
			int num = 0;
			while (m_destructionQueue.get_Count() > 0)
			{
				num++;
				FractureImpactDetails fractureImpactDetails = m_destructionQueue.Dequeue();
				HkdFractureImpactDetails details = fractureImpactDetails.Details;
				if (details.IsValid())
				{
					details.Flag |= HkdFractureImpactDetails.Flags.FLAG_DONT_DELAY_OPERATION;
					for (int i = 0; i < details.GetBreakingBody().BreakableBody.BreakableShape.GetChildrenCount(); i++)
					{
						HkdShapeInstanceInfo child = details.GetBreakingBody().BreakableBody.BreakableShape.GetChild(i);
						child.Shape.GetStrenght();
						for (int j = 0; j < child.Shape.GetChildrenCount(); j++)
						{
							child.Shape.GetChild(j).Shape.GetStrenght();
						}
					}
					fractureImpactDetails.World.DestructionWorld.TriggerDestruction(ref details);
					MySyncDestructions.AddDestructionEffect(MyPerGameSettings.CollisionParticle.LargeGridClose, fractureImpactDetails.ContactInWorld, Vector3.Forward, 0.2f);
					MySyncDestructions.AddDestructionEffect(MyPerGameSettings.DestructionParticle.DestructionHit, fractureImpactDetails.ContactInWorld, Vector3.Forward, 0.1f);
				}
				fractureImpactDetails.Details.RemoveReference();
			}
		}

		private void IterateBodies(HkWorld world)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			int worldVersion;
			bool cacheValid;
			List<HkRigidBody> activeRigidBodiesCache = world.GetActiveRigidBodiesCache(out worldVersion, out cacheValid);
			if (!cacheValid)
			{
				activeRigidBodiesCache.Clear();
				Enumerator<HkRigidBody> enumerator = world.ActiveRigidBodies.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						HkRigidBody current = enumerator.get_Current();
						MyPhysicsBody myPhysicsBody = (MyPhysicsBody)current.UserObject;
						if (myPhysicsBody != null && (!myPhysicsBody.IsKinematic || m_updateKinematicBodies) && myPhysicsBody.Entity.Parent == null && current.Layer != 17)
						{
							activeRigidBodiesCache.Add(current);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				world.UpdateActiveRigidBodiesCache(activeRigidBodiesCache, worldVersion);
			}
			foreach (HkRigidBody item2 in activeRigidBodiesCache)
			{
				MyPhysicsBody item = (MyPhysicsBody)item2.UserObject;
				m_iterationBodies.Add(item);
			}
		}

		private void IterateCharacters(HkWorld world)
		{
			foreach (HkCharacterRigidBody characterRigidBody in world.CharacterRigidBodies)
			{
				m_characterIterationBodies.Add(characterRigidBody);
			}
		}

		public static void ActivateInBox(ref BoundingBoxD box)
		{
			using (MyUtils.ReuseCollection(ref m_tmpEntityResults))
			{
				MyGamePruningStructure.GetTopMostEntitiesInBox(ref box, m_tmpEntityResults, MyEntityQueryType.Dynamic);
				foreach (MyEntity tmpEntityResult in m_tmpEntityResults)
				{
					if (tmpEntityResult.Physics != null && tmpEntityResult.Physics.Enabled && tmpEntityResult.Physics.RigidBody != null)
					{
						tmpEntityResult.Physics.RigidBody.Activate();
					}
				}
			}
		}

		public static void EnqueueDestruction(FractureImpactDetails details)
		{
			m_destructionQueue.Enqueue(details);
		}

		public static void RemoveDestructions(MyEntity entity)
		{
			List<FractureImpactDetails> list = Enumerable.ToList<FractureImpactDetails>((IEnumerable<FractureImpactDetails>)m_destructionQueue);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Entity == entity)
				{
					list[i].Details.RemoveReference();
					list.RemoveAt(i);
					i--;
				}
			}
			m_destructionQueue.Clear();
			foreach (FractureImpactDetails item in list)
			{
				m_destructionQueue.Enqueue(item);
			}
		}

		public static void RemoveDestructions(HkRigidBody body)
		{
			List<FractureImpactDetails> list = Enumerable.ToList<FractureImpactDetails>((IEnumerable<FractureImpactDetails>)m_destructionQueue);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Details.IsValid() || list[i].Details.GetBreakingBody() == body)
				{
					list[i].Details.RemoveReference();
					list.RemoveAt(i);
					i--;
				}
			}
			m_destructionQueue.Clear();
			foreach (FractureImpactDetails item in list)
			{
				m_destructionQueue.Enqueue(item);
			}
		}

		public static void DebugDrawClusters()
		{
			if (Clusters == null)
			{
				return;
			}
			double num = 2000.0;
			MatrixD matrixD = MatrixD.CreateWorld(DebugDrawClustersMatrix.Translation + num * DebugDrawClustersMatrix.Forward, Vector3D.Forward, Vector3D.Up);
			using (MyUtils.ReuseCollection(ref m_resultWorlds))
			{
				Clusters.GetAll(m_resultWorlds);
				BoundingBoxD boundingBoxD = BoundingBoxD.CreateInvalid();
				foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
				{
					boundingBoxD = boundingBoxD.Include(resultWorld.AABB);
				}
				double num2 = boundingBoxD.Size.AbsMax();
				double num3 = num / num2;
				Vector3D center = boundingBoxD.Center;
				boundingBoxD.Min -= center;
				boundingBoxD.Max -= center;
				BoundingBoxD box = new BoundingBoxD(boundingBoxD.Min * num3 * 1.0199999809265137, boundingBoxD.Max * num3 * 1.0199999809265137);
				MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box, matrixD), Color.Green, 0.2f, depthRead: false, smooth: false);
				MyRenderProxy.DebugDrawAxis(matrixD, 50f, depthRead: false);
				if (MySession.Static != null)
				{
					foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
					{
						if (onlinePlayer.Character != null)
						{
							MyRenderProxy.DebugDrawSphere(Vector3D.Transform((onlinePlayer.Character.PositionComp.GetPosition() - center) * num3, matrixD), 1f, Vector3.One, 1f, depthRead: false);
						}
					}
				}
				Clusters.GetAllStaticObjects(m_clusterStaticObjects);
				foreach (BoundingBoxD clusterStaticObject in m_clusterStaticObjects)
				{
					BoundingBoxD box2 = new BoundingBoxD((clusterStaticObject.Min - center) * num3, (clusterStaticObject.Max - center) * num3);
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box2, matrixD), Color.Blue, 0.2f, depthRead: false, smooth: false);
				}
				foreach (MyClusterTree.MyClusterQueryResult resultWorld2 in m_resultWorlds)
				{
					BoundingBoxD box3 = new BoundingBoxD((resultWorld2.AABB.Min - center) * num3, (resultWorld2.AABB.Max - center) * num3);
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(box3, matrixD), Color.White, 0.2f, depthRead: false, smooth: false);
					foreach (HkCharacterRigidBody characterRigidBody in ((HkWorld)resultWorld2.UserData).CharacterRigidBodies)
					{
						BoundingBoxD aABB = resultWorld2.AABB;
						Vector3D vector3D = aABB.Center + characterRigidBody.Position;
						vector3D = (vector3D - center) * num3;
						vector3D = Vector3D.Transform(vector3D, matrixD);
						Vector3D normal = characterRigidBody.LinearVelocity;
						normal = Vector3D.TransformNormal(normal, matrixD) * 10.0;
						if (normal.Length() > 0.0099999997764825821)
						{
							MyRenderProxy.DebugDrawLine3D(vector3D, vector3D + normal, Color.Blue, Color.White, depthRead: false);
						}
					}
					foreach (HkRigidBody rigidBody in ((HkWorld)resultWorld2.UserData).RigidBodies)
					{
						if (rigidBody.GetEntity(0u) != null)
						{
							MyOrientedBoundingBoxD obb = new MyOrientedBoundingBoxD(rigidBody.GetEntity(0u).LocalAABB, rigidBody.GetEntity(0u).WorldMatrix);
							obb.Center = (obb.Center - center) * num3;
							obb.HalfExtent *= num3;
							obb.Transform(matrixD);
							Color color = Color.Yellow;
							if (rigidBody.GetEntity(0u).LocalAABB.Size.Max() > 1000f)
							{
								color = Color.Red;
							}
							MyRenderProxy.DebugDrawOBB(obb, color, 1f, depthRead: false, smooth: false);
							Vector3D normal2 = rigidBody.LinearVelocity;
							normal2 = Vector3D.TransformNormal(normal2, matrixD) * 10.0;
							if (normal2.Length() > 0.0099999997764825821)
<<<<<<< HEAD
							{
								MyRenderProxy.DebugDrawLine3D(obb.Center, obb.Center + normal2, Color.Red, Color.White, depthRead: false);
							}
							if (Vector3D.Distance(obb.Center, MySector.MainCamera.Position) < 10.0)
							{
=======
							{
								MyRenderProxy.DebugDrawLine3D(obb.Center, obb.Center + normal2, Color.Red, Color.White, depthRead: false);
							}
							if (Vector3D.Distance(obb.Center, MySector.MainCamera.Position) < 10.0)
							{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								MyRenderProxy.DebugDrawText3D(obb.Center, rigidBody.GetEntity(0u).ToString(), Color.White, 0.5f, depthRead: false);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Finds closest or any object on the path of the ray from-&gt;to. Uses Storage for voxels for faster 
		/// search but only good for long rays (more or less more than 50m). Use it only in such cases.
		/// </summary>
		/// <param name="from">Start of the ray.</param>
		/// <param name="to">End of the ray.</param>
		/// <param name="any">Indicates if method should return any object found (May not be closest)</param>
		/// <returns>Hit info.</returns>
		public static HitInfo? CastLongRay(Vector3D from, Vector3D to, bool any = false)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.CastRay(from, to, m_resultWorlds);
					HitInfo? hitInfo = null;
					hitInfo = CastRayInternal(from, to, m_resultWorlds, 9);
					if (hitInfo.HasValue)
					{
						if (any)
						{
							return hitInfo;
						}
						to = hitInfo.Value.Position + hitInfo.Value.Position;
					}
					LineD ray = new LineD(from, to);
					MyGamePruningStructure.GetVoxelMapsOverlappingRay(ref ray, m_foundEntities);
					double num = 1.0;
					double num2 = 0.0;
					bool flag = false;
					foreach (MyLineSegmentOverlapResult<MyVoxelBase> foundEntity in m_foundEntities)
					{
						if (foundEntity.Element.GetOrePriority() != -1)
<<<<<<< HEAD
						{
							continue;
						}
						MyVoxelBase rootVoxel = foundEntity.Element.RootVoxel;
						if (rootVoxel.Storage.DataProvider == null)
						{
							continue;
						}
=======
						{
							continue;
						}
						MyVoxelBase rootVoxel = foundEntity.Element.RootVoxel;
						if (rootVoxel.Storage.DataProvider == null)
						{
							continue;
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						Vector3D from2 = Vector3D.Transform(ray.From, rootVoxel.PositionComp.WorldMatrixInvScaled);
						from2 += rootVoxel.SizeInMetresHalf;
						Vector3D to2 = Vector3D.Transform(ray.To, rootVoxel.PositionComp.WorldMatrixInvScaled);
						to2 += rootVoxel.SizeInMetresHalf;
						LineD line = new LineD(from2, to2);
						flag = rootVoxel.Storage.DataProvider.Intersect(ref line, out var startOffset, out var endOffset);
						if (flag)
						{
							if (startOffset < num)
							{
								num = startOffset;
							}
							if (endOffset > num2)
							{
								num2 = endOffset;
							}
						}
					}
					if (!flag)
					{
						return hitInfo;
					}
					to = from + ray.Direction * ray.Length * num2;
					from += ray.Direction * ray.Length * num;
					m_foundEntities.Clear();
					HitInfo? result = CastRayInternal(from, to, m_resultWorlds, 28);
					if (!hitInfo.HasValue)
					{
						return result;
					}
					if (result.HasValue && hitInfo.HasValue && result.Value.HkHitInfo.HitFraction < hitInfo.Value.HkHitInfo.HitFraction)
					{
						return result;
					}
					return hitInfo;
				}
			}
		}

		public static void GetPenetrationsShape(HkShape shape, ref Vector3D translation, ref Quaternion rotation, List<HkBodyCollision> results, int filter)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(translation, m_resultWorlds);
					foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
					{
						Vector3D vector3D = translation;
						BoundingBoxD aABB = resultWorld.AABB;
						Vector3 translation2 = vector3D - aABB.Center;
						((HkWorld)resultWorld.UserData).GetPenetrationsShape(shape, ref translation2, ref rotation, results, filter);
					}
				}
			}
		}

		public static float? CastShape(Vector3D to, HkShape shape, ref MatrixD transform, int filterLayer, float extraPenetration = 0f)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(to, m_resultWorlds);
					if (m_resultWorlds.Count == 0)
					{
						return null;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					return ((HkWorld)myClusterQueryResult.UserData).CastShape(to2, shape, ref transform2, filterLayer, extraPenetration);
				}
			}
		}

		public static float? CastShapeInAllWorlds(Vector3D to, HkShape shape, ref MatrixD transform, int filterLayer, float extraPenetration = 0f)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.CastRay(transform.Translation, to, m_resultWorlds);
					foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
					{
						Matrix transform2 = transform;
						Vector3D translation = transform.Translation;
						BoundingBoxD aABB = resultWorld.AABB;
						transform2.Translation = translation - aABB.Center;
						aABB = resultWorld.AABB;
						Vector3 to2 = to - aABB.Center;
						float? result = ((HkWorld)resultWorld.UserData).CastShape(to2, shape, ref transform2, filterLayer, extraPenetration);
						if (result.HasValue)
						{
							return result;
						}
					}
				}
			}
			return null;
		}

		public static Vector3D? CastShapeReturnPoint(Vector3D to, HkShape shape, ref MatrixD transform, int filterLayer, float extraPenetration)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					m_resultWorlds.Clear();
					Clusters.Intersects(to, m_resultWorlds);
					if (m_resultWorlds.Count == 0)
					{
						return null;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					Vector3? vector = ((HkWorld)myClusterQueryResult.UserData).CastShapeReturnPoint(to2, shape, ref transform2, filterLayer, extraPenetration);
					if (!vector.HasValue)
					{
						return null;
					}
					return (Vector3D)vector.Value + myClusterQueryResult.AABB.Center;
				}
			}
		}

		public static HkContactPoint? CastShapeReturnContact(Vector3D to, HkShape shape, ref MatrixD transform, int filterLayer, float extraPenetration, out Vector3D worldTranslation)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(to, m_resultWorlds);
					worldTranslation = Vector3D.Zero;
					if (m_resultWorlds.Count == 0)
					{
						return null;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					worldTranslation = myClusterQueryResult.AABB.Center;
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					return ((HkWorld)myClusterQueryResult.UserData).CastShapeReturnContact(to2, shape, ref transform2, filterLayer, extraPenetration);
				}
			}
		}

		public static HkContactPointData? CastShapeReturnContactData(Vector3D to, HkShape shape, ref MatrixD transform, uint collisionFilter, float extraPenetration, bool ignoreConvexShape = true)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(to, m_resultWorlds);
					if (m_resultWorlds.Count == 0)
					{
						return null;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					HkContactPointData? hkContactPointData = ((HkWorld)myClusterQueryResult.UserData).CastShapeReturnContactData(to2, shape, ref transform2, collisionFilter, extraPenetration);
					if (!hkContactPointData.HasValue)
					{
						return null;
					}
					HkContactPointData value = hkContactPointData.Value;
					value.HitPosition += myClusterQueryResult.AABB.Center;
					return value;
				}
			}
		}

		public static HitInfo? CastShapeReturnContactBodyData(Vector3D to, HkShape shape, ref MatrixD transform, uint collisionFilter, float extraPenetration, bool ignoreConvexShape = true)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(to, m_resultWorlds);
					if (m_resultWorlds.Count == 0)
					{
						return null;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					HkHitInfo? hkHitInfo = ((HkWorld)myClusterQueryResult.UserData).CastShapeReturnContactBodyData(to2, shape, ref transform2, collisionFilter, extraPenetration);
					if (!hkHitInfo.HasValue)
					{
						return null;
					}
					HkHitInfo value = hkHitInfo.Value;
					return new HitInfo(value, value.Position + myClusterQueryResult.AABB.Center);
				}
			}
		}

		public static bool CastShapeReturnContactBodyDatas(Vector3D to, HkShape shape, ref MatrixD transform, uint collisionFilter, float extraPenetration, List<HitInfo> result, bool ignoreConvexShape = true)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.Intersects(to, m_resultWorlds);
					if (m_resultWorlds.Count == 0)
					{
						return false;
					}
					MyClusterTree.MyClusterQueryResult myClusterQueryResult = m_resultWorlds[0];
					Matrix transform2 = transform;
					transform2.Translation = transform.Translation - myClusterQueryResult.AABB.Center;
					Vector3 to2 = to - myClusterQueryResult.AABB.Center;
					using (MyUtils.ReuseCollection(ref m_resultShapeCasts))
					{
						if (((HkWorld)myClusterQueryResult.UserData).CastShapeReturnContactBodyDatas(to2, shape, ref transform2, collisionFilter, extraPenetration, m_resultShapeCasts))
						{
							foreach (HkHitInfo? resultShapeCast in m_resultShapeCasts)
							{
								HkHitInfo value = resultShapeCast.Value;
								HitInfo hitInfo = default(HitInfo);
								hitInfo.HkHitInfo = value;
								hitInfo.Position = value.Position + myClusterQueryResult.AABB.Center;
								HitInfo item = hitInfo;
								result.Add(item);
							}
							return true;
						}
					}
					return false;
				}
			}
		}

		public static bool IsPenetratingShapeShape(HkShape shape1, ref Vector3D translation1, ref Quaternion rotation1, HkShape shape2, ref Vector3D translation2, ref Quaternion rotation2)
		{
			using (m_raycastLock.Acquire())
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					rotation1.Normalize();
					rotation2.Normalize();
					Clusters.Intersects(translation1, m_resultWorlds);
					foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
					{
						BoundingBoxD aABB = resultWorld.AABB;
						if (aABB.Contains(translation2) != ContainmentType.Contains)
						{
							return false;
						}
						Vector3D vector3D = translation1;
						aABB = resultWorld.AABB;
						Vector3 translation3 = vector3D - aABB.Center;
						Vector3D vector3D2 = translation2;
						aABB = resultWorld.AABB;
						Vector3 translation4 = vector3D2 - aABB.Center;
						if (((HkWorld)resultWorld.UserData).IsPenetratingShapeShape(shape1, ref translation3, ref rotation1, shape2, ref translation4, ref rotation2))
						{
							return true;
						}
					}
					return false;
				}
			}
		}

		public static bool IsPenetratingShapeShape(HkShape shape1, ref Matrix transform1, HkShape shape2, ref Matrix transform2)
		{
			using (m_raycastLock.Acquire())
			{
				return (Clusters.GetList()[0] as HkWorld).IsPenetratingShapeShape(shape1, ref transform1, shape2, ref transform2);
			}
		}

		public static int GetCollisionLayer(string strLayer)
		{
<<<<<<< HEAD
			switch (strLayer)
			{
			case "LightFloatingObjectCollisionLayer":
				return 10;
			case "VoxelLod1CollisionLayer":
				return 11;
			case "NotCollideWithStaticLayer":
				return 12;
			case "StaticCollisionLayer":
				return 13;
			case "CollideWithStaticLayer":
				return 14;
			case "DefaultCollisionLayer":
				return 15;
			case "DynamicDoubledCollisionLayer":
				return 16;
			case "KinematicDoubledCollisionLayer":
				return 17;
			case "CharacterCollisionLayer":
				return 18;
			case "NoCollisionLayer":
				return 19;
			case "TargetDummyLayer":
				return 6;
			case "DebrisCollisionLayer":
				return 20;
			case "GravityPhantomLayer":
				return 21;
			case "CharacterNetworkCollisionLayer":
				return 22;
			case "FloatingObjectCollisionLayer":
				return 23;
			case "ObjectDetectionCollisionLayer":
				return 24;
			case "VirtualMassLayer":
				return 25;
			case "CollectorCollisionLayer":
				return 26;
			case "AmmoLayer":
				return 27;
			case "VoxelCollisionLayer":
				return 28;
			case "ExplosionRaycastLayer":
				return 29;
			case "CollisionLayerWithoutCharacter":
				return 30;
			case "RagdollCollisionLayer":
				return 31;
			case "NoVoxelCollisionLayer":
				return 9;
			case "MissileLayer":
				return 8;
			default:
				return 15;
			}
=======
			return strLayer switch
			{
				"LightFloatingObjectCollisionLayer" => 10, 
				"VoxelLod1CollisionLayer" => 11, 
				"NotCollideWithStaticLayer" => 12, 
				"StaticCollisionLayer" => 13, 
				"CollideWithStaticLayer" => 14, 
				"DefaultCollisionLayer" => 15, 
				"DynamicDoubledCollisionLayer" => 16, 
				"KinematicDoubledCollisionLayer" => 17, 
				"CharacterCollisionLayer" => 18, 
				"NoCollisionLayer" => 19, 
				"TargetDummyLayer" => 6, 
				"DebrisCollisionLayer" => 20, 
				"GravityPhantomLayer" => 21, 
				"CharacterNetworkCollisionLayer" => 22, 
				"FloatingObjectCollisionLayer" => 23, 
				"ObjectDetectionCollisionLayer" => 24, 
				"VirtualMassLayer" => 25, 
				"CollectorCollisionLayer" => 26, 
				"AmmoLayer" => 27, 
				"VoxelCollisionLayer" => 28, 
				"ExplosionRaycastLayer" => 29, 
				"CollisionLayerWithoutCharacter" => 30, 
				"RagdollCollisionLayer" => 31, 
				"NoVoxelCollisionLayer" => 9, 
				"MissileLayer" => 8, 
				_ => 15, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Ensure aabb is inside only one subspace. If no, reorder.
		/// </summary>
		/// <param name="aabb"></param>
		public static void EnsurePhysicsSpace(BoundingBoxD aabb)
		{
			using (m_raycastLock.Acquire())
			{
				Clusters.EnsureClusterSpace(aabb);
			}
		}

		/// <summary>
		/// Change position of object in world. Move object between subspaces if necessary.
		/// </summary>
		/// <param name="id"></param>        
		/// <param name="aabb"></param>
		/// <param name="velocity"></param>
		public static void MoveObject(ulong id, BoundingBoxD aabb, Vector3 velocity)
		{
			Clusters.MoveObject(id, aabb, velocity);
		}

		/// <summary>
		/// Remove object from world, remove also subspace if empty.
		/// </summary>
		/// <param name="id"></param>
		public static void RemoveObject(ulong id)
		{
			Clusters.RemoveObject(id);
		}

		/// <summary>
		/// Return offset of objects subspace center.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Vector3D GetObjectOffset(ulong id)
		{
			return Clusters.GetObjectOffset(id);
		}

<<<<<<< HEAD
		/// <summary>
		/// Attempts to add the entity to the cluster
		/// Creates new cluster if allowed (!SingleCluster.HasValue) and needed (entity is outside of existing clusters).
		/// If not allowed, marks entity as left the world.
		/// </summary>
		/// <param name="entity">entity</param>
		/// <param name="activationHandler">activate handler</param>
		/// <param name="batchRequest">is batch request</param>
		/// <returns>id</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static ulong TryAddEntity(IMyEntity entity, MyPhysicsBody activationHandler, bool batchRequest)
		{
			ulong num;
			if (Clusters != null)
			{
				MyEntity myEntity;
				string tag = (((myEntity = entity as MyEntity) != null) ? myEntity.DebugName : string.Empty);
				BoundingBoxD worldAABB = entity.WorldAABB;
				long entityId = entity.EntityId;
				num = Clusters.AddObject(worldAABB, activationHandler, null, tag, entityId, batchRequest);
			}
			else
			{
				num = ulong.MaxValue;
			}
			if (num == ulong.MaxValue)
			{
				HavokWorld_EntityLeftWorld(activationHandler.RigidBody);
			}
			return num;
		}

		public static void RefreshCollisionFilter(MyPhysicsBody physicsBody)
		{
			m_pendingCollisionFilterRefreshes.Add(physicsBody);
		}

		private static void ProcessCollisionFilterRefreshes()
		{
<<<<<<< HEAD
			foreach (MyPhysicsBody pendingCollisionFilterRefresh in m_pendingCollisionFilterRefreshes)
			{
				if (pendingCollisionFilterRefresh.IsInWorld)
				{
					if (pendingCollisionFilterRefresh.RigidBody != null)
					{
						pendingCollisionFilterRefresh.HavokWorld.RefreshCollisionFilterOnEntity(pendingCollisionFilterRefresh.RigidBody);
					}
					if (pendingCollisionFilterRefresh.RigidBody2 != null)
					{
						pendingCollisionFilterRefresh.HavokWorld.RefreshCollisionFilterOnEntity(pendingCollisionFilterRefresh.RigidBody2);
					}
				}
			}
=======
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyPhysicsBody> enumerator = m_pendingCollisionFilterRefreshes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyPhysicsBody current = enumerator.get_Current();
					if (current.IsInWorld)
					{
						if (current.RigidBody != null)
						{
							current.HavokWorld.RefreshCollisionFilterOnEntity(current.RigidBody);
						}
						if (current.RigidBody2 != null)
						{
							current.HavokWorld.RefreshCollisionFilterOnEntity(current.RigidBody2);
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_pendingCollisionFilterRefreshes.Clear();
		}

		public static ListReader<object>? GetClusterList()
		{
			if (Clusters == null)
			{
				return null;
			}
			return Clusters.GetList();
		}

		public static void GetAll(List<MyClusterTree.MyClusterQueryResult> results)
		{
			Clusters.GetAll(results);
		}

		private void EnsureClusterSpace()
		{
			if (MyFakes.FORCE_CLUSTER_REORDER)
			{
				ForceClustersReorder();
				MyFakes.FORCE_CLUSTER_REORDER = false;
			}
			foreach (MyPhysicsBody iterationBody in m_iterationBodies)
			{
				Vector3 linearVelocity = iterationBody.LinearVelocity;
				if (linearVelocity.LengthSquared() > 0.1f)
				{
					BoundingBoxD aabb = MyClusterTree.AdjustAABBByVelocity(iterationBody.Entity.WorldAABB, linearVelocity);
					Clusters.EnsureClusterSpace(aabb);
				}
			}
			foreach (HkCharacterRigidBody characterIterationBody in m_characterIterationBodies)
			{
				if (characterIterationBody.LinearVelocity.LengthSquared() > 0.1f)
				{
					BoundingBoxD aabb2 = MyClusterTree.AdjustAABBByVelocity(((MyPhysicsBody)characterIterationBody.GetHitRigidBody().UserObject).Entity.PositionComp.WorldAABB, characterIterationBody.LinearVelocity);
					Clusters.EnsureClusterSpace(aabb2);
				}
			}
		}

		public static void SerializeClusters(List<BoundingBoxD> list)
		{
			Clusters.Serialize(list);
		}

		public static void DeserializeClusters(List<BoundingBoxD> list)
		{
			Clusters.Deserialize(list);
		}

		private void Tree_OnClustersReordered()
		{
			MySandboxGame.Log.WriteLine("Clusters reordered");
			ClustersNeedSync = true;
		}

<<<<<<< HEAD
		[Event(null, 1981)]
=======
		[Event(null, 1979)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private static void OnClustersReordered(List<BoundingBoxD> list)
		{
			DeserializeClusters(list);
		}

		internal static void ForceClustersReorder()
		{
			Clusters.ReorderClusters(BoundingBoxD.CreateInvalid(), ulong.MaxValue);
		}

		public bool GetEntityReplicableExistsById(long entityId)
		{
			MyEntity entityByIdOrDefault = MyEntities.GetEntityByIdOrDefault(entityId);
			if (entityByIdOrDefault != null)
			{
				return MyExternalReplicable.FindByObject(entityByIdOrDefault) != null;
			}
			return false;
		}

		public static void ProfileHkCall(Action action)
		{
			HkVDB.SyncTimers(m_threadPool);
			action();
			HkTaskProfiler.ReplayTimers(delegate
			{
			}, delegate
			{
			});
		}

<<<<<<< HEAD
		[Event(null, 2042)]
=======
		[Event(null, 2040)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void UpdateServerDebugCamera(MatrixD wm)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				ClientCameraWM = wm;
			}
		}

<<<<<<< HEAD
		[Event(null, 2054)]
=======
		[Event(null, 2052)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		public static void ControlVDBRecording(string fileName)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else
			{
				VDBRecordFile = ((fileName == null) ? null : Path.Combine(MyFileSystem.UserDataPath, fileName.Replace('\\', '_').Replace('/', '_').Replace(':', '_')));
			}
		}

		public void InformReplicationStarted(MyEntity entity)
		{
			if (m_worldObserver != null && entity != null)
			{
				m_worldObserver.AddReplicatedEntity(entity);
			}
		}

		public void InformReplicationEnded(MyEntity entity)
		{
			if (m_worldObserver != null && entity != null)
			{
				m_worldObserver.RemoveReplicatedEntity(entity);
			}
		}

		public static void CastRay(Vector3D from, Vector3D to, List<HitInfo> toList, int raycastFilterLayer = 0)
		{
			CastRayInternal(ref from, ref to, toList, raycastFilterLayer);
		}

		public static HitInfo? CastRay(Vector3D from, Vector3D to, int raycastFilterLayer = 0)
		{
			return CastRayInternal(ref from, ref to, raycastFilterLayer);
		}

		public static List<HitInfo> CastRay_AllHits(Vector3D from, Vector3D to)
		{
			List<HitInfo> list = new List<HitInfo>();
			CastRayInternal(ref from, ref to, list);
			return list;
		}

		public static void GetPenetrationsBox(ref Vector3 halfExtents, ref Vector3D translation, ref Quaternion rotation, List<HkBodyCollision> results, int filter)
		{
			GetPenetrationsBoxInternal(ref halfExtents, ref translation, ref rotation, results, filter);
		}

		public static MyClusterTree.MyClusterQueryResult? GetHkWorld(ref Vector3D pos)
		{
			using (MyUtils.ReuseCollection(ref m_resultWorlds))
			{
				Clusters.Intersects(pos, m_resultWorlds);
				if (m_resultWorlds.Count == 1)
				{
					return m_resultWorlds[0];
				}
			}
			return null;
		}

		private static void CastRayInternal(ref Vector3D from, ref Vector3D to, List<HitInfo> toList, int raycastFilterLayer = 0)
		{
			toList.Clear();
			using (MyUtils.ReuseCollection(ref m_resultHits))
			{
				using (MyUtils.ReuseCollection(ref m_resultWorlds))
				{
					Clusters.CastRay(from, to, m_resultWorlds);
					foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
					{
						Vector3D vector3D = from;
						BoundingBoxD aABB = resultWorld.AABB;
						Vector3D vector3D2 = vector3D - aABB.Center;
						Vector3D vector3D3 = to;
						aABB = resultWorld.AABB;
						Vector3D vector3D4 = vector3D3 - aABB.Center;
						((HkWorld)resultWorld.UserData)?.CastRay(vector3D2, vector3D4, m_resultHits, raycastFilterLayer);
						foreach (HkHitInfo resultHit in m_resultHits)
						{
							HitInfo item = new HitInfo
							{
								HkHitInfo = resultHit
							};
							Vector3 position = resultHit.Position;
							aABB = resultWorld.AABB;
							item.Position = position + aABB.Center;
							toList.Add(item);
						}
					}
				}
			}
		}

		private static HitInfo? CastRayInternal(ref Vector3D from, ref Vector3D to, int raycastFilterLayer)
		{
			using (MyUtils.ReuseCollection(ref m_resultWorlds))
			{
				Clusters.CastRay(from, to, m_resultWorlds);
				return CastRayInternal(from, to, m_resultWorlds, raycastFilterLayer);
			}
		}

		public static bool CastRay(Vector3D from, Vector3D to, out HitInfo hitInfo, uint raycastCollisionFilter, bool ignoreConvexShape)
		{
			using (MyUtils.ReuseCollection(ref m_resultWorlds))
			{
				Clusters.CastRay(from, to, m_resultWorlds);
				hitInfo = default(HitInfo);
				foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
				{
					BoundingBoxD aABB = resultWorld.AABB;
					Vector3 from2 = from - aABB.Center;
					aABB = resultWorld.AABB;
					Vector3 to2 = to - aABB.Center;
					HkHitInfo info = default(HkHitInfo);
					bool flag = ((HkWorld)resultWorld.UserData).CastRay(from2, to2, out info, raycastCollisionFilter, ignoreConvexShape);
					if (flag)
					{
						Vector3D vector3D = info.Position;
						aABB = resultWorld.AABB;
						hitInfo.Position = vector3D + aABB.Center;
						hitInfo.HkHitInfo = info;
						return flag;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Cast a ray on given worlds and returns closest one from all of the worlds. WARNING: It does not clear worlds list after.
		/// </summary>
		/// <param name="from">Start of ray.</param>
		/// <param name="to">End of ray.</param>
		/// <param name="worlds">Worlds to make test on.</param>
		/// <param name="raycastFilterLayer">Collision filter.</param>
		/// <returns>Hit info. Null if no hit registered.</returns>
		private static HitInfo? CastRayInternal(Vector3D from, Vector3D to, List<MyClusterTree.MyClusterQueryResult> worlds, int raycastFilterLayer = 0)
		{
			float num = float.MaxValue;
			foreach (MyClusterTree.MyClusterQueryResult world in worlds)
			{
				BoundingBoxD aABB = world.AABB;
				Vector3 from2 = from - aABB.Center;
				aABB = world.AABB;
				Vector3 to2 = to - aABB.Center;
				HkHitInfo? hkHitInfo = ((HkWorld)world.UserData).CastRay(from2, to2, raycastFilterLayer);
				if (hkHitInfo.HasValue && hkHitInfo.Value.HitFraction < num)
				{
					Vector3D vector3D = hkHitInfo.Value.Position;
					aABB = world.AABB;
					Vector3D worldPosition = vector3D + aABB.Center;
					HitInfo? result = new HitInfo(hkHitInfo.Value, worldPosition);
					num = hkHitInfo.Value.HitFraction;
					return result;
				}
			}
			return null;
		}

		private static void GetPenetrationsBoxInternal(ref Vector3 halfExtents, ref Vector3D translation, ref Quaternion rotation, List<HkBodyCollision> results, int filter)
		{
			using (MyUtils.ReuseCollection(ref m_resultWorlds))
			{
				Clusters.Intersects(translation, m_resultWorlds);
				foreach (MyClusterTree.MyClusterQueryResult resultWorld in m_resultWorlds)
				{
					Vector3D vector3D = translation;
					BoundingBoxD aABB = resultWorld.AABB;
					Vector3 translation2 = vector3D - aABB.Center;
					((HkWorld)resultWorld.UserData).GetPenetrationsBox(ref halfExtents, ref translation2, ref rotation, results, filter);
				}
			}
		}

		public static ClearToken<HkShapeCollision> GetPenetrationsShapeShape(HkShape shape1, ref Vector3 translation1, ref Quaternion rotation1, HkShape shape2, ref Vector3 translation2, ref Quaternion rotation2)
		{
			MyUtils.Init(ref m_shapeCollisionResultsCache).AssertEmpty();
			((HkWorld)Clusters.GetList()[0]).GetPenetrationsShapeShape(shape1, ref translation1, ref rotation1, shape2, ref translation2, ref rotation2, m_shapeCollisionResultsCache);
			return m_shapeCollisionResultsCache.GetClearToken();
		}

		private void ExecuteParallelRayCasts()
		{
			using (MyEntities.StartAsyncUpdateBlock())
			{
				using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
				{
					ParallelRayCastQuery parallelRayCastQuery = Interlocked.Exchange(ref m_pendingRayCasts, null);
					if (parallelRayCastQuery != null)
					{
						DeliverData deliverData = m_pendingRayCastsParallelPool.Get();
						while (parallelRayCastQuery != null)
						{
							deliverData.Jobs.Add(parallelRayCastQuery);
							parallelRayCastQuery = parallelRayCastQuery.Next;
						}
						Parallel.For(0, deliverData.Jobs.Count, deliverData.ExecuteJob, 1, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Physics, "Raycast"), blocking: true);
						Parallel.Start(deliverData);
					}
				}
			}
		}

		private void CancelParallelRayCasts()
		{
			ParallelRayCastQuery parallelRayCastQuery = Interlocked.Exchange(ref m_pendingRayCasts, null);
			while (parallelRayCastQuery != null)
			{
				ParallelRayCastQuery next = parallelRayCastQuery.Next;
				parallelRayCastQuery.Cancel();
				parallelRayCastQuery = next;
			}
		}

		public static void CastRayParallel(ref Vector3D from, ref Vector3D to, int raycastFilterLayer, Action<HitInfo?> callback)
		{
			ParallelRayCastQuery parallelRayCastQuery = ParallelRayCastQuery.Allocate();
			parallelRayCastQuery.Kind = ParallelRayCastQuery.TKind.RaycastSingle;
			parallelRayCastQuery.RayCastData = new RayCastData
			{
				To = to,
				From = from,
				Callback = callback,
				RayCastFilterLayer = raycastFilterLayer
			};
			EnqueueParallelQuery(parallelRayCastQuery);
		}

		public static void CastRayParallel(ref Vector3D from, ref Vector3D to, List<HitInfo> collector, int raycastFilterLayer, Action<List<HitInfo>> callback)
		{
			ParallelRayCastQuery parallelRayCastQuery = ParallelRayCastQuery.Allocate();
			parallelRayCastQuery.Kind = ParallelRayCastQuery.TKind.RaycastAll;
			parallelRayCastQuery.RayCastData = new RayCastData
			{
				To = to,
				From = from,
				Callback = callback,
				Collector = collector,
				RayCastFilterLayer = raycastFilterLayer
			};
			EnqueueParallelQuery(parallelRayCastQuery);
		}

		public static void GetPenetrationsBoxParallel(ref Vector3 halfExtents, ref Vector3D translation, ref Quaternion rotation, List<HkBodyCollision> results, int filter, Action<List<HkBodyCollision>> callback)
		{
			ParallelRayCastQuery parallelRayCastQuery = ParallelRayCastQuery.Allocate();
			parallelRayCastQuery.Kind = ParallelRayCastQuery.TKind.GetPenetrationsBox;
			parallelRayCastQuery.QueryBoxData = new QueryBoxData
			{
				Filter = filter,
				Results = results,
				Rotation = rotation,
				Callback = callback,
				Translation = translation,
				HalfExtents = halfExtents
			};
			EnqueueParallelQuery(parallelRayCastQuery);
		}

		private static void EnqueueParallelQuery(ParallelRayCastQuery query)
		{
			ParallelRayCastQuery parallelRayCastQuery = m_pendingRayCasts;
			while (true)
			{
				query.Next = parallelRayCastQuery;
				ParallelRayCastQuery parallelRayCastQuery2 = Interlocked.CompareExchange(ref m_pendingRayCasts, query, parallelRayCastQuery);
				if (parallelRayCastQuery2 != parallelRayCastQuery)
				{
					parallelRayCastQuery = parallelRayCastQuery2;
					continue;
				}
				break;
			}
		}

		public static void CastRayWorld(MyClusterTree.MyClusterQueryResult world, Vector3D from, Vector3D to, List<HitInfo> raycastResult, int collisionLayer)
		{
			using (MyUtils.ReuseCollection(ref m_resultHits))
			{
				Vector3 from2 = from - world.AABB.Center;
				Vector3 to2 = to - world.AABB.Center;
				((HkWorld)world.UserData).CastRay(from2, to2, m_resultHits, collisionLayer);
				foreach (HkHitInfo resultHit in m_resultHits)
				{
					raycastResult.Add(new HitInfo
					{
						HkHitInfo = resultHit,
						Position = resultHit.Position + world.AABB.Center
					});
				}
			}
		}

		private void StepWorlds()
		{
			if (!MyFakes.ENABLE_HAVOK_STEP_OPTIMIZERS || (Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static.Settings.EnableSelectivePhysicsUpdates))
			{
				StepWorldsInternal(null);
				return;
			}
			if (m_optimizeNextFrame)
			{
				m_optimizeNextFrame = false;
				StepWorldsInternal(m_timings);
				EnableOptimizations(m_timings);
				m_timings.Clear();
			}
			else
			{
				Stopwatch obj = Stopwatch.StartNew();
				StepWorldsInternal(null);
				double totalMilliseconds = obj.get_Elapsed().TotalMilliseconds;
				bool flag = (totalMilliseconds > (double)(m_averageFrameTime * 3f) && totalMilliseconds > 10.0) || totalMilliseconds >= 30.0;
				bool flag2 = totalMilliseconds < (double)(m_averageFrameTime * 1.5f) && !flag;
				int num = (flag ? 1000 : 100);
				m_averageFrameTime = (float)((double)(m_averageFrameTime * ((float)(num - 1) / (float)num)) + totalMilliseconds * (double)(1f / (float)num));
				_ = MyFakes.ENABLE_HAVOK_STEP_OPTIMIZERS_TIMINGS;
				if (m_optimizationsEnabled)
				{
					if (flag2)
					{
						m_fastFrames++;
						if (m_fastFrames > 10)
						{
							DisableOptimizations();
						}
					}
					else
					{
						m_fastFrames = 0;
					}
				}
				else if (flag)
				{
					m_slowFrames++;
					if (m_slowFrames > 10)
					{
						m_slowFrames = 0;
						m_optimizeNextFrame = true;
					}
				}
				else if (flag2 && m_slowFrames > 0)
				{
					m_slowFrames--;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_TOI_OPTIMIZED_GRIDS)
			{
				DisableGridTOIsOptimizer.Static.DebugDraw();
			}
		}

		private void EnableOptimizations(List<MyTuple<HkWorld, MyTimeSpan>> timings)
		{
			m_optimizationsEnabled = true;
			MyLog.Default.WriteLine("Optimizing physics step of " + timings.Count + " worlds");
			foreach (IPhysicsStepOptimizer optimizer in m_optimizers)
			{
				optimizer.EnableOptimizations(timings);
			}
		}

		private void DisableOptimizations()
		{
			m_optimizationsEnabled = false;
			foreach (IPhysicsStepOptimizer optimizer in m_optimizers)
			{
				optimizer.DisableOptimizations();
			}
		}

		private void InitStepOptimizer()
		{
			m_optimizeNextFrame = false;
			m_optimizationsEnabled = false;
			m_averageFrameTime = 16.6666679f;
			m_optimizers = new List<IPhysicsStepOptimizer>
			{
				new DisableGridTOIsOptimizer()
			};
		}

		private void UnloadStepOptimizer()
		{
			foreach (IPhysicsStepOptimizer optimizer in m_optimizers)
			{
				optimizer.Unload();
			}
			m_optimizers.Clear();
		}

		private void StepWorldsInternal(List<MyTuple<HkWorld, MyTimeSpan>> timings)
		{
			if (timings != null)
			{
				StepWorldsMeasured(timings);
			}
			else if (MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING)
			{
				StepWorldsParallel();
			}
			else
			{
				StepWorldsSequential();
			}
			if (HkBaseSystem.IsOutOfMemory)
			{
				throw new OutOfMemoryException("Havok run out of memory");
			}
		}

		private void StepWorldsSequential()
		{
			foreach (HkWorld item in Clusters.GetList())
			{
				StepSingleWorld(item);
			}
		}

		private void StepWorldsMeasured(List<MyTuple<HkWorld, MyTimeSpan>> timings)
		{
			foreach (HkWorld item in Clusters.GetList())
			{
				Stopwatch obj = Stopwatch.StartNew();
				StepSingleWorld(item);
				MyTimeSpan arg = MyTimeSpan.FromTicks(obj.get_ElapsedTicks());
				timings.Add(MyTuple.Create(item, arg));
			}
		}

		private void StepWorldsParallel()
		{
			ListReader<MyClusterTree.MyCluster> clusters = Clusters.GetClusters();
			using (MyEntities.StartAsyncUpdateBlock())
			{
				using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
				{
					m_jobQueue.WaitPolicy = HkJobQueue.WaitPolicyT.WAIT_INDEFINITELY;
					m_threadPool.ExecuteJobQueue(m_jobQueue);
					foreach (MyClusterTree.MyCluster item in clusters)
					{
						HkWorld hkWorld = item.UserData as HkWorld;
						if (hkWorld != null && IsClusterActive(item.ClusterId, hkWorld.CharacterRigidBodies.Count))
						{
							hkWorld.ExecutePendingCriticalOperations();
							hkWorld.UnmarkForWrite();
							hkWorld.InitMtStep(m_jobQueue, 0.0166666675f * MyFakes.SIMULATION_SPEED);
						}
					}
					m_jobQueue.WaitPolicy = HkJobQueue.WaitPolicyT.WAIT_UNTIL_ALL_WORK_COMPLETE;
					m_jobQueue.ProcessAllJobs();
					m_threadPool.WaitForCompletion();
					foreach (MyClusterTree.MyCluster item2 in clusters)
					{
						HkWorld hkWorld2 = item2.UserData as HkWorld;
						if (hkWorld2 != null && IsClusterActive(item2.ClusterId, hkWorld2.CharacterRigidBodies.Count))
						{
							hkWorld2.FinishMtStep(m_jobQueue, m_threadPool);
							hkWorld2.MarkForWrite();
						}
					}
				}
			}
		}

		private bool IsClusterActive(int clusterId, int rigidBodiesCount)
		{
			if (m_worldObserver != null && rigidBodiesCount == 0)
			{
				return m_worldObserver.IsClusterActive(clusterId);
			}
			return true;
		}

		private void StepSingleWorld(HkWorld world)
		{
			_ = MyFakes.DEFORMATION_LOGGING;
			world.ExecutePendingCriticalOperations();
			world.UnmarkForWrite();
			MyEntities.AsyncUpdateToken? asyncUpdateToken;
			HkAccessControl.AccessStateToken? accessStateToken;
			if (MyFakes.ENABLE_HAVOK_MULTITHREADING)
			{
				asyncUpdateToken = MyEntities.StartAsyncUpdateBlock();
				accessStateToken = HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead);
			}
			else
			{
				asyncUpdateToken = null;
				accessStateToken = null;
			}
			if (MyFakes.TWO_STEP_SIMULATIONS)
			{
				world.StepSimulation(0.008333334f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
				world.StepSimulation(0.008333334f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
			}
			else
			{
				world.StepSimulation(0.0166666675f * MyFakes.SIMULATION_SPEED, MyFakes.ENABLE_HAVOK_MULTITHREADING);
			}
			world.MarkForWrite();
			asyncUpdateToken?.Dispose();
			accessStateToken?.Dispose();
			_ = MyFakes.DEFORMATION_LOGGING;
		}

		public static void CommitSchedulingSettingToServer()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => SetScheduling, MyFakes.ENABLE_HAVOK_MULTITHREADING, MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING);
		}

		[Event(null, 207)]
		[Reliable]
		[Server]
		public static void SetScheduling(bool multithread, bool parallel)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserAdmin(MyEventContext.Current.Sender.Value))
			{
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				return;
			}
			MyFakes.ENABLE_HAVOK_MULTITHREADING = multithread;
			MyFakes.ENABLE_HAVOK_PARALLEL_SCHEDULING = parallel;
		}
	}
}
