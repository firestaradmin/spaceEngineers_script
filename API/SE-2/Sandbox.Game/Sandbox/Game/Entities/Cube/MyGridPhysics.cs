using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Havok;
using ParallelTasks;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Entities.EnvironmentItems;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Profiler;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyGridPhysics : MyPhysicsBody
	{
		public struct ExplosionInfo
		{
			public Vector3D Position;

			public MyExplosionTypeEnum ExplosionType;

			public float Radius;

			public bool ShowParticles;

			public bool GenerateDebris;
		}

		private enum PredictionDisqualificationReason : byte
		{
			None,
			NoEntity,
			EntityIsStatic,
			EntityIsNotMoving
		}

		/// <summary>
		/// Information about dirty (added, removed,...) blocks.
		/// </summary>
		private class MyDirtyBlocksInfo
		{
			public ConcurrentCachingList<BoundingBoxI> DirtyParts = new ConcurrentCachingList<BoundingBoxI>();

			public HashSet<Vector3I> DirtyBlocks = new HashSet<Vector3I>();

			public void Clear()
			{
				DirtyParts.ClearList();
				DirtyBlocks.Clear();
			}
		}

		private enum GridEffectType
		{
			Collision,
			Destruction,
			Dust
		}

		private struct GridCollisionHit
		{
			public Vector3D RelativePosition;

			public Vector3 Normal;

			public Vector3 RelativeVelocity;

			public float SeparatingSpeed;

			public float Impulse;
		}

		private struct GridEffect
		{
			public GridEffectType Type;

			public Vector3D Position;

			public Vector3 Normal;

			public float Scale;

			public float SeparatingSpeed;

			public float Impulse;
		}

		private class CollisionParticleEffect
		{
			public MyParticleEffect Effect;

			public Vector3D RelativePosition;

			public Vector3 Normal;

			public Vector3 SeparatingVelocity;

			public int RemainingTime;

			public float Impulse;
		}

		private class Sandbox_Game_Entities_Cube_MyGridPhysics_003C_003EActor
		{
		}

		private static readonly float LargeGridDeformationRatio = 1f;

		private static readonly float SmallGridDeformationRatio = 2.5f;

		private const int MAX_NUM_CONTACTS_PER_FRAME = 10;

		private const int MAX_NUM_CONTACTS_PER_FRAME_SIMPLE_GRID = 1;

		private static readonly int MaxEffectsPerFrame = 3;

		private static readonly int SparksEffectDelayPerContactMs = 1000;

		/// <summary>
		/// units m/s
		/// </summary>
		private const float SPEED_OF_LIGHT_IN_VACUUM = 299792448f;

		private const int TOIs_THRESHOLD = 1;

		private const int FRAMES_TO_REMEMBER_TOI_IMPACT = 10;

		private const float BREAK_OFFSET_MULTIPLIER = 0.7f;

		private readonly float MINIMUM_GRID_INTEGRITY_MASS = 10f;

		public static float PREDICTION_IMPULSE_SCALE = 0.005f;

		public const int COLLISION_SPARK_LIMIT_COUNT = 3;

		public const int COLLISION_SPARK_LIMIT_TIME = 20;

		public static readonly float LargeShipMaxAngularVelocityLimit = MathHelper.ToRadians(18000f);

		public static readonly float SmallShipMaxAngularVelocityLimit = MathHelper.ToRadians(36000f);

		public const float MAX_SHIP_SPEED = 149896224f;

		public const float MAX = 100f;

		public const int PLANET_CRASH_MIN_BLOCK_COUNT = 5;

		public const int ACCUMULATION_TIME = 30;

		public const int CRASH_LIMIT = 90;

		private static readonly List<MyEntity> m_predictedContactEntitiesToRemove = new List<MyEntity>();

		private static readonly MyConcurrentPool<HashSet<Vector3I>> m_dirtyCubesPool = new MyConcurrentPool<HashSet<Vector3I>>(10, delegate(HashSet<Vector3I> x)
		{
			x.Clear();
		});

		private static readonly MyConcurrentPool<Dictionary<MySlimBlock, float>> m_damagedCubesPool = new MyConcurrentPool<Dictionary<MySlimBlock, float>>(10, delegate(Dictionary<MySlimBlock, float> x)
		{
			x.Clear();
		});

		[ThreadStatic]
		private static List<Vector3I> m_tmpCubeList;

		private bool m_appliedSlowdownThisFrame;

		private int m_removeBlocksCallbackScheduled;

<<<<<<< HEAD
		private ulong m_frameCollided;
=======
		public bool LowSimulationQuality;

		public float DeformationRatio;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private ulong m_frameFirstImpact;

		private float m_impactDot;

		private readonly List<Vector3D> m_contactPosCache = new List<Vector3D>();

		private readonly ConcurrentDictionary<MySlimBlock, byte> m_removedCubes = new ConcurrentDictionary<MySlimBlock, byte>();

		private readonly Dictionary<MyEntity, bool> m_predictedContactEntities = new Dictionary<MyEntity, bool>();

		private List<ushort> m_tmpContactId = new List<ushort>();

		private MyConcurrentDictionary<ushort, int> m_lastContacts = new MyConcurrentDictionary<ushort, int>();

		private MyCubeGrid m_grid;

		private MyGridShape m_shape;

		private List<ExplosionInfo> m_explosions = new List<ExplosionInfo>();

		private readonly MyDirtyBlocksInfo m_dirtyCubesInfo = new MyDirtyBlocksInfo();

		private bool m_isClientPredicted;

		private ulong m_isClientPredictedLastFrameCheck;

		private bool m_isServer;

		private Vector3 m_cachedGravity;

		private MyParticleEffect m_planetCrash_Effect;

		private Vector3D? m_planetCrash_CenterPoint;

		private Vector3? m_planetCrash_Normal;

		private bool m_planetCrash_IsStarted;

		private int m_planetCrash_TimeBetweenPoints;

<<<<<<< HEAD
		private int m_planetCrash_CrashAccumulation;
=======
		private MyConcurrentQueue<GridCollisionHit> m_gridCollisionEffects = new MyConcurrentQueue<GridCollisionHit>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private float m_planetCrash_ScaleCurrent = 1f;

<<<<<<< HEAD
		private float m_planetCrash_ScaleGoal = 1f;

		private float m_planetCrash_generationMultiplier;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_debrisPerFrame;

		private const int MaxDebrisPerFrame = 3;

<<<<<<< HEAD
=======
		private static float IMP = 1E-07f;

		private static float ACCEL = 0.1f;

		private static float RATIO = 0.2f;

		public static float PREDICTION_IMPULSE_SCALE = 0.005f;

		private const int TOIs_THRESHOLD = 1;

		private const int FRAMES_TO_REMEMBER_TOI_IMPACT = 10;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_toiCountThisFrame;

		private int m_lastTOIFrame;

		private HkCollidableQualityType m_savedQuality = HkCollidableQualityType.Invalid;

		private MyConcurrentQueue<GridEffect> m_gridEffects = new MyConcurrentQueue<GridEffect>();

		private MyConcurrentQueue<GridCollisionHit> m_gridCollisionEffects = new MyConcurrentQueue<GridCollisionHit>();

		private List<CollisionParticleEffect> m_collisionParticles = new List<CollisionParticleEffect>();

		private HashSet<MySlimBlock> m_blocksInContact = new HashSet<MySlimBlock>();

		private List<MyPhysics.HitInfo> m_hitList = new List<MyPhysics.HitInfo>();

		private List<HkdBreakableBodyInfo> m_newBodies = new List<HkdBreakableBodyInfo>();

		private List<HkdShapeInstanceInfo> m_children = new List<HkdShapeInstanceInfo>();

		private static List<HkdShapeInstanceInfo> m_tmpChildren_RemoveShapes = new List<HkdShapeInstanceInfo>();

		private static List<HkdShapeInstanceInfo> m_tmpChildren_CompoundIds = new List<HkdShapeInstanceInfo>();

		private static List<string> m_tmpShapeNames = new List<string>();

		private static HashSet<MySlimBlock> m_tmpBlocksToDelete = new HashSet<MySlimBlock>();

		private static HashSet<MySlimBlock> m_tmpBlocksUpdateDamage = new HashSet<MySlimBlock>();

		private static HashSet<ushort> m_tmpCompoundIds = new HashSet<ushort>();

		private static List<MyDefinitionId> m_tmpDefinitions = new List<MyDefinitionId>();

		private bool m_recreateBody;

		private Vector3 m_oldLinVel;

		private Vector3 m_oldAngVel;

		private List<HkdBreakableBody> m_newBreakableBodies = new List<HkdBreakableBody>();

		private List<MyFracturedBlock.Info> m_fractureBlocksCache = new List<MyFracturedBlock.Info>();

		private Dictionary<Vector3I, List<HkdShapeInstanceInfo>> m_fracturedBlocksShapes = new Dictionary<Vector3I, List<HkdShapeInstanceInfo>>();

		private List<MyFractureComponentBase.Info> m_fractureBlockComponentsCache = new List<MyFractureComponentBase.Info>();

		private Dictionary<MySlimBlock, List<HkdShapeInstanceInfo>> m_fracturedSlimBlocksShapes = new Dictionary<MySlimBlock, List<HkdShapeInstanceInfo>>();

		private List<HkdShapeInstanceInfo> m_childList = new List<HkdShapeInstanceInfo>();

<<<<<<< HEAD
		public int DisableGravity { get; set; }

		public float DeformationRatio { get; set; }
=======
		public MyTimeSpan PredictedContactLastTime { get; private set; }

		public int PredictedContactsCounter { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyGridShape Shape => m_shape;

		public bool LowSimulationQuality { get; set; }

		public MyTimeSpan PredictedContactLastTime { get; private set; }

		public int PredictedContactsCounter { get; set; }

		public bool PredictCollisions
		{
			get
			{
				ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
				if (Volatile.Read(ref m_isClientPredictedLastFrameCheck) != simulationFrameCounter)
				{
					bool flag = m_grid.ClosestParentId != 0;
					bool value = !Sync.IsServer && m_grid.IsClientPredicted && !flag;
					Volatile.Write(ref m_isClientPredicted, value);
					m_isClientPredictedLastFrameCheck = simulationFrameCounter;
				}
				return m_isClientPredicted;
			}
		}

		public override float Mass
		{
			get
			{
				if (RigidBody != null)
				{
					return RigidBody.Mass;
				}
				return 0f;
			}
		}

		public override HkRigidBody RigidBody
		{
			get
			{
				return base.RigidBody;
			}
			protected set
			{
				base.RigidBody = value;
				UpdateContactCallbackLimit();
			}
		}

		public override int HavokCollisionSystemID
		{
			get
			{
				return base.HavokCollisionSystemID;
			}
			protected set
			{
				if (HavokCollisionSystemID != value)
				{
					base.HavokCollisionSystemID = value;
					m_grid.HavokSystemIDChanged(value);
				}
			}
		}

		public override Vector3 Gravity
		{
			get
			{
				return m_cachedGravity;
			}
			set
			{
				m_cachedGravity = value;
				HkRigidBody rigidBody = RigidBody;
				if (rigidBody != null)
				{
					rigidBody.Gravity = value;
				}
			}
		}

		private bool IsTOIOptimized => m_savedQuality != HkCollidableQualityType.Invalid;

		public static float ShipMaxLinearVelocity()
		{
			return Math.Max(LargeShipMaxLinearVelocity(), SmallShipMaxLinearVelocity());
		}

		public static float LargeShipMaxLinearVelocity()
		{
			return Math.Max(0f, Math.Min(149896224f, MySector.EnvironmentDefinition.LargeShipMaxSpeed));
		}

		public static float SmallShipMaxLinearVelocity()
		{
			return Math.Max(0f, Math.Min(149896224f, MySector.EnvironmentDefinition.SmallShipMaxSpeed));
		}

		public static float GetShipMaxLinearVelocity(MyCubeSize size)
		{
			if (size != 0)
			{
				return SmallShipMaxLinearVelocity();
			}
			return LargeShipMaxLinearVelocity();
		}

		public static float GetShipMaxAngularVelocity(MyCubeSize size)
		{
			if (size != 0)
			{
				return GetSmallShipMaxAngularVelocity();
			}
			return GetLargeShipMaxAngularVelocity();
		}

		public static float GetLargeShipMaxAngularVelocity()
		{
			return Math.Max(0f, Math.Min(LargeShipMaxAngularVelocityLimit, MySector.EnvironmentDefinition.LargeShipMaxAngularSpeedInRadians));
		}

		public static float GetSmallShipMaxAngularVelocity()
		{
			if (MyFakes.TESTING_VEHICLES)
			{
				return float.MaxValue;
			}
			return Math.Max(0f, Math.Min(SmallShipMaxAngularVelocityLimit, MySector.EnvironmentDefinition.SmallShipMaxAngularSpeedInRadians));
		}

		public MyGridPhysics(MyCubeGrid grid, MyGridShape shape = null, bool staticPhysics = false)
			: base(grid, GetFlags(grid))
		{
			m_grid = grid;
			m_shape = shape;
			DeformationRatio = ((m_grid.GridSizeEnum == MyCubeSize.Large) ? LargeGridDeformationRatio : SmallGridDeformationRatio);
			base.MaterialType = MyMaterialType.METAL;
			if (staticPhysics)
			{
				Flags = RigidBodyFlag.RBF_KINEMATIC;
			}
			CreateBody();
			if (MyFakes.ENABLE_PHYSICS_HIGH_FRICTION)
			{
				Friction = MyFakes.PHYSICS_HIGH_FRICTION;
			}
			m_isServer = Sync.IsServer;
		}

<<<<<<< HEAD
=======
		public override void Close()
		{
			base.Close();
			if (m_planetCrash_Effect != null)
			{
				m_planetCrash_Effect.Stop(instant: false);
			}
			foreach (CollisionParticleEffect collisionParticle in m_collisionParticles)
			{
				CollisionParticleEffect effect = collisionParticle;
				effect.RemainingTime = -1;
				FinalizeCollisionParticleEffect(ref effect);
			}
			if (m_shape != null)
			{
				m_shape.Dispose();
				m_shape = null;
			}
		}

		private static RigidBodyFlag GetFlags(MyCubeGrid grid)
		{
			if (!grid.IsStatic)
			{
				if (grid.GridSizeEnum != 0)
				{
					return RigidBodyFlag.RBF_DEFAULT;
				}
				return MyPerGameSettings.LargeGridRBFlag;
			}
			return RigidBodyFlag.RBF_STATIC;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void CreateBody()
		{
			if (m_shape == null)
			{
				m_shape = new MyGridShape(m_grid);
			}
			if (m_grid.GridSizeEnum == MyCubeSize.Large && !IsStatic)
			{
				InitialSolverDeactivation = HkSolverDeactivation.Off;
			}
			ContactPointDelay = 0;
			CreateFromCollisionObject(m_shape, Vector3.Zero, MatrixD.Identity, m_shape.MassProperties);
			RigidBody.ContactPointCallbackEnabled = true;
			RigidBody.ContactSoundCallbackEnabled = true;
			if (!MyPerGameSettings.Destruction)
			{
				RigidBody.ContactPointCallback += RigidBody_ContactPointCallback;
				if (!Sync.IsServer)
				{
					RigidBody.CollisionAddedCallback += RigidBody_CollisionAddedCallbackClient;
					RigidBody.CollisionRemovedCallback += RigidBody_CollisionRemovedCallbackClient;
				}
			}
			else
			{
				RigidBody.ContactPointCallback += RigidBody_ContactPointCallback_Destruction;
				BreakableBody.BeforeControllerOperation += BreakableBody_BeforeControllerOperation;
				BreakableBody.AfterControllerOperation += BreakableBody_AfterControllerOperation;
			}
			RigidBody.LinearDamping = MyPerGameSettings.DefaultLinearDamping;
			RigidBody.AngularDamping = MyPerGameSettings.DefaultAngularDamping;
			if (m_grid.BlocksDestructionEnabled)
			{
				RigidBody.BreakLogicHandler = BreakLogicHandler;
				RigidBody.BreakPartsHandler = BreakPartsHandler;
			}
			if (RigidBody2 != null)
			{
				RigidBody2.ContactPointCallbackEnabled = true;
				if (!MyPerGameSettings.Destruction)
				{
					RigidBody2.ContactPointCallback += RigidBody_ContactPointCallback;
				}
				if (m_grid.BlocksDestructionEnabled)
				{
					RigidBody2.BreakPartsHandler = BreakPartsHandler;
					RigidBody2.BreakLogicHandler = BreakLogicHandler;
				}
			}
			RigidBodyFlag flags = GetFlags(m_grid);
			SetDefaultRigidBodyMaxVelocities();
			if (IsStatic)
			{
				RigidBody.Layer = 13;
			}
			else if (m_grid.GridSizeEnum == MyCubeSize.Large)
			{
				RigidBody.Layer = ((flags == RigidBodyFlag.RBF_DOUBLED_KINEMATIC && MyFakes.ENABLE_DOUBLED_KINEMATIC) ? 16 : 15);
			}
			else if (m_grid.GridSizeEnum == MyCubeSize.Small)
			{
				RigidBody.Layer = 15;
			}
			if (RigidBody2 != null)
			{
				RigidBody2.Layer = 17;
			}
			if (MyPerGameSettings.BallFriendlyPhysics)
			{
				RigidBody.Restitution = 0f;
				if (RigidBody2 != null)
				{
					RigidBody2.Restitution = 0f;
				}
			}
			Enabled = true;
		}

		public float GetMaxLinearVelocity()
		{
			return GetShipMaxLinearVelocity(m_grid.GridSizeEnum);
		}

		public float GetMaxAngularVelocity()
		{
			return GetShipMaxAngularVelocity(m_grid.GridSizeEnum);
		}

		public float GetMaxRelaxedLinearVelocity()
		{
			return GetMaxLinearVelocity() * 10f;
		}

		public float GetMaxRelaxedAngularVelocity()
		{
			return GetMaxAngularVelocity() * 100f;
		}

		public override void Close()
		{
			base.Close();
			if (m_planetCrash_Effect != null)
			{
				m_planetCrash_Effect.Stop(instant: false);
			}
			foreach (CollisionParticleEffect collisionParticle in m_collisionParticles)
			{
				CollisionParticleEffect effect = collisionParticle;
				effect.RemainingTime = -1;
				FinalizeCollisionParticleEffect(ref effect);
			}
			if (m_shape != null)
			{
				m_shape.Dispose();
				m_shape = null;
			}
		}

		private static RigidBodyFlag GetFlags(MyCubeGrid grid)
		{
			if (!grid.IsStatic)
			{
				if (grid.GridSizeEnum != 0)
				{
					return RigidBodyFlag.RBF_DEFAULT;
				}
				return MyPerGameSettings.LargeGridRBFlag;
			}
			return RigidBodyFlag.RBF_STATIC;
		}

		public void SetDefaultRigidBodyMaxVelocities()
		{
			if (IsStatic)
			{
				RigidBody.MaxLinearVelocity = LargeShipMaxLinearVelocity();
				RigidBody.MaxAngularVelocity = GetLargeShipMaxAngularVelocity();
			}
			else if (m_grid.GridSizeEnum == MyCubeSize.Large)
			{
				RigidBody.MaxLinearVelocity = LargeShipMaxLinearVelocity();
				RigidBody.MaxAngularVelocity = GetLargeShipMaxAngularVelocity();
			}
			else if (m_grid.GridSizeEnum == MyCubeSize.Small)
			{
				RigidBody.MaxLinearVelocity = SmallShipMaxLinearVelocity();
				RigidBody.MaxAngularVelocity = GetSmallShipMaxAngularVelocity();
			}
		}

		public void SetRelaxedRigidBodyMaxVelocities()
		{
			RigidBody.MaxLinearVelocity = GetMaxRelaxedLinearVelocity();
			RigidBody.MaxAngularVelocity = GetMaxRelaxedAngularVelocity();
		}

		private HkBreakOffLogicResult BreakLogicHandler(HkRigidBody otherBody, uint shapeKey, ref float maxImpulse)
		{
			if (maxImpulse == 0f)
			{
				maxImpulse = Shape.BreakImpulse;
			}
			ulong user = 0uL;
			IMyEntity entity = otherBody.GetEntity(0u);
			MyPlayer controllingPlayer = MySession.Static.Players.GetControllingPlayer(entity as MyEntity);
			if (controllingPlayer != null)
			{
				user = controllingPlayer.Id.SteamId;
			}
			if (!MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.Damage, 0L, user) || (!MySession.Static.Settings.EnableVoxelDestruction && entity is MyVoxelBase))
			{
				return HkBreakOffLogicResult.DoNotBreakOff;
			}
			HkBreakOffLogicResult result = HkBreakOffLogicResult.UseLimit;
			if (!Sync.IsServer)
			{
				result = HkBreakOffLogicResult.DoNotBreakOff;
			}
			else if (RigidBody == null || base.Entity.MarkedForClose || otherBody == null)
			{
				result = HkBreakOffLogicResult.DoNotBreakOff;
			}
			else
			{
				IMyEntity entity2 = otherBody.GetEntity(0u);
				if (entity2 == null)
				{
					return HkBreakOffLogicResult.DoNotBreakOff;
				}
				MyMissile myMissile;
				if (entity2 is Sandbox.Game.WorldEnvironment.MyEnvironmentSector || entity2 is MyFloatingObject || entity2 is MyDebrisBase)
				{
					result = HkBreakOffLogicResult.DoNotBreakOff;
				}
				else if (entity2 is MyCharacter)
				{
					result = HkBreakOffLogicResult.DoNotBreakOff;
				}
				else if (entity2.GetTopMostParent() == base.Entity)
				{
					result = HkBreakOffLogicResult.DoNotBreakOff;
				}
				else if ((myMissile = entity2 as MyMissile) != null && myMissile.HasHealthPool)
				{
					result = HkBreakOffLogicResult.DoNotBreakOff;
				}
				else
				{
					MyCubeGrid myCubeGrid = entity2 as MyCubeGrid;
					if (!MySession.Static.Settings.EnableSubgridDamage && myCubeGrid != null && MyCubeGridGroups.Static.Physical.HasSameGroup(m_grid, myCubeGrid))
					{
						result = HkBreakOffLogicResult.DoNotBreakOff;
					}
					else if (base.Entity is MyCubeGrid || myCubeGrid != null)
					{
						result = HkBreakOffLogicResult.UseLimit;
					}
				}
				if (base.WeldInfo.Children.get_Count() > 0)
				{
					base.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable(RigidBody, Shape.BreakImpulse);
				}
			}
			_ = MyFakes.DEFORMATION_LOGGING;
			return result;
		}

		protected override void ActivateCollision()
		{
			if (m_world != null)
			{
				HavokCollisionSystemID = m_world.GetCollisionFilter().GetNewSystemGroup();
			}
		}

		public override void Activate(object world, ulong clusterObjectID)
		{
			if (MyPerGameSettings.Destruction && IsStatic)
			{
				Shape.FindConnectionsToWorld();
			}
			base.Activate(world, clusterObjectID);
			MarkBreakable((HkWorld)world);
		}

		public override void ActivateBatch(object world, ulong clusterObjectID)
		{
			if (MyPerGameSettings.Destruction && IsStatic)
			{
				Shape.FindConnectionsToWorld();
			}
			base.ActivateBatch(world, clusterObjectID);
			MarkBreakable((HkWorld)world);
		}

		public override void Deactivate(object world)
		{
			DisableTOIOptimization();
			UnmarkBreakable((HkWorld)world);
			base.Deactivate(world);
		}

		public override void DeactivateBatch(object world)
		{
			UnmarkBreakable((HkWorld)world);
			base.DeactivateBatch(world);
		}

		public override HkShape GetShape()
		{
			return Shape;
		}

		private void MarkBreakable(HkWorld world)
		{
			if (m_grid.BlocksDestructionEnabled)
			{
				m_shape.MarkBreakable(world, RigidBody);
				if (RigidBody2 != null)
				{
					m_shape.MarkBreakable(world, RigidBody2);
				}
			}
		}

		private void UnmarkBreakable(HkWorld world)
		{
			if (m_grid.BlocksDestructionEnabled)
			{
				if (m_shape != null)
				{
					m_shape.UnmarkBreakable(world, RigidBody);
				}
				if (RigidBody2 != null && m_shape != null)
				{
					m_shape.UnmarkBreakable(world, RigidBody2);
				}
			}
		}

		private void PlanetCrashEffect_Update()
		{
			if (!MyFakes.PLANET_CRASH_ENABLED || Sandbox.Engine.Platform.Game.IsDedicated)
			{
				return;
			}
			m_planetCrash_TimeBetweenPoints++;
			if (m_planetCrash_CrashAccumulation > 0)
			{
				m_planetCrash_CrashAccumulation--;
			}
			if (!IsPlanetCrashing())
			{
				m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, PlanetCrashEffect_Update);
			}
			PlanetCrashEffect_Reduce();
			if (m_planetCrash_Effect == null && m_planetCrash_generationMultiplier > 0.01f)
			{
				MatrixD effectMatrix = Matrix.Identity;
				Vector3D worldPosition = Matrix.Identity.Translation;
				MyParticlesManager.TryCreateParticleEffect("PlanetCrash", ref effectMatrix, ref worldPosition, uint.MaxValue, out m_planetCrash_Effect);
			}
			if (m_planetCrash_Effect != null)
			{
				m_planetCrash_Effect.UserBirthMultiplier = PlanetCrash_GetMultiplier();
<<<<<<< HEAD
				m_planetCrash_Effect.WorldMatrix = MatrixD.CreateWorld(m_planetCrash_CenterPoint.Value, -m_planetCrash_Normal.Value, Vector3.CalculatePerpendicularVector(m_planetCrash_Normal.Value));
=======
				MyParticleEffect planetCrash_Effect = m_planetCrash_Effect;
				Matrix m = Matrix.CreateWorld(m_planetCrash_CenterPoint.Value, -m_planetCrash_Normal.Value, Vector3.CalculatePerpendicularVector(m_planetCrash_Normal.Value));
				planetCrash_Effect.WorldMatrix = m;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_planetCrash_Effect.UserScale = m_planetCrash_ScaleCurrent * 0.1f;
			}
			if (m_planetCrash_ScaleGoal > m_planetCrash_ScaleCurrent)
			{
				m_planetCrash_ScaleCurrent *= 1.06f;
			}
			else
			{
				m_planetCrash_ScaleCurrent *= 0.995f;
			}
			if (m_planetCrash_generationMultiplier < 0.01f)
			{
				m_planetCrash_generationMultiplier = 0f;
				m_planetCrash_ScaleGoal = 1f;
				m_planetCrash_ScaleCurrent = 1f;
				m_planetCrash_CrashAccumulation = 0;
				m_planetCrash_IsStarted = false;
				if (m_planetCrash_Effect != null)
				{
					m_planetCrash_Effect.Stop(instant: false);
					m_planetCrash_Effect = null;
				}
			}
		}

		private void PlanetCrashEffect_AddCollision(Vector3D position, float separationSpeed, Vector3 normal, MyVoxelBase voxel = null)
		{
			if (!MyFakes.PLANET_CRASH_ENABLED || IsStatic || Sandbox.Engine.Platform.Game.IsDedicated || m_grid.BlocksCount < 5)
			{
				return;
			}
			float mass = MyGridPhysicalGroupData.GetGroupSharedProperties(m_grid, checkMultithreading: false).Mass;
			float num = separationSpeed * separationSpeed;
			bool planetCrash_IsStarted = m_planetCrash_IsStarted;
			if ((!planetCrash_IsStarted && ((mass < 50000f && num < 2500f) || (mass < 500000f && num < 900f) || (mass < 3000000f && num < 200f) || num < 50f || mass < 15000f)) || (planetCrash_IsStarted && (mass < 15000f || num < 50f)))
			{
				return;
			}
			Vector3 value = MyGravityProviderSystem.CalculateNaturalGravityInPoint(position);
			if ((double)value.LengthSquared() < 0.01)
			{
				return;
			}
			if (!m_planetCrash_IsStarted)
			{
				m_planetCrash_TimeBetweenPoints = 0;
				m_planetCrash_CrashAccumulation += 30;
				if (m_planetCrash_CrashAccumulation < 90)
				{
					return;
				}
			}
			float num2 = Math.Max((float)Math.Log10(mass - 30000f) - 3f, 0.3f);
			float num3 = (float)Math.Log10(num);
			float num4 = Math.Max(0f, num3 - 2f);
			m_planetCrash_ScaleGoal = 1.5f * num2 * num4;
			if (!m_planetCrash_IsStarted)
			{
				m_planetCrash_ScaleCurrent = 0.8f * m_planetCrash_ScaleGoal;
			}
			m_planetCrash_IsStarted = true;
			m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, PlanetCrashEffect_Update, 22);
			if (!m_planetCrash_CenterPoint.HasValue)
			{
				m_planetCrash_CenterPoint = position;
				m_planetCrash_Normal = Vector3.Normalize(value);
			}
			else
			{
				(position - m_planetCrash_CenterPoint.Value).Length();
				m_planetCrash_CenterPoint = position;
				m_planetCrash_Normal = Vector3.Normalize(0.85f * m_planetCrash_Normal.Value + 0.15f * normal);
			}
			m_planetCrash_generationMultiplier = 100f;
		}

		private float PlanetCrash_GetMultiplier()
		{
			return m_planetCrash_generationMultiplier * 0.025f;
		}

		private void PlanetCrashEffect_Reduce()
		{
			int num = 60;
			if (m_planetCrash_TimeBetweenPoints < num)
			{
				m_planetCrash_generationMultiplier -= 0.3f;
			}
			else
			{
				m_planetCrash_generationMultiplier *= 0.92f;
			}
		}

		public bool IsPlanetCrashing()
		{
			return m_planetCrash_IsStarted;
		}

		public bool PlanetCrashingNeedsUpdates()
		{
			if (!IsPlanetCrashing())
			{
				return m_planetCrash_CrashAccumulation > 0;
			}
			return true;
		}

		public bool IsPlanetCrashing_PointConcealed(Vector3D point)
		{
			if (!m_planetCrash_IsStarted)
			{
				return false;
			}
			Vector3 vector = point - m_planetCrash_CenterPoint.Value;
			float num = Math.Abs(Vector3.Dot(vector, m_planetCrash_Normal.Value));
			float num2 = 2f * m_planetCrash_ScaleCurrent;
			float num3 = 4f * m_planetCrash_ScaleCurrent;
			if (num < num2 && vector.LengthSquared() < num3 * num3)
			{
				return true;
			}
			return false;
		}

		private void RigidBody_ContactPointCallback(ref HkContactPointEvent value)
		{
			RigidBody_ContactPointCallbackImpl(ref value);
		}

		private void RigidBody_ContactPointCallbackImpl(ref HkContactPointEvent value)
		{
			if (m_grid == null || m_grid.Physics == null)
			{
				return;
			}
			if (value.IsToi && Interlocked.Increment(ref m_toiCountThisFrame) == 2)
			{
				m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateTOIOptimizer, 26);
			}
			if (Math.Abs(value.SeparatingVelocity) < 0.3f && (value.Base.GetRigidBody(0).IsEnvironment || value.Base.GetRigidBody(1).IsEnvironment))
			{
				return;
			}
			bool AisThis;
			IMyEntity otherEntity = value.GetOtherEntity(m_grid, out AisThis);
			if (otherEntity == null)
			{
				return;
			}
			if (PredictCollisions)
			{
				PredictContactImpulse(otherEntity, ref value);
			}
			MyGridContactInfo myGridContactInfo = new MyGridContactInfo(ref value, m_grid, otherEntity as MyEntity);
			if (myGridContactInfo.CollidingEntity is MyCharacter || myGridContactInfo.CollidingEntity.MarkedForClose)
			{
				return;
			}
			if (AisThis)
			{
				value.ContactPoint.Flip();
			}
			HkContactPoint contactPoint = value.ContactPoint;
			bool flag = myGridContactInfo.CollidingEntity is MyVoxelPhysics || myGridContactInfo.CollidingEntity is MyVoxelMap;
			if (flag)
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_VOXEL_CONTACT_MATERIAL)
				{
					MyVoxelMaterialDefinition voxelSurfaceMaterial = myGridContactInfo.VoxelSurfaceMaterial;
					if (voxelSurfaceMaterial != null)
					{
						MyRenderProxy.DebugDrawText3D(ClusterToWorld(contactPoint.Position), voxelSurfaceMaterial.Id.SubtypeName + "(" + voxelSurfaceMaterial.Friction.ToString("F2") + ";" + voxelSurfaceMaterial.Restitution.ToString("F2") + ")", Color.Red, 0.7f, depthRead: false);
					}
				}
				if (m_grid.Render != null)
				{
					m_grid.Render.ResetLastVoxelContactTimer();
				}
			}
			bool num = !myGridContactInfo.IsKnown;
			bool flag2 = MyPerGameSettings.EnableCollisionSparksEffect && !IsPlanetCrashing() && (myGridContactInfo.CollidingEntity is MyCubeGrid || flag);
			myGridContactInfo.HandleEvents();
			if (MyDebugDrawSettings.DEBUG_DRAW_FRICTION)
			{
				Vector3D vector3D = ClusterToWorld(contactPoint.Position);
				Vector3 vector = -GetVelocityAtPoint(vector3D);
				vector *= 0.1f;
				float num2 = Math.Abs(Gravity.Dot(contactPoint.Normal) * value.ContactProperties.Friction);
				if (vector.Length() > 0.5f)
				{
					vector.Normalize();
					MyRenderProxy.DebugDrawArrow3D(vector3D, vector3D + num2 * vector, Color.Gray, Color.Gray, depthRead: false, 0.1, null, 0.5f, persistent: true);
				}
			}
			if (num)
			{
				MyVoxelMaterialDefinition voxelSurfaceMaterial2 = myGridContactInfo.VoxelSurfaceMaterial;
				if (voxelSurfaceMaterial2 != null)
				{
					HkContactPointProperties contactProperties = value.ContactProperties;
					contactProperties.Friction *= voxelSurfaceMaterial2.Friction;
					contactProperties.Restitution *= voxelSurfaceMaterial2.Restitution * ((m_grid.GridSizeEnum == MyCubeSize.Small) ? 0.4f : 0.25f);
				}
			}
			if (!LowSimulationQuality && m_isServer && (otherEntity is MyCubeGrid || flag))
			{
				MyEntity.ContactPointData.ContactPointDataTypes contactPointDataTypes = MyEntity.ContactPointData.ContactPointDataTypes.None;
				Vector3 separatingVelocity = CalculateSeparatingVelocity(value.Base.BodyA, value.Base.BodyB, contactPoint.Position);
				float num3 = separatingVelocity.Length();
				if (flag)
				{
					contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_PlanetCrash;
				}
				if (myGridContactInfo.EnableParticles)
				{
					if (flag2 && (num3 > 1f || value.ContactProperties.MaxImpulse > 5000f) && (IsStatic || MyGridPhysicalGroupData.GetGroupSharedProperties(m_grid, checkMultithreading: false).Mass > 5000f))
					{
						int count = m_lastContacts.Count;
						if (m_lastContacts.TryAdd(value.ContactPointId, MySandboxGame.TotalGamePlayTimeInMilliseconds))
						{
							if ((num3 > 0.3f && value.ContactProperties.MaxImpulse > 20000f) || num3 > 0.8f)
							{
								Vector3 vector2 = separatingVelocity / num3;
								if ((double)Math.Abs(Vector3.Dot(contactPoint.Normal, vector2)) > 0.75 || num3 < 2f)
								{
									contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_Collision;
								}
								else if (RigidBody != null)
								{
									contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_GridCollision;
								}
							}
							if (count == 0)
							{
								m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, ProcessContacts, 23);
							}
						}
					}
					if (MyPerGameSettings.EnableCollisionSparksEffect && flag && !IsPlanetCrashing() && Math.Abs(value.SeparatingVelocity * (m_grid.Mass / 100000f)) > 0.25f)
					{
						contactPointDataTypes |= MyEntity.ContactPointData.ContactPointDataTypes.Particle_Dust;
					}
				}
				if (contactPointDataTypes != 0)
				{
					Vector3 normal = contactPoint.Normal;
					Vector3D worldPosition = ClusterToWorld(contactPoint.Position);
					if (Sync.IsServer && MyMultiplayer.Static != null)
					{
						MyCubeGrid myCubeGrid = base.Entity as MyCubeGrid;
						Vector3 relativePosition = worldPosition - myCubeGrid.PositionComp.GetPosition();
						myCubeGrid.UpdateParticleContactPoint(otherEntity.EntityId, ref relativePosition, ref normal, ref separatingVelocity, num3, value.ContactProperties.MaxImpulse, contactPointDataTypes);
					}
					else
					{
						PlayCollisionParticlesInternal(otherEntity, ref worldPosition, ref normal, ref separatingVelocity, num3, value.ContactProperties.MaxImpulse, contactPointDataTypes);
					}
				}
			}
			if (AisThis)
			{
				value.ContactPoint.Flip();
			}
		}

		private void ProcessContacts()
		{
			if (m_lastContacts.Count > 0)
			{
				m_tmpContactId.Clear();
				foreach (KeyValuePair<ushort, int> lastContact in m_lastContacts)
				{
					if (MySandboxGame.TotalGamePlayTimeInMilliseconds > lastContact.Value + SparksEffectDelayPerContactMs)
					{
						m_tmpContactId.Add(lastContact.Key);
					}
				}
				foreach (ushort item in m_tmpContactId)
				{
					m_lastContacts.Remove(item);
				}
			}
			if (m_lastContacts.Count == 0)
			{
				m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, ProcessContacts);
			}
		}

		public void PlayCollisionParticlesInternal(IMyEntity otherEntity, ref Vector3D worldPosition, ref Vector3 normal, ref Vector3 separatingVelocity, float separatingSpeed, float impulse, MyEntity.ContactPointData.ContactPointDataTypes flags)
		{
			if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_PlanetCrash) != 0)
			{
				PlanetCrashEffect_AddCollision(worldPosition, separatingSpeed, normal, otherEntity as MyVoxelBase);
			}
			if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_Collision) != 0)
			{
				AddCollisionEffect(worldPosition, normal, separatingSpeed, impulse);
			}
			if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_GridCollision) != 0)
			{
				AddGridCollisionEffect(worldPosition - RigidBody.Position, normal, separatingVelocity, separatingSpeed, impulse);
			}
			if ((flags & MyEntity.ContactPointData.ContactPointDataTypes.Particle_Dust) != 0)
			{
				float scale = MathHelper.Clamp(Math.Abs(separatingSpeed * (m_grid.Mass / 100000f)) / 10f, 0.2f, 2f);
				AddDustEffect(worldPosition, scale);
			}
		}

		private static Vector3 GetGridPosition(HkContactPoint contactPoint, HkRigidBody gridBody, MyCubeGrid grid, int body)
		{
			return Vector3.Transform(contactPoint.Position + ((body == 0) ? 0.1f : (-0.1f)) * contactPoint.Normal, Matrix.Invert(gridBody.GetRigidBodyMatrix()));
		}

		private MyGridContactInfo ReduceVelocities(MyGridContactInfo info)
		{
			info.Event.AccessVelocities(0);
			info.Event.AccessVelocities(1);
			if (!info.CollidingEntity.Physics.IsStatic && info.CollidingEntity.Physics.Mass < 600f)
			{
				info.CollidingEntity.Physics.LinearVelocity /= 2f;
			}
			if (!IsStatic && MyDestructionHelper.MassFromHavok(Mass) < 600f)
			{
				LinearVelocity /= 2f;
			}
			info.Event.UpdateVelocities(0);
			info.Event.UpdateVelocities(1);
			return info;
		}

		private bool BreakPartsHandler(ref HkBreakOffPoints breakOffPoints, ref HkArrayUInt32 brokenKeysOut)
		{
			bool flag = false;
			if (!MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return flag;
			}
			for (int i = 0; i < breakOffPoints.Count; i++)
			{
				HkBreakOffPointInfo pt = breakOffPoints[i];
				flag |= BreakAtPoint(ref pt);
			}
			return false;
		}

		private bool BreakAtPoint(ref HkBreakOffPointInfo pt)
		{
			pt.ContactPosition = ClusterToWorld(pt.ContactPoint.Position);
			IMyEntity entity = pt.CollidingBody.GetEntity(0u);
			if (entity.Physics == null || entity.Physics.RigidBody == null)
			{
				return false;
			}
			Vector3 vector = CalculateSeparatingVelocity(RigidBody, pt.CollidingBody, ref pt.ContactPoint);
			float num = Math.Min(IsStatic ? entity.Physics.Mass : Mass, entity.Physics.IsStatic ? Mass : entity.Physics.Mass);
			float num2 = vector.Length();
			if (entity is MyVoxelBase && m_grid.BlocksCount == 1)
			{
				num2 += pt.BreakingImpulse / num;
			}
			float num3 = num2 * num;
			float num4 = num2;
			_ = MyFakes.DEFORMATION_LOGGING;
			if (num3 <= 21000f || num4 <= 0f)
			{
				return false;
			}
<<<<<<< HEAD
			bool flag = false;
			if (m_grid != null && m_grid.IntegrityMass < MINIMUM_GRID_INTEGRITY_MASS)
			{
				flag = true;
			}
			bool flag2 = PerformDeformationOnGroup((MyEntity)base.Entity, (MyEntity)entity, ref pt, num4);
			if (!flag)
			{
				pt.ContactPointDirection *= -1f;
				flag2 |= PerformDeformationOnGroup((MyEntity)entity, (MyEntity)base.Entity, ref pt, num4);
			}
=======
			PerformDeformationOnGroup((MyEntity)base.Entity, (MyEntity)entity, ref pt, num4);
			pt.ContactPointDirection *= -1f;
			PerformDeformationOnGroup((MyEntity)entity, (MyEntity)base.Entity, ref pt, num4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return false;
		}

		private Vector3 CalculateSeparatingVelocity(HkRigidBody bodyA, HkRigidBody bodyB, ref HkContactPoint cp)
		{
			return CalculateSeparatingVelocity(bodyA, bodyB, cp.Position);
		}

		private Vector3 CalculateSeparatingVelocity(HkRigidBody bodyA, HkRigidBody bodyB, Vector3 position)
		{
			Vector3 vector = Vector3.Zero;
			if (!bodyA.IsFixed)
			{
				Vector3 vector2 = position - bodyA.CenterOfMassWorld;
				vector = Vector3.Cross(bodyA.AngularVelocity, vector2);
				vector.Add(bodyA.LinearVelocity);
			}
			Vector3 vector3 = Vector3.Zero;
			if (!bodyB.IsFixed)
			{
				Vector3 vector4 = position - bodyB.CenterOfMassWorld;
				vector3 = Vector3.Cross(bodyB.AngularVelocity, vector4);
				vector3.Add(bodyB.LinearVelocity);
<<<<<<< HEAD
=======
			}
			return vector - vector3;
		}

		private Vector3 CalculateSeparatingVelocity(MyCubeGrid first, MyCubeGrid second, Vector3 position)
		{
			Vector3D vector3D = ClusterToWorld(position);
			Vector3 vector = Vector3.Zero;
			if (!first.IsStatic && first.Physics != null)
			{
				Vector3 vector2 = (Vector3)vector3D - first.Physics.CenterOfMassWorld;
				vector = Vector3.Cross(first.Physics.AngularVelocity, vector2);
				vector.Add(first.Physics.LinearVelocity);
			}
			Vector3 vector3 = Vector3.Zero;
			if (!second.IsStatic && second.Physics != null)
			{
				Vector3 vector4 = vector3D - second.Physics.CenterOfMassWorld;
				vector3 = Vector3.Cross(second.Physics.AngularVelocity, vector4);
				vector3.Add(second.Physics.LinearVelocity);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return vector - vector3;
		}

		private bool PerformDeformationOnGroup(MyEntity entity, MyEntity other, ref HkBreakOffPointInfo pt, float separatingVelocity)
		{
			bool result = false;
			if (!entity.MarkedForClose)
			{
				if (entity.PositionComp.WorldAABB.Inflate(0.10000000149011612).Contains(pt.ContactPosition) != 0)
				{
					MyGridPhysics myGridPhysics = entity.Physics as MyGridPhysics;
					if (myGridPhysics != null)
					{
						result = myGridPhysics.PerformDeformation(ref pt, fromBreakParts: false, separatingVelocity, other);
					}
				}
				else
				{
					_ = MyFakes.DEFORMATION_LOGGING;
				}
			}
			return result;
		}

<<<<<<< HEAD
=======
		private bool BreakPartsHandler(ref HkBreakOffPoints breakOffPoints, ref HkArrayUInt32 brokenKeysOut)
		{
			bool flag = false;
			if (!MySessionComponentSafeZones.IsActionAllowed(m_grid, MySafeZoneAction.Damage, 0L, 0uL))
			{
				return flag;
			}
			for (int i = 0; i < breakOffPoints.Count; i++)
			{
				HkBreakOffPointInfo pt = breakOffPoints[i];
				flag |= BreakAtPoint(ref pt, ref brokenKeysOut);
			}
			return false;
		}

		private static float CalculateSoften(float softAreaPlanarInv, float softAreaVerticalInv, ref Vector3 normal, Vector3 contactToTarget)
		{
			Vector3.Dot(ref normal, ref contactToTarget, out var result);
			if (result < 0f)
			{
				result = 0f - result;
			}
			float num = 1f - result * softAreaVerticalInv;
			if (num <= 0f)
			{
				return 0f;
			}
			float num2 = contactToTarget.LengthSquared() - result * result;
			if (num2 <= 0f)
			{
				return num;
			}
			float num3 = (float)Math.Sqrt(num2);
			float num4 = 1f - num3 * softAreaPlanarInv;
			if (num4 <= 0f)
			{
				return 0f;
			}
			return num * num4;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void PerformMeteoriteDeformation(in MyMeteor.MyMeteorGameLogic.ContactProperties pt)
		{
			float num = 0.3f + Math.Max(0f, (float)Math.Sqrt((double)Math.Abs(pt.SeparatingVelocity) + Math.Pow(pt.CollidingBody.Mass, 0.72)) / 10f);
			num *= 6f;
			num = Math.Min(num, 5f);
			float num2 = (float)Math.Pow(pt.CollidingBody.Mass, 0.15000000596046448);
			num2 -= 0.3f;
			num2 *= ((m_grid.GridSizeEnum == MyCubeSize.Large) ? 4f : 1f);
			float num3 = num;
			num3 *= ((m_grid.GridSizeEnum == MyCubeSize.Large) ? 1f : 0.2f);
			MatrixD worldMatrixNormalizedInv = m_grid.PositionComp.WorldMatrixNormalizedInv;
<<<<<<< HEAD
			Vector3 localPos = Vector3D.Transform(pt.Position, worldMatrixNormalizedInv);
			Vector3 vector = Vector3.TransformNormal(pt.Normal, worldMatrixNormalizedInv) * pt.Direction;
			bool flag = ApplyDeformation(num, num2, num3, localPos, vector, MyDamageType.Deformation, 0f, (m_grid.GridSizeEnum == MyCubeSize.Large) ? 0.6f : 0.16f, 0L);
=======
			Vector3D vector3D = Vector3D.Transform(pt.Position, worldMatrixNormalizedInv);
			Vector3 vector = Vector3.TransformNormal(pt.Normal, worldMatrixNormalizedInv) * pt.Direction;
			bool flag = ApplyDeformation(num, num2, num3, vector3D, vector, MyDamageType.Deformation, 0f, (m_grid.GridSizeEnum == MyCubeSize.Large) ? 0.6f : 0.16f, 0L);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyPhysics.CastRay(pt.Position, pt.Position - num3 * Vector3.Normalize(pt.Normal), m_hitList);
			foreach (MyPhysics.HitInfo hit in m_hitList)
			{
				IMyEntity hitEntity = hit.HkHitInfo.GetHitEntity();
				if (hitEntity != m_grid.Components && hitEntity is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = hitEntity as MyCubeGrid;
					worldMatrixNormalizedInv = myCubeGrid.PositionComp.WorldMatrixNormalizedInv;
<<<<<<< HEAD
					localPos = Vector3D.Transform(pt.Position, worldMatrixNormalizedInv);
					vector = Vector3.TransformNormal(pt.Normal, worldMatrixNormalizedInv) * pt.Direction;
					myCubeGrid.Physics.ApplyDeformation(num, num2 * ((m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum) ? 1f : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2f : 0.25f)), num3 * ((m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum) ? 1f : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2.5f : 0.2f)), localPos, vector, MyDamageType.Deformation, 0f, (myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.6f : 0.16f, 0L);
=======
					vector3D = Vector3D.Transform(pt.Position, worldMatrixNormalizedInv);
					vector = Vector3.TransformNormal(pt.Normal, worldMatrixNormalizedInv) * pt.Direction;
					myCubeGrid.Physics.ApplyDeformation(num, num2 * ((m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum) ? 1f : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2f : 0.25f)), num3 * ((m_grid.GridSizeEnum == myCubeGrid.GridSizeEnum) ? 1f : ((myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 2.5f : 0.2f)), vector3D, vector, MyDamageType.Deformation, 0f, (myCubeGrid.GridSizeEnum == MyCubeSize.Large) ? 0.6f : 0.16f, 0L);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			m_hitList.Clear();
			float num4 = Math.Max(m_grid.GridSize, num * ((m_grid.GridSizeEnum == MyCubeSize.Large) ? 0.25f : 0.05f));
			if (num4 > 0f && num > m_grid.GridSize / 2f && flag)
			{
				ExplosionInfo explosionInfo = default(ExplosionInfo);
				explosionInfo.Position = pt.Position;
				explosionInfo.ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION;
				explosionInfo.Radius = num4;
				explosionInfo.ShowParticles = true;
				explosionInfo.GenerateDebris = true;
				ExplosionInfo info = explosionInfo;
				EnqueueExplosion(in info);
			}
			else
			{
				AddCollisionEffect(pt.Position, vector, 0f, 0f);
			}
		}

		private bool PerformDeformation(ref HkBreakOffPointInfo pt, bool fromBreakParts, float separatingVelocity, MyEntity otherEntity)
		{
			if (!m_grid.BlocksDestructionEnabled)
			{
				_ = MyFakes.DEFORMATION_LOGGING;
				return false;
			}
			bool flag = false;
			ulong simulationFrameCounter = MySandboxGame.Static.SimulationFrameCounter;
			if (m_frameCollided == simulationFrameCounter)
			{
				foreach (Vector3D item in m_contactPosCache)
				{
					if (Vector3D.DistanceSquared(pt.ContactPosition, item) < (double)(m_grid.GridSize * m_grid.GridSize / 4f))
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				if (simulationFrameCounter - m_frameCollided > 100)
				{
					m_frameFirstImpact = simulationFrameCounter;
					m_impactDot = 0f;
				}
				m_appliedSlowdownThisFrame = false;
				m_contactPosCache.Clear();
			}
			bool flag2 = otherEntity is MyVoxelBase;
			bool flag3 = flag2 && !Vector3.IsZero(ref m_cachedGravity);
			bool otherEntityDeformable = !flag2 && !(otherEntity is MyTrees);
<<<<<<< HEAD
			float num = 1f;
			float num2;
			if (IsStatic)
			{
				num2 = ((!otherEntity.Physics.IsStatic) ? Math.Min(1f, otherEntity.Physics.Mass / MyFakes.DEFORMATION_MASS_THR) : 1f);
			}
			else
			{
				num2 = Math.Min(1f, Mass / MyFakes.DEFORMATION_MASS_THR);
				if (!otherEntity.Physics.IsStatic)
				{
					num = otherEntity.Physics.Mass / (otherEntity.Physics.Mass + Mass);
				}
			}
			float num3 = 1f;
			float num4 = separatingVelocity * num2 * MyFakes.DEFORMATION_OFFSET_RATIO;
=======
			float num = ((!IsStatic) ? Math.Min(1f, Mass / MyFakes.DEFORMATION_MASS_THR) : ((!otherEntity.Physics.IsStatic) ? Math.Min(1f, otherEntity.Physics.Mass / MyFakes.DEFORMATION_MASS_THR) : 1f));
			float num2 = 1f;
			float num3 = separatingVelocity * num * MyFakes.DEFORMATION_OFFSET_RATIO;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyCubeGrid otherGrid = otherEntity as MyCubeGrid;
			bool isLargeGrid = m_grid.GridSizeEnum == MyCubeSize.Large;
			if (!IsStatic && !otherEntity.Physics.IsStatic)
			{
<<<<<<< HEAD
				num4 = ((otherGrid == null || m_grid.GridSizeEnum == otherGrid.GridSizeEnum) ? (num4 * 0.5f) : ((!isLargeGrid) ? (num4 * 1.6f) : (num4 * 0.105f)));
=======
				num3 = ((otherGrid == null || m_grid.GridSizeEnum == otherGrid.GridSizeEnum) ? (num3 * 0.5f) : ((!isLargeGrid) ? (num3 * 1.6f) : (num3 * 0.105f)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (otherEntityDeformable)
			{
				if (otherGrid != null && m_grid.GridSizeEnum != otherGrid.GridSizeEnum)
				{
					if (isLargeGrid)
					{
						num4 *= 0.09f;
					}
					else
					{
						num3 = 4.5f;
						num4 *= 0.22f;
					}
				}
				else
				{
					num4 *= 0.5f;
				}
			}
			else if (m_grid.PositionComp.LocalAABB.Volume() < 20f && separatingVelocity / 60f > m_grid.GridSize / 5f)
			{
				num4 *= 30f;
			}
			else if (isLargeGrid || !flag2)
			{
				num4 *= 1.5f;
			}
			if (flag2)
			{
<<<<<<< HEAD
				float num5 = (isLargeGrid ? 6.8f : 8f);
=======
				float num4 = (isLargeGrid ? 6.8f : 8f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				float val = MyFakes.DEFORMATION_OFFSET_MAX * 10f;
				num4 = Math.Min(num4 / num5, val);
			}
			else
			{
				num4 = Math.Min(num4, MyFakes.DEFORMATION_OFFSET_MAX);
			}
			if (num4 <= 0.1f)
			{
				_ = MyFakes.DEFORMATION_LOGGING;
				return false;
			}
<<<<<<< HEAD
			float num6 = (isLargeGrid ? 6f : 1.2f) * num3;
			float num7 = Math.Max(0.5f, num4 * num);
			num7 *= (isLargeGrid ? 1.5f : 1f);
			MatrixD worldMatrixNormalizedInv = m_grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3 localPos = Vector3D.Transform(pt.ContactPosition, worldMatrixNormalizedInv);
=======
			float num5 = (isLargeGrid ? 6f : 1.2f) * num2;
			float num6 = (isLargeGrid ? 1.5f : 1f) * num3;
			MatrixD worldMatrixNormalizedInv = m_grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3D vector3D = Vector3D.Transform(pt.ContactPosition, worldMatrixNormalizedInv);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3 vector = -GetVelocityAtPoint(pt.ContactPosition);
			Vector3 vector2 = pt.ContactPointDirection * pt.ContactPoint.Normal;
			if (!vector.IsValid() || vector.LengthSquared() < 25f)
			{
				vector = vector2;
			}
			Vector3 localNormal = Vector3.TransformNormal(vector, worldMatrixNormalizedInv);
<<<<<<< HEAD
			float num8 = localNormal.Normalize();
			bool flag4 = num8 < 3f;
=======
			float num7 = localNormal.Normalize();
			bool flag4 = num7 < 3f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Vector3 vector3 = -vector;
			vector3.Normalize();
			float num9 = Math.Abs(Vector3.Dot(vector3, vector2));
			if (m_impactDot == 0f)
			{
				m_impactDot = num9;
			}
			else
			{
				m_impactDot = m_impactDot * 0.5f + num9 * 0.5f;
			}
			if (flag2 && isLargeGrid)
			{
				num7 *= MyFakes.DEFORMATION_DAMAGE_MULTIPLIER;
				num6 *= MyFakes.DEFORMATION_DAMAGE_MULTIPLIER * 2f;
				if (flag3)
				{
					float num10 = 1f + m_impactDot * m_impactDot / 2f;
					num7 *= num10;
					num6 *= num10;
				}
			}
			int destroyed;
<<<<<<< HEAD
			bool result = ApplyDeformation(num4, num6, num7, localPos, localNormal, MyDamageType.Deformation, out destroyed, 0f, 0f, (otherEntity != null) ? otherEntity.EntityId : 0);
=======
			bool result = ApplyDeformation(num3, num5, num6, vector3D, localNormal, MyDamageType.Deformation, out destroyed, 0f, 0f, (otherEntity != null) ? otherEntity.EntityId : 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW)
			{
				MyRenderProxy.DebugDrawArrow3D(pt.ContactPosition, pt.ContactPosition + Vector3.Normalize(vector) * destroyed, Color.Red, Color.Red, depthRead: false, 0.1, base.Entity.DisplayName, 0.5f, persistent: true);
			}
			_ = MyFakes.DEFORMATION_LOGGING;
			if (destroyed > 0)
			{
				if (!flag)
				{
					m_contactPosCache.Add(pt.ContactPosition);
				}
				m_frameCollided = simulationFrameCounter;
			}
			Vector3 cpPosition;
			HkRigidBody rigidBody;
			Vector3 linearVelocity;
			Vector3 velocityMass;
			if (!IsStatic)
			{
				cpPosition = pt.ContactPoint.Position;
				if (MyFakes.DEFORMATION_APPLY_IMPULSE)
				{
					HkRigidBody rigidBody2 = RigidBody;
					Vector3 impulse = -(rigidBody2.LinearVelocity * rigidBody2.Mass) * destroyed * MyFakes.DEFORMATION_IMPULSE_FACTOR;
					rigidBody2.ApplyPointImpulse(impulse, cpPosition);
				}
				else if (otherEntity != null)
				{
					if (flag2)
					{
						if (!m_appliedSlowdownThisFrame)
						{
							m_appliedSlowdownThisFrame = true;
							MySandboxGame.Static.Invoke("ApplyColisionForce", this, delegate(object context)
							{
								MyGridPhysics myGridPhysics = (MyGridPhysics)context;
								HkRigidBody rigidBody3 = myGridPhysics.RigidBody;
								if (!(rigidBody3 == null))
								{
									bool flag7 = myGridPhysics.m_grid.GridSizeEnum == MyCubeSize.Small;
									float impactDot = myGridPhysics.m_impactDot;
<<<<<<< HEAD
									ulong num19 = MySandboxGame.Static.SimulationFrameCounter - myGridPhysics.m_frameFirstImpact;
									bool num20 = num19 < 100 && rigidBody3.LinearVelocity.LengthSquared() > 25f;
									bool flag8 = num19 > 50 || (impactDot > 0.8f && num19 > 10) || flag7;
									bool flag9 = num19 > 100 || flag7;
									if (num20)
=======
									ulong num18 = MySandboxGame.Static.SimulationFrameCounter - myGridPhysics.m_frameFirstImpact;
									bool num19 = num18 < 100 && rigidBody3.LinearVelocity.LengthSquared() > 25f;
									bool flag8 = num18 > 50 || (impactDot > 0.8f && num18 > 10) || flag7;
									bool flag9 = num18 > 100 || flag7;
									if (num19)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
									{
										Vector3 angularVelocity = rigidBody3.AngularVelocity;
										rigidBody3.AngularVelocity -= angularVelocity * (1f - impactDot) / 2f;
									}
									if (flag8)
									{
<<<<<<< HEAD
										float num21 = 1f - impactDot;
										float num22 = (1f - num21 * num21 + impactDot) / 1.5f;
										if ((double)impactDot < 0.5)
										{
											num22 /= 2f;
										}
										Vector3 linearVelocity4 = rigidBody3.LinearVelocity;
										float maxLinearVelocity = myGridPhysics.GetMaxLinearVelocity();
										float val4 = num22 * linearVelocity4.Length() / (maxLinearVelocity * 1.5f);
=======
										float num20 = 1f - impactDot;
										float num21 = (1f - num20 * num20 + impactDot) / 1.5f;
										if ((double)impactDot < 0.5)
										{
											num21 /= 2f;
										}
										Vector3 linearVelocity4 = rigidBody3.LinearVelocity;
										float maxLinearVelocity = myGridPhysics.GetMaxLinearVelocity();
										float val4 = num21 * linearVelocity4.Length() / (maxLinearVelocity * 1.5f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
										val4 = Math.Min(val4, flag9 ? 0.2f : 0.1f);
										if ((double)impactDot > 0.5)
										{
											val4 *= 1f + impactDot * 0.5f;
										}
										if (!Vector3.IsZero(ref myGridPhysics.m_cachedGravity))
										{
											Vector3 vector4 = -linearVelocity4;
											Vector3 vector5 = myGridPhysics.m_cachedGravity.Project(vector4);
											Vector3 vector6 = vector5 * val4 * 2f;
											if (flag9)
											{
												Vector3 vector7 = (vector4 - vector5) * val4;
												vector6 += vector7;
											}
											rigidBody3.LinearVelocity += vector6;
										}
										else if (flag7)
										{
											Vector3 vector8 = -linearVelocity4;
											Vector3 vector9 = val4 * 1f * vector8;
											rigidBody3.LinearVelocity += vector9;
										}
									}
								}
							});
						}
					}
					else if (destroyed > 0)
					{
						rigidBody = RigidBody;
						linearVelocity = rigidBody.LinearVelocity;
						velocityMass = linearVelocity * rigidBody.Mass;
						if (MyEntities.IsAsyncUpdateInProgress && !MyPhysics.InsideSimulation)
						{
							MyEntities.InvokeLater(ApplyImpulses);
						}
						else
						{
							ApplyImpulses();
						}
					}
				}
			}
			if (Sync.IsServer && MyFakes.DEFORMATION_EXPLOSIONS && m_grid.BlocksCount > 10 && !flag4)
			{
				bool flag5 = m_grid.GridSizeEnum == MyCubeSize.Large;
<<<<<<< HEAD
				float num11 = Math.Min(1f, num8 / 20f);
				float num12 = (1f - m_impactDot) * 0.6f + (flag5 ? 1.4f : 1.5f);
				float num13 = (flag5 ? 0.25f : 0.06f);
				float num14 = (flag5 ? 0.3f : 0.4f);
				bool flag6 = !Vector3.IsZero(m_cachedGravity);
				float num15 = ((m_grid.GridSizeEnum == MyCubeSize.Small) ? MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER_SMALL_GRID : MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER);
				float val2 = (m_grid.GridSize + (float)Math.Sqrt(destroyed) * num13) * num12 * num11 * num15;
				val2 = Math.Min(val2, MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * num12);
				Vector3D vector3D = pt.ContactPosition + vector2 * val2 * 0.5f;
				Vector3D position = vector3D + vector3 * val2 * 0.95f / (flag6 ? 2f : 1f);
				float num16 = num12 * 0.75f - m_impactDot * num14;
				float val3 = val2 * num16 * MathHelper.Lerp(0.4f, 1f, Math.Min(1f, num8 / 50f));
				val3 = Math.Min(val3, MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * num12 * 1.5f);
=======
				float num10 = Math.Min(1f, num7 / 20f);
				float num11 = (1f - m_impactDot) * 0.6f + (flag5 ? 1.4f : 1.5f);
				float num12 = (flag5 ? 0.25f : 0.06f);
				float num13 = (flag5 ? 0.3f : 0.4f);
				bool flag6 = !Vector3.IsZero(m_cachedGravity);
				float num14 = ((m_grid.GridSizeEnum == MyCubeSize.Small) ? MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER_SMALL_GRID : MyFakes.DEFORMATION_VOXEL_CUTOUT_MULTIPLIER);
				float val2 = (m_grid.GridSize + (float)Math.Sqrt(destroyed) * num12) * num11 * num10 * num14;
				val2 = Math.Min(val2, MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * num11);
				Vector3D vector3D2 = pt.ContactPosition + vector2 * val2 * 0.5f;
				Vector3D position = vector3D2 + vector3 * val2 * 0.95f / (flag6 ? 2f : 1f);
				float num15 = num11 * 0.75f - m_impactDot * num13;
				float val3 = val2 * num15 * MathHelper.Lerp(0.4f, 1f, Math.Min(1f, num7 / 50f));
				val3 = Math.Min(val3, MyFakes.DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS * num11 * 1.5f);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (flag6 && (double)m_impactDot > 0.7)
				{
					val3 *= 1.35f;
				}
				ExplosionInfo info = new ExplosionInfo
				{
					Position = vector3D2,
					ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION,
					Radius = val2,
					ShowParticles = false,
					GenerateDebris = true
				};
				EnqueueExplosion(in info);
<<<<<<< HEAD
				ulong num17 = MySandboxGame.Static.SimulationFrameCounter - m_frameFirstImpact;
				if (flag6 && (double)m_impactDot < 0.7 && ((isLargeGrid && num17 < 10) || !isLargeGrid))
=======
				ulong num16 = MySandboxGame.Static.SimulationFrameCounter - m_frameFirstImpact;
				if (flag6 && (double)m_impactDot < 0.7 && ((isLargeGrid && num16 < 10) || !isLargeGrid))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					info = new ExplosionInfo
					{
						Position = position,
						ExplosionType = MyExplosionTypeEnum.GRID_DESTRUCTION,
						Radius = val3,
						ShowParticles = false,
						GenerateDebris = true
					};
					EnqueueExplosion(in info);
				}
				_ = MyFakes.DEFORMATION_LOGGING;
			}
			return result;
			void ApplyImpulses()
			{
				if (!otherEntity.Closed)
				{
					if (otherEntity.Physics.IsStatic)
					{
						if (otherEntityDeformable && otherGrid != null)
						{
<<<<<<< HEAD
							float num18 = MyFakes.DEFORMATION_VELOCITY_RELAY_STATIC;
							if (isLargeGrid)
							{
								num18 /= 8f;
							}
							else if (otherGrid.GridSizeEnum == MyCubeSize.Large)
							{
								num18 *= 4f;
							}
							Vector3 linearVelocity2 = Vector3.Lerp(linearVelocity, Vector3.Zero, num18);
=======
							float num17 = MyFakes.DEFORMATION_VELOCITY_RELAY_STATIC;
							if (isLargeGrid)
							{
								num17 /= 8f;
							}
							else if (otherGrid.GridSizeEnum == MyCubeSize.Large)
							{
								num17 *= 4f;
							}
							Vector3 linearVelocity2 = Vector3.Lerp(linearVelocity, Vector3.Zero, num17);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							rigidBody.LinearVelocity = linearVelocity2;
						}
						else if (!isLargeGrid)
						{
							Vector3 impulse2 = -velocityMass * ((float)destroyed / 40f);
							rigidBody.ApplyPointImpulse(impulse2, cpPosition);
						}
					}
					else if (otherGrid != null && m_grid.GridSizeEnum != otherGrid.GridSizeEnum && !isLargeGrid)
					{
						Vector3 linearVelocity3 = Vector3.Lerp(linearVelocity, otherGrid.Physics.LinearVelocity, MyFakes.DEFORMATION_VELOCITY_RELAY);
						_ = MyFakes.DEFORMATION_LOGGING;
						rigidBody.LinearVelocity = linearVelocity3;
					}
				}
			}
		}

		public bool ApplyDeformation(float deformationOffset, float softAreaPlanar, float softAreaVertical, Vector3 localPos, Vector3 localNormal, MyStringHash damageType, float offsetThreshold = 0f, float lowerRatioLimit = 0f, long attackerId = 0L)
		{
			int blocksDestroyedByThisCp;
			return ApplyDeformation(deformationOffset, softAreaPlanar, softAreaVertical, localPos, localNormal, damageType, out blocksDestroyedByThisCp, offsetThreshold, lowerRatioLimit, attackerId);
		}

		/// <summary>
		/// Applies deformation, returns true when block was destroyed (explosion should be generated)
		/// </summary>
		/// <param name="deformationOffset">Amount of deformation in the localPos</param>        
		/// <param name="softAreaPlanar"></param>
		/// <param name="softAreaVertical"></param>
		/// <param name="localPos"></param>
		/// <param name="localNormal"></param>
		/// <param name="damageType"></param>
		/// <param name="blocksDestroyedByThisCp"></param>
		/// <param name="offsetThreshold">When deformation offset for bone is lower then threshold, it won't move the bone at all or do damage</param>
		/// <param name="lowerRatioLimit"></param>
		/// <param name="attackerId"></param>
		/// <returns></returns>
		public bool ApplyDeformation(float deformationOffset, float softAreaPlanar, float softAreaVertical, Vector3 localPos, Vector3 localNormal, MyStringHash damageType, out int blocksDestroyedByThisCp, float offsetThreshold = 0f, float lowerRatioLimit = 0f, long attackerId = 0L)
		{
			blocksDestroyedByThisCp = 0;
			bool result = false;
			if (!m_grid.BlocksDestructionEnabled)
			{
				return result;
			}
			float num = m_grid.GridSize * 0.7f;
			float num2 = localNormal.AbsMax() * deformationOffset;
			float num3 = 1f - num / num2;
			if (damageType == MyDamageType.Bullet)
			{
				num3 = 0f;
			}
			bool isServer = Sync.IsServer;
			float softAreaPlanarInv = 1f / softAreaPlanar;
			float softAreaVerticalInv = 1f / softAreaVertical;
			Vector3I gridPos = Vector3I.Round((localPos + m_grid.GridSize / 2f) / m_grid.GridSize);
			Vector3D axis = localNormal;
			Vector3 up = MyUtils.GetRandomPerpendicularVector(ref axis);
			Vector3 vector = Vector3.Cross(up, localNormal);
			MyDamageInformation info = new MyDamageInformation(isDeformation: true, 1f, MyDamageType.Deformation, attackerId);
			if (num3 > 0f)
			{
				float num4 = softAreaVertical;
				float num5 = softAreaPlanar - num * softAreaPlanar / num2;
				Vector3 vector2 = up * num5;
				Vector3 vector3 = vector * num5;
				Vector3 vector4 = localPos - vector2 - vector3;
				Vector3 vector5 = localPos - vector2 + vector3;
				Vector3 vector6 = localPos + vector2 - vector3;
				Vector3 vector7 = localPos + vector2 + vector3;
				Vector3 vector8 = localPos + localNormal * num4;
				BoundingBoxI boundingBoxI = BoundingBoxI.CreateInvalid();
				boundingBoxI.Include(Vector3I.Round(vector8 / m_grid.GridSize));
				boundingBoxI.Include(Vector3I.Round(vector4 / m_grid.GridSize));
				boundingBoxI.Include(Vector3I.Round(vector6 / m_grid.GridSize));
				boundingBoxI.Include(Vector3I.Round(vector5 / m_grid.GridSize));
				boundingBoxI.Include(Vector3I.Round(vector7 / m_grid.GridSize));
<<<<<<< HEAD
				if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DEFORMATIONS)
				{
					MatrixD worldMatrix = GetWorldMatrix();
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector8, worldMatrix), Vector3D.Transform(vector4, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector8, worldMatrix), Vector3D.Transform(vector6, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector8, worldMatrix), Vector3D.Transform(vector5, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector8, worldMatrix), Vector3D.Transform(vector7, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector4, worldMatrix), Vector3D.Transform(vector6, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector6, worldMatrix), Vector3D.Transform(vector5, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector5, worldMatrix), Vector3D.Transform(vector7, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3D(Vector3D.Transform(vector7, worldMatrix), Vector3D.Transform(vector4, worldMatrix), Color.Red, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(new BoundingBoxD(boundingBoxI.Min * m_grid.GridSize, boundingBoxI.Max * m_grid.GridSize), worldMatrix), Color.Brown, 1f, depthRead: true, smooth: false, persistent: true);
					MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(localPos, worldMatrix), Vector3D.TransformNormal(up * 50f, worldMatrix), Color.Purple, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
					MyRenderProxy.DebugDrawArrow3DDir(Vector3D.Transform(localPos, worldMatrix), Vector3D.TransformNormal(localNormal * 50f, worldMatrix), Color.Purple, null, depthRead: false, 0.1, null, 0.5f, persistent: true);
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				float val = 1f;
				BoundingBoxI entity = BoundingBoxI.CreateInvalid();
				Vector3I vector3I = Vector3I.Max(boundingBoxI.Min, m_grid.Min);
				Vector3I vector3I2 = Vector3I.Min(boundingBoxI.Max, m_grid.Max);
				Vector3I vector3I3 = default(Vector3I);
				vector3I3.X = vector3I.X;
				while (vector3I3.X <= vector3I2.X)
				{
					vector3I3.Y = vector3I.Y;
					while (vector3I3.Y <= vector3I2.Y)
					{
						vector3I3.Z = vector3I.Z;
						for (; vector3I3.Z <= vector3I2.Z; vector3I3.Z++)
						{
							float num6 = 1f;
							if (vector3I3 != gridPos)
							{
								Vector3 closestCorner = m_grid.GetClosestCorner(vector3I3, localPos);
								num6 = CalculateSoften(softAreaPlanarInv, softAreaVerticalInv, ref localNormal, closestCorner - localPos);
								if (num6 == 0f)
								{
									continue;
								}
							}
							float num7 = num2 * num6;
							if (!(num7 > num))
							{
								continue;
							}
							MySlimBlock cubeBlock = m_grid.GetCubeBlock(vector3I3);
							if (cubeBlock == null)
							{
								continue;
							}
							if (isServer)
							{
								if (cubeBlock.UseDamageSystem)
								{
									info.Amount = 1f;
									MyDamageSystem.Static.RaiseBeforeDamageApplied(cubeBlock, ref info);
									if (info.Amount == 0f)
									{
										continue;
									}
								}
								val = Math.Min(val, cubeBlock.DeformationRatio);
								if (Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio) * num7 > num)
								{
									result = true;
									if (m_removedCubes.TryAdd(cubeBlock, (byte)0))
									{
										_ = MyFakes.DEFORMATION_LOGGING;
										blocksDestroyedByThisCp++;
										entity.Include(ref cubeBlock.Min);
										entity.Include(ref cubeBlock.Max);
									}
									if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DEFORMATIONS)
									{
										MyRenderProxy.DebugDrawSphere(Vector3D.Transform((Vector3D)vector3I3 * (double)m_grid.GridSize, GetWorldMatrix()), 0.5f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
									}
								}
							}
							else
							{
								val = Math.Min(val, cubeBlock.DeformationRatio);
								if (Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio) * num7 > num)
								{
									result = true;
									blocksDestroyedByThisCp++;
								}
							}
						}
						vector3I3.Y++;
					}
					vector3I3.X++;
				}
				if (blocksDestroyedByThisCp > 0)
				{
					if (isServer)
					{
						entity.Inflate(1);
						m_dirtyCubesInfo.DirtyParts.Add(entity);
						ScheduleRemoveBlocksCallbacks();
						m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
					}
				}
				else
				{
					val = Math.Max(val, 0.2f);
					softAreaPlanar *= val;
					softAreaVertical *= val;
				}
			}
			if (blocksDestroyedByThisCp == 0 && MySession.Static.HighSimulationQuality)
			{
				Parallel.Start(delegate
				{
					DeformBones(deformationOffset, gridPos, softAreaPlanar, softAreaVertical, localNormal, localPos, damageType, offsetThreshold, lowerRatioLimit, attackerId, up);
				}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.Deformations, "DeformBones"));
			}
			return result;
		}

		private void ScheduleRemoveBlocksCallbacks()
		{
			if (Interlocked.Exchange(ref m_removeBlocksCallbackScheduled, 1) != 0)
			{
				return;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				m_removeBlocksCallbackScheduled = 0;
				bool flag = true;
				while (flag)
				{
					flag = false;
					foreach (KeyValuePair<MySlimBlock, byte> removedCube in m_removedCubes)
					{
						flag = true;
						MySlimBlock key = removedCube.Key;
<<<<<<< HEAD
						m_removedCubes.Remove(key);
=======
						m_removedCubes.Remove<MySlimBlock, byte>(key);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (!key.IsDestroyed)
						{
							key.CubeGrid.RemoveDestroyedBlock(key, 0L);
						}
					}
				}
			}, "ApplyDeformation/RemoveDestroyedBlock");
		}

		private void DeformBones(float deformationOffset, Vector3I gridPos, float softAreaPlanar, float softAreaVertical, Vector3 localNormal, Vector3 localPos, MyStringHash damageType, float offsetThreshold, float lowerRatioLimit, long attackerId, Vector3 up)
		{
			if (!MySession.Static.Ready)
			{
				return;
			}
			if (m_tmpCubeList == null)
			{
				m_tmpCubeList = new List<Vector3I>(8);
			}
			float softAreaVerticalInv = 1f / softAreaVertical;
			float softAreaPlanarInv = 1f / softAreaPlanar;
			float num = 1f / m_grid.GridSize;
			float num2 = m_grid.GridSize * 0.5f;
			Vector3I vector3I = Vector3I.Round((localPos + new Vector3(m_grid.GridSizeHalf)) / num2) - gridPos * 2;
			float num3 = softAreaPlanar * num * 2f;
			float z = softAreaVertical * num * 2f;
			BoundingBox aABB = new MyOrientedBoundingBox(gridPos * 2 + vector3I, new Vector3(num3, num3, z), Quaternion.CreateFromForwardUp(localNormal, up)).GetAABB();
			Vector3I value = Vector3I.Floor((Vector3I.Floor(aABB.Min) - Vector3I.One) * 0.5f);
			Vector3I value2 = Vector3I.Ceiling((Vector3I.Ceiling(aABB.Max) - Vector3I.One) * 0.5f);
			value = Vector3I.Max(value, m_grid.Min);
			value2 = Vector3I.Min(value2, m_grid.Max);
			bool isServer = Sync.IsServer;
			Vector3I vector3I2 = gridPos * 2;
			Vector3 vector = new Vector3(m_grid.GridSize * 0.25f);
			float num4 = m_grid.GridSize * 0.7f;
			offsetThreshold /= (float)((m_grid.GridSizeEnum == MyCubeSize.Large) ? 1 : 5);
			bool flag = false;
			HashSet<Vector3I> dirtyCubes = m_dirtyCubesPool.Get();
			Dictionary<MySlimBlock, float> damagedCubes = m_damagedCubesPool.Get();
			MyDamageInformation damageInfo = new MyDamageInformation(isDeformation: true, 1f, MyDamageType.Deformation, attackerId);
			value = Vector3I.Max(value, m_grid.Min);
			value2 = Vector3I.Min(value2, m_grid.Max);
			Vector3I cube = default(Vector3I);
			cube.X = value.X;
			while (cube.X <= value2.X)
			{
				cube.Y = value.Y;
				while (cube.Y <= value2.Y)
				{
					cube.Z = value.Z;
					while (cube.Z <= value2.Z)
					{
						MySlimBlock existingCubeForBoneDeformations = m_grid.GetExistingCubeForBoneDeformations(ref cube, ref damageInfo);
						if (existingCubeForBoneDeformations != null && !existingCubeForBoneDeformations.IsDestroyed && !m_removedCubes.ContainsKey(existingCubeForBoneDeformations))
						{
							bool flag2 = true;
							for (int i = 0; i < MyGridSkeleton.BoneOffsets.Length; i++)
							{
								Vector3I vector3I3 = MyGridSkeleton.BoneOffsets[i];
								Vector3I pos = cube * 2 + vector3I3;
								Vector3 vector2 = pos * m_grid.GridSize * 0.5f - vector;
								m_grid.Skeleton.GetBone(ref pos, out var bone);
								float num5 = CalculateSoften(softAreaPlanarInv, softAreaVerticalInv, ref localNormal, bone + vector2 - localPos);
								if (num5 == 0f)
								{
									if (flag2 && i >= 7)
									{
										break;
									}
									continue;
								}
								flag2 = false;
								float num6 = Math.Max(lowerRatioLimit, existingCubeForBoneDeformations.DeformationRatio);
								if (deformationOffset * num6 < offsetThreshold)
								{
									continue;
								}
								float num7 = deformationOffset * num5;
								float num8 = num7 * num6;
								Vector3 vector3 = localNormal * num8;
								bone += vector3;
								float num9 = bone.AbsMax();
								if (((!(damageType != MyDamageType.Bullet) || !(damageType != MyDamageType.Drill)) && !(num9 < num4)) || !(num8 > 0.05f))
								{
									continue;
								}
								Vector3I vector3I4 = pos - vector3I2;
								if (num9 > num4)
								{
									m_tmpCubeList.Clear();
									Vector3I boneOffset = vector3I4;
									Vector3I cube2 = gridPos;
									m_grid.Skeleton.Wrap(ref cube2, ref boneOffset);
									m_grid.Skeleton.GetAffectedCubes(cube2, boneOffset, m_tmpCubeList, m_grid);
									bool flag3 = true;
									foreach (Vector3I tmpCube in m_tmpCubeList)
									{
										MySlimBlock cubeBlock = m_grid.GetCubeBlock(tmpCube);
										if (cubeBlock != null && !cubeBlock.IsDestroyed)
										{
											num6 = Math.Max(lowerRatioLimit, cubeBlock.DeformationRatio);
											flag3 &= num7 * num6 > num4 && cubeBlock.UsesDeformation;
										}
									}
									if (!flag3)
<<<<<<< HEAD
									{
										continue;
									}
									foreach (Vector3I tmpCube2 in m_tmpCubeList)
									{
=======
									{
										continue;
									}
									foreach (Vector3I tmpCube2 in m_tmpCubeList)
									{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
										MySlimBlock cubeBlock2 = m_grid.GetCubeBlock(tmpCube2);
										if (cubeBlock2 != null && !cubeBlock2.IsDestroyed)
										{
											_ = MyFakes.DEFORMATION_LOGGING;
											if (isServer)
<<<<<<< HEAD
											{
												flag = true;
												m_removedCubes.TryAdd(cubeBlock2, 0);
												damagedCubes.Remove(cubeBlock2);
											}
											else
											{
=======
											{
												flag = true;
												m_removedCubes.TryAdd(cubeBlock2, (byte)0);
												damagedCubes.Remove(cubeBlock2);
											}
											else
											{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
												AddDirtyBlock(cubeBlock2);
											}
										}
									}
									continue;
								}
								dirtyCubes.Add(cube);
								if (!isServer)
								{
									continue;
								}
								m_grid.Skeleton.SetBone(ref pos, ref bone);
								m_grid.AddDirtyBone(gridPos, vector3I4);
								m_grid.AddBoneToSend(pos);
								if (damageType != MyDamageType.Bullet)
								{
									_ = ((IMyDestroyableObject)existingCubeForBoneDeformations).Integrity / existingCubeForBoneDeformations.MaxIntegrity;
									float num10 = 1f - num9 / num4;
									float num11 = existingCubeForBoneDeformations.MaxIntegrity * (1f - num10) - existingCubeForBoneDeformations.CurrentDamage;
									if (num11 > 0f)
									{
										damagedCubes.TryGetValue(existingCubeForBoneDeformations, out var value3);
										value3 = Math.Max(num11, value3);
										damagedCubes[existingCubeForBoneDeformations] = value3;
									}
								}
							}
						}
						cube.Z++;
					}
					cube.Y++;
				}
				cube.X++;
			}
			if (flag)
			{
				ScheduleRemoveBlocksCallbacks();
			}
			MySandboxGame.Static.Invoke(delegate
			{
				bool flag4 = true;
				if (MySession.Static.Ready)
				{
<<<<<<< HEAD
					m_dirtyCubesInfo.DirtyBlocks.UnionWith(dirtyCubes);
=======
					m_dirtyCubesInfo.DirtyBlocks.UnionWith((IEnumerable<Vector3I>)dirtyCubes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
					foreach (KeyValuePair<MySlimBlock, float> item in damagedCubes)
					{
						if (!item.Key.IsDestroyed && !item.Key.CubeGrid.Closed)
						{
<<<<<<< HEAD
							((IMyDestroyableObject)item.Key).DoDamage(item.Value, damageType, sync: false, (MyHitInfo?)null, attackerId, 0L, shouldDetonateAmmo: true);
=======
							((IMyDestroyableObject)item.Key).DoDamage(item.Value, damageType, sync: false, (MyHitInfo?)null, attackerId, 0L);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					if (isServer)
					{
						flag4 = false;
						Parallel.Start(delegate
						{
							MySlimBlock.SendDamageBatch(damagedCubes, damageType, attackerId);
							m_damagedCubesPool.Return(damagedCubes);
						});
					}
				}
				m_dirtyCubesPool.Return(dirtyCubes);
				if (flag4)
				{
					m_damagedCubesPool.Return(damagedCubes);
				}
			}, "DeformBones");
		}

		/// <summary>
		/// Calculates soft coefficient at target point
		/// </summary>
		private static float CalculateSoften(float softAreaPlanarInv, float softAreaVerticalInv, ref Vector3 normal, Vector3 contactToTarget)
		{
			Vector3.Dot(ref normal, ref contactToTarget, out var result);
			if (result < 0f)
			{
				result = 0f - result;
			}
			float num = 1f - result * softAreaVerticalInv;
			if (num <= 0f)
			{
				return 0f;
			}
			float num2 = contactToTarget.LengthSquared() - result * result;
			if (num2 <= 0f)
			{
				return num;
			}
			float num3 = (float)Math.Sqrt(num2);
			float num4 = 1f - num3 * softAreaPlanarInv;
			if (num4 <= 0f)
			{
				return 0f;
			}
			return num * num4;
		}

		private void AddGridCollisionEffect(Vector3D relativePosition, Vector3 normal, Vector3 relativeVelocity, float separatingSpeed, float impulse)
		{
			if (!MyFakes.ENABLE_COLLISION_EFFECTS || m_gridEffects.Count >= MaxEffectsPerFrame)
			{
				return;
			}
			m_gridCollisionEffects.Enqueue(new GridCollisionHit
			{
				RelativePosition = relativePosition,
				Normal = normal,
				RelativeVelocity = relativeVelocity,
				SeparatingSpeed = separatingSpeed,
				Impulse = impulse
			});
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_gridCollisionEffects.Count > 0)
				{
					m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, ProcessGridCollisionEffects, 22);
				}
			}, "AddGridCollisionEffect");
		}

		private void AddCollisionEffect(Vector3D position, Vector3 normal, float separatingSpeed, float impulse)
		{
			if (!MyFakes.ENABLE_COLLISION_EFFECTS || m_gridEffects.Count >= MaxEffectsPerFrame)
			{
				return;
			}
			m_gridEffects.Enqueue(new GridEffect
			{
				Type = GridEffectType.Collision,
				Position = position,
				Normal = normal,
				Scale = 1f,
				SeparatingSpeed = separatingSpeed,
				Impulse = impulse
			});
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_gridEffects.Count > 0)
				{
					m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, ProcessGridEffects, 22);
				}
			}, "AddCollisionEffect");
		}

		private void AddDustEffect(Vector3D position, float scale)
		{
			if (m_gridEffects.Count >= MaxEffectsPerFrame)
			{
				return;
			}
			m_gridEffects.Enqueue(new GridEffect
			{
				Type = GridEffectType.Dust,
				Position = position,
				Normal = Vector3.Forward,
				Scale = scale
			});
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_gridEffects.Count > 0)
				{
					m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, ProcessGridEffects, 22);
				}
			}, "AddDustEffect");
		}

		private void AddDestructionEffect(Vector3D position, Vector3 direction)
		{
			if (!MyFakes.ENABLE_DESTRUCTION_EFFECTS || m_gridEffects.Count >= MaxEffectsPerFrame)
			{
				return;
			}
			m_gridEffects.Enqueue(new GridEffect
			{
				Type = GridEffectType.Destruction,
				Position = position,
				Normal = direction,
				Scale = 1f
			});
			MySandboxGame.Static.Invoke(delegate
			{
				if (m_gridEffects.Count > 0)
				{
					m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, ProcessGridEffects, 22);
				}
			}, "AddDestructionEffect");
		}

		public static float GetCollisionSparkMultiplier(float separatingVelocity, bool isLargeGrid)
		{
			float num = 0.1f;
			float num2 = 2f;
			float num3 = 110f;
			float num4 = separatingVelocity / num3;
			float num5 = num4 * num2 + (1f - num4) * num;
			if (isLargeGrid)
			{
				return num5 * 2f;
			}
			return num5;
		}

		public static float GetCollisionSparkScale(float impulseApplied, bool isLargeGrid)
		{
			if (impulseApplied < 1000f)
			{
				return 0.05f + 5E-05f * impulseApplied;
			}
			if (impulseApplied < 60000f)
			{
				return 0.1f + 9E-06f * (impulseApplied - 1000f);
			}
			if (impulseApplied < 1000000f)
			{
				return 0.63f + 7E-07f * (impulseApplied - 60000f);
			}
			return 1.3f;
		}

		private void CreateGridCollisionEffect(GridCollisionHit e)
		{
			double num = 0.5;
			for (int i = 0; i < m_collisionParticles.Count; i++)
			{
				CollisionParticleEffect collisionParticleEffect = m_collisionParticles[i];
				if (Vector3D.DistanceSquared(collisionParticleEffect.RelativePosition, e.RelativePosition) < num)
				{
					if (collisionParticleEffect.RemainingTime < 20 || collisionParticleEffect.Impulse < e.Impulse)
					{
						collisionParticleEffect.RelativePosition = e.RelativePosition;
						collisionParticleEffect.SeparatingVelocity = e.RelativeVelocity;
						collisionParticleEffect.Normal = e.Normal;
						collisionParticleEffect.RemainingTime = 20;
						collisionParticleEffect.Impulse = e.Impulse;
					}
					return;
				}
			}
			if (m_collisionParticles.Count < 3)
			{
				m_collisionParticles.Add(new CollisionParticleEffect
				{
					Effect = null,
					RemainingTime = 20,
					Normal = e.Normal,
					SeparatingVelocity = e.RelativeVelocity,
					RelativePosition = e.RelativePosition,
					Impulse = e.Impulse
				});
				if (m_collisionParticles.Count == 1)
				{
					m_grid.Schedule(MyCubeGrid.UpdateQueue.AfterSimulation, UpdateCollisionParticleEffects, 22);
				}
			}
		}

		private void CreateEffect(GridEffect e)
		{
			Vector3D worldPosition = e.Position;
			Vector3 normal = e.Normal;
			float scale = e.Scale;
			MyParticleEffect effect;
			switch (e.Type)
			{
			case GridEffectType.Collision:
			{
				float num = (float)Vector3D.DistanceSquared(MySector.MainCamera.Position, worldPosition);
				scale = MyPerGameSettings.CollisionParticle.Scale;
				float collisionSparkMultiplier = GetCollisionSparkMultiplier(e.SeparatingSpeed, m_grid.GridSizeEnum == MyCubeSize.Large);
				float num2 = 0.5f * GetCollisionSparkScale(e.Impulse, m_grid.GridSizeEnum == MyCubeSize.Large);
				string effectName = ((m_grid.GridSizeEnum != 0) ? MyPerGameSettings.CollisionParticle.SmallGridClose : ((num > MyPerGameSettings.CollisionParticle.CloseDistanceSq) ? MyPerGameSettings.CollisionParticle.LargeGridDistant : MyPerGameSettings.CollisionParticle.LargeGridClose));
				MatrixD matrixD = MatrixD.CreateFromDir(normal);
				MatrixD effectMatrix = MatrixD.CreateWorld(worldPosition, matrixD.Forward, matrixD.Up);
				if (MyParticlesManager.TryCreateParticleEffect(effectName, ref effectMatrix, ref worldPosition, uint.MaxValue, out effect))
				{
					effect.UserScale = scale * num2;
					effect.UserBirthMultiplier = collisionSparkMultiplier;
				}
				break;
			}
			case GridEffectType.Destruction:
				scale = MyPerGameSettings.DestructionParticle.Scale;
				if (m_grid.GridSizeEnum != 0)
				{
					scale = 0.05f;
				}
				MySyncDestructions.AddDestructionEffect(MyPerGameSettings.DestructionParticle.DestructionSmokeLarge, worldPosition, normal, scale);
				break;
			case GridEffectType.Dust:
			{
				MatrixD effectMatrix = MatrixD.CreateTranslation(worldPosition);
				if (MyParticlesManager.TryCreateParticleEffect("PlanetCrashDust", ref effectMatrix, ref worldPosition, uint.MaxValue, out effect))
				{
					effect.UserScale = scale;
				}
				break;
			}
			}
		}

		public static void CreateDestructionEffect(string effectName, Vector3D position, Vector3 direction, float scale)
		{
			MatrixD matrixD = MatrixD.CreateFromDir(direction);
<<<<<<< HEAD
			MatrixD effectMatrix = MatrixD.CreateWorld(position, matrixD.Forward, matrixD.Up);
			if (MyParticlesManager.TryCreateParticleEffect(effectName, ref effectMatrix, ref position, uint.MaxValue, out var effect))
=======
			if (MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(position, matrixD.Forward, matrixD.Up), out var effect))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				effect.UserScale = scale;
			}
		}

		private void ProcessGridEffects()
		{
			while (m_gridEffects.Count > 0)
			{
				CreateEffect(m_gridEffects.Dequeue());
			}
		}

		private void ProcessGridCollisionEffects()
		{
			while (m_gridCollisionEffects.Count > 0)
			{
				CreateGridCollisionEffect(m_gridCollisionEffects.Dequeue());
			}
		}

		public void UpdateCollisionParticleEffects()
		{
			int num = 0;
			while (num < m_collisionParticles.Count)
			{
				CollisionParticleEffect effect = m_collisionParticles[num];
				UpdateCollisionParticleEffect(ref effect);
				if (effect.RemainingTime < 0)
				{
					FinalizeCollisionParticleEffect(ref effect);
					m_collisionParticles.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			if (m_collisionParticles.Count == 0)
			{
				m_grid.DeSchedule(MyCubeGrid.UpdateQueue.AfterSimulation, UpdateCollisionParticleEffects);
			}
		}

		private float ComputeDirecionalSparkMultiplier(float speed)
		{
			float num = 1f;
			float num2 = 10f;
			float num3 = speed / 110f;
			return num3 * num2 + (1f - num3) * num;
		}

		private float ComputeDirecionalSparkScale(float impulse)
		{
			if (impulse < 1000f)
			{
				return 1f;
			}
			if (impulse < 10000f)
			{
				return 1f + 5E-05f * (impulse - 1000f);
			}
			if (impulse < 100000f)
			{
				return 1.45f + 1.5E-05f * (impulse - 10000f);
			}
			return 2.8f;
		}

		private void UpdateCollisionParticleEffect(ref CollisionParticleEffect effect, bool countDown = true)
		{
			if (effect.RemainingTime >= 20)
			{
				float num = effect.SeparatingVelocity.Length();
				Vector3 vector = effect.SeparatingVelocity / num;
				Vector3 forward = -vector + 1.1f * Vector3.Dot(vector, effect.Normal) * effect.Normal;
				if (effect.Effect == null)
				{
					MatrixD effectMatrix = MatrixD.CreateWorld(effect.RelativePosition + RigidBody.Position, forward, effect.Normal);
					Vector3D worldPosition = effectMatrix.Translation;
					MyParticlesManager.TryCreateParticleEffect("Collision_Sparks_Directional", ref effectMatrix, ref worldPosition, m_grid.Render.ParentIDs[0], out effect.Effect);
				}
				if (effect.Effect != null && RigidBody != null)
				{
					effect.Effect.WorldMatrix = MatrixD.CreateWorld(effect.RelativePosition + RigidBody.Position, forward, effect.Normal);
					effect.Effect.UserBirthMultiplier = ComputeDirecionalSparkMultiplier(num);
					effect.Effect.UserScale = 0.5f * ComputeDirecionalSparkScale(effect.Impulse);
				}
			}
			else if (effect.Effect != null && RigidBody != null)
			{
				Vector3D trans = effect.RelativePosition + RigidBody.Position;
				effect.Effect.SetTranslation(ref trans);
			}
			if (countDown)
			{
				effect.RemainingTime--;
			}
		}

		private void FinalizeCollisionParticleEffect(ref CollisionParticleEffect effect)
		{
			if (effect.Effect != null)
			{
				effect.Effect.Stop(instant: false);
			}
		}

		public override MyStringHash GetMaterialAt(Vector3D worldPos)
		{
			Vector3D fractionalGridPosition = Vector3D.Transform(worldPos, m_grid.PositionComp.WorldMatrixNormalizedInv) / m_grid.GridSize;
			m_grid.FixTargetCubeLite(out var cube, fractionalGridPosition);
			MySlimBlock mySlimBlock = m_grid.GetCubeBlock(cube);
			if (mySlimBlock == null)
			{
				return base.GetMaterialAt(worldPos);
			}
			if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
			{
<<<<<<< HEAD
				mySlimBlock = ((MyCompoundCubeBlock)mySlimBlock.FatBlock).GetBlocks()[0];
=======
				Vector3D trans = effect.RelativePosition + RigidBody.Position;
				effect.Effect.SetTranslation(ref trans);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyStringHash subtypeId = mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
			if (!(subtypeId != MyStringHash.NullOrEmpty))
			{
				return base.GetMaterialAt(worldPos);
			}
			return subtypeId;
		}

		/// <summary>
		/// For large continuous areas use AddDirtyArea, internal processing is much faster because taking advantage of aabb tests
		/// </summary>
		/// <param name="block"></param>
		public void AddDirtyBlock(MySlimBlock block)
		{
			m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI
			{
				Min = block.Min,
				Max = block.Max
			});
			m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
		}

<<<<<<< HEAD
		public void AddDirtyArea(Vector3I min, Vector3I max)
		{
			m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI
			{
				Min = min,
				Max = max
			});
			m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
		}

		public bool IsDirty()
		{
			if (m_dirtyCubesInfo.DirtyBlocks.Count <= 0)
			{
				return !m_dirtyCubesInfo.DirtyParts.IsEmpty;
			}
			return true;
		}

		public List<HkShape> GetShapesFromPosition(Vector3I pos)
		{
			return m_shape.GetShapesFromPosition(pos);
		}

		public void UpdateShape()
		{
			if (!m_grid.CanHavePhysics())
			{
=======
		public override MyStringHash GetMaterialAt(Vector3D worldPos)
		{
			Vector3D fractionalGridPosition = Vector3.Transform(worldPos, m_grid.PositionComp.WorldMatrixNormalizedInv) / m_grid.GridSize;
			m_grid.FixTargetCubeLite(out var cube, fractionalGridPosition);
			MySlimBlock mySlimBlock = m_grid.GetCubeBlock(cube);
			if (mySlimBlock == null)
			{
				return base.GetMaterialAt(worldPos);
			}
			if (mySlimBlock.FatBlock is MyCompoundCubeBlock)
			{
				mySlimBlock = ((MyCompoundCubeBlock)mySlimBlock.FatBlock).GetBlocks()[0];
			}
			MyStringHash subtypeId = mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeId;
			if (!(subtypeId != MyStringHash.NullOrEmpty))
			{
				return base.GetMaterialAt(worldPos);
			}
			return subtypeId;
		}

		public void AddDirtyBlock(MySlimBlock block)
		{
			m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI
			{
				Min = block.Min,
				Max = block.Max
			});
			m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
		}

		public void AddDirtyArea(Vector3I min, Vector3I max)
		{
			m_dirtyCubesInfo.DirtyParts.Add(new BoundingBoxI
			{
				Min = min,
				Max = max
			});
			m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
		}

		public bool IsDirty()
		{
			if (m_dirtyCubesInfo.DirtyBlocks.get_Count() <= 0)
			{
				return !m_dirtyCubesInfo.DirtyParts.IsEmpty;
			}
			return true;
		}

		public List<HkShape> GetShapesFromPosition(Vector3I pos)
		{
			return m_shape.GetShapesFromPosition(pos);
		}

		public void UpdateShape()
		{
			if (!m_grid.CanHavePhysics())
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			HashSet<Vector3I> dirtyBlocks = m_dirtyCubesInfo.DirtyBlocks;
			m_dirtyCubesInfo.DirtyParts.ApplyChanges();
			BoundingBox dirtyBox = BoundingBox.CreateInvalid();
			bool dirtyParts = m_dirtyCubesInfo.DirtyParts.Count > 0;
			Vector3I vector3I = default(Vector3I);
			foreach (BoundingBoxI dirtyPart in m_dirtyCubesInfo.DirtyParts)
			{
				vector3I.X = dirtyPart.Min.X;
				while (vector3I.X <= dirtyPart.Max.X)
				{
					vector3I.Y = dirtyPart.Min.Y;
					while (vector3I.Y <= dirtyPart.Max.Y)
					{
						vector3I.Z = dirtyPart.Min.Z;
						while (vector3I.Z <= dirtyPart.Max.Z)
						{
							dirtyBlocks.Add(vector3I);
							dirtyBox = dirtyBox.Include(vector3I * m_grid.GridSize);
							vector3I.Z++;
						}
						vector3I.Y++;
					}
					vector3I.X++;
				}
			}
<<<<<<< HEAD
			bool flag = dirtyBlocks.Count > 0;
=======
			bool flag = dirtyBlocks.get_Count() > 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (flag)
			{
				UpdateContactCallbackLimit();
			}
			if (!m_recreateBody)
			{
				if (flag)
				{
					if (RigidBody.IsActive && !base.HavokWorld.ActiveRigidBodies.Contains(RigidBody))
					{
						base.HavokWorld.RigidBodyActivated(RigidBody);
					}
					m_shape.UnmarkBreakable((base.WeldedRigidBody != null) ? base.WeldedRigidBody : RigidBody);
					m_shape.RefreshBlocks((base.WeldedRigidBody != null) ? base.WeldedRigidBody : RigidBody, dirtyBlocks);
					if (MyEntities.IsAsyncUpdateInProgress)
					{
						MyEntities.InvokeLater(delegate
						{
							OnRefreshComplete();
							ActivateInArea();
						});
					}
					else
					{
						OnRefreshComplete();
						ActivateInArea();
					}
				}
			}
			else
			{
				RecreateBreakableBody(dirtyBlocks);
				m_recreateBody = false;
				m_grid.RaisePhysicsChanged();
			}
			m_dirtyCubesInfo.Clear();
			void ActivateInArea()
			{
				if (dirtyParts)
				{
					dirtyBox.Inflate(0.5f + m_grid.GridSize);
					BoundingBoxD box = dirtyBox.Transform(m_grid.WorldMatrix);
					MyPhysics.ActivateInBox(ref box);
				}
			}
		}

		private void EnqueueExplosion(in ExplosionInfo info)
		{
			m_explosions.Add(info);
			if (m_explosions.Count == 1)
			{
				m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateExplosions, 22);
			}
		}

		private void UpdateExplosions()
		{
			m_debrisPerFrame = 0;
			if (m_explosions.Count <= 0)
			{
				return;
			}
			if (Sync.IsServer)
			{
				m_grid.PerformCutouts(m_explosions);
				float num = m_grid.Physics.LinearVelocity.Length();
				foreach (ExplosionInfo explosion in m_explosions)
				{
					if (num > 0f && explosion.GenerateDebris && m_debrisPerFrame < 3)
					{
						MyDebris.Static.CreateDirectedDebris(explosion.Position, m_grid.Physics.LinearVelocity / num, m_grid.GridSize, m_grid.GridSize * 1.5f, 0f, (float)Math.E * 449f / 777f, 6, num);
						m_debrisPerFrame++;
					}
				}
			}
			m_explosions.Clear();
		}

		private void OnRefreshComplete()
		{
			m_shape.MarkBreakable((base.WeldedRigidBody != null) ? base.WeldedRigidBody : RigidBody);
			m_grid.SetInventoryMassDirty();
			m_shape.SetMass((base.WeldedRigidBody != null) ? base.WeldedRigidBody : RigidBody);
			m_shape.UpdateShape((base.WeldedRigidBody != null) ? base.WeldedRigidBody : RigidBody, (base.WeldedRigidBody != null) ? null : RigidBody2, BreakableBody);
			MyGridPhysicalHierarchy.Static.UpdateRoot(m_grid);
			m_grid.RaisePhysicsChanged();
		}

		public void UpdateMass()
		{
			if (RigidBody.GetMotionType() != HkMotionType.Keyframed)
			{
				float mass = RigidBody.Mass;
				m_shape.RefreshMass();
				if (RigidBody.Mass != mass && !RigidBody.IsActive)
				{
					RigidBody.Activate();
				}
				m_grid.RaisePhysicsChanged();
				MyGridPhysicalHierarchy.Static.UpdateRoot(m_grid);
			}
		}

		public void AddBlock(MySlimBlock block)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = block.Min.X;
			while (vector3I.X <= block.Max.X)
			{
				vector3I.Y = block.Min.Y;
				while (vector3I.Y <= block.Max.Y)
				{
					vector3I.Z = block.Min.Z;
					while (vector3I.Z <= block.Max.Z)
					{
						m_dirtyCubesInfo.DirtyBlocks.Add(vector3I);
						vector3I.Z++;
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, UpdateShape, 24, parallel: true);
		}

		protected override void CreateBody(ref HkShape shape, HkMassProperties? massProperties)
		{
			if (MyPerGameSettings.Destruction)
			{
				shape = CreateBreakableBody(shape, massProperties);
				return;
			}
			HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
			hkRigidBodyCinfo.AngularDamping = m_angularDamping;
			hkRigidBodyCinfo.LinearDamping = m_linearDamping;
			hkRigidBodyCinfo.Shape = shape;
			hkRigidBodyCinfo.SolverDeactivation = InitialSolverDeactivation;
			hkRigidBodyCinfo.ContactPointCallbackDelay = ContactPointDelay;
			if (massProperties.HasValue)
			{
				hkRigidBodyCinfo.SetMassProperties(massProperties.Value);
			}
			MyPhysicsBody.GetInfoFromFlags(hkRigidBodyCinfo, Flags);
			if (m_grid.IsStatic)
			{
				hkRigidBodyCinfo.MotionType = HkMotionType.Dynamic;
				hkRigidBodyCinfo.QualityType = HkCollidableQualityType.Moving;
			}
			RigidBody = new HkRigidBody(hkRigidBodyCinfo);
			if (m_grid.IsStatic)
			{
				RigidBody.UpdateMotionType(HkMotionType.Fixed);
			}
		}

		private static void DisconnectBlock(MySlimBlock a)
		{
			a.DisconnectFaces.Add(Vector3I.Left);
			a.DisconnectFaces.Add(Vector3I.Right);
			a.DisconnectFaces.Add(Vector3I.Forward);
			a.DisconnectFaces.Add(Vector3I.Backward);
			a.DisconnectFaces.Add(Vector3I.Up);
			a.DisconnectFaces.Add(Vector3I.Down);
		}

		private void AddFaces(MySlimBlock a, Vector3I ab)
		{
			if (!a.DisconnectFaces.Contains(ab * Vector3I.UnitX))
			{
				a.DisconnectFaces.Add(ab * Vector3I.UnitX);
			}
			if (!a.DisconnectFaces.Contains(ab * Vector3I.UnitY))
			{
				a.DisconnectFaces.Add(ab * Vector3I.UnitY);
			}
			if (!a.DisconnectFaces.Contains(ab * Vector3I.UnitZ))
			{
				a.DisconnectFaces.Add(ab * Vector3I.UnitZ);
			}
		}

		public override void DebugDraw()
		{
			if (MyDebugDrawSettings.BREAKABLE_SHAPE_CONNECTIONS && BreakableBody != null)
			{
				MySlimBlock mySlimBlock = null;
				List<MyPhysics.HitInfo> list = new List<MyPhysics.HitInfo>();
				MyPhysics.CastRay(MySector.MainCamera.Position, MySector.MainCamera.Position + MySector.MainCamera.ForwardVector * 25f, list, 30);
				foreach (MyPhysics.HitInfo item in list)
				{
					if (item.HkHitInfo.GetHitEntity() is MyCubeGrid)
					{
						MyCubeGrid obj = item.HkHitInfo.GetHitEntity() as MyCubeGrid;
						mySlimBlock = obj.GetCubeBlock(obj.WorldToGridInteger(item.Position + MySector.MainCamera.ForwardVector * 0.2f));
						break;
					}
				}
				int num = 0;
				List<HkdConnection> list2 = new List<HkdConnection>();
				BreakableBody.BreakableShape.GetConnectionList(list2);
				foreach (HkdConnection item2 in list2)
				{
					Vector3D vector3D = ClusterToWorld(Vector3.Transform(item2.PivotA, RigidBody.GetRigidBodyMatrix()));
					Vector3D vector3D2 = ClusterToWorld(Vector3.Transform(item2.PivotB, RigidBody.GetRigidBodyMatrix()));
					if (mySlimBlock != null && mySlimBlock.CubeGrid.WorldToGridInteger(vector3D) == mySlimBlock.Position)
					{
						vector3D += (vector3D2 - vector3D) * 0.05000000074505806;
						MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, Color.Red, Color.Blue, depthRead: false);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.075f, Color.White, 1f, depthRead: false);
					}
					if (mySlimBlock != null && mySlimBlock.CubeGrid.WorldToGridInteger(vector3D2) == mySlimBlock.Position)
					{
						vector3D += Vector3.One * 0.02f;
						vector3D2 += Vector3.One * 0.02f;
						MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, Color.Red, Color.Green, depthRead: false);
						MyRenderProxy.DebugDrawSphere(vector3D2, 0.025f, Color.Green, 1f, depthRead: false);
					}
					if (num > 1000)
					{
						break;
					}
				}
			}
			Shape.DebugDraw();
			base.DebugDraw();
		}

		protected override void OnWelded(MyPhysicsBody other)
		{
			base.OnWelded(other);
			Shape.RefreshMass();
			if (m_grid.BlocksDestructionEnabled)
			{
				if (base.HavokWorld != null)
				{
					base.HavokWorld.BreakOffPartsUtil.MarkEntityBreakable(RigidBody, Shape.BreakImpulse);
				}
				if (Sync.IsServer)
				{
					if (RigidBody.BreakLogicHandler == null)
					{
						RigidBody.BreakLogicHandler = BreakLogicHandler;
					}
					if (RigidBody.BreakPartsHandler == null)
					{
						RigidBody.BreakPartsHandler = BreakPartsHandler;
					}
				}
			}
			m_grid.HavokSystemIDChanged(other.HavokCollisionSystemID);
		}

		protected override void OnUnwelded(MyPhysicsBody other)
		{
			base.OnUnwelded(other);
			Shape.RefreshMass();
			m_grid.HavokSystemIDChanged(HavokCollisionSystemID);
			if (!m_grid.IsStatic)
			{
				m_grid.RecalculateGravity();
			}
		}

		/// <summary>
		/// Checks if the last block in grid has fracture component and converts it into fracture pieces.
		/// </summary>
		/// <param name="doubledKinematic"></param>
		/// <param name="isPredicted"></param>
		public void ConvertToDynamic(bool doubledKinematic, bool isPredicted)
		{
			if (RigidBody == null || base.Entity == null || base.Entity.Closed || base.HavokWorld == null)
			{
				return;
			}
			Flags = (doubledKinematic ? RigidBodyFlag.RBF_DOUBLED_KINEMATIC : RigidBodyFlag.RBF_DEFAULT);
			bool flag = true;
			if (base.IsWelded || base.WeldInfo.Children.get_Count() > 0)
			{
				if (base.WeldedRigidBody != null && base.WeldedRigidBody.Quality == HkCollidableQualityType.Fixed)
				{
					base.WeldedRigidBody.UpdateMotionType(HkMotionType.Dynamic);
					base.WeldedRigidBody.Quality = HkCollidableQualityType.Moving;
					if (doubledKinematic && !MyPerGameSettings.Destruction)
					{
						base.WeldedRigidBody.Layer = 16;
					}
					else
					{
						base.WeldedRigidBody.Layer = 15;
					}
				}
				MyWeldingGroups.Static.GetGroup((MyEntity)base.Entity).GroupData.UpdateParent((MyEntity)((base.WeldInfo.Parent != null) ? base.WeldInfo.Parent.Entity : base.Entity));
				flag &= base.WeldInfo.Parent == null;
			}
			if (flag)
			{
				HkMotionType hkMotionType = ((Sync.IsServer || isPredicted) ? HkMotionType.Dynamic : HkMotionType.Fixed);
				if (hkMotionType != RigidBody.GetMotionType())
				{
					NotifyConstraintsRemovedFromWorld();
					RigidBody.UpdateMotionType(hkMotionType);
					NotifyConstraintsAddedToWorld();
				}
				RigidBody.Quality = HkCollidableQualityType.Moving;
				if (doubledKinematic && !MyPerGameSettings.Destruction)
				{
					Flags = RigidBodyFlag.RBF_DOUBLED_KINEMATIC;
					RigidBody.Layer = 16;
				}
				else
				{
					Flags = RigidBodyFlag.RBF_DEFAULT;
					RigidBody.Layer = 15;
				}
				UpdateContactCallbackLimit();
				RigidBody.AddGravity();
				ActivateCollision();
				MyPhysics.RefreshCollisionFilter(this);
				RigidBody.Activate();
				if (RigidBody.InWorld)
				{
					base.HavokWorld.RigidBodyActivated(RigidBody);
					InvokeOnBodyActiveStateChanged(active: true);
				}
			}
			UpdateMass();
		}

		public void ConvertToStatic()
		{
			Flags = RigidBodyFlag.RBF_STATIC;
			bool flag = true;
			if (base.IsWelded || base.WeldInfo.Children.get_Count() > 0)
			{
				if (base.WeldedRigidBody != null && base.WeldedRigidBody.Quality != 0)
				{
					base.WeldedRigidBody.UpdateMotionType(HkMotionType.Fixed);
					base.WeldedRigidBody.Quality = HkCollidableQualityType.Fixed;
					base.WeldedRigidBody.Layer = 13;
				}
				MyWeldingGroups.Static.GetGroup((MyEntity)base.Entity).GroupData.UpdateParent((MyEntity)((base.WeldInfo.Parent != null) ? base.WeldInfo.Parent.Entity : base.Entity));
				flag &= base.WeldInfo.Parent == null;
			}
			UpdateMass();
			if (flag)
			{
				bool isActive = RigidBody.IsActive;
				NotifyConstraintsRemovedFromWorld();
				RigidBody.UpdateMotionType(HkMotionType.Fixed);
				RigidBody.Quality = HkCollidableQualityType.Fixed;
				RigidBody.Layer = 13;
				NotifyConstraintsAddedToWorld();
				ActivateCollision();
				MyPhysics.RefreshCollisionFilter(this);
				RigidBody.Activate();
				if (RigidBody.InWorld)
				{
					if (isActive)
					{
						InvokeOnBodyActiveStateChanged(active: false);
					}
					base.HavokWorld.RigidBodyActivated(RigidBody);
				}
				HkGroupFilter.GetSystemGroupFromFilterInfo(RigidBody.GetCollisionFilterInfo());
			}
			if (RigidBody2 != null)
			{
				if (RigidBody2.InWorld)
				{
					base.HavokWorld.RemoveRigidBody(RigidBody2);
				}
				RigidBody2.Dispose();
			}
			RigidBody2 = null;
		}

		private void RigidBody_CollisionAddedCallbackClient(ref HkCollisionEvent e)
		{
			if (!PredictCollisions)
			{
				return;
			}
			MyEntity myEntity = (MyEntity)e.GetOtherEntity(m_grid);
			MyGridContactInfo.ContactFlags flag;
			switch (GetEligibilityForPredictedImpulses(myEntity, out flag))
			{
			case PredictionDisqualificationReason.EntityIsNotMoving:
				m_predictedContactEntities[myEntity] = true;
				break;
			case PredictionDisqualificationReason.None:
			{
				e.Disable();
				int nrContactPoints = e.NrContactPoints;
				for (int i = 0; i < nrContactPoints; i++)
				{
					e.GetContactPointPropertiesAt(i).SetFlag(flag);
				}
				break;
			}
			}
		}

		private void RigidBody_CollisionRemovedCallbackClient(ref HkCollisionEvent e)
		{
			MyEntity myEntity = (MyEntity)e.GetOtherEntity(m_grid);
			if (myEntity != null && m_predictedContactEntities.ContainsKey(myEntity))
			{
				m_predictedContactEntities[myEntity] = false;
			}
		}

		public bool AnyPredictedContactEntities()
		{
			BoundingBoxD box = base.Entity.PositionComp.WorldAABB.Inflate(5.0);
			foreach (KeyValuePair<MyEntity, bool> predictedContactEntity in m_predictedContactEntities)
			{
				if (predictedContactEntity.Key.MarkedForClose || (!predictedContactEntity.Value && !predictedContactEntity.Key.PositionComp.WorldAABB.Intersects(ref box)))
				{
					m_predictedContactEntitiesToRemove.Add(predictedContactEntity.Key);
				}
			}
			foreach (MyEntity item in m_predictedContactEntitiesToRemove)
			{
				m_predictedContactEntities.Remove(item);
			}
			m_predictedContactEntitiesToRemove.Clear();
			return m_predictedContactEntities.Count > 0;
		}

		private void PredictContactImpulse(IMyEntity otherEntity, ref HkContactPointEvent e)
		{
			if (!e.FirstCallbackForFullManifold)
			{
				return;
			}
			MyGridContactInfo.ContactFlags flag = e.ContactProperties.GetFlags();
			if ((flag & (MyGridContactInfo.ContactFlags.PredictedCollision | MyGridContactInfo.ContactFlags.PredictedCollision_Disabled)) == 0)
			{
				switch (GetEligibilityForPredictedImpulses(otherEntity, out flag))
				{
				case PredictionDisqualificationReason.None:
					break;
				case PredictionDisqualificationReason.EntityIsNotMoving:
					MarkPredictedContactImpulse();
					return;
				default:
					return;
				}
				e.ContactProperties.SetFlag(flag);
			}
			if ((flag & MyGridContactInfo.ContactFlags.PredictedCollision_Disabled) != 0)
			{
				return;
			}
			MarkPredictedContactImpulse();
			float num = PredictContactMass(otherEntity);
			float mass = MyGridPhysicalGroupData.GetGroupSharedProperties(m_grid, checkMultithreading: false).Mass;
			if (!(num <= 0f) && !(mass <= 0f))
			{
				HkRigidBody rigidBody = RigidBody;
				bool num2 = e.Base.BodyA == rigidBody;
				int bodyIndex = ((!num2) ? 1 : 0);
				e.AccessVelocities(bodyIndex);
				float num3 = mass + num;
				float num4 = 1f - mass / num3;
				float num5 = (otherEntity.Physics.LinearVelocity - LinearVelocity).Length() * num3 * num4 * PREDICTION_IMPULSE_SCALE;
				Vector3 vector = e.ContactPoint.Normal;
				if (!num2)
				{
					vector = -vector;
				}
				Vector3 vector2 = vector * num5 / mass;
				rigidBody.LinearVelocity += vector2;
				e.UpdateVelocities(bodyIndex);
			}
		}

		private static float PredictContactMass(IMyEntity entity)
		{
			if (entity is MyEntitySubpart)
			{
				entity = entity.Parent;
				MyCubeBlock myCubeBlock = entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					entity = myCubeBlock.CubeGrid;
				}
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				return MyGridPhysicalGroupData.GetGroupSharedProperties(myCubeGrid, checkMultithreading: false).Mass;
			}
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				return myCharacter.CurrentMass;
			}
			if (entity is MyDebrisBase || entity is MyFloatingObject)
			{
				return 1f;
			}
			return (entity as MyInventoryBagEntity)?.Physics.Mass ?? 0f;
		}

		private PredictionDisqualificationReason GetEligibilityForPredictedImpulses(IMyEntity otherEntity, out MyGridContactInfo.ContactFlags flag)
		{
			flag = (MyGridContactInfo.ContactFlags)0;
			if (otherEntity == null)
			{
				return PredictionDisqualificationReason.NoEntity;
			}
			if (otherEntity is MyVoxelBase || otherEntity is Sandbox.Game.WorldEnvironment.MyEnvironmentSector)
			{
				return PredictionDisqualificationReason.EntityIsStatic;
			}
			MyCubeGrid myCubeGrid = otherEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				if (MyFixedGrids.IsRooted(myCubeGrid))
				{
					return PredictionDisqualificationReason.EntityIsStatic;
				}
				if (MyCubeGridGroups.Static.Physical.HasSameGroup(m_grid, myCubeGrid))
				{
					flag = MyGridContactInfo.ContactFlags.PredictedCollision_Disabled;
					return PredictionDisqualificationReason.None;
				}
				if (otherEntity.Physics.LinearVelocity.LengthSquared() < 1f)
				{
					return PredictionDisqualificationReason.EntityIsNotMoving;
				}
			}
			flag = MyGridContactInfo.ContactFlags.PredictedCollision;
			return PredictionDisqualificationReason.None;
		}

		private void MarkPredictedContactImpulse()
		{
			PredictedContactsCounter++;
			PredictedContactLastTime = MySandboxGame.Static.SimulationTime;
		}

		public void UpdateTOIOptimizer()
		{
			if (!Enabled || RigidBody == null)
			{
				m_lastTOIFrame = 0;
				return;
			}
			m_lastTOIFrame = MySession.Static.GameplayFrameCounter;
			m_toiCountThisFrame = 0;
		}

		public void ConsiderDisablingTOIs()
		{
			if (MySession.Static.GameplayFrameCounter <= m_lastTOIFrame + 10)
			{
				m_savedQuality = RigidBody.Quality;
				RigidBody.Quality = HkCollidableQualityType.Debris;
				DisableGridTOIsOptimizer.Static.Register(this);
			}
		}

		public void DisableTOIOptimization()
		{
			if (IsTOIOptimized)
			{
				if (RigidBody != null)
				{
					RigidBody.Quality = m_savedQuality;
					m_savedQuality = HkCollidableQualityType.Invalid;
				}
				DisableGridTOIsOptimizer.Static.Unregister(this);
			}
		}

		private void UpdateContactCallbackLimit()
		{
			HkRigidBody rigidBody = RigidBody;
			if (!(rigidBody == null))
			{
				int num2 = (rigidBody.CallbackLimit = ((m_grid.IsClientPredicted || Sync.IsServer) ? ((m_grid.BlocksCount <= 5) ? 1 : 10) : 0));
			}
		}

		public override void FracturedBody_AfterReplaceBody(ref HkdReplaceBodyEvent e)
		{
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0580: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
			if (!MyFakes.ENABLE_AFTER_REPLACE_BODY || !Sync.IsServer || m_recreateBody)
			{
				return;
			}
			base.HavokWorld.DestructionWorld.RemoveBreakableBody(e.OldBody);
			m_oldLinVel = RigidBody.LinearVelocity;
			m_oldAngVel = RigidBody.AngularVelocity;
			MyPhysics.RemoveDestructions(RigidBody);
			e.GetNewBodies(m_newBodies);
			if (m_newBodies.Count == 0)
			{
				return;
			}
			bool flag = false;
			m_tmpBlocksToDelete.Clear();
			m_tmpBlocksUpdateDamage.Clear();
			MySlimBlock mySlimBlock = null;
			foreach (HkdBreakableBodyInfo newBody in m_newBodies)
			{
				if (!newBody.IsFracture() || (MyFakes.ENABLE_FRACTURE_COMPONENT && m_grid.BlocksCount == 1 && m_grid.IsStatic && MyDestructionHelper.IsFixed(newBody)))
				{
					m_newBreakableBodies.Add(MyFracturedPiecesManager.Static.GetBreakableBody(newBody));
					FindFracturedBlocks(newBody);
					base.HavokWorld.DestructionWorld.RemoveBreakableBody(newBody);
					continue;
				}
				HkdBreakableBody breakableBody = MyFracturedPiecesManager.Static.GetBreakableBody(newBody);
				Matrix m = breakableBody.GetRigidBody().GetRigidBodyMatrix();
				Vector3D translation = ClusterToWorld(m.Translation);
				HkdBreakableShape breakableShape = breakableBody.BreakableShape;
				HkVec3IProperty hkVec3IProperty = breakableShape.GetProperty(255);
				if (!hkVec3IProperty.IsValid() && breakableShape.IsCompound())
<<<<<<< HEAD
				{
					hkVec3IProperty = breakableShape.GetChild(0).Shape.GetProperty(255);
				}
				bool flag2 = false;
				MySlimBlock cubeBlock = m_grid.GetCubeBlock(hkVec3IProperty.Value);
				MyCompoundCubeBlock myCompoundCubeBlock = ((cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null);
				if (cubeBlock != null)
				{
=======
				{
					hkVec3IProperty = breakableShape.GetChild(0).Shape.GetProperty(255);
				}
				bool flag2 = false;
				MySlimBlock cubeBlock = m_grid.GetCubeBlock(hkVec3IProperty.Value);
				MyCompoundCubeBlock myCompoundCubeBlock = ((cubeBlock != null) ? (cubeBlock.FatBlock as MyCompoundCubeBlock) : null);
				if (cubeBlock != null)
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (mySlimBlock == null)
					{
						mySlimBlock = cubeBlock;
					}
					if (!flag)
<<<<<<< HEAD
					{
						AddDestructionEffect(m_grid.GridIntegerToWorld(cubeBlock.Position), Vector3.Forward);
						flag = true;
					}
					MatrixD worldMatrix = m;
					worldMatrix.Translation = translation;
					if (MyFakes.ENABLE_FRACTURE_COMPONENT)
					{
=======
					{
						AddDestructionEffect(m_grid.GridIntegerToWorld(cubeBlock.Position), Vector3.Forward);
						flag = true;
					}
					MatrixD worldMatrix = m;
					worldMatrix.Translation = translation;
					if (MyFakes.ENABLE_FRACTURE_COMPONENT)
					{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						HkSimpleValueProperty hkSimpleValueProperty = breakableShape.GetProperty(256);
						if (hkSimpleValueProperty.IsValid())
						{
							m_tmpCompoundIds.Add((ushort)hkSimpleValueProperty.ValueUInt);
						}
						else if (!hkSimpleValueProperty.IsValid() && breakableShape.IsCompound())
						{
							m_tmpChildren_CompoundIds.Clear();
							breakableShape.GetChildren(m_tmpChildren_CompoundIds);
							foreach (HkdShapeInstanceInfo tmpChildren_CompoundId in m_tmpChildren_CompoundIds)
							{
								HkSimpleValueProperty hkSimpleValueProperty2 = tmpChildren_CompoundId.Shape.GetProperty(256);
								if (hkSimpleValueProperty2.IsValid())
<<<<<<< HEAD
								{
									m_tmpCompoundIds.Add((ushort)hkSimpleValueProperty2.ValueUInt);
								}
							}
						}
						bool flag3 = true;
						if (m_tmpCompoundIds.Count > 0)
						{
							foreach (ushort tmpCompoundId in m_tmpCompoundIds)
							{
								MySlimBlock block = myCompoundCubeBlock.GetBlock(tmpCompoundId);
								if (block == null)
								{
									flag3 = false;
									continue;
								}
								m_tmpDefinitions.Add(block.BlockDefinition.Id);
								flag3 &= RemoveShapesFromFracturedBlocks(breakableBody, block, tmpCompoundId, m_tmpBlocksToDelete, m_tmpBlocksUpdateDamage);
							}
						}
						else
						{
							m_tmpDefinitions.Add(cubeBlock.BlockDefinition.Id);
							flag3 = RemoveShapesFromFracturedBlocks(breakableBody, cubeBlock, null, m_tmpBlocksToDelete, m_tmpBlocksUpdateDamage);
						}
						if (flag3)
						{
							if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, m_tmpDefinitions, (myCompoundCubeBlock != null) ? myCompoundCubeBlock : cubeBlock.FatBlock) == null)
=======
								{
									m_tmpCompoundIds.Add((ushort)hkSimpleValueProperty2.ValueUInt);
								}
							}
						}
						bool flag3 = true;
						if (m_tmpCompoundIds.get_Count() > 0)
						{
							Enumerator<ushort> enumerator3 = m_tmpCompoundIds.GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									ushort current2 = enumerator3.get_Current();
									MySlimBlock block = myCompoundCubeBlock.GetBlock(current2);
									if (block == null)
									{
										flag3 = false;
										continue;
									}
									m_tmpDefinitions.Add(block.BlockDefinition.Id);
									flag3 &= RemoveShapesFromFracturedBlocks(breakableBody, block, current2, m_tmpBlocksToDelete, m_tmpBlocksUpdateDamage);
								}
							}
							finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								((IDisposable)enumerator3).Dispose();
							}
						}
						else
						{
							m_tmpDefinitions.Add(cubeBlock.BlockDefinition.Id);
							flag3 = RemoveShapesFromFracturedBlocks(breakableBody, cubeBlock, null, m_tmpBlocksToDelete, m_tmpBlocksUpdateDamage);
						}
<<<<<<< HEAD
=======
						if (flag3)
						{
							if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, m_tmpDefinitions, (myCompoundCubeBlock != null) ? myCompoundCubeBlock : cubeBlock.FatBlock) == null)
							{
								flag2 = true;
							}
						}
						else
						{
							flag2 = true;
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						m_tmpChildren_CompoundIds.Clear();
						m_tmpCompoundIds.Clear();
						m_tmpDefinitions.Clear();
					}
					else if (MyDestructionHelper.CreateFracturePiece(breakableBody, ref worldMatrix, null, (myCompoundCubeBlock != null) ? myCompoundCubeBlock : cubeBlock.FatBlock) == null)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
				if (flag2)
				{
					base.HavokWorld.DestructionWorld.RemoveBreakableBody(newBody);
					MyFracturedPiecesManager.Static.ReturnToPool(breakableBody);
				}
			}
			m_newBodies.Clear();
			if (mySlimBlock != null)
			{
				MyAudioComponent.PlayDestructionSound(mySlimBlock);
			}
			if (MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				FindFractureComponentBlocks();
				foreach (MyFractureComponentBase.Info item in m_fractureBlockComponentsCache)
				{
					m_tmpBlocksToDelete.Remove(((MyCubeBlock)item.Entity).SlimBlock);
				}
<<<<<<< HEAD
				foreach (MySlimBlock item2 in m_tmpBlocksToDelete)
				{
					m_tmpBlocksUpdateDamage.Remove(item2);
				}
				foreach (MySlimBlock item3 in m_tmpBlocksToDelete)
				{
					if (item3.IsMultiBlockPart)
					{
						MyCubeGridMultiBlockInfo multiBlockInfo = item3.CubeGrid.GetMultiBlockInfo(item3.MultiBlockId);
						if (multiBlockInfo != null && multiBlockInfo.Blocks.Count > 1 && item3.GetFractureComponent() != null)
						{
							item3.ApplyDestructionDamage(0f);
						}
					}
					if (item3.FatBlock != null)
					{
						item3.FatBlock.OnDestroy();
=======
				Enumerator<MySlimBlock> enumerator5 = m_tmpBlocksToDelete.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						MySlimBlock current4 = enumerator5.get_Current();
						m_tmpBlocksUpdateDamage.Remove(current4);
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
				enumerator5 = m_tmpBlocksToDelete.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						MySlimBlock current5 = enumerator5.get_Current();
						if (current5.IsMultiBlockPart)
						{
							MyCubeGridMultiBlockInfo multiBlockInfo = current5.CubeGrid.GetMultiBlockInfo(current5.MultiBlockId);
							if (multiBlockInfo != null && multiBlockInfo.Blocks.get_Count() > 1 && current5.GetFractureComponent() != null)
							{
								current5.ApplyDestructionDamage(0f);
							}
						}
						if (current5.FatBlock != null)
						{
							current5.FatBlock.OnDestroy();
						}
						m_grid.RemoveBlockWithId(current5, updatePhysics: true);
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
				enumerator5 = m_tmpBlocksUpdateDamage.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						MySlimBlock current6 = enumerator5.get_Current();
						MyFractureComponentCubeBlock fractureComponent = current6.GetFractureComponent();
						if (fractureComponent != null)
						{
							current6.ApplyDestructionDamage(fractureComponent.GetIntegrityRatioFromFracturedPieceCounts());
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					m_grid.RemoveBlockWithId(item3, updatePhysics: true);
				}
<<<<<<< HEAD
				foreach (MySlimBlock item4 in m_tmpBlocksUpdateDamage)
				{
					MyFractureComponentCubeBlock fractureComponent = item4.GetFractureComponent();
					if (fractureComponent != null)
					{
						item4.ApplyDestructionDamage(fractureComponent.GetIntegrityRatioFromFracturedPieceCounts());
					}
				}
			}
			else
			{
				foreach (MySlimBlock item5 in m_tmpBlocksToDelete)
				{
					MySlimBlock cubeBlock2 = m_grid.GetCubeBlock(item5.Position);
					if (cubeBlock2 != null)
					{
						if (cubeBlock2.FatBlock != null)
=======
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
			}
			else
			{
				Enumerator<MySlimBlock> enumerator5 = m_tmpBlocksToDelete.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						MySlimBlock current7 = enumerator5.get_Current();
						MySlimBlock cubeBlock2 = m_grid.GetCubeBlock(current7.Position);
						if (cubeBlock2 != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							cubeBlock2.FatBlock.OnDestroy();
						}
						m_grid.RemoveBlock(cubeBlock2, updatePhysics: true);
					}
				}
<<<<<<< HEAD
=======
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_tmpBlocksToDelete.Clear();
			m_tmpBlocksUpdateDamage.Clear();
			m_recreateBody = true;
		}

		/// <summary>
		/// Removes breakable shapes form fracture component.
		/// </summary>
		/// <param name="bBody">body with shapes</param>
		/// <param name="block">block</param>
		/// <param name="compoundId"></param>
		/// <param name="blocksToDelete">collection of blocks to remove from grid</param>
		/// <param name="blocksUpdateDamage">collection of blocks for updating their damage according to remaining shapes count</param>
		/// <returns>true if block was processes otherwise false (block in compound does not exist)</returns>
		private bool RemoveShapesFromFracturedBlocks(HkdBreakableBody bBody, MySlimBlock block, ushort? compoundId, HashSet<MySlimBlock> blocksToDelete, HashSet<MySlimBlock> blocksUpdateDamage)
		{
			MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
			if (fractureComponent != null)
			{
				bool flag = false;
				HkdBreakableShape breakableShape = bBody.BreakableShape;
				if (IsBreakableShapeCompound(breakableShape))
				{
					m_tmpShapeNames.Clear();
					m_tmpChildren_RemoveShapes.Clear();
					breakableShape.GetChildren(m_tmpChildren_RemoveShapes);
					int count = m_tmpChildren_RemoveShapes.Count;
					for (int i = 0; i < count; i++)
					{
						HkdShapeInstanceInfo hkdShapeInstanceInfo = m_tmpChildren_RemoveShapes[i];
						if (string.IsNullOrEmpty(hkdShapeInstanceInfo.ShapeName))
						{
							hkdShapeInstanceInfo.Shape.GetChildren(m_tmpChildren_RemoveShapes);
						}
					}
					m_tmpChildren_RemoveShapes.ForEach(delegate(HkdShapeInstanceInfo c)
					{
						string shapeName = c.ShapeName;
						if (!string.IsNullOrEmpty(shapeName))
						{
							m_tmpShapeNames.Add(shapeName);
						}
					});
					if (m_tmpShapeNames.Count != 0)
					{
						flag = fractureComponent.RemoveChildShapes(m_tmpShapeNames);
						MySyncDestructions.RemoveShapesFromFractureComponent(block.CubeGrid.EntityId, block.Position, compoundId ?? ushort.MaxValue, m_tmpShapeNames);
					}
					m_tmpChildren_RemoveShapes.Clear();
					m_tmpShapeNames.Clear();
				}
				else
				{
					string name = bBody.BreakableShape.Name;
					flag = fractureComponent.RemoveChildShapes(new string[1] { name });
					MySyncDestructions.RemoveShapeFromFractureComponent(block.CubeGrid.EntityId, block.Position, compoundId ?? ushort.MaxValue, name);
				}
				if (flag)
				{
					blocksToDelete.Add(block);
				}
				else
				{
					blocksUpdateDamage.Add(block);
				}
			}
			else
			{
				blocksToDelete.Add(block);
			}
			return true;
		}

		private static bool IsBreakableShapeCompound(HkdBreakableShape shape)
		{
			if (!string.IsNullOrEmpty(shape.Name) && !shape.IsCompound())
			{
				return shape.GetChildrenCount() > 0;
			}
			return true;
		}

		public List<MyFracturedBlock.Info> GetFracturedBlocks()
		{
			return m_fractureBlocksCache;
		}

		public List<MyFractureComponentBase.Info> GetFractureBlockComponents()
		{
			return m_fractureBlockComponentsCache;
		}

		public void ClearFractureBlockComponents()
		{
			m_fractureBlockComponentsCache.Clear();
		}

		private void BreakableBody_AfterControllerOperation(HkdBreakableBody b)
		{
			if (m_recreateBody)
			{
				b.BreakableShape.SetStrenghtRecursively(MyDestructionConstants.STRENGTH, 0.7f);
			}
		}

		private void BreakableBody_BeforeControllerOperation(HkdBreakableBody b)
		{
			if (m_recreateBody)
			{
				b.BreakableShape.SetStrenghtRecursively(float.MaxValue, 0.7f);
			}
		}

		private void RigidBody_ContactPointCallback_Destruction(ref HkContactPointEvent value)
		{
			MyGridContactInfo info = new MyGridContactInfo(ref value, m_grid);
			if (info.IsKnown)
			{
				return;
			}
			MyCubeGrid currentEntity = info.CurrentEntity;
			if (currentEntity == null || currentEntity.Physics == null || currentEntity.Physics.RigidBody == null)
			{
				return;
			}
			_ = currentEntity.Physics.RigidBody;
			MyPhysicsBody physicsBody = value.GetPhysicsBody(0);
			MyPhysicsBody physicsBody2 = value.GetPhysicsBody(1);
			if (physicsBody == null || physicsBody2 == null)
			{
				return;
			}
			IMyEntity entity = physicsBody.Entity;
			IMyEntity myEntity = physicsBody2.Entity;
			if (entity == null || myEntity == null || entity.Physics == null || myEntity.Physics == null || (entity is MyFracturedPiece && myEntity is MyFracturedPiece))
			{
				return;
			}
			HkRigidBody bodyA = value.Base.BodyA;
			HkRigidBody hkRigidBody = value.Base.BodyB;
			info.HandleEvents();
			if (bodyA.HasProperty(254) || hkRigidBody.HasProperty(254) || info.CollidingEntity is MyCharacter || info.CollidingEntity == null || info.CollidingEntity.MarkedForClose)
			{
				return;
			}
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyCubeGrid myCubeGrid2 = myEntity as MyCubeGrid;
			if (myCubeGrid2 == null && myEntity is MyEntitySubpart)
			{
				while (myEntity != null && !(myEntity is MyCubeGrid))
				{
					myEntity = myEntity.Parent;
				}
				if (myEntity != null)
				{
					physicsBody2 = myEntity.Physics as MyPhysicsBody;
					hkRigidBody = physicsBody2.RigidBody;
					myCubeGrid2 = myEntity as MyCubeGrid;
				}
			}
			if (myCubeGrid != null && myCubeGrid2 != null && MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid) == MyCubeGridGroups.Static.Physical.GetGroup(myCubeGrid2))
			{
				return;
			}
			Math.Abs(value.SeparatingVelocity);
			Vector3 velocityAtPoint = bodyA.GetVelocityAtPoint(info.Event.ContactPoint.Position);
			Vector3 velocityAtPoint2 = hkRigidBody.GetVelocityAtPoint(info.Event.ContactPoint.Position);
			float num = velocityAtPoint.Length();
			float num2 = velocityAtPoint2.Length();
			Vector3 vector = ((num > 0f) ? Vector3.Normalize(velocityAtPoint) : Vector3.Zero);
			Vector3 vector2 = ((num2 > 0f) ? Vector3.Normalize(velocityAtPoint2) : Vector3.Zero);
			float num3 = MyDestructionHelper.MassFromHavok(bodyA.Mass);
			float num4 = MyDestructionHelper.MassFromHavok(hkRigidBody.Mass);
			float num5 = num * num3;
			float num6 = num2 * num4;
			float num7 = ((num > 0f) ? Vector3.Dot(vector, value.ContactPoint.Normal) : 0f);
			float num8 = ((num2 > 0f) ? Vector3.Dot(vector2, value.ContactPoint.Normal) : 0f);
			num *= Math.Abs(num7);
			num2 *= Math.Abs(num8);
			bool flag = num3 == 0f;
			bool flag2 = num4 == 0f;
			bool flag3 = entity is MyFracturedPiece || (myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Small);
			bool flag4 = myEntity is MyFracturedPiece || (myCubeGrid2 != null && myCubeGrid2.GridSizeEnum == MyCubeSize.Small);
			Vector3.Dot(vector, vector2);
			float maxDestructionRadius = 0.5f;
			num5 *= info.ImpulseMultiplier;
			num6 *= info.ImpulseMultiplier;
			MyHitInfo hitInfo = default(MyHitInfo);
			Vector3D contactPosition = info.ContactPosition;
			hitInfo.Normal = value.ContactPoint.Normal;
			if (num7 < 0f)
			{
				if (entity is MyFracturedPiece)
				{
					num5 /= 10f;
				}
				num5 *= Math.Abs(num7);
				if (((num5 > 2000f && num > 2f && !flag4) || (num5 > 500f && num > 10f)) && (flag2 || num5 / num6 > 10f))
				{
					hitInfo.Position = contactPosition + 0.1f * hitInfo.Normal;
					num5 -= num3;
					if (Sync.IsServer && num5 > 0f)
					{
						if (myCubeGrid != null)
						{
							Vector3 gridPosition = GetGridPosition(value.ContactPoint, bodyA, myCubeGrid, 0);
							myCubeGrid.DoDamage(num5, hitInfo, gridPosition, myCubeGrid2?.EntityId ?? 0);
						}
						else
						{
							MyDestructionHelper.TriggerDestruction(num5, (MyPhysicsBody)entity.Physics, info.ContactPosition, value.ContactPoint.Normal, maxDestructionRadius);
						}
						hitInfo.Position = contactPosition - 0.1f * hitInfo.Normal;
						if (myCubeGrid2 != null)
						{
							Vector3 gridPosition2 = GetGridPosition(value.ContactPoint, hkRigidBody, myCubeGrid2, 1);
							myCubeGrid2.DoDamage(num5, hitInfo, gridPosition2, myCubeGrid?.EntityId ?? 0);
						}
						else
						{
							MyDestructionHelper.TriggerDestruction(num5, (MyPhysicsBody)myEntity.Physics, info.ContactPosition, value.ContactPoint.Normal, maxDestructionRadius);
						}
						ReduceVelocities(info);
					}
					MyDecals.HandleAddDecal(entity, hitInfo, Vector3.Zero);
					MyDecals.HandleAddDecal(myEntity, hitInfo, Vector3.Zero);
				}
			}
			if (!(num8 < 0f))
			{
				return;
			}
			if (myEntity is MyFracturedPiece)
			{
				num6 /= 10f;
			}
			num6 *= Math.Abs(num8);
			if (((!(num6 > 2000f) || !(num2 > 2f) || flag3) && (!(num6 > 500f) || !(num2 > 10f))) || (!flag && !(num6 / num5 > 10f)))
			{
				return;
			}
			hitInfo.Position = contactPosition + 0.1f * hitInfo.Normal;
			num6 -= num4;
			if (Sync.IsServer && num6 > 0f)
			{
				if (myCubeGrid != null)
				{
					Vector3 gridPosition3 = GetGridPosition(value.ContactPoint, bodyA, myCubeGrid, 0);
					myCubeGrid.DoDamage(num6, hitInfo, gridPosition3, myCubeGrid2?.EntityId ?? 0);
				}
				else
				{
					MyDestructionHelper.TriggerDestruction(num6, (MyPhysicsBody)entity.Physics, info.ContactPosition, value.ContactPoint.Normal, maxDestructionRadius);
				}
				hitInfo.Position = contactPosition - 0.1f * hitInfo.Normal;
				if (myCubeGrid2 != null)
				{
					Vector3 gridPosition4 = GetGridPosition(value.ContactPoint, hkRigidBody, myCubeGrid2, 1);
					myCubeGrid2.DoDamage(num6, hitInfo, gridPosition4, myCubeGrid?.EntityId ?? 0);
				}
				else
				{
					MyDestructionHelper.TriggerDestruction(num6, (MyPhysicsBody)myEntity.Physics, info.ContactPosition, value.ContactPoint.Normal, maxDestructionRadius);
				}
				ReduceVelocities(info);
			}
			MyDecals.HandleAddDecal(entity, hitInfo, Vector3.Zero);
			MyDecals.HandleAddDecal(myEntity, hitInfo, Vector3.Zero);
		}

		private HkShape CreateBreakableBody(HkShape shape, HkMassProperties? massProperties)
		{
			HkMassProperties massProperties2 = (massProperties.HasValue ? massProperties.Value : default(HkMassProperties));
			if (!Shape.BreakableShape.IsValid())
			{
				Shape.CreateBreakableShape();
			}
			HkdBreakableShape hkdBreakableShape = Shape.BreakableShape;
			if (!hkdBreakableShape.IsValid())
			{
				hkdBreakableShape = new HkdBreakableShape(shape);
				if (massProperties.HasValue)
				{
					HkMassProperties massProperties3 = massProperties.Value;
					hkdBreakableShape.SetMassProperties(ref massProperties3);
				}
				else
				{
					hkdBreakableShape.SetMassRecursively(50f);
				}
			}
			else
			{
				hkdBreakableShape.BuildMassProperties(ref massProperties2);
			}
			shape = hkdBreakableShape.GetShape();
			HkRigidBodyCinfo hkRigidBodyCinfo = new HkRigidBodyCinfo();
			hkRigidBodyCinfo.AngularDamping = m_angularDamping;
			hkRigidBodyCinfo.LinearDamping = m_linearDamping;
			hkRigidBodyCinfo.SolverDeactivation = (m_grid.IsStatic ? InitialSolverDeactivation : HkSolverDeactivation.Low);
			hkRigidBodyCinfo.ContactPointCallbackDelay = ContactPointDelay;
			hkRigidBodyCinfo.Shape = shape;
			hkRigidBodyCinfo.SetMassProperties(massProperties2);
			MyPhysicsBody.GetInfoFromFlags(hkRigidBodyCinfo, Flags);
			if (m_grid.IsStatic)
			{
				hkRigidBodyCinfo.MotionType = HkMotionType.Dynamic;
				hkRigidBodyCinfo.QualityType = HkCollidableQualityType.Moving;
			}
			HkRigidBody hkRigidBody = new HkRigidBody(hkRigidBodyCinfo);
			if (m_grid.IsStatic)
			{
				hkRigidBody.UpdateMotionType(HkMotionType.Fixed);
			}
			hkRigidBody.EnableDeactivation = true;
			BreakableBody = new HkdBreakableBody(hkdBreakableShape, hkRigidBody, null, Matrix.Identity);
			BreakableBody.AfterReplaceBody += FracturedBody_AfterReplaceBody;
			return shape;
		}

		private void FindFracturedBlocks(HkdBreakableBodyInfo b)
		{
			HkdBreakableBodyHelper hkdBreakableBodyHelper = new HkdBreakableBodyHelper(b);
			hkdBreakableBodyHelper.GetRigidBodyMatrix();
			hkdBreakableBodyHelper.GetChildren(m_children);
			foreach (HkdShapeInstanceInfo child in m_children)
			{
				if (!child.IsFracturePiece())
<<<<<<< HEAD
				{
					continue;
				}
				Vector3I value = ((HkVec3IProperty)child.Shape.GetProperty(255)).Value;
				if (!m_grid.CubeExists(value))
				{
					continue;
				}
=======
				{
					continue;
				}
				Vector3I value = ((HkVec3IProperty)child.Shape.GetProperty(255)).Value;
				if (!m_grid.CubeExists(value))
				{
					continue;
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (MyFakes.ENABLE_FRACTURE_COMPONENT)
				{
					MySlimBlock cubeBlock = m_grid.GetCubeBlock(value);
					if (cubeBlock != null && FindFractureComponentBlocks(cubeBlock, child))
					{
					}
				}
				else
				{
					if (!m_fracturedBlocksShapes.ContainsKey(value))
					{
						m_fracturedBlocksShapes[value] = new List<HkdShapeInstanceInfo>();
					}
					m_fracturedBlocksShapes[value].Add(child);
				}
			}
			if (!MyFakes.ENABLE_FRACTURE_COMPONENT)
			{
				foreach (Vector3I key in m_fracturedBlocksShapes.Keys)
				{
					List<HkdShapeInstanceInfo> list = m_fracturedBlocksShapes[key];
					foreach (HkdShapeInstanceInfo item2 in list)
					{
						Matrix m = item2.GetTransform();
						m.Translation = Vector3.Zero;
						item2.SetTransform(ref m);
					}
					HkdBreakableShape hkdBreakableShape = new HkdCompoundBreakableShape(null, list);
					((HkdCompoundBreakableShape)hkdBreakableShape).RecalcMassPropsFromChildren();
					HkMassProperties massProperties = default(HkMassProperties);
					hkdBreakableShape.BuildMassProperties(ref massProperties);
					HkdBreakableShape hkdBreakableShape2 = hkdBreakableShape;
					hkdBreakableShape2 = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref massProperties);
					foreach (HkdShapeInstanceInfo item3 in list)
					{
						HkdShapeInstanceInfo shapeInfo = item3;
						hkdBreakableShape2.AddShape(ref shapeInfo);
					}
					hkdBreakableShape.RemoveReference();
					ConnectPiecesInBlock(hkdBreakableShape2, list);
					MyFracturedBlock.Info info = default(MyFracturedBlock.Info);
					info.Shape = hkdBreakableShape2;
					info.Position = key;
					info.Compound = true;
					MyFracturedBlock.Info item = info;
					MySlimBlock cubeBlock2 = m_grid.GetCubeBlock(key);
					if (cubeBlock2 == null)
					{
						hkdBreakableShape2.RemoveReference();
						continue;
					}
					if (cubeBlock2.FatBlock is MyFracturedBlock)
					{
						MyFracturedBlock myFracturedBlock = cubeBlock2.FatBlock as MyFracturedBlock;
						item.OriginalBlocks = myFracturedBlock.OriginalBlocks;
						item.Orientations = myFracturedBlock.Orientations;
						item.MultiBlocks = myFracturedBlock.MultiBlocks;
					}
					else if (cubeBlock2.FatBlock is MyCompoundCubeBlock)
					{
						item.OriginalBlocks = new List<MyDefinitionId>();
						item.Orientations = new List<MyBlockOrientation>();
						MyCompoundCubeBlock obj = cubeBlock2.FatBlock as MyCompoundCubeBlock;
						bool flag = false;
						ListReader<MySlimBlock> blocks = obj.GetBlocks();
						foreach (MySlimBlock item4 in blocks)
						{
							item.OriginalBlocks.Add(item4.BlockDefinition.Id);
							item.Orientations.Add(item4.Orientation);
							flag = flag || item4.IsMultiBlockPart;
						}
						if (flag)
						{
							item.MultiBlocks = new List<MyFracturedBlock.MultiBlockPartInfo>();
							foreach (MySlimBlock item5 in blocks)
							{
								if (item5.IsMultiBlockPart)
								{
									item.MultiBlocks.Add(new MyFracturedBlock.MultiBlockPartInfo
									{
										MultiBlockDefinition = item5.MultiBlockDefinition.Id,
										MultiBlockId = item5.MultiBlockId
									});
								}
								else
								{
									item.MultiBlocks.Add(null);
								}
							}
						}
					}
					else
					{
						item.OriginalBlocks = new List<MyDefinitionId>();
						item.Orientations = new List<MyBlockOrientation>();
						item.OriginalBlocks.Add(cubeBlock2.BlockDefinition.Id);
						item.Orientations.Add(cubeBlock2.Orientation);
						if (cubeBlock2.IsMultiBlockPart)
						{
							item.MultiBlocks = new List<MyFracturedBlock.MultiBlockPartInfo>();
							item.MultiBlocks.Add(new MyFracturedBlock.MultiBlockPartInfo
							{
								MultiBlockDefinition = cubeBlock2.MultiBlockDefinition.Id,
								MultiBlockId = cubeBlock2.MultiBlockId
							});
						}
					}
					m_fractureBlocksCache.Add(item);
				}
			}
			m_fracturedBlocksShapes.Clear();
			m_children.Clear();
		}

		/// <summary>
		/// Searches for all children shapes of the block (or block inside compound block). The given block is always the block from grid (not from compound block).
		/// </summary>
		/// <param name="block">grid block</param>
		/// <param name="shapeInst">shape instnce for the block</param>
		/// <returns></returns>
		private bool FindFractureComponentBlocks(MySlimBlock block, HkdShapeInstanceInfo shapeInst)
		{
			HkdBreakableShape shape = shapeInst.Shape;
			if (IsBreakableShapeCompound(shape))
			{
				bool flag = false;
				List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
				shape.GetChildren(list);
				{
					foreach (HkdShapeInstanceInfo item in list)
					{
						flag |= FindFractureComponentBlocks(block, item);
					}
					return flag;
				}
			}
			ushort? num = null;
			if (shape.HasProperty(256))
			{
				num = (ushort)((HkSimpleValueProperty)shape.GetProperty(256)).ValueUInt;
			}
			MyCompoundCubeBlock myCompoundCubeBlock = block.FatBlock as MyCompoundCubeBlock;
			if (myCompoundCubeBlock != null)
			{
				if (!num.HasValue)
				{
					return false;
				}
				MySlimBlock block2 = myCompoundCubeBlock.GetBlock(num.Value);
				if (block2 == null)
				{
					return false;
				}
				block = block2;
			}
			if (!m_fracturedSlimBlocksShapes.ContainsKey(block))
			{
				m_fracturedSlimBlocksShapes[block] = new List<HkdShapeInstanceInfo>();
			}
			m_fracturedSlimBlocksShapes[block].Add(shapeInst);
			return true;
		}

		/// <summary>
		/// Searches for blocks which will create fracture components from cached m_fracturedSlimBlocksShapes
		/// </summary>
		private void FindFractureComponentBlocks()
		{
			foreach (KeyValuePair<MySlimBlock, List<HkdShapeInstanceInfo>> fracturedSlimBlocksShape in m_fracturedSlimBlocksShapes)
			{
				MySlimBlock key = fracturedSlimBlocksShape.Key;
				List<HkdShapeInstanceInfo> value = fracturedSlimBlocksShape.Value;
				if (key.FatBlock.Components.Has<MyFractureComponentBase>())
				{
					continue;
				}
				int totalBreakableShapeChildrenCount = key.GetTotalBreakableShapeChildrenCount();
				if (key.BlockDefinition.CreateFracturedPieces && totalBreakableShapeChildrenCount == value.Count)
				{
					continue;
				}
				foreach (HkdShapeInstanceInfo item2 in value)
				{
					item2.SetTransform(ref Matrix.Identity);
<<<<<<< HEAD
				}
				HkdBreakableShape hkdBreakableShape = new HkdCompoundBreakableShape(null, value);
				((HkdCompoundBreakableShape)hkdBreakableShape).RecalcMassPropsFromChildren();
				HkMassProperties massProperties = default(HkMassProperties);
				hkdBreakableShape.BuildMassProperties(ref massProperties);
				HkdBreakableShape hkdBreakableShape2 = hkdBreakableShape;
				hkdBreakableShape2 = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref massProperties);
				foreach (HkdShapeInstanceInfo item3 in value)
				{
					HkdShapeInstanceInfo shapeInfo = item3;
					hkdBreakableShape2.AddShape(ref shapeInfo);
				}
=======
				}
				HkdBreakableShape hkdBreakableShape = new HkdCompoundBreakableShape(null, value);
				((HkdCompoundBreakableShape)hkdBreakableShape).RecalcMassPropsFromChildren();
				HkMassProperties massProperties = default(HkMassProperties);
				hkdBreakableShape.BuildMassProperties(ref massProperties);
				HkdBreakableShape hkdBreakableShape2 = hkdBreakableShape;
				hkdBreakableShape2 = new HkdBreakableShape(hkdBreakableShape.GetShape(), ref massProperties);
				foreach (HkdShapeInstanceInfo item3 in value)
				{
					HkdShapeInstanceInfo shapeInfo = item3;
					hkdBreakableShape2.AddShape(ref shapeInfo);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				hkdBreakableShape.RemoveReference();
				ConnectPiecesInBlock(hkdBreakableShape2, value);
				MyFractureComponentBase.Info info = default(MyFractureComponentBase.Info);
				info.Entity = key.FatBlock;
				info.Shape = hkdBreakableShape2;
				info.Compound = true;
				MyFractureComponentBase.Info item = info;
				m_fractureBlockComponentsCache.Add(item);
			}
			m_fracturedSlimBlocksShapes.Clear();
		}

		private static void ConnectPiecesInBlock(HkdBreakableShape parent, List<HkdShapeInstanceInfo> shapeList)
		{
			for (int i = 0; i < shapeList.Count; i++)
			{
				for (int j = 0; j < shapeList.Count; j++)
				{
					if (i != j)
					{
						MyGridShape.ConnectShapesWithChildren(parent, shapeList[i].Shape, shapeList[j].Shape);
					}
				}
			}
		}

		private void RecreateBreakableBody(HashSet<Vector3I> dirtyBlocks)
		{
			_ = RigidBody.IsFixedOrKeyframed;
			int layer = RigidBody.Layer;
			HkWorld havokWorld = m_grid.Physics.HavokWorld;
			foreach (HkdBreakableBody newBreakableBody in m_newBreakableBodies)
			{
				MyFracturedPiecesManager.Static.ReturnToPool(newBreakableBody);
			}
			HkRigidBody rigidBody = BreakableBody.GetRigidBody();
			_ = rigidBody.LinearVelocity;
			_ = rigidBody.AngularVelocity;
			if (m_grid.BlocksCount > 0)
			{
				Shape.UnmarkBreakable(RigidBody);
				Shape.RefreshBlocks(RigidBody, dirtyBlocks);
				Shape.MarkBreakable(RigidBody);
				Shape.UpdateShape(RigidBody, RigidBody2, BreakableBody);
				CloseRigidBody();
				HkShape shape = m_shape;
				CreateBody(ref shape, null);
				RigidBody.Layer = layer;
				RigidBody.ContactPointCallbackEnabled = true;
				RigidBody.ContactSoundCallbackEnabled = true;
				RigidBody.ContactPointCallback += RigidBody_ContactPointCallback_Destruction;
				BreakableBody.BeforeControllerOperation += BreakableBody_BeforeControllerOperation;
				BreakableBody.AfterControllerOperation += BreakableBody_AfterControllerOperation;
				Matrix worldMatrix = base.Entity.PositionComp.WorldMatrixRef;
				worldMatrix.Translation = WorldToCluster(base.Entity.PositionComp.GetPosition());
				RigidBody.SetWorldMatrix(worldMatrix);
				RigidBody.UserObject = this;
				base.Entity.Physics.LinearVelocity = m_oldLinVel;
				base.Entity.Physics.AngularVelocity = m_oldAngVel;
				m_grid.DetectDisconnectsAfterFrame();
				Shape.CreateConnectionToWorld(BreakableBody, havokWorld);
				base.HavokWorld.DestructionWorld.AddBreakableBody(BreakableBody);
			}
			else
			{
				m_grid.Close();
			}
			m_newBreakableBodies.Clear();
			m_fractureBlocksCache.Clear();
		}

		public bool CheckLastDestroyedBlockFracturePieces()
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (m_grid.BlocksCount == 1 && !m_grid.IsStatic)
			{
				MySlimBlock mySlimBlock = Enumerable.First<MySlimBlock>((IEnumerable<MySlimBlock>)m_grid.GetBlocks());
				if (mySlimBlock.FatBlock != null)
				{
					MyCompoundCubeBlock myCompoundCubeBlock = mySlimBlock.FatBlock as MyCompoundCubeBlock;
					if (myCompoundCubeBlock != null)
					{
						bool flag = true;
						List<MySlimBlock> list = new List<MySlimBlock>(myCompoundCubeBlock.GetBlocks());
						foreach (MySlimBlock item in list)
						{
							flag = flag && item.FatBlock.Components.Has<MyFractureComponentBase>();
						}
						if (flag)
						{
							foreach (MySlimBlock item2 in list)
							{
								MyFractureComponentCubeBlock fractureComponent = item2.GetFractureComponent();
								ushort? blockId = myCompoundCubeBlock.GetBlockId(item2);
								if (fractureComponent != null)
								{
									MyDestructionHelper.CreateFracturePiece(fractureComponent, sync: true);
								}
								m_grid.RemoveBlockWithId(item2.Position, blockId.Value, updatePhysics: true);
							}
							return flag;
						}
						return flag;
					}
					MyFractureComponentCubeBlock fractureComponent2 = mySlimBlock.GetFractureComponent();
					if (fractureComponent2 != null)
					{
						MyDestructionHelper.CreateFracturePiece(fractureComponent2, sync: true);
						m_grid.RemoveBlock(mySlimBlock, updatePhysics: true);
					}
					return fractureComponent2 != null;
				}
			}
			return false;
		}

		internal ushort? GetContactCompoundId(Vector3I position, Vector3D constactPos)
		{
			List<HkdBreakableShape> list = new List<HkdBreakableShape>();
			GetRigidBodyMatrix(out m_bodyMatrix);
			Quaternion breakableBodyRotation = Quaternion.CreateFromRotationMatrix(m_bodyMatrix);
			if (BreakableBody == null)
			{
				MyLog.Default.WriteLine("BreakableBody was null in GetContactCounpoundId!");
			}
			if (base.HavokWorld.DestructionWorld == null)
			{
				MyLog.Default.WriteLine("HavokWorld.DestructionWorld was null in GetContactCompoundId!");
			}
			HkDestructionUtils.FindAllBreakableShapesIntersectingSphere(base.HavokWorld.DestructionWorld, BreakableBody, breakableBodyRotation, m_bodyMatrix.Translation, WorldToCluster(constactPos), 0.1f, list);
			ushort? result = null;
			foreach (HkdBreakableShape item in list)
			{
				if (!item.IsValid())
				{
					continue;
				}
				HkVec3IProperty hkVec3IProperty = item.GetProperty(255);
				if (hkVec3IProperty.IsValid() && !(position != hkVec3IProperty.Value))
				{
					HkSimpleValueProperty hkSimpleValueProperty = item.GetProperty(256);
					if (hkSimpleValueProperty.IsValid())
					{
						return (ushort)hkSimpleValueProperty.ValueUInt;
					}
				}
			}
			return result;
		}
	}
}
