using System;
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Components.Session;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Library.Threading;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 887, typeof(MyObjectBuilder_SessionComponentSmartUpdater), null, false)]
	public class MySessionComponentSmartUpdater : MySessionComponentBase
	{
		private static readonly int TIMER_MAX = 100;

<<<<<<< HEAD
		public MySessionComponentSmartUpdaterDefinition UpdaterDefinition;
=======
		public new MySessionComponentSmartUpdaterDefinition Definition;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyConcurrentHashSet<long> m_updaterGrids = new MyConcurrentHashSet<long>();

		private MyDistributedUpdater<ConcurrentCachingList<MyCubeGrid>, MyCubeGrid> m_updater = new MyDistributedUpdater<ConcurrentCachingList<MyCubeGrid>, MyCubeGrid>(1000);

		private MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_updaterMovement = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);

		private Dictionary<long, int> m_treeGrids = new Dictionary<long, int>();

		private MyDynamicAABBTreeD m_tree = new MyDynamicAABBTreeD();

		private int m_timer;

		private static readonly SpinLockRef m_movedLock = new SpinLockRef();

		private HashSet<MyEntity> m_moved = new HashSet<MyEntity>();

		private HashSet<MyEntity> m_movedSwap = new HashSet<MyEntity>();

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			return base.GetObjectBuilder() as MyObjectBuilder_SessionComponentSmartUpdater;
		}

		public override void InitFromDefinition(MySessionComponentDefinition definition)
		{
			base.InitFromDefinition(definition);
<<<<<<< HEAD
			UpdaterDefinition = definition as MySessionComponentSmartUpdaterDefinition;
=======
			Definition = definition as MySessionComponentSmartUpdaterDefinition;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!MyFakes.ENABLE_SMART_UPDATER || !Sync.IsServer)
			{
				return;
			}
			foreach (MyEntity entity in MyEntities.GetEntities())
			{
				MyCubeGrid grid;
				if ((grid = entity as MyCubeGrid) != null)
				{
					AddGridToUpdater(grid);
				}
			}
			MyEntities.OnEntityAdd += EntityAddedToScene;
			MyEntities.OnEntityRemove += EntityRemovedFromScene;
			MyEntity.UpdateGamePruningStructureExtCallBack = (Action<MyEntity>)Delegate.Combine(MyEntity.UpdateGamePruningStructureExtCallBack, new Action<MyEntity>(EntityMoved));
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			MyEntities.OnEntityAdd -= EntityAddedToScene;
			MyEntities.OnEntityRemove -= EntityRemovedFromScene;
			MyEntity.UpdateGamePruningStructureExtCallBack = (Action<MyEntity>)Delegate.Remove(MyEntity.UpdateGamePruningStructureExtCallBack, new Action<MyEntity>(EntityMoved));
		}

		private void EntityMoved(MyEntity obj)
		{
			if (m_treeGrids.ContainsKey(obj.EntityId))
			{
				using (m_movedLock.Acquire())
				{
					m_moved.Add(obj);
				}
			}
		}

		private void EntityRemovedFromScene(MyEntity obj)
		{
			MyCubeGrid grid;
			if ((grid = obj as MyCubeGrid) != null)
			{
				RemoveGridFromUpdater(grid);
			}
		}

		private void EntityAddedToScene(MyEntity obj)
		{
			MyCubeGrid grid;
			if ((grid = obj as MyCubeGrid) != null)
			{
				AddGridToUpdater(grid);
			}
		}

		private void AddGridToUpdater(MyCubeGrid grid)
		{
			if (!m_treeGrids.ContainsKey(grid.EntityId))
			{
				BoundingBoxD aabb = GetEntityAABB(grid);
				int value = m_tree.AddProxy(ref aabb, grid, 0u);
				m_treeGrids.Add(grid.EntityId, value);
			}
		}

		private void RemoveGridFromUpdater(MyCubeGrid grid)
		{
			if (m_treeGrids.ContainsKey(grid.EntityId))
			{
				m_tree.RemoveProxy(m_treeGrids[grid.EntityId]);
				m_treeGrids.Remove(grid.EntityId);
			}
		}

		public bool RegisterToUpdater(MyCubeGrid grid)
		{
			if (m_updaterGrids.Contains(grid.EntityId))
			{
				return false;
			}
			m_updaterGrids.Add(grid.EntityId);
			m_updater.List.Add(grid);
			return true;
		}

		public bool UnregisterFromUpdater(MyCubeGrid grid)
		{
			if (!m_updaterGrids.Contains(grid.EntityId))
			{
				return false;
			}
			m_updaterGrids.Remove(grid.EntityId);
			m_updater.List.Remove(grid);
			return true;
		}

		public override void UpdateBeforeSimulation()
		{
<<<<<<< HEAD
=======
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.UpdateBeforeSimulation();
			if (!MyFakes.ENABLE_SMART_UPDATER || !Sync.IsServer)
			{
				return;
			}
			m_timer--;
			if (m_timer <= 0)
			{
				m_timer = TIMER_MAX;
				using (m_movedLock.Acquire())
				{
					MyUtils.Swap(ref m_moved, ref m_movedSwap);
				}
				m_updaterMovement.List.ClearImmediate();
<<<<<<< HEAD
				foreach (MyEntity item in m_movedSwap)
				{
					m_updaterMovement.List.Add(item);
=======
				Enumerator<MyEntity> enumerator = m_movedSwap.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyEntity current = enumerator.get_Current();
						m_updaterMovement.List.Add(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_updaterMovement.List.ApplyChanges();
			}
			m_updaterMovement.Update();
			m_updaterMovement.Iterate(delegate(MyEntity x)
			{
				MoveInternal(x);
			});
			m_movedSwap.Clear();
			m_updater.List.ApplyChanges();
			m_updater.Update();
			m_updater.Iterate(delegate(MyCubeGrid x)
			{
				if (x == null)
				{
					MyLog.Default.Error("Grid is null in SmartUpdater. This must not happen.");
					m_updater.List.Remove(x);
				}
				else
				{
					BoundingSphereD sphere = new BoundingSphereD(x.PositionComp.GetPosition(), 3000.0);
					List<MyEntity> list = new List<MyEntity>();
					m_tree.OverlapAllBoundingSphere(ref sphere, list);
					if ((list.Count == 1 && list[0] == x) || list.Count == 0)
					{
						x.OnGridPresenceUpdate(isAnyGridPresent: false);
					}
					else
					{
						x.OnGridPresenceUpdate(isAnyGridPresent: true);
					}
				}
			});
		}

		private void MoveInternal(MyEntity moved)
		{
			if (m_treeGrids.ContainsKey(moved.EntityId))
			{
				BoundingBoxD aabb = GetEntityAABB(moved);
				m_tree.MoveProxy(m_treeGrids[moved.EntityId], ref aabb, Vector3D.Zero);
			}
		}

		private static BoundingBoxD GetEntityAABB(MyEntity entity)
		{
			BoundingBoxD result = entity.PositionComp.WorldAABB;
			if (entity.Physics != null)
			{
				result = result.Include(entity.WorldMatrix.Translation + entity.Physics.LinearVelocity * 0.0166666675f * 5f);
			}
			return result;
		}
	}
}
