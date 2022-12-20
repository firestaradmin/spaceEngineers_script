using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.EnvironmentItems;
<<<<<<< HEAD
using Sandbox.Game.GameSystems;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
using Sandbox.Game.WorldEnvironment.Definitions;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders.Components;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Weapons
{
	internal struct MyProjectile
	{
		private enum MyProjectileStateEnum : byte
		{
			Active,
			Killed,
			CloseToHit
		}

<<<<<<< HEAD
		private static MyDynamicObjectPool<List<IMyProjectileDetector>> m_detectorPool = new MyDynamicObjectPool<List<IMyProjectileDetector>>(100);

		internal static readonly int CHECK_INTERSECTION_INTERVAL_HIGH_Q = 5;

		internal static readonly int CHECK_INTERSECTION_INTERVAL_HIGHEST_Q = 3;

		internal static readonly int CHECK_INTERSECTION_INTERVAL_LOW_Q = 15;

		internal static int CHECK_INTERSECTION_INTERVAL = CHECK_INTERSECTION_INTERVAL_HIGH_Q;

		internal const float TARGET_ACCURACY = 0.33f;

		private static List<MyPhysics.HitInfo> m_raycastResult;

		private static List<MyLineSegmentOverlapResult<MyEntity>> m_entityRaycastResult;

		private static List<MyLineSegmentOverlapResult<IMyProjectileDetector>> m_detectorsInLine;

		private static Random m_random = new Random();

		private const float m_impulseMultiplier = 0.5f;

		private static MyStringHash m_hashBolt = MyStringHash.GetOrCompute("Bolt");

		private static MyStringId ID_PROJECTILE_TRAIL_LINE = MyStringId.GetOrCompute("ProjectileTrailLine");

		public static readonly MyTimedItemCache CollisionSoundsTimedCache = new MyTimedItemCache(60);

		public static readonly MyTimedItemCache CollisionParticlesTimedCache = new MyTimedItemCache(200);

		public static double CollisionSoundSpaceMapping = 0.039999999105930328;

		public static double CollisionParticlesSpaceMapping = 0.800000011920929;

		public static double MaxImpactSoundDistanceSq = 10000.0;

		private static readonly MyTimedItemCache m_prefetchedVoxelRaysTimedCache = new MyTimedItemCache(4000);

		private const double m_prefetchedVoxelRaysSourceMapping = 0.5;

		private const double m_prefetchedVoxelRaysDirectionMapping = 50.0;

		private static readonly float PROJECTILE_POLYLINE_DESIRED_LENGTH = 120f;

		private static readonly double MINIMUM_DISTANCE_SQRD = 4.0;

		private static Func<bool> canHear = () => true;

		private int m_index;
=======
		private static MyDynamicObjectPool<List<MySafeZone>> m_safeZonePool = new MyDynamicObjectPool<List<MySafeZone>>(100);

		internal static int CHECK_INTERSECTION_INTERVAL = 5;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyProjectileStateEnum m_state;

		private Vector3D m_origin;

		private Vector3D m_velocity_Combined;

		private Vector3D m_directionNormalized;

		private Vector3 m_inheritedVelocity;

		private Vector3D m_initialVelocity;

		private float m_maxTrajectory;

		private Vector3D m_position;

		private MyEntity[] m_ignoredEntities;

		private MyEntity m_weapon;

<<<<<<< HEAD
		private List<IMyProjectileDetector> m_detectorsInTrajectory;
=======
		private List<MySafeZone> m_safeZonesInTrajectory;

		private bool m_supressHitIndicator;

		private MyCharacterHitInfo m_charHitInfo;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private bool m_supressHitIndicator;

<<<<<<< HEAD
		private byte m_lengthMultiplier;
=======
		public float LengthMultiplier;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyProjectileAmmoDefinition m_projectileAmmoDefinition;

		private MyWeaponDefinition m_weaponDefinition;
<<<<<<< HEAD

		private ulong m_owningPlayer;
=======

		private MyStringId m_projectileTrailMaterialId;

		public ulong OwningPlayer;

		public MyEntity OwnerEntity;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyEntity m_ownerEntity;

		private MyEntity m_ownerEntityAbsolute;

		private int m_checkIntersectionIndex;

		private Vector3 m_cachedGravity;

		private int m_totalFrames;

<<<<<<< HEAD
		private MyClusterTree.MyClusterQueryResult? m_hkWorld;

		private bool m_positionChecked;
=======
		private static List<MyPhysics.HitInfo> m_raycastResult;

		private static List<MyLineSegmentOverlapResult<MyEntity>> m_entityRaycastResult;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private int m_closeHitStateCounter;

		public int Index
		{
			get
			{
				return m_index;
			}
			set
			{
				m_index = value;
			}
		}

		public Vector3D Position => m_position;

		public Vector3D Velocity => m_velocity_Combined;

		public Vector3D Origin => m_origin;

		public Vector3D CachedGravity => m_cachedGravity;

		public float MaxTrajectory => m_maxTrajectory;

<<<<<<< HEAD
		public MyProjectileAmmoDefinition ProjectileAmmoDefinition => m_projectileAmmoDefinition;
=======
		public static double MaxImpactSoundDistanceSq = 10000.0;

		private static readonly MyTimedItemCache m_prefetchedVoxelRaysTimedCache = new MyTimedItemCache(4000);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public MyWeaponDefinition WeaponDefinition => m_weaponDefinition;

		public IMyEntity OwnerEntity => m_ownerEntity;

		public IMyEntity OwnerEntityAbsolute => m_ownerEntityAbsolute;

		public ulong OwningPlayer => m_owningPlayer;

<<<<<<< HEAD
		/// <summary>        
		/// Projectile count multiplier - when real rate of fire it 45, but we shoot only 10 projectiles as optimization count multiplier will be 4.5
		/// </summary>
		/// <param name="index"></param>
		/// <param name="ammoDefinition"></param>
		/// <param name="weaponDefinition"></param>
		/// <param name="ignoreEntities"></param>
		/// <param name="origin"></param>
		/// <param name="inheritedVelocity"></param>
		/// <param name="directionNormalized">IMPORTANT: Direction vector must be normalized!</param>
		/// <param name="weapon"></param>
		/// <param name="ownerEntity"></param>
		/// <param name="ownerEntityAbsolute"></param>
		/// <param name="owningPlayer"></param>
		/// <param name="supressHitIndicator"></param>
		public void Start(int index, MyProjectileAmmoDefinition ammoDefinition, MyWeaponDefinition weaponDefinition, MyEntity[] ignoreEntities, Vector3D origin, Vector3 inheritedVelocity, Vector3 directionNormalized, MyEntity weapon, MyEntity ownerEntity, MyEntity ownerEntityAbsolute, ulong owningPlayer, bool supressHitIndicator = false)
=======
		public void Start(MyProjectileAmmoDefinition ammoDefinition, MyWeaponDefinition weaponDefinition, MyEntity[] ignoreEntities, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, MyEntity weapon, bool supressHitIndicator = false)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			m_index = index;
			m_ownerEntity = ownerEntity;
			m_ownerEntityAbsolute = ownerEntityAbsolute;
			m_owningPlayer = owningPlayer;
			m_checkIntersectionIndex = int.MaxValue;
			m_projectileAmmoDefinition = ammoDefinition;
			m_weaponDefinition = weaponDefinition;
<<<<<<< HEAD
			m_state = MyProjectileStateEnum.Active;
=======
			m_state = MyProjectileStateEnum.ACTIVE;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_ignoredEntities = ignoreEntities;
			m_origin = origin + 0.1 * (Vector3D)directionNormalized;
			m_position = m_origin;
			m_weapon = weapon;
			m_supressHitIndicator = supressHitIndicator;
<<<<<<< HEAD
			m_inheritedVelocity = inheritedVelocity;
			m_totalFrames = 0;
=======
			if (ammoDefinition.ProjectileTrailMaterial != null)
			{
				m_projectileTrailMaterialId = MyStringId.GetOrCompute(ammoDefinition.ProjectileTrailMaterial);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (ammoDefinition.ProjectileTrailProbability >= MyUtils.GetRandomFloat(0f, 1f))
			{
				m_lengthMultiplier = 40;
			}
			else
			{
				m_lengthMultiplier = 0;
			}
			SetInitialVelocities(ammoDefinition, directionNormalized, inheritedVelocity, out m_velocity_Combined);
			m_initialVelocity = m_velocity_Combined;
			m_directionNormalized = m_velocity_Combined;
			m_directionNormalized.Normalize();
			if (MyFakes.PROJECTILES_APPLY_GRAVITY)
			{
				m_cachedGravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(m_position);
			}
			m_maxTrajectory = ammoDefinition.MaxTrajectory;
			bool flag = true;
			if (weaponDefinition != null)
			{
				m_maxTrajectory *= weaponDefinition.RangeMultiplier;
				flag = weaponDefinition.UseRandomizedRange;
			}
			if (flag)
			{
				m_maxTrajectory *= MyUtils.GetRandomFloat(0.8f, 1f);
			}
<<<<<<< HEAD
			m_positionChecked = false;
			Vector3D vector3D = m_origin + m_directionNormalized * m_maxTrajectory;
			LineD ray = new LineD(m_origin, vector3D, m_maxTrajectory);
			if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
			{
				MyRenderProxy.DebugDrawLine3D(m_origin, vector3D, Color.Green, Color.Green, depthRead: true, persistent: true);
			}
			using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
			{
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, m_entityRaycastResult);
				PrefetchEntityDetectors(m_entityRaycastResult);
=======
			m_checkIntersectionIndex = checkIntersectionCounter++ % CHECK_INTERSECTION_INTERVAL;
			m_positionChecked = false;
			using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
			{
				LineD ray = new LineD(m_origin, m_origin + m_directionNormalized * m_maxTrajectory, m_maxTrajectory);
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, m_entityRaycastResult);
				PrefetchSafezones(m_entityRaycastResult);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				PrefetchVoxelPhysicsIfNeeded(m_entityRaycastResult, ref ray);
			}
		}

<<<<<<< HEAD
		public static void SetInitialVelocities(MyProjectileAmmoDefinition ammoDefinition, Vector3D directionNormalized, Vector3D inheritedVelocity, out Vector3D velocity_Combined)
		{
			float num = ammoDefinition.DesiredSpeed * ((ammoDefinition.SpeedVar > 0f) ? MyUtils.GetRandomFloat(1f - ammoDefinition.SpeedVar, 1f + ammoDefinition.SpeedVar) : 1f);
			Vector3D vector3D = directionNormalized * num;
			velocity_Combined = inheritedVelocity + vector3D;
		}

		private void PrefetchVoxelPhysicsIfNeeded(List<MyLineSegmentOverlapResult<MyEntity>> entities, ref LineD line)
		{
			int num = MyTuple.CombineHashCodes((Vector3D.Floor(line.From) * 0.5).GetHashCode(), Vector3D.Floor(m_directionNormalized * 50.0).GetHashCode());
			if (m_prefetchedVoxelRaysTimedCache.IsItemPresent(num, MySandboxGame.TotalSimulationTimeInMilliseconds))
			{
				return;
			}
			entities.Sort(MyLineSegmentOverlapResult<MyEntity>.DistanceComparer);
			MyVoxelPhysics myVoxelPhysics = null;
			foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
			{
				MyVoxelPhysics myVoxelPhysics2;
				if ((myVoxelPhysics2 = entity.Element as MyVoxelPhysics) != null)
				{
					if (myVoxelPhysics != null)
					{
						continue;
					}
					myVoxelPhysics = myVoxelPhysics2;
					myVoxelPhysics2.PrefetchShapeOnRay(ref line);
				}
				MyPlanet myPlanet;
				if ((myPlanet = entity.Element as MyPlanet) != null)
				{
					myPlanet.PrefetchShapeOnRay(ref line, prefetchOnlyNew: true);
				}
			}
		}

		private void PrefetchEntityDetectors(List<MyLineSegmentOverlapResult<MyEntity>> entities)
		{
			foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
			{
				IMyProjectileDetector myProjectileDetector;
				if ((myProjectileDetector = entity.Element as IMyProjectileDetector) != null && myProjectileDetector.IsDetectorEnabled)
				{
					if (m_detectorsInTrajectory == null)
					{
						m_detectorsInTrajectory = m_detectorPool.Allocate();
					}
					m_detectorsInTrajectory.Add(myProjectileDetector);
=======
		private void PrefetchVoxelPhysicsIfNeeded(List<MyLineSegmentOverlapResult<MyEntity>> entities, ref LineD line)
		{
			int num = MyTuple.CombineHashCodes((Vector3D.Floor(line.From) * 0.5).GetHashCode(), Vector3D.Floor(m_directionNormalized * 50.0).GetHashCode());
			if (m_prefetchedVoxelRaysTimedCache.IsItemPresent(num, MySandboxGame.TotalSimulationTimeInMilliseconds))
			{
				return;
			}
			foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
			{
				MyVoxelPhysics myVoxelPhysics;
				if ((myVoxelPhysics = entity.Element as MyVoxelPhysics) != null)
				{
					myVoxelPhysics.PrefetchShapeOnRay(ref line);
				}
			}
		}

		private void PrefetchSafezones(List<MyLineSegmentOverlapResult<MyEntity>> entities)
		{
			foreach (MyLineSegmentOverlapResult<MyEntity> entity in entities)
			{
				MySafeZone mySafeZone;
				if ((mySafeZone = entity.Element as MySafeZone) != null && (mySafeZone.AllowedActions & MySafeZoneAction.Shooting) == 0)
				{
					if (m_safeZonesInTrajectory == null)
					{
						m_safeZonesInTrajectory = m_safeZonePool.Allocate();
					}
					m_safeZonesInTrajectory.Add(mySafeZone);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private bool IsIgnoredEntity(IMyEntity entity)
		{
			if (m_ignoredEntities != null)
			{
				MyEntity[] ignoredEntities = m_ignoredEntities;
				foreach (MyEntity myEntity in ignoredEntities)
				{
					if (entity == myEntity)
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		///  Update position, check collisions, etc.        
		/// </summary>
		/// <returns>false if projectile dies/timeouts in this tick.</returns>
		public bool Update()
		{
			if (m_state == MyProjectileStateEnum.Killed)
			{
				return false;
			}
			if (MyProjectiles.Static.PhysicsDirty)
			{
				m_hkWorld = null;
			}
			Vector3D position = m_position;
			m_totalFrames++;
			float num = 0.0166666675f * MyFakes.SIMULATION_SPEED;
			m_velocity_Combined += m_cachedGravity * num;
			m_directionNormalized = m_velocity_Combined;
			m_directionNormalized.Normalize();
			float num2 = num * (float)m_totalFrames;
			m_position = m_origin + m_initialVelocity * num2 + m_cachedGravity * (num2 * num2) / 2f;
			if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
			{
				MyRenderProxy.DebugDrawPoint(m_position, Color.Green, depthRead: true, persistent: true);
			}
			if (((Vector3)(m_position - m_origin)).LengthSquared() >= m_maxTrajectory * m_maxTrajectory)
			{
				m_state = MyProjectileStateEnum.Killed;
				return false;
			}
<<<<<<< HEAD
			int num3 = ((m_state != 0) ? 1 : CHECK_INTERSECTION_INTERVAL);
			if (MyFakes.PROJECTILES_APPLY_GRAVITY && m_cachedGravity != Vector3.Zero)
			{
				float num4 = GetInaccuracyInMeters(m_cachedGravity, num3) / 2f;
				while (num4 > 0.33f && num3 > CHECK_INTERSECTION_INTERVAL_HIGHEST_Q)
				{
					num3--;
					num4 = GetInaccuracyInMeters(m_cachedGravity, num3) / 2f;
				}
			}
			if (m_checkIntersectionIndex < num3)
=======
			m_checkIntersectionIndex = ++m_checkIntersectionIndex % CHECK_INTERSECTION_INTERVAL;
			if (m_checkIntersectionIndex != 0 && m_positionChecked)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_checkIntersectionIndex++;
				return true;
			}
<<<<<<< HEAD
			m_checkIntersectionIndex = 1 + ((!m_positionChecked) ? m_random.Next(CHECK_INTERSECTION_INTERVAL_HIGH_Q) : 0);
			Vector3D zero = Vector3D.Zero;
			if (m_state == MyProjectileStateEnum.CloseToHit)
			{
				Vector3 vector = Vector3.Normalize(m_velocity_Combined - m_inheritedVelocity);
				float num5 = (float)m_velocity_Combined.Length();
				float num6 = 0.0166666675f * MyFakes.SIMULATION_SPEED * (float)num3;
				zero = position + num5 * vector * num6;
			}
			else
			{
				num2 = num * (float)(m_totalFrames + num3);
				zero = m_origin + m_initialVelocity * num2 + m_cachedGravity * (num2 * num2) / 2f;
			}
			LineD line = new LineD(m_positionChecked ? position : m_origin, zero);
			m_positionChecked = true;
			if (MyFakes.PROJECTILES_EARLY_EXIT_CHECK && !IsThereAnyPossibleHit(ref line))
			{
				return true;
			}
			if (!GetHitEntityAndPosition(line, out var entity, out var hitInfoRet, out var customdata))
			{
				if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
				{
					if (m_state != 0)
					{
						MyRenderProxy.DebugDrawLine3D(position, zero, Color.Red, Color.Red, depthRead: true, persistent: true);
						MyRenderProxy.DebugDrawSphere(zero, 0.1f, Color.Purple, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
					}
					else
					{
						MyRenderProxy.DebugDrawLine3D(position, zero, Color.Gold, Color.Gold, depthRead: true, persistent: true);
						MyRenderProxy.DebugDrawSphere(zero, 0.1f, Color.GreenYellow, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
					}
				}
				if (m_state == MyProjectileStateEnum.CloseToHit)
				{
					m_closeHitStateCounter--;
					if (m_closeHitStateCounter <= 0)
					{
						m_state = MyProjectileStateEnum.Killed;
						return false;
					}
				}
				return true;
			}
			if (m_state == MyProjectileStateEnum.Active)
			{
				Vector3D vector3D = line.From - hitInfoRet.Position;
				Vector3D vector3D2 = line.From - m_position;
				if (vector3D.LengthSquared() > vector3D2.LengthSquared())
				{
					if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
					{
						MyRenderProxy.DebugDrawLine3D(line.From, m_position, Color.Red, Color.Red, depthRead: true, persistent: true);
						MyRenderProxy.DebugDrawPoint(m_position, Color.Purple, depthRead: true, persistent: true);
					}
					m_state = MyProjectileStateEnum.CloseToHit;
					m_closeHitStateCounter = num3;
					return true;
				}
				MyHitInfo myHitInfo = hitInfoRet;
				Vector3 vector2 = Vector3.Normalize(m_velocity_Combined - m_inheritedVelocity);
				float num7 = (float)m_velocity_Combined.Length();
				float num8 = 0.0166666675f * MyFakes.SIMULATION_SPEED * (float)num3;
				zero = position + num7 * vector2 * num8;
				line = new LineD(line.From, zero);
				if (!GetHitEntityAndPosition(line, out entity, out hitInfoRet, out customdata))
				{
					hitInfoRet = myHitInfo;
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
			{
				MyRenderProxy.DebugDrawSphere(hitInfoRet.Position, 0.2f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
				MyRenderProxy.DebugDrawLine3D(line.From, hitInfoRet.Position, Color.Yellow, Color.Red, depthRead: true, persistent: true);
			}
			if (entity != null && IsIgnoredEntity(entity))
			{
				return true;
			}
			OnHit(entity, ref hitInfoRet, customdata, ref line);
			return false;
		}

		private static bool IsThereAnyPossibleHit(ref LineD line)
		{
			using (MyUtils.ReuseCollection(ref m_entityRaycastResult))
			{
				MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref line, m_entityRaycastResult);
				if (m_entityRaycastResult.Count == 0)
				{
					if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
					{
						MyRenderProxy.DebugDrawLine3D(line.From, line.To, Color.DarkGray, Color.DarkGray, depthRead: true, persistent: true);
						MyRenderProxy.DebugDrawPoint(line.To, Color.Red, depthRead: true, persistent: true);
					}
					return false;
				}
			}
			return true;
		}

		public void OnHit(IMyEntity entity, ref MyHitInfo hitInfo, object customdata, ref LineD line)
		{
			if (entity == null)
			{
				m_state = MyProjectileStateEnum.Killed;
				return;
			}
			m_position = hitInfo.Position;
			m_position += line.Direction * 0.01;
			bool flag = false;
			MyCharacter myCharacter;
			if ((myCharacter = entity as MyCharacter) != null)
			{
				IStoppableAttackingTool stoppableAttackingTool;
				if ((stoppableAttackingTool = myCharacter.CurrentWeapon as IStoppableAttackingTool) != null)
				{
					stoppableAttackingTool.StopShooting(m_ownerEntity);
				}
				if (customdata != null)
				{
					flag = ((MyCharacterHitInfo)customdata).HitHead && m_projectileAmmoDefinition.HeadShot;
				}
			}
			float num = 1f;
			IMyHandheldGunObject<MyGunBase> myHandheldGunObject;
			if ((myHandheldGunObject = m_weapon as IMyHandheldGunObject<MyGunBase>) != null)
			{
				MyGunBase gunBase = myHandheldGunObject.GunBase;
				if (gunBase?.WeaponProperties?.WeaponDefinition != null)
=======
			Vector3D to = position + CHECK_INTERSECTION_INTERVAL * (m_velocity_Projectile * 0.01666666753590107 * MyFakes.SIMULATION_SPEED);
			LineD line = new LineD(m_positionChecked ? position : m_origin, to);
			m_positionChecked = true;
			if (!GetHitEntityAndPosition(line, out var entity, out var hitInfoRet, out var customdata))
			{
				return true;
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
			{
				MyRenderProxy.DebugDrawSphere(hitInfoRet.Position, 0.2f, Color.Red, 1f, depthRead: true, smooth: false, cull: true, persistent: true);
				MyRenderProxy.DebugDrawLine3D(line.From, line.To, Color.Yellow, Color.Red, depthRead: true, persistent: true);
			}
			if (entity != null && IsIgnoredEntity(entity))
			{
				return true;
			}
			m_position = hitInfoRet.Position;
			m_position += line.Direction * 0.01;
			if (entity != null)
			{
				bool flag = false;
				MyCharacter myCharacter;
				if ((myCharacter = entity as MyCharacter) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					IStoppableAttackingTool stoppableAttackingTool;
					if ((stoppableAttackingTool = myCharacter.CurrentWeapon as IStoppableAttackingTool) != null)
					{
						stoppableAttackingTool.StopShooting(OwnerEntity);
					}
					if (customdata != null)
					{
						flag = ((MyCharacterHitInfo)customdata).HitHead && m_projectileAmmoDefinition.HeadShot;
					}
				}
<<<<<<< HEAD
			}
			else if (m_weaponDefinition != null)
			{
				num = m_weaponDefinition.DamageMultiplier;
			}
			float num2 = ((!(entity is IMyCharacter)) ? m_projectileAmmoDefinition.ProjectileMassDamage : (flag ? m_projectileAmmoDefinition.ProjectileHeadShotDamage : m_projectileAmmoDefinition.ProjectileHealthDamage));
			num2 *= num;
			hitInfo.Velocity = m_velocity_Combined;
			Vector3 impulse = m_projectileAmmoDefinition.ProjectileHitImpulse * 0.5f * m_directionNormalized;
			if (entity is MyCharacter)
			{
				impulse *= 100f;
			}
			GetSurfaceAndMaterial(entity, ref line, ref m_position, hitInfo.ShapeKey, out var surfaceImpact, out var materialType);
			MyStringHash hitVoxelMaterial = materialType;
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null)
			{
				Vector3D worldPosition = hitInfo.Position;
				MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref worldPosition);
				if (materialAt != null)
				{
					hitVoxelMaterial = materialAt.Id.SubtypeId;
				}
			}
			bool flag2 = MySessionComponentSafeZones.IsActionAllowed(hitInfo.Position, MySafeZoneAction.Damage, 0L, m_owningPlayer);
			if (!flag2)
			{
				num2 = 0f;
			}
			MyProjectileHitInfo hitInfo2 = new MyProjectileHitInfo
			{
				Damage = num2,
				Velocity = hitInfo.Velocity,
				HitNormal = hitInfo.Normal,
				HitPosition = hitInfo.Position,
				HitShapeKey = hitInfo.ShapeKey,
				HitEntity = entity,
				HitMaterial = materialType,
				HitVoxelMaterial = hitVoxelMaterial,
				Impulse = impulse,
				AddDecals = true,
				AddHitIndicator = !m_supressHitIndicator,
				AddHitParticles = true,
				PlayHitSound = true,
				AddSZNotification = true
			};
			MyProjectiles.Static.InvokeOnHit(ref this, ref hitInfo2);
			m_supressHitIndicator = !hitInfo2.AddHitIndicator;
			if (hitInfo2.PlayHitSound && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				PlayHitSound(materialType, entity, hitInfo.Position, m_projectileAmmoDefinition.PhysicalMaterial);
			}
			if (hitInfo2.AddSZNotification && !flag2 && m_owningPlayer == MySession.Static?.LocalHumanPlayer?.Id.SteamId)
			{
				MyHud.Notifications.Add(MyNotificationSingletons.DamageTurnedOff);
			}
			if (hitInfo2.Damage > 0f)
			{
				DoDamage(hitInfo2.Damage, hitInfo, customdata, entity, null, flag);
				MyCharacter myCharacter2;
				if ((myCharacter2 = entity as MyCharacter) != null && myCharacter2.ControllerInfo != null && myCharacter2.ControllerInfo.IsLocallyHumanControlled() && MyGuiScreenHudSpace.Static != null)
				{
					MyGuiScreenHudSpace.Static.AddDamageIndicator(hitInfo2.Damage, hitInfo, m_origin);
				}
				if (m_projectileAmmoDefinition.ProjectileExplosionDamage != 0f)
				{
					ExecuteExplosion(entity as MyEntity, in hitInfo.Position, in hitInfo.Normal);
				}
			}
			if (hitInfo2.AddDecals)
			{
				MyDecals.HandleAddDecal(entity, hitInfo, Vector3.Zero, hitInfo2.HitMaterial, m_projectileAmmoDefinition.PhysicalMaterial, customdata as MyCharacterHitInfo, hitInfo2.Damage, hitInfo2.HitVoxelMaterial);
			}
			if (hitInfo2.AddHitParticles && !Sandbox.Engine.Platform.Game.IsDedicated && !CollisionParticlesTimedCache.IsPlaceUsed(hitInfo.Position, CollisionParticlesSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds + MyRandom.Instance.Next(0, CollisionParticlesTimedCache.EventTimeoutMs / 2)))
			{
				MyCubeBlock obj = entity as MyCubeBlock;
				IMyEntity entity2 = entity;
				if (obj != null && entity.Parent != null)
=======
				float num = 1f;
				IMyHandheldGunObject<MyGunBase> myHandheldGunObject;
				if ((myHandheldGunObject = m_weapon as IMyHandheldGunObject<MyGunBase>) != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MyGunBase gunBase = myHandheldGunObject.GunBase;
					if (gunBase?.WeaponProperties?.WeaponDefinition != null)
					{
						num = gunBase.WeaponProperties.WeaponDefinition.DamageMultiplier;
					}
				}
<<<<<<< HEAD
				if (!MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Hit, hitInfo.Position, hitInfo.Normal, m_projectileAmmoDefinition.PhysicalMaterial, hitInfo2.HitMaterial, entity2) && surfaceImpact != MySurfaceImpactEnum.Character)
				{
					CreateBasicHitParticles(m_projectileAmmoDefinition.ProjectileOnHitEffectName, ref hitInfo.Position, ref hitInfo.Normal, ref line.Direction, entity, m_weapon, 1f, m_ownerEntity);
				}
			}
			if (hitInfo2.Impulse.LengthSquared() > 0f && flag2 && (m_weapon == null || entity.GetTopMostParent() != m_weapon.GetTopMostParent()) && entity.Physics != null && entity.Physics.Enabled && !entity.Physics.IsStatic)
			{
				entity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, hitInfo2.Impulse, hitInfo2.HitPosition, Vector3.Zero);
=======
				else if (m_weaponDefinition != null)
				{
					num = m_weaponDefinition.DamageMultiplier;
				}
				GetSurfaceAndMaterial(entity, ref line, ref m_position, hitInfoRet.ShapeKey, out var surfaceImpact, out var materialType);
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					PlayHitSound(materialType, entity, hitInfoRet.Position, m_projectileAmmoDefinition.PhysicalMaterial);
				}
				hitInfoRet.Velocity = m_velocity_Combined;
				float num2 = ((!(entity is IMyCharacter)) ? m_projectileAmmoDefinition.ProjectileMassDamage : (flag ? m_projectileAmmoDefinition.ProjectileHeadShotDamage : m_projectileAmmoDefinition.ProjectileHealthDamage));
				num2 *= num;
				if (!MySessionComponentSafeZones.IsActionAllowed(hitInfoRet.Position, MySafeZoneAction.Damage, 0L, OwningPlayer))
				{
					num2 = 0f;
					if (OwningPlayer == MySession.Static?.LocalHumanPlayer?.Id.SteamId)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.DamageTurnedOff);
					}
				}
				if (num2 > 0f)
				{
					DoDamage(num2, hitInfoRet, customdata, entity, null, flag);
					MyCharacter myCharacter2;
					if ((myCharacter2 = entity as MyCharacter) != null && myCharacter2.ControllerInfo != null && myCharacter2.ControllerInfo.IsLocallyHumanControlled() && MyGuiScreenHudSpace.Static != null)
					{
						MyGuiScreenHudSpace.Static.AddDamageIndicator(num2, hitInfoRet, m_origin);
					}
				}
				MyStringHash voxelMaterial = materialType;
				MyVoxelBase myVoxelBase = entity as MyVoxelBase;
				if (myVoxelBase != null)
				{
					Vector3D worldPosition = hitInfoRet.Position;
					MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref worldPosition);
					if (materialAt != null)
					{
						voxelMaterial = materialAt.Id.SubtypeId;
					}
				}
				MyDecals.HandleAddDecal(entity, hitInfoRet, Vector3.Zero, materialType, m_projectileAmmoDefinition.PhysicalMaterial, customdata as MyCharacterHitInfo, num2, voxelMaterial);
				if (!Sandbox.Engine.Platform.Game.IsDedicated && !CollisionParticlesTimedCache.IsPlaceUsed(hitInfoRet.Position, CollisionParticlesSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds + MyRandom.Instance.Next(0, CollisionParticlesTimedCache.EventTimeoutMs / 2)))
				{
					_ = Vector3.Zero;
					MyCubeBlock obj = entity as MyCubeBlock;
					IMyEntity entity2 = entity;
					if (obj != null && entity.Parent != null)
					{
						entity2 = entity.Parent;
					}
					if (!MyMaterialPropertiesHelper.Static.TryCreateCollisionEffect(MyMaterialPropertiesHelper.CollisionType.Hit, hitInfoRet.Position, hitInfoRet.Normal, m_projectileAmmoDefinition.PhysicalMaterial, materialType, entity2) && surfaceImpact != MySurfaceImpactEnum.CHARACTER)
					{
						CreateBasicHitParticles(m_projectileAmmoDefinition.ProjectileOnHitEffectName, ref hitInfoRet.Position, ref hitInfoRet.Normal, ref line.Direction, entity, m_weapon, 1f, OwnerEntity);
					}
				}
				if (num2 > 0f && (m_weapon == null || entity.GetTopMostParent() != m_weapon.GetTopMostParent()))
				{
					ApplyProjectileForce(entity, hitInfoRet.Position, m_directionNormalized, isPlayerShip: false, m_projectileAmmoDefinition.ProjectileHitImpulse * 0.5f);
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private float GetInaccuracyInMeters(Vector3 Gravity, int updates)
		{
			float num = Gravity.Length();
			float num2 = (float)updates * 0.0166666675f * MyFakes.SIMULATION_SPEED;
			return num * num2 * num2 / 2f;
		}

		private static void CreateBasicHitParticles(string effectName, ref Vector3D hitPoint, ref Vector3 normal, ref Vector3D direction, IMyEntity physObject, MyEntity weapon, float scale, MyEntity ownerEntity = null)
		{
			Vector3D vector3D = Vector3D.Reflect(direction, normal);
			new MyUtilRandomVector3ByDeviatingVector(vector3D);
			MatrixD matrixD = MatrixD.CreateFromDir(normal);
<<<<<<< HEAD
			MatrixD effectMatrix = MatrixD.CreateWorld(hitPoint, matrixD.Forward, matrixD.Up);
			Vector3D worldPosition = effectMatrix.Translation;
			if (MyParticlesManager.TryCreateParticleEffect(effectName, ref effectMatrix, ref worldPosition, weapon.Render.ParentIDs[0], out var effect))
=======
			if (MyParticlesManager.TryCreateParticleEffect(effectName, MatrixD.CreateWorld(hitPoint, matrixD.Forward, matrixD.Up), out var effect))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				effect.UserScale = scale;
			}
		}

		private bool GetHitEntityAndPosition(LineD line, out IMyEntity entity, out MyHitInfo hitInfoRet, out object customdata)
		{
			entity = null;
			customdata = null;
			hitInfoRet = default(MyHitInfo);
<<<<<<< HEAD
			bool detectorHit = false;
			float detectorDistSq = float.MaxValue;
			IMyEntity detectorEntity = null;
			Vector3 detectorNorm = Vector3.UnitX;
			Vector3D detectorPos = Vector3.Zero;
			using (MyUtils.ReuseCollection(ref m_detectorsInLine))
			{
				MyProjectiles.Static.OverlapDetectorsLineSegment(ref line, m_detectorsInLine);
				foreach (MyLineSegmentOverlapResult<IMyProjectileDetector> item in m_detectorsInLine)
				{
					CheckDetector(item.Element);
				}
			}
			if (m_detectorsInTrajectory != null)
			{
				foreach (IMyProjectileDetector item2 in m_detectorsInTrajectory)
				{
					CheckDetector(item2);
=======
			bool safezoneHit = false;
			float safezoneDistSq = float.MaxValue;
			IMyEntity myEntity = null;
			Vector3 normal = Vector3.UnitX;
			Vector3 vector = Vector3.Zero;
			if (m_safeZonesInTrajectory != null)
			{
				foreach (MySafeZone item in m_safeZonesInTrajectory)
				{
					if (item.Enabled && item.GetIntersectionWithLine(ref line, out var v, useCollisionModel: true, IntersectionFlags.ALL_TRIANGLES))
					{
						float num = (float)(v.Value - line.From).LengthSquared();
						if (!(num >= safezoneDistSq))
						{
							safezoneHit = true;
							myEntity = item;
							normal = -line.Direction;
							vector = v.Value;
							safezoneDistSq = num;
						}
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			int num2 = 0;
			using (MyUtils.ReuseCollection(ref m_raycastResult))
			{
				GetEntities(ref line);
				do
				{
					if (num2 < m_raycastResult.Count)
					{
<<<<<<< HEAD
						MyPhysics.HitInfo hitInfo = m_raycastResult[num];
=======
						MyPhysics.HitInfo hitInfo = m_raycastResult[num2];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						entity = hitInfo.HkHitInfo.GetHitEntity() as MyEntity;
						hitInfoRet.Position = hitInfo.Position;
						hitInfoRet.Normal = hitInfo.HkHitInfo.Normal;
						hitInfoRet.ShapeKey = hitInfo.HkHitInfo.GetShapeKey(0);
					}
					if (IsIgnoredEntity(entity))
					{
						entity = null;
					}
<<<<<<< HEAD
					IMyEntity myEntity = entity;
					if (myEntity == null)
					{
						continue;
					}
					MyCharacter myCharacter;
					if ((myCharacter = myEntity as MyCharacter) == null)
					{
						MyCubeGrid myCubeGrid;
						if ((myCubeGrid = myEntity as MyCubeGrid) == null)
						{
							MyVoxelBase myVoxelBase;
							if ((myVoxelBase = myEntity as MyVoxelBase) != null && myVoxelBase.GetIntersectionWithLine(ref line, out var t, IntersectionFlags.DIRECT_TRIANGLES) && IsHitCloserThanDetector(t.Value.IntersectionPointInWorldSpace))
							{
								hitInfoRet.Position = t.Value.IntersectionPointInWorldSpace;
								hitInfoRet.Normal = t.Value.NormalInWorldSpace;
								hitInfoRet.ShapeKey = 0u;
								detectorHit = false;
							}
							continue;
						}
						MyCubeGrid.MyCubeGridHitInfo info = new MyCubeGrid.MyCubeGridHitInfo();
						if (!myCubeGrid.GetIntersectionWithLine(ref line, ref info) || info == null)
						{
							continue;
=======
					MyCharacter myCharacter;
					MyCubeGrid myCubeGrid;
					MyVoxelBase myVoxelBase;
					MyIntersectionResultLineTriangleEx? t;
					if ((myCharacter = entity as MyCharacter) != null)
					{
						if (myCharacter.GetIntersectionWithLine(ref line, ref m_charHitInfo))
						{
							if (IsHitCloserThanSafezone(m_charHitInfo.Triangle.IntersectionPointInWorldSpace))
							{
								hitInfoRet.Position = m_charHitInfo.Triangle.IntersectionPointInWorldSpace;
								hitInfoRet.Normal = m_charHitInfo.Triangle.NormalInWorldSpace;
								customdata = m_charHitInfo;
								safezoneHit = false;
							}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES)
						{
							DrawDebugGridHit(entity, info);
						}
<<<<<<< HEAD
						if (IsHitCloserThanDetector(info.Triangle.IntersectionPointInWorldSpace))
						{
							hitInfoRet.Position = info.Triangle.IntersectionPointInWorldSpace;
							hitInfoRet.Normal = info.Triangle.NormalInWorldSpace;
=======
					}
					else if ((myCubeGrid = entity as MyCubeGrid) != null)
					{
						bool intersectionWithLine = myCubeGrid.GetIntersectionWithLine(ref line, ref m_cubeGridHitInfo);
						if (MyDebugDrawSettings.DEBUG_DRAW_PROJECTILES && intersectionWithLine && m_cubeGridHitInfo != null)
						{
							MyTriangle_Vertices inputTriangle = m_cubeGridHitInfo.Triangle.Triangle.InputTriangle;
							MySlimBlock cubeBlock = ((MyCubeGrid)entity).GetCubeBlock(m_cubeGridHitInfo.Position);
							Vector3 vector2 = toWorldCoordinates(cubeBlock, inputTriangle.Vertex0);
							Vector3 vector3 = toWorldCoordinates(cubeBlock, inputTriangle.Vertex1);
							Vector3 vector4 = toWorldCoordinates(cubeBlock, inputTriangle.Vertex2);
							MyRenderProxy.DebugDrawLine3D(vector2, vector3, Color.Purple, Color.Purple, depthRead: true, persistent: true);
							MyRenderProxy.DebugDrawLine3D(vector3, vector4, Color.Purple, Color.Purple, depthRead: true, persistent: true);
							MyRenderProxy.DebugDrawLine3D(vector4, vector2, Color.Purple, Color.Purple, depthRead: true, persistent: true);
						}
						if (intersectionWithLine && IsHitCloserThanSafezone(m_cubeGridHitInfo.Triangle.IntersectionPointInWorldSpace))
						{
							hitInfoRet.Position = m_cubeGridHitInfo.Triangle.IntersectionPointInWorldSpace;
							hitInfoRet.Normal = m_cubeGridHitInfo.Triangle.NormalInWorldSpace;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							if (Vector3.Dot(hitInfoRet.Normal, line.Direction) > 0f)
							{
								hitInfoRet.Normal = -hitInfoRet.Normal;
							}
<<<<<<< HEAD
							detectorHit = false;
						}
						MyCube myCube = info.Triangle.UserObject as MyCube;
						if (myCube != null && myCube.CubeBlock.FatBlock != null && myCube.CubeBlock.FatBlock.Physics == null)
						{
							entity = myCube.CubeBlock.FatBlock;
						}
						continue;
					}
					MyCharacterHitInfo info2 = new MyCharacterHitInfo();
					if (myCharacter.GetIntersectionWithLine(ref line, ref info2))
					{
						if (IsHitCloserThanDetector(info2.Triangle.IntersectionPointInWorldSpace))
						{
							hitInfoRet.Position = info2.Triangle.IntersectionPointInWorldSpace;
							hitInfoRet.Normal = info2.Triangle.NormalInWorldSpace;
							customdata = info2;
							detectorHit = false;
						}
					}
					else
					{
						entity = null;
=======
							safezoneHit = false;
						}
						if (m_cubeGridHitInfo != null)
						{
							MyCube myCube = m_cubeGridHitInfo.Triangle.UserObject as MyCube;
							if (myCube != null && myCube.CubeBlock.FatBlock != null && myCube.CubeBlock.FatBlock.Physics == null)
							{
								entity = myCube.CubeBlock.FatBlock;
							}
						}
					}
					else if ((myVoxelBase = entity as MyVoxelBase) != null && myVoxelBase.GetIntersectionWithLine(ref line, out t, IntersectionFlags.DIRECT_TRIANGLES) && IsHitCloserThanSafezone(t.Value.IntersectionPointInWorldSpace))
					{
						hitInfoRet.Position = t.Value.IntersectionPointInWorldSpace;
						hitInfoRet.Normal = t.Value.NormalInWorldSpace;
						hitInfoRet.ShapeKey = 0u;
						safezoneHit = false;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				while (entity == null && ++num2 < m_raycastResult.Count);
			}
			if (safezoneHit)
			{
				entity = myEntity;
				hitInfoRet.Normal = normal;
				hitInfoRet.Position = vector;
				return true;
			}
			return entity != null;
			bool IsHitCloserThanSafezone(Vector3D otherHit)
			{
				if (!safezoneHit)
				{
					return true;
				}
				if ((float)(otherHit - line.From).LengthSquared() < safezoneDistSq)
				{
					return true;
				}
				return false;
			}
			static Vector3 toWorldCoordinates(IMySlimBlock block, Vector3 localCoords)
			{
				block.Orientation.GetMatrix(out var result);
				return Vector3.Transform(Vector3.Transform(localCoords, result) + block.Position * block.CubeGrid.GridSize, block.CubeGrid.WorldMatrix);
			}
			if (detectorHit)
			{
				entity = detectorEntity;
				hitInfoRet.Normal = detectorNorm;
				hitInfoRet.Position = detectorPos;
			}
			return entity != null;
			void CheckDetector(IMyProjectileDetector detector)
			{
				if (detector.IsDetectorEnabled && detector.GetDetectorIntersectionWithLine(ref line, out var hit))
				{
					float num2 = (float)(hit.Value - line.From).LengthSquared();
					if (!(num2 >= detectorDistSq))
					{
						detectorHit = true;
						detectorEntity = detector.HitEntity;
						detectorNorm = -line.Direction;
						detectorPos = hit.Value;
						detectorDistSq = num2;
					}
				}
			}
			bool IsHitCloserThanDetector(Vector3D otherHit)
			{
				if (!detectorHit)
				{
					return true;
				}
				if ((float)(otherHit - line.From).LengthSquared() < detectorDistSq)
				{
					return true;
				}
				return false;
			}
		}

		private void DrawDebugGridHit(IMyEntity entity, MyCubeGrid.MyCubeGridHitInfo cubeGridHitInfo)
		{
			MyTriangle_Vertices inputTriangle = cubeGridHitInfo.Triangle.Triangle.InputTriangle;
			MySlimBlock cubeBlock = ((MyCubeGrid)entity).GetCubeBlock(cubeGridHitInfo.Position);
			Vector3D vector3D = toWorldCoordinates(cubeBlock, inputTriangle.Vertex0);
			Vector3D vector3D2 = toWorldCoordinates(cubeBlock, inputTriangle.Vertex1);
			Vector3D vector3D3 = toWorldCoordinates(cubeBlock, inputTriangle.Vertex2);
			MyRenderProxy.DebugDrawLine3D(vector3D, vector3D2, Color.Purple, Color.Purple, depthRead: true, persistent: true);
			MyRenderProxy.DebugDrawLine3D(vector3D2, vector3D3, Color.Purple, Color.Purple, depthRead: true, persistent: true);
			MyRenderProxy.DebugDrawLine3D(vector3D3, vector3D, Color.Purple, Color.Purple, depthRead: true, persistent: true);
			Vector3D toWorldCoordinates(IMySlimBlock block, Vector3 localCoords)
			{
				block.Orientation.GetMatrix(out var result);
				return Vector3.Transform(Vector3.Transform(localCoords, result) + block.Position * block.CubeGrid.GridSize, block.CubeGrid.WorldMatrix);
			}
		}

		private void GetEntities(ref LineD line)
		{
			bool flag = false;
			if (MyFakes.PROJECTILES_APPLY_RAYCAST_OPTIMIZATION)
			{
				if (!m_hkWorld.HasValue)
				{
					m_hkWorld = MyPhysics.GetHkWorld(ref m_position);
				}
				if (m_hkWorld.HasValue)
				{
					MyClusterTree.MyClusterQueryResult value = m_hkWorld.Value;
					bool num = value.AABB.Contains(line.To) == ContainmentType.Contains;
					if (num && value.AABB.Contains(line.From) == ContainmentType.Contains)
					{
						flag = true;
						MyPhysics.CastRayWorld(value, line.From, line.To, m_raycastResult, 15);
					}
					if (!num)
					{
						m_hkWorld = null;
					}
				}
			}
			if (!flag)
			{
				MyPhysics.CastRay(line.From, line.To, m_raycastResult, 15);
			}
		}

		private void DoDamage(float damage, MyHitInfo hitInfo, object customdata, IMyEntity damagedEntity, IMyEntity realHitEntity = null, bool isHeadshot = false)
		{
			_ = (MyEntity)MySession.Static.ControlledEntity;
			MySession.MyHitIndicatorTarget myHitIndicatorTarget = MySession.MyHitIndicatorTarget.None;
			bool flag = false;
			bool flag2 = false;
			MyCharacter myCharacter = damagedEntity as MyCharacter;
			if (myCharacter != null)
			{
				flag2 = IsFriendly(myCharacter);
				flag = flag2 && !MySession.Static.Settings.EnableFriendlyFire && m_weapon is MyAutomaticRifleGun;
			}
<<<<<<< HEAD
			if (m_ownerEntityAbsolute != null && m_ownerEntityAbsolute.Equals(MySession.Static.ControlledEntity) && (damagedEntity is IMyDestroyableObject || damagedEntity is MyCubeGrid))
=======
			if (OwnerEntityAbsolute != null && OwnerEntityAbsolute.Equals(MySession.Static.ControlledEntity) && (damagedEntity is IMyDestroyableObject || damagedEntity is MyCubeGrid))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MySession.Static.TotalDamageDealt += (uint)damage;
			}
			if (Sync.IsServer)
			{
				if (myCharacter != null)
				{
					myHitIndicatorTarget = GetCharacterHitTarget(myCharacter, isHeadshot, flag2);
				}
				if (m_projectileAmmoDefinition.PhysicalMaterial == m_hashBolt)
				{
					IMyDestroyableObject myDestroyableObject = damagedEntity as IMyDestroyableObject;
					if (myDestroyableObject != null && myCharacter != null)
					{
						float integrity = myDestroyableObject.Integrity;
						myDestroyableObject.DoDamage(damage, MyDamageType.Bolt, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0, realHitEntity?.EntityId ?? 0);
						if (myCharacter != null && !flag2 && myDestroyableObject.Integrity <= 0f && integrity > 0f)
						{
							myHitIndicatorTarget = MySession.MyHitIndicatorTarget.Kill;
						}
					}
				}
				else
				{
					MyCubeGrid myCubeGrid = damagedEntity as MyCubeGrid;
					MyCubeBlock myCubeBlock = damagedEntity as MyCubeBlock;
					MySlimBlock mySlimBlock = null;
					if (myCubeBlock != null)
					{
						myCubeGrid = myCubeBlock.CubeGrid;
						mySlimBlock = myCubeBlock.SlimBlock;
					}
					else if (myCubeGrid != null)
					{
						mySlimBlock = myCubeGrid.GetTargetedBlock(hitInfo.Position - 0.001f * hitInfo.Normal);
						if (mySlimBlock != null)
						{
							myCubeBlock = mySlimBlock.FatBlock;
						}
					}
					IMyDestroyableObject myDestroyableObject2;
					if (myCubeGrid != null)
					{
						myHitIndicatorTarget = MySession.MyHitIndicatorTarget.Grid;
						if (myCubeGrid.Physics != null && myCubeGrid.Physics.Enabled && (myCubeGrid.BlocksDestructionEnabled || MyFakes.ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE))
						{
							bool flag3 = false;
							if (mySlimBlock != null && (myCubeGrid.BlocksDestructionEnabled || mySlimBlock.ForceBlockDestructible))
							{
								mySlimBlock.DoDamage(damage, MyDamageType.Bullet, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0, realHitEntity?.EntityId ?? 0);
								if (myCubeBlock == null)
								{
									flag3 = true;
								}
							}
							if (myCubeGrid.BlocksDestructionEnabled && flag3)
							{
								ApllyDeformationCubeGrid(damage, hitInfo.Position, myCubeGrid);
							}
						}
					}
					else if (damagedEntity is MyEntitySubpart)
					{
						myHitIndicatorTarget = MySession.MyHitIndicatorTarget.Grid;
						IMyEntity myEntity = damagedEntity;
						while (myEntity.Parent != null && myEntity.Parent is MyEntitySubpart)
						{
							myEntity = myEntity.Parent;
						}
						if (myEntity.Parent != null && myEntity.Parent.Parent is MyCubeGrid)
						{
							hitInfo.Position = myEntity.Parent.WorldAABB.Center;
							DoDamage(damage, hitInfo, customdata, myEntity.Parent.Parent, (realHitEntity != null) ? realHitEntity : damagedEntity);
						}
					}
					else if ((myDestroyableObject2 = damagedEntity as IMyDestroyableObject) != null)
					{
						float integrity2 = myDestroyableObject2.Integrity;
						myDestroyableObject2.DoDamage(flag ? 0f : damage, MyDamageType.Bullet, sync: true, hitInfo, (m_weapon != null) ? GetSubpartOwner(m_weapon).EntityId : 0, realHitEntity?.EntityId ?? 0);
						if (myCharacter != null && !flag2 && myDestroyableObject2.Integrity <= 0f && integrity2 > 0f)
						{
							myHitIndicatorTarget = MySession.MyHitIndicatorTarget.Kill;
						}
					}
				}
			}
			if (!m_supressHitIndicator && myHitIndicatorTarget != MySession.MyHitIndicatorTarget.None)
			{
<<<<<<< HEAD
				MySession.HitIndicatorActivation(myHitIndicatorTarget, m_owningPlayer);
=======
				MySession.HitIndicatorActivation(myHitIndicatorTarget, OwningPlayer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private MySession.MyHitIndicatorTarget GetCharacterHitTarget(IMyDestroyableObject destroyable, bool headshot, bool friendly)
		{
			if (destroyable == null)
			{
				return MySession.MyHitIndicatorTarget.None;
			}
			if (friendly)
			{
				return MySession.MyHitIndicatorTarget.Friend;
			}
			if (destroyable.Integrity > 0f)
			{
				if (!headshot)
				{
					return MySession.MyHitIndicatorTarget.Character;
				}
				return MySession.MyHitIndicatorTarget.Headshot;
			}
			return MySession.MyHitIndicatorTarget.Character;
		}

		private bool IsFriendly(MyCharacter character)
		{
			if (character != null)
			{
				long playerIdentityId = character.GetPlayerIdentityId();
<<<<<<< HEAD
				MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(MySession.Static.Players.TryGetIdentityId(m_owningPlayer), playerIdentityId);
=======
				MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(MySession.Static.Players.TryGetIdentityId(OwningPlayer), playerIdentityId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (relationPlayerPlayer == MyRelationsBetweenPlayers.Self || relationPlayerPlayer == MyRelationsBetweenPlayers.Allies)
				{
					return true;
				}
			}
			return false;
		}

		private MyEntity GetSubpartOwner(MyEntity entity)
		{
			if (entity == null)
			{
				return null;
			}
			if (!(entity is MyEntitySubpart))
			{
				return entity;
			}
			MyEntity myEntity = entity;
			while (myEntity is MyEntitySubpart && myEntity != null)
			{
				myEntity = myEntity.Parent;
			}
			if (myEntity == null)
			{
				return entity;
			}
			return myEntity;
		}

		public static void GetSurfaceAndMaterial(IMyEntity entity, ref LineD line, ref Vector3D hitPosition, uint shapeKey, out MySurfaceImpactEnum surfaceImpact, out MyStringHash materialType)
		{
			MyVoxelBase myVoxelBase = entity as MyVoxelBase;
			if (myVoxelBase != null)
			{
				materialType = MyMaterialType.ROCK;
				surfaceImpact = MySurfaceImpactEnum.Destructible;
				MyVoxelMaterialDefinition materialAt = myVoxelBase.GetMaterialAt(ref hitPosition);
				if (materialAt != null)
				{
					materialType = materialAt.MaterialTypeNameHash;
				}
				return;
			}
			if (entity is MyCharacter)
			{
				surfaceImpact = MySurfaceImpactEnum.Character;
				materialType = MyMaterialType.CHARACTER;
				if ((entity as MyCharacter).Definition.PhysicalMaterial != null)
				{
					materialType = MyStringHash.GetOrCompute((entity as MyCharacter).Definition.PhysicalMaterial);
				}
				return;
			}
			if (entity is MyFloatingObject)
			{
				MyFloatingObject myFloatingObject = entity as MyFloatingObject;
				materialType = ((myFloatingObject.VoxelMaterial != null) ? MyMaterialType.ROCK : ((myFloatingObject.ItemDefinition != null && myFloatingObject.ItemDefinition.PhysicalMaterial != MyStringHash.NullOrEmpty) ? myFloatingObject.ItemDefinition.PhysicalMaterial : MyMaterialType.METAL));
				surfaceImpact = MySurfaceImpactEnum.Metal;
				return;
			}
			if (entity is Sandbox.Game.WorldEnvironment.MyEnvironmentSector)
			{
				surfaceImpact = MySurfaceImpactEnum.Metal;
				materialType = MyMaterialType.METAL;
				Sandbox.Game.WorldEnvironment.MyEnvironmentSector obj = entity as Sandbox.Game.WorldEnvironment.MyEnvironmentSector;
				int itemFromShapeKey = obj.GetItemFromShapeKey(shapeKey);
				if (obj.GetEnvironmentalItemDefinitionId(itemFromShapeKey) == "Tree")
				{
					surfaceImpact = MySurfaceImpactEnum.Destructible;
					materialType = MyMaterialType.WOOD;
				}
				return;
			}
			if (entity is MyTrees)
			{
				surfaceImpact = MySurfaceImpactEnum.Destructible;
				materialType = MyMaterialType.WOOD;
				return;
			}
			if (entity is IMyHandheldGunObject<MyGunBase>)
			{
				surfaceImpact = MySurfaceImpactEnum.Metal;
				materialType = MyMaterialType.METAL;
				MyGunBase gunBase = (entity as IMyHandheldGunObject<MyGunBase>).GunBase;
				if (gunBase != null && gunBase.WeaponProperties != null && gunBase.WeaponProperties.WeaponDefinition != null)
				{
					materialType = gunBase.WeaponProperties.WeaponDefinition.PhysicalMaterial;
				}
				return;
			}
			surfaceImpact = MySurfaceImpactEnum.Metal;
			materialType = MyMaterialType.METAL;
			MyCubeGrid myCubeGrid = entity as MyCubeGrid;
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			MySlimBlock mySlimBlock = null;
			if (myCubeBlock != null)
			{
				myCubeGrid = myCubeBlock.CubeGrid;
				mySlimBlock = myCubeBlock.SlimBlock;
			}
			else if (myCubeGrid != null)
			{
				mySlimBlock = myCubeGrid.GetTargetedBlock(hitPosition);
				if (mySlimBlock != null)
				{
					myCubeBlock = mySlimBlock.FatBlock;
				}
			}
			if (myCubeGrid == null || mySlimBlock == null)
			{
				return;
			}
			if (mySlimBlock.BlockDefinition.PhysicalMaterial != null && !mySlimBlock.BlockDefinition.PhysicalMaterial.Id.TypeId.IsNull)
			{
				materialType = MyStringHash.GetOrCompute(mySlimBlock.BlockDefinition.PhysicalMaterial.Id.SubtypeName);
			}
			else
			{
				if (myCubeBlock == null)
				{
					return;
				}
				MyIntersectionResultLineTriangleEx? t = null;
				myCubeBlock.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES);
				if (t.HasValue)
				{
					switch (myCubeBlock.ModelCollision.GetDrawTechnique(t.Value.Triangle.TriangleIndex))
					{
					case MyMeshDrawTechnique.GLASS:
						materialType = MyStringHash.GetOrCompute("Glass");
						break;
					case MyMeshDrawTechnique.HOLO:
						materialType = MyStringHash.GetOrCompute("Glass");
						break;
					case MyMeshDrawTechnique.SHIELD:
						materialType = MyStringHash.GetOrCompute("Shield");
						break;
					case MyMeshDrawTechnique.SHIELD_LIT:
						materialType = MyStringHash.GetOrCompute("ShieldLit");
						break;
					}
				}
			}
		}

<<<<<<< HEAD
=======
		private void StopEffect()
		{
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void PlayHitSound(MyStringHash materialType, IMyEntity entity, Vector3D position, MyStringHash thisType)
		{
			bool flag = CollisionSoundsTimedCache.IsPlaceUsed(position, CollisionSoundSpaceMapping, MySandboxGame.TotalSimulationTimeInMilliseconds);
			if (m_ownerEntity is MyWarhead || flag)
			{
				return;
			}
			MyEntity myEntity = MySession.Static.ControlledEntity?.Entity;
			if (myEntity == null || Vector3D.DistanceSquared(myEntity.PositionComp.GetPosition(), position) > MaxImpactSoundDistanceSq)
			{
				return;
			}
			MyEntity myEntity = MySession.Static.ControlledEntity?.Entity;
			if (myEntity == null || Vector3D.DistanceSquared(myEntity.PositionComp.GetPosition(), position) > MaxImpactSoundDistanceSq)
			{
				return;
			}
			MyEntity3DSoundEmitter myEntity3DSoundEmitter = MyAudioComponent.TryGetSoundEmitter();
			if (myEntity3DSoundEmitter == null)
			{
				return;
			}
			MySoundPair mySoundPair = null;
			mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Hit, thisType, materialType);
			if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
			{
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
			}
			if (mySoundPair.SoundId.IsNull && entity is MyVoxelBase)
			{
				materialType = MyMaterialType.ROCK;
				mySoundPair = MyMaterialPropertiesHelper.Static.GetCollisionCue(MyMaterialPropertiesHelper.CollisionType.Start, thisType, materialType);
			}
			if (mySoundPair == null || mySoundPair == MySoundPair.Empty)
<<<<<<< HEAD
			{
				return;
			}
			myEntity3DSoundEmitter.Entity = (MyEntity)entity;
			myEntity3DSoundEmitter.SetPosition(m_position);
			myEntity3DSoundEmitter.SetVelocity(Vector3.Zero);
			if (MySession.Static != null && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && entity == myEntity)
			{
				myEntity3DSoundEmitter.EmitterMethods[0].Add(canHear);
=======
			{
				return;
			}
			myEntity3DSoundEmitter.Entity = (MyEntity)entity;
			myEntity3DSoundEmitter.SetPosition(m_position);
			myEntity3DSoundEmitter.SetVelocity(Vector3.Zero);
			if (MySession.Static != null && MyFakes.ENABLE_NEW_SOUNDS && MySession.Static.Settings.RealisticSound && entity == myEntity)
			{
				Func<bool> canHear = () => true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myEntity3DSoundEmitter.StoppedPlaying += delegate(MyEntity3DSoundEmitter e)
				{
					e.EmitterMethods[0].Remove(canHear, immediate: true);
				};
<<<<<<< HEAD
=======
				myEntity3DSoundEmitter.EmitterMethods[0].Add(canHear);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			myEntity3DSoundEmitter.PlaySound(mySoundPair);
		}

		private void ApllyDeformationCubeGrid(float damage, Vector3D hitPosition, MyCubeGrid grid)
		{
			MatrixD worldMatrixNormalizedInv = grid.PositionComp.WorldMatrixNormalizedInv;
			Vector3D vector3D = Vector3D.Transform(hitPosition, worldMatrixNormalizedInv);
			Vector3D vector3D2 = Vector3D.TransformNormal(m_directionNormalized, worldMatrixNormalizedInv);
			float deformationOffset = MyFakes.DEFORMATION_PROJECTILE_OFFSET_RATIO * damage;
			float value = 0.011904f * damage;
			float value2 = 0.008928f * damage;
			value = MathHelper.Clamp(value, grid.GridSize * 0.75f, grid.GridSize * 1.3f);
			value2 = MathHelper.Clamp(value2, grid.GridSize * 0.9f, grid.GridSize * 1.3f);
			grid.Physics.ApplyDeformation(deformationOffset, value, value2, vector3D, vector3D2, MyDamageType.Bullet, 0f, 0f, 0L);
		}

		private void ExecuteExplosion(MyEntity entity, in Vector3D hitPoint, in Vector3 hitNormal)
		{
			if (Sync.IsServer && entity != null)
			{
				MyHitInfo myHitInfo = default(MyHitInfo);
				myHitInfo.Position = hitPoint;
				myHitInfo.Normal = hitNormal;
				MyHitInfo hitInfo = myHitInfo;
				MyDecals.HandleAddDecal(entity, hitInfo, Vector3.Zero, m_projectileAmmoDefinition.PhysicalMaterial);
				float projectileExplosionRadius = m_projectileAmmoDefinition.ProjectileExplosionRadius;
				BoundingSphereD explosionSphere = new BoundingSphereD(m_position, projectileExplosionRadius);
				MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
				myExplosionInfo.PlayerDamage = m_projectileAmmoDefinition.ProjectileExplosionDamage;
				myExplosionInfo.Damage = m_projectileAmmoDefinition.ProjectileExplosionDamage;
				myExplosionInfo.ExplosionType = MyExplosionTypeEnum.ProjectileExplosion;
				myExplosionInfo.ExplosionSphere = explosionSphere;
				myExplosionInfo.LifespanMiliseconds = 700;
				myExplosionInfo.HitEntity = entity;
				myExplosionInfo.ParticleScale = 1f;
				myExplosionInfo.OwnerEntity = m_ownerEntity;
				myExplosionInfo.Direction = Vector3.Normalize(m_position - m_origin);
				myExplosionInfo.VoxelExplosionCenter = explosionSphere.Center + projectileExplosionRadius * m_directionNormalized * 0.25;
				myExplosionInfo.ExplosionFlags = MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS;
				myExplosionInfo.VoxelCutoutScale = 0.3f;
				myExplosionInfo.PlaySound = true;
				myExplosionInfo.ApplyForceAndDamage = true;
				myExplosionInfo.OriginEntity = m_weapon.EntityId;
				myExplosionInfo.KeepAffectedBlocks = true;
				myExplosionInfo.IgnoreFriendlyFireSetting = false;
				MyExplosionInfo explosionInfo = myExplosionInfo;
				if (entity != null && entity.Physics != null)
				{
					explosionInfo.Velocity = entity.Physics.LinearVelocity;
				}
				MyExplosions.AddExplosion(ref explosionInfo);
			}
		}

		/// <summary>
		/// Draw the projectile but only if desired polyline trail distance can fit in the trajectory (otherwise we will see polyline growing from the origin and it's ugly).
		///  Or draw if this is last draw of this projectile (useful for short-distance shots).
		/// </summary>
		public void Draw()
		{
<<<<<<< HEAD
			if (m_state == MyProjectileStateEnum.Killed || !m_positionChecked)
=======
			if (m_state == MyProjectileStateEnum.KILLED || !m_positionChecked)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			double num = Vector3D.Distance(m_position, m_origin);
			if (!(num > 0.0))
			{
				return;
			}
<<<<<<< HEAD
			Vector3D vector3D = m_position - m_directionNormalized * PROJECTILE_POLYLINE_DESIRED_LENGTH * 0.01666666753590107;
			Vector3D vector3D2 = Vector3D.Normalize(m_position - vector3D);
			double num2 = (float)(int)m_lengthMultiplier * m_projectileAmmoDefinition.ProjectileTrailScale;
=======
			Vector3D vector3D = m_position - m_directionNormalized * 120.0 * 0.01666666753590107;
			Vector3D vector3D2 = Vector3D.Normalize(m_position - vector3D);
			double num2 = LengthMultiplier * m_projectileAmmoDefinition.ProjectileTrailScale;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			num2 *= (double)(MyParticlesManager.Paused ? 0.6f : MyUtils.GetRandomFloat(0.6f, 0.8f));
			if (num < num2)
			{
				num2 = num;
			}
			vector3D = ((m_state != 0 && !(num * num >= m_velocity_Combined.LengthSquared() * 0.01666666753590107 * (double)CHECK_INTERSECTION_INTERVAL)) ? (m_position - ((num - num2) * (double)MyUtils.GetRandomFloat(0f, 1f) + num2) * vector3D2) : (m_position - num2 * vector3D2));
<<<<<<< HEAD
			if (Vector3D.DistanceSquared(vector3D, m_origin) < MINIMUM_DISTANCE_SQRD)
=======
			if (Vector3D.DistanceSquared(vector3D, m_origin) < 4.0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			float num3 = (MyParticlesManager.Paused ? 1f : MyUtils.GetRandomFloat(1f, 2f));
			float num4 = (MyParticlesManager.Paused ? 0.2f : MyUtils.GetRandomFloat(0.2f, 0.3f)) * m_projectileAmmoDefinition.ProjectileTrailScale;
			num4 *= MathHelper.Lerp(0.2f, 0.8f, MySector.MainCamera.Zoom.GetZoomLevel());
			float num5 = 1f;
			float num6 = 10f;
			if (num2 > 0.0)
			{
				if (!string.IsNullOrEmpty(m_projectileAmmoDefinition.ProjectileTrailMaterial))
				{
<<<<<<< HEAD
					MyTransparentGeometry.AddLineBillboard(m_projectileAmmoDefinition.ProjectileTrailMaterialId, new Vector4(m_projectileAmmoDefinition.ProjectileTrailColor * num6, 1f), vector3D, vector3D2, (float)num2, num4);
=======
					MyTransparentGeometry.AddLineBillboard(m_projectileTrailMaterialId, new Vector4(m_projectileAmmoDefinition.ProjectileTrailColor * num6, 1f), vector3D, vector3D2, (float)num2, num4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					MyTransparentGeometry.AddLineBillboard(ID_PROJECTILE_TRAIL_LINE, new Vector4(m_projectileAmmoDefinition.ProjectileTrailColor * num3 * num6, 1f) * num5, vector3D, vector3D2, (float)num2, num4);
				}
			}
		}

		public void Close()
		{
			m_weapon = null;
<<<<<<< HEAD
			m_ownerEntity = null;
			m_ignoredEntities = null;
			if (m_detectorsInTrajectory != null)
			{
				m_detectorsInTrajectory.Clear();
				m_detectorPool.Deallocate(m_detectorsInTrajectory);
				m_detectorsInTrajectory = null;
			}
			m_hkWorld = null;
		}

		public void Remove()
		{
			m_state = MyProjectileStateEnum.Killed;
=======
			OwnerEntity = null;
			m_ignoredEntities = null;
			if (m_safeZonesInTrajectory != null)
			{
				m_safeZonesInTrajectory.Clear();
				m_safeZonePool.Deallocate(m_safeZonesInTrajectory);
				m_safeZonesInTrajectory = null;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
