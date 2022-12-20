using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
<<<<<<< HEAD
using Sandbox.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyMissiles : MySessionComponentBase, IMyMissiles
	{
		private struct MissileId
		{
			public SerializableDefinitionId WeaponDefinitionId;

			public SerializableDefinitionId AmmoMagazineId;
		}

		private static MyMissiles m_static;

		private readonly Dictionary<MissileId, Queue<MyMissile>> m_missiles = new Dictionary<MissileId, Queue<MyMissile>>();

<<<<<<< HEAD
		private readonly MyDynamicAABBTreeD m_missileTree = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);

		private Queue<MyObjectBuilder_Missile> m_newMissiles = new Queue<MyObjectBuilder_Missile>();

		public static MyMissiles Static => m_static;

		public override Type[] Dependencies => new Type[1] { typeof(MyPhysics) };

		public event Action<IMyMissile> OnMissileAdded;

		public event Action<IMyMissile> OnMissileRemoved;

		public event MissileMoveDelegate OnMissileMoved;

		public event Action<IMyMissile> OnMissileCollided;
=======
		private static Queue<MyObjectBuilder_Missile> m_newMissiles = new Queue<MyObjectBuilder_Missile>();

		public override Type[] Dependencies => new Type[1] { typeof(MyPhysics) };
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public override void LoadData()
		{
			m_static = this;
		}

		protected override void UnloadData()
		{
			foreach (KeyValuePair<MissileId, Queue<MyMissile>> missile in m_missiles)
			{
				while (missile.Value.get_Count() > 0)
				{
					missile.Value.Dequeue().Close();
				}
			}
			m_missiles.Clear();
			m_missileTree.Clear();
			m_newMissiles.Clear();
			m_static = null;
		}

		public void Add(MyObjectBuilder_Missile builder)
		{
			if (MyEntities.IsAsyncUpdateInProgress)
			{
				lock (m_newMissiles)
				{
					if (m_newMissiles.Count == 0)
					{
						MyEntities.InvokeLater(AddMissiles);
					}
					m_newMissiles.Enqueue(builder);
				}
			}
			else
			{
				AddMissileInternal(builder);
			}
		}

		private void AddMissileInternal(MyObjectBuilder_Missile builder)
		{
			if (MyEntities.IsAsyncUpdateInProgress)
			{
				lock (m_newMissiles)
				{
					if (m_newMissiles.get_Count() == 0)
					{
						MyEntities.InvokeLater(AddMissiles);
					}
					m_newMissiles.Enqueue(builder);
				}
			}
			else
			{
				AddMissileInternal(builder);
			}
		}

		private static void AddMissileInternal(MyObjectBuilder_Missile builder)
		{
			MissileId missileId = default(MissileId);
			missileId.AmmoMagazineId = builder.AmmoMagazineId;
			missileId.WeaponDefinitionId = builder.WeaponDefinitionId;
			MissileId key = missileId;
<<<<<<< HEAD
			if (m_missiles.TryGetValue(key, out var value) && value.Count > 0)
=======
			if (m_missiles.TryGetValue(key, out var value) && value.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyMissile myMissile = value.Dequeue();
				myMissile.UpdateData(builder);
				myMissile.m_pruningProxyId = -1;
				MyEntities.Add(myMissile);
				RegisterMissile(myMissile);
			}
			else
			{
				MyEntities.CreateFromObjectBuilderParallel(builder, addToScene: true, delegate(MyEntity x)
				{
					MyMissile myMissile2 = x as MyMissile;
					myMissile2.m_pruningProxyId = -1;
					RegisterMissile(myMissile2);
				});
			}
		}

<<<<<<< HEAD
		private void AddMissiles()
=======
		private static void AddMissiles()
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			lock (m_newMissiles)
			{
				MyObjectBuilder_Missile result;
<<<<<<< HEAD
				while (QueueExtensions.TryDequeue(m_newMissiles, out result))
=======
				while (m_newMissiles.TryDequeue<MyObjectBuilder_Missile>(out result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					AddMissileInternal(result);
				}
			}
		}

<<<<<<< HEAD
		public void Remove(long entityId)
=======
		public static void Remove(long entityId)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			MyMissile myMissile = MyEntities.GetEntityById(entityId) as MyMissile;
			if (myMissile != null)
			{
				Return(myMissile);
			}
		}

		public void Return(MyMissile missile)
		{
			if (missile.InScene)
			{
				MissileId missileId = default(MissileId);
				missileId.AmmoMagazineId = missile.AmmoMagazineId;
				missileId.WeaponDefinitionId = missile.WeaponDefinitionId;
				MissileId key = missileId;
				if (!m_missiles.TryGetValue(key, out var value))
				{
					value = new Queue<MyMissile>();
					m_missiles.Add(key, value);
				}
				value.Enqueue(missile);
				MyEntities.Remove(missile);
				UnregisterMissile(missile);
				m_missiles.Remove(key);
				missile.Close();
			}
		}

		private void RegisterMissile(MyMissile missile)
		{
			if (missile.m_pruningProxyId == -1)
			{
				BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
				BoundingBoxD.CreateFromSphere(ref sphere, out var result);
				missile.m_pruningProxyId = m_missileTree.AddProxy(ref result, missile, 0u);
				this.OnMissileAdded?.Invoke(missile);
			}
		}

		private void UnregisterMissile(MyMissile missile)
		{
			if (missile.m_pruningProxyId != -1)
			{
				m_missileTree.RemoveProxy(missile.m_pruningProxyId);
				missile.m_pruningProxyId = -1;
				this.OnMissileRemoved?.Invoke(missile);
			}
		}

		public void OnMissileHasMoved(MyMissile missile, ref Vector3 velocity)
		{
			if (missile.m_pruningProxyId != -1)
			{
				this.OnMissileMoved?.Invoke(missile, ref velocity);
				BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
				BoundingBoxD.CreateFromSphere(ref sphere, out var result);
				m_missileTree.MoveProxy(missile.m_pruningProxyId, ref result, velocity);
			}
		}

		public void GetAllMissilesInSphere(ref BoundingSphereD sphere, List<MyEntity> result)
		{
			m_missileTree.OverlapAllBoundingSphere(ref sphere, result, clear: false);
		}

		public void TriggerCollision(MyMissile missile)
		{
			this.OnMissileCollided?.Invoke(missile);
		}
	}
}
