using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
<<<<<<< HEAD
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.ModAPI;
using VRage.Utils;
=======
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRageMath;
using VRageMath.Spatial;

namespace Sandbox.Game.Weapons
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
	public class MyProjectiles : MySessionComponentBase, IMyProjectiles
	{
<<<<<<< HEAD
		private static MyProjectiles m_static;

		private int m_projectileCount;

		private MyProjectile[] m_projectiles;

		private object m_projectileCreationLock = new object();

		private readonly List<Tuple<int, HitInterceptor>> m_interceptors = new List<Tuple<int, HitInterceptor>>();

		private readonly MyDynamicAABBTreeD m_detectors = new MyDynamicAABBTreeD();

		private readonly Dictionary<IMyProjectileDetector, int> m_detectorsIds = new Dictionary<IMyProjectileDetector, int>();

		internal bool PhysicsDirty = true;

		public static MyProjectiles Static => m_static;

		public event OnProjectileAddedRemoved OnProjectileAdded;

		public event OnProjectileAddedRemoved OnProjectileRemoving;
=======
		private int m_projectileCount;

		private MyProjectile[] m_projectiles;

		private static MyProjectiles m_static;

		private object m_projectileCreationLock = new object();

		public static MyProjectiles Static => m_static;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public override void LoadData()
		{
			m_static = this;
<<<<<<< HEAD
			if (MyFakes.PROJECTILES_APPLY_RAYCAST_OPTIMIZATION)
			{
				MyClusterTree clusters = MyPhysics.Clusters;
				clusters.EntityAdded = (Action<long, int>)Delegate.Combine(clusters.EntityAdded, (Action<long, int>)delegate
				{
					PhysicsDirty = true;
				});
				MyClusterTree clusters2 = MyPhysics.Clusters;
				clusters2.EntityRemoved = (Action<long, int>)Delegate.Combine(clusters2.EntityRemoved, (Action<long, int>)delegate
				{
					PhysicsDirty = true;
				});
				MyClusterTree clusters3 = MyPhysics.Clusters;
				clusters3.OnAfterClusterCreated = (Action<int, BoundingBoxD>)Delegate.Combine(clusters3.OnAfterClusterCreated, (Action<int, BoundingBoxD>)delegate
				{
					PhysicsDirty = true;
				});
				MyClusterTree clusters4 = MyPhysics.Clusters;
				clusters4.OnClusterRemoved = (Action<object, int>)Delegate.Combine(clusters4.OnClusterRemoved, (Action<object, int>)delegate
				{
					PhysicsDirty = true;
				});
				MyPhysics.Clusters.OnClustersReordered = delegate
				{
					PhysicsDirty = true;
				};
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override void UnloadData()
		{
			m_static = null;
<<<<<<< HEAD
		}

		private ref MyProjectile AllocatedProjectile()
		{
			m_projectileCount++;
			ArrayExtensions.EnsureCapacity(ref m_projectiles, m_projectileCount, 2f);
			return ref m_projectiles[m_projectileCount - 1];
		}

		private bool CanSpawnProjectile(MyEntity owner, out ulong owningPlayerId)
		{
			owningPlayerId = MySession.Static.Players.GetControllingPlayer(owner)?.Id.SteamId ?? 0;
			return MySessionComponentSafeZones.CanPerformAction(MySafeZoneAction.Shooting, owningPlayerId);
		}

		public void Add(MyWeaponPropertiesWrapper props, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, IMyGunBaseUser user, MyEntity owner)
		{
			MyEntity myEntity = user.Owner ?? ((user.IgnoreEntities != null && user.IgnoreEntities.Length != 0) ? user.IgnoreEntities[0] : null);
			bool supressHitIndicator = false;
			if (!CanSpawnProjectile(myEntity, out var owningPlayerId))
			{
				return;
			}
			MyLargeTurretBase myLargeTurretBase;
			if ((myLargeTurretBase = user as MyLargeTurretBase) != null)
			{
				if (myLargeTurretBase.ControllerInfo == null || myLargeTurretBase.ControllerInfo.Controller == null)
				{
					supressHitIndicator = true;
				}
				else
				{
					owningPlayerId = myLargeTurretBase.ControllerInfo.Controller.Player.Id.SteamId;
				}
			}
			AddInternal(props.WeaponDefinition, props.GetCurrentAmmoDefinitionAs<MyProjectileAmmoDefinition>(), origin, initialVelocity, directionNormalized, myEntity, owner, user.Weapon, user.IgnoreEntities, supressHitIndicator, owningPlayerId);
		}

		public void AddInternal(MyWeaponDefinition weaponDefinition, MyProjectileAmmoDefinition ammoDefinition, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, MyEntity ownerEntity, MyEntity ownerEntityAbsolute, MyEntity weapon, MyEntity[] ignoredEntities, bool supressHitIndicator = false, ulong owningPlayer = 0uL)
		{
			ref MyProjectile reference = ref AllocatedProjectile();
			lock (m_projectileCreationLock)
			{
				reference.Start(m_projectileCount - 1, ammoDefinition, weaponDefinition, ignoredEntities, origin, initialVelocity, directionNormalized, weapon, ownerEntity, ownerEntityAbsolute, owningPlayer, supressHitIndicator);
			}
			if (this.OnProjectileAdded != null)
			{
				MyProjectileInfo projectile = new MyProjectileInfo(reference.Index, reference.Origin, reference.Position, reference.Velocity, reference.CachedGravity, reference.MaxTrajectory, reference.ProjectileAmmoDefinition, reference.WeaponDefinition, reference.OwnerEntity, reference.OwnerEntityAbsolute, reference.OwningPlayer);
				this.OnProjectileAdded?.Invoke(ref projectile, m_projectileCount - 1);
=======
		}

		private ref MyProjectile AllocatedProjectile()
		{
			m_projectileCount++;
			ArrayExtensions.EnsureCapacity(ref m_projectiles, m_projectileCount, 2f);
			return ref m_projectiles[m_projectileCount - 1];
		}

		private bool CanSpawnProjectile(MyEntity owner, out ulong owningPlayerId)
		{
			owningPlayerId = MySession.Static.Players.GetControllingPlayer(owner)?.Id.SteamId ?? 0;
			return MySessionComponentSafeZones.CanPerformAction(MySafeZoneAction.Shooting, owningPlayerId);
		}

		public void Add(MyWeaponPropertiesWrapper props, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, IMyGunBaseUser user, MyEntity owner)
		{
			MyEntity myEntity = user.Owner ?? ((user.IgnoreEntities != null && user.IgnoreEntities.Length != 0) ? user.IgnoreEntities[0] : null);
			bool supressHitIndicator = false;
			if (!CanSpawnProjectile(myEntity, out var owningPlayerId))
			{
				return;
			}
			MyLargeTurretBase myLargeTurretBase;
			if ((myLargeTurretBase = user as MyLargeTurretBase) != null)
			{
				if (myLargeTurretBase.ControllerInfo == null || myLargeTurretBase.ControllerInfo.Controller == null)
				{
					supressHitIndicator = true;
				}
				else
				{
					owningPlayerId = myLargeTurretBase.ControllerInfo.Controller.Player.Id.SteamId;
				}
			}
			lock (m_projectileCreationLock)
			{
				ref MyProjectile reference = ref AllocatedProjectile();
				reference.Start(props.GetCurrentAmmoDefinitionAs<MyProjectileAmmoDefinition>(), props.WeaponDefinition, user.IgnoreEntities, origin, initialVelocity, directionNormalized, user.Weapon, supressHitIndicator);
				reference.OwnerEntity = myEntity;
				reference.OwnerEntityAbsolute = owner;
				reference.OwningPlayer = owningPlayerId;
			}
		}

		public void AddShrapnel(MyProjectileAmmoDefinition ammoDefinition, MyEntity[] ignoreEntities, Vector3 origin, Vector3 initialVelocity, Vector3 directionNormalized, bool groupStart, float thicknessMultiplier, float trailProbability, MyEntity weapon, MyEntity ownerEntity = null, float projectileCountMultiplier = 1f)
		{
			if (ownerEntity == null && !ignoreEntities.IsNullOrEmpty())
			{
				ownerEntity = ignoreEntities[0];
			}
			if (CanSpawnProjectile(ownerEntity, out var owningPlayerId))
			{
				ref MyProjectile reference = ref AllocatedProjectile();
				reference.Start(ammoDefinition, null, ignoreEntities, origin, initialVelocity, directionNormalized, weapon);
				reference.OwnerEntity = ownerEntity;
				reference.OwningPlayer = owningPlayerId;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		///  Update active projectiles. If projectile dies/timeouts, remove it from the list.
		/// </summary>
		public override void UpdateBeforeSimulation()
		{
			MyProjectile.CHECK_INTERSECTION_INTERVAL = (MySession.Static.HighSimulationQuality ? 5 : 15);
			for (int i = 0; i < m_projectileCount; i++)
			{
				ref MyProjectile reference = ref m_projectiles[i];
				if (!reference.Update())
				{
<<<<<<< HEAD
					if (this.OnProjectileRemoving != null)
					{
						MyProjectileInfo projectile = new MyProjectileInfo(reference.Index, reference.Origin, reference.Position, reference.Velocity, reference.CachedGravity, reference.MaxTrajectory, reference.ProjectileAmmoDefinition, reference.WeaponDefinition, reference.OwnerEntity, reference.OwnerEntityAbsolute, reference.OwningPlayer);
						this.OnProjectileRemoving?.Invoke(ref projectile, i);
					}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					reference.Close();
					int num = m_projectileCount - 1;
					ref MyProjectile reference2 = ref m_projectiles[num];
					if (i != num)
					{
						reference = reference2;
					}
					reference2 = default(MyProjectile);
<<<<<<< HEAD
					reference.Index = i;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					i--;
					m_projectileCount--;
				}
			}
<<<<<<< HEAD
			PhysicsDirty = false;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void Draw()
		{
			for (int i = 0; i < m_projectileCount; i++)
<<<<<<< HEAD
			{
				m_projectiles[i].Draw();
			}
		}

		public void AddOnHitInterceptor(int priority, HitInterceptor interceptor)
		{
			Tuple<int, HitInterceptor> item = new Tuple<int, HitInterceptor>(priority, interceptor);
			m_interceptors.Add(item);
			m_interceptors.Sort((Tuple<int, HitInterceptor> x, Tuple<int, HitInterceptor> y) => x.Item1 - y.Item1);
		}

		public void RemoveOnHitInterceptor(HitInterceptor interceptor)
		{
			int num = m_interceptors.FindIndex((Tuple<int, HitInterceptor> x) => x.Item2 == interceptor);
			if (num >= 0)
			{
				m_interceptors.RemoveAt(num);
			}
		}

		public MyProjectileInfo GetProjectile(int index)
		{
			ref MyProjectile reference = ref m_projectiles[index];
			return new MyProjectileInfo(reference.Index, reference.Origin, reference.Position, reference.Velocity, reference.CachedGravity, reference.MaxTrajectory, reference.ProjectileAmmoDefinition, reference.WeaponDefinition, reference.OwnerEntity, reference.OwnerEntityAbsolute, reference.OwningPlayer);
		}

		public void MarkProjectileForDestroy(int index)
		{
			if (index >= 0 && index < m_projectileCount)
			{
				m_projectiles[index].Remove();
			}
		}

		public int GetAllProjectileCount()
		{
			return m_projectileCount;
		}

		public void Add(MyDefinitionBase weaponDefinition, MyDefinitionBase ammoDefinition, Vector3D origin, Vector3 initialVelocity, Vector3 directionNormalized, MyEntity owningEntity, MyEntity owningEntityAbsolute, MyEntity weapon, MyEntity[] ignoredEntities, bool supressHitIndicator = false, ulong owningPlayer = 0uL)
		{
			AddInternal(weaponDefinition as MyWeaponDefinition, ammoDefinition as MyProjectileAmmoDefinition, origin, initialVelocity, directionNormalized, owningEntity, owningEntityAbsolute, weapon, ignoredEntities, supressHitIndicator, owningPlayer);
		}

		internal void InvokeOnHit(ref MyProjectile projectile, ref MyProjectileHitInfo hitInfo)
		{
			if (m_interceptors.Count <= 0)
			{
				return;
			}
			MyProjectileInfo projectile2 = new MyProjectileInfo(projectile.Index, projectile.Origin, projectile.Position, projectile.Velocity, projectile.CachedGravity, projectile.MaxTrajectory, projectile.ProjectileAmmoDefinition, projectile.WeaponDefinition, projectile.OwnerEntity, projectile.OwnerEntityAbsolute, projectile.OwningPlayer);
			foreach (Tuple<int, HitInterceptor> interceptor in m_interceptors)
			{
				interceptor.Item2(ref projectile2, ref hitInfo);
			}
		}

		public void AddHitDetector(IMyProjectileDetector detector)
		{
			if (!m_detectorsIds.ContainsKey(detector))
			{
				BoundingBoxD aabb = detector.DetectorAABB;
				int value = m_detectors.AddProxy(ref aabb, detector, 0u);
				m_detectorsIds.Add(detector, value);
			}
		}

		public void RemoveHitDetector(IMyProjectileDetector detector)
		{
			if (m_detectorsIds.TryGetValue(detector, out var value))
			{
				m_detectors.RemoveProxy(value);
				m_detectorsIds.Remove(detector);
			}
		}

		public void GetSurfaceAndMaterial(IMyEntity entity, ref LineD line, ref Vector3D hitPosition, uint shapeKey, out MySurfaceImpactEnum surfaceImpact, out MyStringHash materialType)
		{
			MyProjectile.GetSurfaceAndMaterial(entity, ref line, ref hitPosition, shapeKey, out surfaceImpact, out materialType);
		}

		internal void OverlapDetectorsBoundingSphere(ref BoundingSphereD aabb, List<IMyProjectileDetector> overlappingDetectors)
		{
			if (m_detectorsIds.Count != 0)
			{
				m_detectors.OverlapAllBoundingSphere(ref aabb, overlappingDetectors);
			}
		}

		internal void OverlapDetectorsLineSegment(ref LineD line, List<MyLineSegmentOverlapResult<IMyProjectileDetector>> overlappingDetectors)
		{
			if (m_detectorsIds.Count != 0)
			{
				m_detectors.OverlapAllLineSegment(ref line, overlappingDetectors);
=======
			{
				m_projectiles[i].Draw();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
