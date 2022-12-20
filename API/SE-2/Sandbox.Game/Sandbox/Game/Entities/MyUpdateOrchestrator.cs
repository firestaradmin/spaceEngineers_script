using System;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyUpdateOrchestrator : IMyUpdateOrchestrator
	{
		private readonly CachingList<MyEntity> m_entitiesForUpdateOnce;

		private readonly MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity> m_entitiesForUpdate;

		private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate10;

		private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForUpdate100;

		private readonly MyDistributedUpdater<CachingList<MyEntity>, MyEntity> m_entitiesForSimulate;

		private readonly Dictionary<string, int> m_typesStats;

		public MyUpdateOrchestrator()
		{
			m_entitiesForUpdateOnce = new CachingList<MyEntity>();
			m_entitiesForUpdate = new MyDistributedUpdater<ConcurrentCachingList<MyEntity>, MyEntity>(1);
			m_entitiesForUpdate10 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(10);
			m_entitiesForUpdate100 = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(100);
			m_entitiesForSimulate = new MyDistributedUpdater<CachingList<MyEntity>, MyEntity>(1);
			m_typesStats = new Dictionary<string, int>();
		}

		public void Unload()
		{
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void AddEntity(MyEntity entity)
		{
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.BEFORE_NEXT_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdateOnce.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_10TH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate10.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.EACH_100TH_FRAME) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForUpdate100.List.Add(entity);
			}
			if ((entity.NeedsUpdate & MyEntityUpdateEnum.SIMULATE) > MyEntityUpdateEnum.NONE)
			{
				m_entitiesForSimulate.List.Add(entity);
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RemoveEntity(MyEntity entity, bool immediate)
		{
			if ((entity.Flags & EntityFlags.NeedsUpdateBeforeNextFrame) != 0)
			{
				m_entitiesForUpdateOnce.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate) != 0)
			{
				m_entitiesForUpdate.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate10) != 0)
			{
				m_entitiesForUpdate10.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsUpdate100) != 0)
			{
				m_entitiesForUpdate100.List.Remove(entity, immediate);
			}
			if ((entity.Flags & EntityFlags.NeedsSimulate) != 0)
			{
				m_entitiesForSimulate.List.Remove(entity, immediate);
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void EntityFlagsChanged(MyEntity entity)
		{
			RemoveEntity(entity, immediate: false);
			AddEntity(entity);
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void InvokeLater(Action action, string debugName = null)
		{
			MySandboxGame.Static.Invoke(action, debugName ?? MyDebugUtils.GetDebugName(action.Method));
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchOnceBeforeFrame()
		{
			m_entitiesForUpdateOnce.ApplyChanges();
			foreach (MyEntity item in m_entitiesForUpdateOnce)
			{
				item.NeedsUpdate &= ~MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				if (!item.MarkedForClose)
				{
					item.UpdateOnceBeforeFrame();
				}
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchBeforeSimulation()
		{
			m_entitiesForUpdate.List.ApplyChanges();
			m_entitiesForUpdate.Update();
			MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "DispatchBeforeSimulation");
			m_entitiesForUpdate.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateBeforeSimulation();
				}
			});
			m_entitiesForUpdate10.List.ApplyChanges();
			m_entitiesForUpdate10.Update();
			m_entitiesForUpdate10.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateBeforeSimulation10();
				}
			});
			m_entitiesForUpdate100.List.ApplyChanges();
			m_entitiesForUpdate100.Update();
			m_entitiesForUpdate100.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateBeforeSimulation100();
				}
			});
			MySimpleProfiler.End("DispatchBeforeSimulation");
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchSimulate()
		{
			m_entitiesForSimulate.List.ApplyChanges();
			m_entitiesForSimulate.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.Simulate();
				}
			});
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchAfterSimulation()
		{
			m_entitiesForUpdate.List.ApplyChanges();
			MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "DispatchAfterSimulation");
			m_entitiesForUpdate.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation();
				}
			});
			m_entitiesForUpdate10.List.ApplyChanges();
			m_entitiesForUpdate10.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation10();
				}
			});
			m_entitiesForUpdate100.List.ApplyChanges();
			m_entitiesForUpdate100.Iterate(delegate(MyEntity x)
			{
				if (!x.MarkedForClose)
				{
					x.UpdateAfterSimulation100();
				}
			});
			MySimpleProfiler.End("DispatchAfterSimulation");
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchUpdatingStopped()
		{
			for (int i = 0; i < m_entitiesForUpdate.List.Count; i++)
			{
				m_entitiesForUpdate.List[i].UpdatingStopped();
			}
		}

		public void DebugDraw()
		{
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
			foreach (MyEntity item in m_entitiesForUpdate.List)
			{
				string name = item.GetType().Name;
				if (!m_typesStats.ContainsKey(name))
				{
					m_typesStats.Add(name, 0);
				}
				m_typesStats[name]++;
			}
			float scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item2 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
=======
			foreach (KeyValuePair<string, int> item2 in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item2.Key + ": " + item2.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
			foreach (MyEntity item3 in m_entitiesForUpdate10.List)
			{
				string name2 = item3.GetType().Name;
				if (!m_typesStats.ContainsKey(name2))
				{
					m_typesStats.Add(name2, 0);
				}
				m_typesStats[name2]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update10:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item4 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
=======
			foreach (KeyValuePair<string, int> item4 in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item4.Key + ": " + item4.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
			foreach (MyEntity item5 in m_entitiesForUpdate100.List)
			{
				string name3 = item5.GetType().Name;
				if (!m_typesStats.ContainsKey(name3))
				{
					m_typesStats.Add(name3, 0);
				}
				m_typesStats[name3]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update100:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item6 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
=======
			foreach (KeyValuePair<string, int> item6 in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item6.Key + ": " + item6.Value + "x", Color.Yellow, scale);
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
		}
	}
}
