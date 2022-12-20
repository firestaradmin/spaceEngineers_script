using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Utils;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Weapons
{
	[MyEntityType(typeof(MyObjectBuilder_Missile), true)]
	public sealed class MyMissile : MyAmmoBase, IMyEventProxy, IMyEventOwner, IMyDestroyableObject, IMyMissile, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
	{
		private class Sandbox_Game_Weapons_MyMissile_003C_003EActor : IActivator, IActivator<MyMissile>
		{
			private sealed override object CreateInstance()
			{
				return new MyMissile();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyMissile CreateInstance()
			{
				return new MyMissile();
			}

			MyMissile IActivator<MyMissile>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public static bool DEBUG_DRAW_MISSILE_TRAJECTORY = false;

		public static float SPAWN_DISTANCE = 4f;

		private MyMissileAmmoDefinition m_missileAmmoDefinition;

		private float m_maxTrajectory;

		private MyParticleEffect m_smokeEffect;

		private MyExplosionTypeEnum m_explosionType;

		private MyPhysicalMaterialDefinition.CollisionProperty? m_collisionProperty;

		private MyEntity m_collidedEntity;

		private Vector3D? m_collisionPoint;

		private Vector3 m_collisionNormal;

		private float m_explosionDamage;

		private uint m_collisionShapeKey;

		private long m_owner;

		private readonly float m_smokeEffectOffsetMultiplier = 0.4f;

		private Vector3 m_linearVelocity;

		private MyWeaponPropertiesWrapper m_weaponProperties;

		private long m_launcherId;

		private bool m_reachedMaxSpeed;

		private float m_healthPool;

		private bool m_hasHealthPool;

		private bool m_scheduleFlightSound;

		internal int m_pruningProxyId = -1;

		private readonly MyEntity3DSoundEmitter m_soundEmitter;

		private bool m_removed;

		private Vector3D m_previousPosition;

		private MatrixD? m_resetPosition;

		private Vector3 m_gravity;

		private List<MyLineSegmentOverlapResult<MyEntity>> m_hitRaycastResult;

		public SerializableDefinitionId AmmoMagazineId => m_weaponProperties.AmmoMagazineId;

		public SerializableDefinitionId WeaponDefinitionId => m_weaponProperties.WeaponDefinitionId;

<<<<<<< HEAD
		public MyPhysicalMaterialDefinition CurrentMissileMaterial => MyDefinitionManager.Static.GetPhysicalMaterialDefinition(m_missileAmmoDefinition.PhysicalMaterial.String);

		public float MaxTrajectory
		{
			get
			{
				return m_maxTrajectory;
			}
			set
			{
				m_maxTrajectory = value;
			}
		}
=======
		private bool UseDamageSystem { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public long Owner
		{
			get
			{
				return m_owner;
			}
			set
			{
				m_owner = value;
			}
		}

		public long LauncherId
		{
			get
			{
				return m_launcherId;
			}
			set
			{
				m_launcherId = value;
			}
		}

		public Vector3 LinearVelocity
		{
			get
			{
				return m_linearVelocity;
			}
			set
			{
				m_linearVelocity = value;
			}
		}

		public Vector3D Origin
		{
			get
			{
				return m_origin;
			}
			set
			{
				m_origin = value;
			}
		}

		public MyDefinitionBase WeaponDefinition => m_weaponProperties.WeaponDefinition;

		public MyDefinitionBase AmmoDefinition => m_missileAmmoDefinition;

		public MyDefinitionBase AmmoMagazineDefinition => AmmoMagazineDefinition;

		public MyParticleEffect ParticleEffect
		{
			get
			{
				return m_smokeEffect;
			}
			set
			{
				m_smokeEffect = value;
			}
		}

		public MyExplosionTypeEnum ExplosionType
		{
			get
			{
				return m_explosionType;
			}
			set
			{
				m_explosionType = value;
			}
		}

		public MyEntity CollidedEntity => m_collidedEntity;

		public Vector3D? CollisionPoint => m_collisionPoint;

		public Vector3 CollisionNormal => m_collisionNormal;

		public float ExplosionDamage
		{
			get
			{
				return m_explosionDamage;
			}
			set
			{
				m_explosionDamage = value;
			}
		}

		public float HealthPool
		{
			get
			{
				return m_healthPool;
			}
			set
			{
				m_healthPool = value;
			}
		}

		public bool ShouldExplode
		{
			get
			{
				return m_shouldExplode;
			}
			set
			{
				m_shouldExplode = value;
			}
		}

		public bool HasHealthPool => m_hasHealthPool;

		private bool UseDamageSystem { get; set; }

		float IMyDestroyableObject.Integrity => 1f;

		bool IMyDestroyableObject.UseDamageSystem => UseDamageSystem;

		public MyMissile()
		{
			if (!Sync.IsDedicated)
			{
				m_soundEmitter = new MyEntity3DSoundEmitter(this);
			}
			if (m_soundEmitter != null && MySession.Static.Settings.RealisticSound && MyFakes.ENABLE_NEW_SOUNDS)
			{
				MyCharacter myCharacter;
				Func<bool> entity = () => (myCharacter = MySession.Static.ControlledEntity?.Entity as MyCharacter) != null && myCharacter == m_collidedEntity;
				m_soundEmitter.EmitterMethods[1].Add(entity);
				m_soundEmitter.EmitterMethods[0].Add(entity);
			}
			base.Flags |= EntityFlags.IsNotGamePrunningStructureObject;
			if (Sync.IsDedicated)
			{
				base.Flags &= ~EntityFlags.UpdateRender;
				base.InvalidateOnMove = false;
			}
		}

		public override void Init(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyObjectBuilder_Missile myObjectBuilder_Missile = (MyObjectBuilder_Missile)objectBuilder;
			base.Init(objectBuilder);
			m_weaponProperties = new MyWeaponPropertiesWrapper(myObjectBuilder_Missile.WeaponDefinitionId);
			m_weaponProperties.ChangeAmmoMagazine(myObjectBuilder_Missile.AmmoMagazineId);
			m_missileAmmoDefinition = m_weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
			m_explosionDamage = m_missileAmmoDefinition.MissileExplosionDamage;
			Init(m_weaponProperties, m_missileAmmoDefinition.MissileModelName, m_missileAmmoDefinition.MissileMass, spherePhysics: false, capsulePhysics: true, bulletType: true, Sync.IsServer);
			UseDamageSystem = true;
			m_maxTrajectory = m_missileAmmoDefinition.MaxTrajectory;
			base.SyncFlag = true;
			m_collisionPoint = null;
			m_owner = myObjectBuilder_Missile.Owner;
			m_originEntity = myObjectBuilder_Missile.OriginEntity;
			m_linearVelocity = myObjectBuilder_Missile.LinearVelocity;
			m_launcherId = myObjectBuilder_Missile.LauncherId;
			m_healthPool = m_missileAmmoDefinition.MissileHealthPool;
			m_hasHealthPool = m_healthPool != 0f;
			base.OnPhysicsChanged += OnMissilePhysicsChanged;
		}

		private void OnMissilePhysicsChanged(MyEntity entity)
		{
			if (base.Physics != null && base.Physics.RigidBody != null)
			{
				base.Physics.RigidBody.CallbackLimit = 1;
			}
		}

		public void UpdateData(MyObjectBuilder_EntityBase objectBuilder)
		{
			MyObjectBuilder_Missile myObjectBuilder_Missile = (MyObjectBuilder_Missile)objectBuilder;
			if (objectBuilder.PositionAndOrientation.HasValue)
			{
				MyPositionAndOrientation value = objectBuilder.PositionAndOrientation.Value;
				MatrixD worldMatrix = MatrixD.CreateWorld(value.Position, value.Forward, value.Up);
				base.PositionComp.SetWorldMatrix(ref worldMatrix, null, forceUpdate: false, updateChildren: true, updateLocal: true, skipTeleportCheck: false, forceUpdateAllChildren: false, ignoreAssert: true);
			}
			base.EntityId = myObjectBuilder_Missile.EntityId;
			m_owner = myObjectBuilder_Missile.Owner;
			m_originEntity = myObjectBuilder_Missile.OriginEntity;
			m_linearVelocity = myObjectBuilder_Missile.LinearVelocity;
			m_launcherId = myObjectBuilder_Missile.LauncherId;
			m_healthPool = myObjectBuilder_Missile.HealthPool;
			m_hasHealthPool = m_healthPool != 0f;
			m_collisionPoint = null;
			m_markedToDestroy = false;
			m_removed = false;
		}

		public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
		{
			MyObjectBuilder_Missile obj = (MyObjectBuilder_Missile)base.GetObjectBuilder(copy);
			obj.LinearVelocity = base.Physics.LinearVelocity;
			obj.AmmoMagazineId = m_weaponProperties.AmmoMagazineId;
			obj.WeaponDefinitionId = m_weaponProperties.WeaponDefinitionId;
			obj.Owner = m_owner;
			obj.OriginEntity = m_originEntity;
			obj.LauncherId = m_launcherId;
			obj.HealthPool = m_healthPool;
			m_hasHealthPool = m_healthPool != 0f;
			return obj;
		}

		public static MyObjectBuilder_Missile PrepareBuilder(MyWeaponPropertiesWrapper weaponProperties, Vector3D position, Vector3 initialVelocity, Vector3 direction, long owner, long originEntity, long launcherId)
		{
			position = GetSpawnPoint(position, direction);
			MyObjectBuilder_Missile myObjectBuilder_Missile = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_Missile>();
			myObjectBuilder_Missile.LinearVelocity = initialVelocity;
			myObjectBuilder_Missile.AmmoMagazineId = weaponProperties.AmmoMagazineId;
			myObjectBuilder_Missile.WeaponDefinitionId = weaponProperties.WeaponDefinitionId;
			myObjectBuilder_Missile.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
<<<<<<< HEAD
			myObjectBuilder_Missile.PositionAndOrientation = new MyPositionAndOrientation(position, direction, Vector3.CalculatePerpendicularVector(direction));
=======
			Vector3D vector3D = position + direction * 4.0;
			if (!MyPhysics.CastRay(position, vector3D).HasValue)
			{
				position = vector3D;
			}
			myObjectBuilder_Missile.PositionAndOrientation = new MyPositionAndOrientation(position, direction, Vector3D.CalculatePerpendicularVector(direction));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myObjectBuilder_Missile.Owner = owner;
			myObjectBuilder_Missile.OriginEntity = originEntity;
			myObjectBuilder_Missile.LauncherId = launcherId;
			myObjectBuilder_Missile.EntityId = MyEntityIdentifier.AllocateId();
			MyMissileAmmoDefinition currentAmmoDefinitionAs = weaponProperties.GetCurrentAmmoDefinitionAs<MyMissileAmmoDefinition>();
			myObjectBuilder_Missile.HealthPool = currentAmmoDefinitionAs.MissileHealthPool;
			return myObjectBuilder_Missile;
		}

		public static Vector3D GetSpawnPoint(Vector3D position, Vector3D direction, bool raycast = true)
		{
			Vector3D vector3D = position + Vector3D.ClampToSphere(direction, SPAWN_DISTANCE);
			if (raycast && MyPhysics.CastRay(position, vector3D).HasValue)
			{
				return position;
			}
			return vector3D;
		}

		/// <summary>
		/// This method really starts the missile. IMPORTANT: Direction vector must be normalized!
		/// </summary>
		/// <param name="source"></param>
		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			m_shouldExplode = false;
			Start(base.PositionComp.GetPosition(), m_linearVelocity, base.WorldMatrix.Forward);
			m_previousPosition = m_origin;
			if (m_physicsEnabled)
			{
				base.Physics.RigidBody.MaxLinearVelocity = m_missileAmmoDefinition.DesiredSpeed;
				base.Physics.RigidBody.Layer = 8;
				base.Physics.CanUpdateAccelerations = false;
				if (m_hasHealthPool)
				{
					base.Physics.RigidBody.InertiaTensor = new Matrix(10000f, 0f, 0f, 0f, 10000f, 0f, 0f, 0f, 10000f);
					base.Physics.RigidBody.Restitution = 0f;
				}
			}
			m_explosionType = MyExplosionTypeEnum.CUSTOM;
			if (!Sync.IsDedicated)
			{
				if (m_weaponDefinition.WeaponAmmoDatas[1].FlightSound != null)
				{
					m_scheduleFlightSound = true;
				}
				MatrixD worldMatrixRef = base.PositionComp.WorldMatrixRef;
				worldMatrixRef.Translation -= worldMatrixRef.Forward * m_smokeEffectOffsetMultiplier;
				Vector3D worldPosition = worldMatrixRef.Translation;
				string text = ((m_missileAmmoDefinition.MissileTrailEffect == null) ? "Smoke_Missile" : m_missileAmmoDefinition.MissileTrailEffect);
				if (text != string.Empty)
				{
					MyParticlesManager.TryCreateParticleEffect(text, ref MatrixD.Identity, ref worldPosition, base.Render.GetRenderObjectID(), out m_smokeEffect);
				}
<<<<<<< HEAD
=======
				MatrixD worldMatrixRef = base.PositionComp.WorldMatrixRef;
				worldMatrixRef.Translation -= worldMatrixRef.Forward * m_smokeEffectOffsetMultiplier;
				Vector3D worldPosition = worldMatrixRef.Translation;
				MyParticlesManager.TryCreateParticleEffect("Smoke_Missile", ref MatrixD.Identity, ref worldPosition, base.Render.GetRenderObjectID(), out m_smokeEffect);
				(MyEntities.GetEntityById(m_launcherId) as IMyMissileGunObject)?.MissileShootEffect();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_gravity = MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.PositionComp.GetPosition());
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (m_smokeEffect != null)
			{
				m_smokeEffect.Stop(instant: false);
				m_smokeEffect = null;
			}
			if (m_soundEmitter != null)
			{
				m_soundEmitter.StopSound(forced: true);
			}
			base.OnPhysicsChanged -= OnMissilePhysicsChanged;
		}

		/// <summary>
		/// Updates resource.
		/// </summary>
		public override void UpdateBeforeSimulation()
		{
			if (m_scheduleFlightSound && m_soundEmitter != null && !m_soundEmitter.IsPlaying)
			{
				m_soundEmitter.StopSound(forced: true);
				m_soundEmitter.ClearSecondaryCue();
				MySoundPair flightSound = m_weaponDefinition.WeaponAmmoDatas[1].FlightSound;
				m_soundEmitter.PlaySound(flightSound, stopPrevious: true);
				m_scheduleFlightSound = false;
			}
			if (m_shouldExplode)
			{
				ExecuteExplosion();
				return;
			}
			base.UpdateBeforeSimulation();
			if (m_physicsEnabled)
			{
				base.Physics.AngularVelocity = Vector3.Zero;
			}
			if (!m_reachedMaxSpeed)
			{
				m_linearVelocity = GetMissileSpeedNextFrame(m_linearVelocity, base.PositionComp.WorldMatrixRef.Forward, m_missileAmmoDefinition, out m_reachedMaxSpeed);
			}
			if (MyFakes.ENABLE_MISSILES_NATURAL_GRAVITY && m_missileAmmoDefinition.MissileGravityEnabled)
			{
<<<<<<< HEAD
				float num = 0.0166666675f * MyFakes.SIMULATION_SPEED;
				m_linearVelocity += m_gravity * num;
=======
				m_linearVelocity += base.PositionComp.WorldMatrixRef.Forward * m_missileAmmoDefinition.MissileAcceleration * 0.01666666753590107;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			Vector3D vector3D = (m_previousPosition = base.PositionComp.GetPosition());
			Vector3 velocity = m_linearVelocity;
			if (m_physicsEnabled)
			{
				base.Physics.LinearVelocity = velocity;
			}
			else
			{
				base.PositionComp.SetPosition(vector3D + velocity * 0.0166666675f);
			}
			if (Vector3D.DistanceSquared(vector3D, m_origin) >= (double)(m_maxTrajectory * m_maxTrajectory) || (m_hasHealthPool && m_healthPool <= 0f))
			{
				m_explosionType = MyExplosionTypeEnum.MISSILE_EXPLOSION;
				MarkForExplosion();
			}
			MyMissiles.Static.OnMissileHasMoved(this, ref velocity);
			if (m_soundEmitter != null)
			{
				m_soundEmitter.Update();
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (m_resetPosition.HasValue)
			{
				MatrixD worldMatrix = m_resetPosition.Value;
				base.PositionComp.SetWorldMatrix(ref worldMatrix);
				if (m_physicsEnabled)
				{
					base.Physics.LinearVelocity = m_linearVelocity;
				}
				m_resetPosition = null;
			}
			Vector3D position = base.PositionComp.GetPosition();
			if (DEBUG_DRAW_MISSILE_TRAJECTORY)
			{
				MyRenderProxy.DebugDrawLine3D(m_previousPosition, position, Color.Yellow, Color.Yellow, depthRead: true, persistent: true);
				MyRenderProxy.DebugDrawPoint(position, Color.Green, depthRead: true, persistent: true);
			}
		}

		public static Vector3 GetMissileSpeedNextFrame(Vector3 currentSpeed, Vector3 forwardVector, MyMissileAmmoDefinition ammoDefinition, out bool reachedMaxSpeed)
		{
			reachedMaxSpeed = true;
			if (currentSpeed.Length() < ammoDefinition.DesiredSpeed)
			{
				if (ammoDefinition.MissileSkipAcceleration)
				{
					currentSpeed = Vector3.ClampToSphere(forwardVector, ammoDefinition.DesiredSpeed);
				}
				else
				{
					currentSpeed += forwardVector * ammoDefinition.MissileAcceleration * 0.0166666675f;
					reachedMaxSpeed = false;
				}
			}
			if (currentSpeed.Length() > ammoDefinition.DesiredSpeed)
			{
				Vector3.ClampToSphere(ref currentSpeed, ammoDefinition.DesiredSpeed);
				reachedMaxSpeed = true;
			}
			return currentSpeed;
		}

		private void ExecuteExplosion()
		{
			if (!Sync.IsServer)
			{
				Return();
				return;
			}
			PlaceDecal();
			float missileExplosionRadius = m_missileAmmoDefinition.MissileExplosionRadius;
			BoundingSphereD explosionSphere = new BoundingSphereD(base.PositionComp.GetPosition(), missileExplosionRadius);
			MyEntity myEntity = null;
			MyIdentity myIdentity = Sync.Players.TryGetIdentity(m_owner);
			if (myIdentity != null)
			{
				myEntity = myIdentity.Character;
			}
			else
			{
				MyEntity entity = null;
				MyEntities.TryGetEntityById(m_owner, out entity);
				myEntity = entity;
			}
			MyEntity entityById = MyEntities.GetEntityById(m_launcherId, allowClosed: true);
			MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
			myExplosionInfo.PlayerDamage = 0f;
			myExplosionInfo.Damage = m_explosionDamage;
			myExplosionInfo.ExplosionType = m_explosionType;
			myExplosionInfo.ExplosionSphere = explosionSphere;
			myExplosionInfo.LifespanMiliseconds = 700;
			myExplosionInfo.HitEntity = m_collidedEntity;
			myExplosionInfo.ParticleScale = 1f;
			myExplosionInfo.OwnerEntity = myEntity;
			myExplosionInfo.CustomEffect = (m_collisionProperty.HasValue ? m_collisionProperty.Value.ParticleEffect : null);
			myExplosionInfo.CustomSound = (m_collisionProperty.HasValue ? m_collisionProperty.Value.Sound : null);
			myExplosionInfo.EffectHitAngle = (m_collisionProperty.HasValue ? m_collisionProperty.Value.EffectHitAngle : MyObjectBuilder_MaterialPropertiesDefinition.EffectHitAngle.None);
			myExplosionInfo.Direction = Vector3.Normalize(base.PositionComp.GetPosition() - m_origin);
			myExplosionInfo.DirectionNormal = m_collisionNormal;
			myExplosionInfo.VoxelExplosionCenter = explosionSphere.Center + missileExplosionRadius * base.WorldMatrix.Forward * 0.25;
			myExplosionInfo.ExplosionFlags = MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION | MyExplosionFlags.CREATE_PARTICLE_DEBRIS;
			myExplosionInfo.VoxelCutoutScale = 0.3f;
			myExplosionInfo.PlaySound = true;
			myExplosionInfo.ApplyForceAndDamage = true;
			myExplosionInfo.OriginEntity = m_originEntity;
			myExplosionInfo.KeepAffectedBlocks = true;
			myExplosionInfo.IgnoreFriendlyFireSetting = entityById == null || !(entityById is MyAutomaticRifleGun);
			MyExplosionInfo explosionInfo = myExplosionInfo;
			if (m_collidedEntity != null && m_collidedEntity.Physics != null)
			{
				explosionInfo.Velocity = m_collidedEntity.Physics.LinearVelocity;
			}
			if (!m_markedToDestroy)
			{
				explosionInfo.ExplosionFlags |= MyExplosionFlags.CREATE_PARTICLE_EFFECT;
			}
			MyExplosions.AddExplosion(ref explosionInfo);
			if (m_collidedEntity != null && !(m_collidedEntity is MyAmmoBase) && m_collidedEntity.Physics != null && !m_collidedEntity.Physics.IsStatic)
			{
				m_collidedEntity.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, 100f * base.Physics.LinearVelocity, m_collisionPoint, null);
			}
			Return();
		}

		private void Done()
		{
			if (m_collidedEntity != null)
			{
				m_collidedEntity.Unpin();
				m_collidedEntity = null;
			}
		}

		private void Return()
		{
			Done();
			MyMissiles.Static.Return(this);
		}

		private void PlaceDecal()
		{
			if (m_collidedEntity != null && m_collisionPoint.HasValue)
			{
				MyHitInfo myHitInfo = default(MyHitInfo);
				myHitInfo.Position = m_collisionPoint.Value;
				myHitInfo.Normal = m_collisionNormal;
				MyHitInfo hitInfo = myHitInfo;
				MyDecals.HandleAddDecal(m_collidedEntity, hitInfo, Vector3.Zero, m_missileAmmoDefinition.PhysicalMaterial);
			}
		}

		private void MarkForExplosion(bool force = true)
		{
			if (m_markedToDestroy)
			{
				Return();
			}
			if (force || (m_hasHealthPool && m_healthPool <= 0f))
			{
				Explode();
			}
			else if (m_hasHealthPool && m_healthPool > 0f)
			{
				HitEntity();
			}
		}

		private void Explode()
		{
			m_shouldExplode = true;
			if (Sync.IsServer && !m_removed)
			{
				(MyEntities.GetEntityById(m_launcherId) as IMyMissileGunObject)?.RemoveMissile(base.EntityId);
				m_removed = true;
			}
		}

		private void HitEntity()
		{
			if (!Sync.IsServer || !m_collisionPoint.HasValue || m_collidedEntity == null)
			{
				return;
			}
			MyCubeGrid myCubeGrid = m_collidedEntity as MyCubeGrid;
			MyCubeBlock myCubeBlock = m_collidedEntity as MyCubeBlock;
			MySlimBlock mySlimBlock = null;
			if (myCubeBlock != null)
			{
				myCubeGrid = myCubeBlock.CubeGrid;
				mySlimBlock = myCubeBlock.SlimBlock;
			}
			else if (myCubeGrid != null)
			{
				mySlimBlock = myCubeGrid.GetTargetedBlock(m_collisionPoint.Value - 0.001f * m_collisionNormal);
				if (mySlimBlock != null)
				{
					myCubeBlock = mySlimBlock.FatBlock;
				}
			}
			if (myCubeGrid != null && myCubeGrid.Physics != null && myCubeGrid.Physics.Enabled)
			{
				if (!myCubeGrid.BlocksDestructionEnabled || (m_originEntity == myCubeGrid.EntityId && !MySession.Static.Settings.EnableTurretsFriendlyFire))
				{
					m_healthPool = 0f;
					Explode();
				}
				else
				{
					Vector3D vector3D = base.PositionComp.GetPosition() + m_linearVelocity * 0.0166666675f;
					if (DEBUG_DRAW_MISSILE_TRAJECTORY)
					{
						MyRenderProxy.DebugDrawLine3D(m_collisionPoint.Value, vector3D, Color.Yellow, Color.Red, depthRead: true, persistent: true);
					}
					using (MyUtils.ReuseCollection(ref m_hitRaycastResult))
					{
						LineD ray = new LineD(m_collisionPoint.Value, vector3D);
						MyGamePruningStructure.GetTopmostEntitiesOverlappingRay(ref ray, m_hitRaycastResult);
						if (m_hitRaycastResult.Count > 1)
						{
							HitMultipleGrids(m_hitRaycastResult, vector3D);
						}
						else
						{
							HitGrid(myCubeGrid, vector3D);
						}
					}
				}
			}
			m_collidedEntity.Unpin();
			m_collidedEntity = null;
		}

		private void HitMultipleGrids(List<MyLineSegmentOverlapResult<MyEntity>> m_hitRaycastResult, Vector3D nextPosition)
		{
			List<MyCube> list = new List<MyCube>();
			foreach (MyLineSegmentOverlapResult<MyEntity> item in m_hitRaycastResult)
			{
				MyCubeGrid myCubeGrid;
				if ((myCubeGrid = item.Element as MyCubeGrid) != null)
				{
					List<MyCube> collection = myCubeGrid.RayCastBlocksAllOrdered(m_collisionPoint.Value, nextPosition);
					list.AddRange(collection);
				}
			}
			list.SortNoAlloc((MyCube x, MyCube y) => Vector3D.DistanceSquared(m_collisionPoint.Value, x.CubeBlock.WorldPosition).CompareTo(Vector3D.DistanceSquared(m_collisionPoint.Value, y.CubeBlock.WorldPosition)));
			HitCubes(list);
		}

		private void HitGrid(MyCubeGrid grid, Vector3D nextPosition)
		{
			List<MyCube> cubeList = grid.RayCastBlocksAllOrdered(m_collisionPoint.Value, nextPosition);
			HitCubes(cubeList);
		}

		private void HitCubes(List<MyCube> cubeList)
		{
			MySlimBlock mySlimBlock = null;
			HashSet<long> hashSet = new HashSet<long>();
			foreach (MyCube cube in cubeList)
			{
				mySlimBlock = cube.CubeBlock;
				if (hashSet.Contains(mySlimBlock.UniqueId))
				{
					continue;
				}
				hashSet.Add(mySlimBlock.UniqueId);
				if (mySlimBlock != null && (cube.CubeBlock.CubeGrid.BlocksDestructionEnabled || mySlimBlock.ForceBlockDestructible))
				{
					float healthPool = m_healthPool;
					MyHitInfo myHitInfo = default(MyHitInfo);
					myHitInfo.Normal = m_collisionNormal;
					myHitInfo.Position = m_collisionPoint.Value;
					myHitInfo.Velocity = m_linearVelocity;
					myHitInfo.ShapeKey = m_collisionShapeKey;
					MyHitInfo value = myHitInfo;
					float num = mySlimBlock.DoMaximumDamage(m_healthPool, value, m_launcherId);
					if (float.IsPositiveInfinity(num))
					{
						m_healthPool = 0f;
					}
					else
					{
						m_healthPool -= num;
					}
					if (m_healthPool <= 0f || m_healthPool == healthPool || mySlimBlock.Integrity > 0f)
					{
						base.PositionComp.SetPosition(mySlimBlock.WorldPosition);
						Explode();
						break;
					}
				}
			}
			if (!m_removed && cubeList.Count != 0)
			{
				m_resetPosition = base.PositionComp.WorldMatrix;
			}
			else
			{
				m_resetPosition = null;
			}
		}

		public override void MarkForDestroy()
		{
			Return();
		}

		/// <summary>
		/// Kills this missile. Must be called at her end (after explosion or timeout)
		/// This method must be called when this object dies or is removed
		/// E.g. it removes lights, sounds, etc
		/// </summary>
		protected override void Closing()
		{
			base.Closing();
			Done();
		}

		protected override void OnContactStart(ref MyPhysics.MyContactPointEvent value)
		{
			if (base.MarkedForClose || m_collidedEntity != null)
			{
				return;
			}
			MyEntity myEntity = value.ContactPointEvent.GetOtherEntity(this) as MyEntity;
			if (myEntity == null)
			{
				return;
			}
			myEntity.Pin();
<<<<<<< HEAD
			int bodyIdx = 0;
			MyPhysicsBody otherPhysicsBody = value.ContactPointEvent.GetOtherPhysicsBody(this);
			if (otherPhysicsBody != null)
			{
				VRage.ModAPI.IMyEntity otherEntity = value.ContactPointEvent.GetOtherEntity(this);
				LineD line = new LineD(otherEntity.GetPosition(), value.Position);
				Vector3D hitPosition = value.Position;
				MyProjectile.GetSurfaceAndMaterial(otherEntity, ref line, ref hitPosition, value.ContactPointEvent.GetShapeKey(1), out var _, out var materialType);
				if (CurrentMissileMaterial != null && CurrentMissileMaterial.CollisionProperties.ContainsKey(MyMaterialPropertiesHelper.CollisionType.Hit) && CurrentMissileMaterial.CollisionProperties[MyMaterialPropertiesHelper.CollisionType.Hit].ContainsKey(materialType))
				{
					m_collisionProperty = CurrentMissileMaterial.CollisionProperties[MyMaterialPropertiesHelper.CollisionType.Hit][materialType];
				}
				if (otherPhysicsBody.Entity == this)
				{
					bodyIdx = 1;
				}
			}
			if (m_collisionProperty.HasValue && !string.IsNullOrEmpty(m_collisionProperty.Value.ParticleEffect))
			{
				m_explosionType = MyExplosionTypeEnum.CUSTOM;
			}
			else
			{
				m_explosionType = MyExplosionTypeEnum.MISSILE_EXPLOSION;
			}
			m_collidedEntity = myEntity;
			m_collisionPoint = value.Position;
			m_collisionNormal = value.Normal;
			m_collisionShapeKey = value.ContactPointEvent.GetShapeKey(bodyIdx);
			bool forceExplosion = !m_hasHealthPool || m_collidedEntity is MyVoxelBase || m_collidedEntity is MyCharacter || m_collidedEntity is MyFloatingObject || m_collidedEntity is MyInventoryBagEntity;
			MyMissiles.Static.TriggerCollision(this);
			if (!Sync.IsServer)
			{
				PlaceDecal();
				return;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MarkForExplosion(forceExplosion);
=======
			m_collidedEntity = myEntity;
			m_collisionPoint = value.Position;
			m_collisionNormal = value.Normal;
			if (!Sync.IsServer)
			{
				PlaceDecal();
				return;
			}
			MySandboxGame.Static.Invoke(delegate
			{
				MarkForExplosion();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}, "MyMissile - collision invoke");
		}

		private void DoDamage(float damage, MyStringHash damageType, bool sync, long attackerId)
		{
			if (sync)
			{
				if (Sync.IsServer)
				{
					MySyncDamage.DoDamageSynced(this, damage, damageType, attackerId);
				}
				return;
			}
			if (UseDamageSystem)
			{
				MyDamageSystem.Static.RaiseDestroyed(this, new MyDamageInformation(isDeformation: false, damage, damageType, attackerId));
			}
			MarkForExplosion();
		}

		public bool IsCharacterIdFriendly(long charId)
		{
			MyRelationsBetweenPlayers relationPlayerPlayer = MyIDModule.GetRelationPlayerPlayer(Owner, charId);
			if (relationPlayerPlayer == MyRelationsBetweenPlayers.Self || relationPlayerPlayer == MyRelationsBetweenPlayers.Allies)
			{
				return true;
			}
			return false;
		}

		void IMyDestroyableObject.OnDestroy()
		{
		}

<<<<<<< HEAD
		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId, long realHitEntityId, bool shouldDetonateAmmo)
=======
		bool IMyDestroyableObject.DoDamage(float damage, MyStringHash damageType, bool sync, MyHitInfo? hitInfo, long attackerId, long realHitEntityId = 0L)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			DoDamage(damage, damageType, sync, attackerId);
			return true;
		}

		public void Destroy()
		{
			MyMissiles.Static.Remove(base.EntityId);
		}
	}
}
