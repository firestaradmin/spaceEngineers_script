using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Havok;
using ParallelTasks;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Library.Parallelization;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities
{
	public class MyParallelEntityUpdateOrchestrator : IMyUpdateOrchestrator
	{
		private readonly Dictionary<MyEntity, MyParallelUpdateFlags> m_lastUpdateRecord;

		private readonly HashSet<MyEntity> m_entitiesToAdd;

		private readonly HashSet<MyEntity> m_entitiesToRemove;

		private readonly List<MyEntity> m_entitiesForUpdateOnce;

		private readonly HashSet<MyEntity> m_entitiesForUpdate;

		private readonly HashSet<IMyParallelUpdateable> m_entitiesForUpdateParallel;

		private readonly MyDistributedUpdater<List<MyEntity>, MyEntity> m_entitiesForUpdate10;

		private readonly MyDistributedUpdater<List<MyEntity>, MyEntity> m_entitiesForUpdate100;

		private List<(Action Callback, string DebugName)> m_callbacksPendingExecution;

		private List<(Action Callback, string DebugName)> m_callbacksPendingExecutionSwap;

		private readonly List<MyEntity> m_entitiesForSimulate;

		private readonly Action<IMyParallelUpdateable> m_parallelUpdateHandlerBeforeSimulation;

		private readonly Action<IMyParallelUpdateable> m_parallelUpdateHandlerAfterSimulation;

<<<<<<< HEAD
		/// <summary>
		/// Priority of the worker threads where work is scheduled.
		/// </summary>
		public static WorkPriority WorkerPriority;

		/// <summary>
		/// Global switch to force serial execution.
		/// </summary>
		public static bool ForceSerialUpdate;

		/// <summary>
		/// Lock for all thread safe operations.
		/// </summary>
=======
		public static WorkPriority WorkerPriority;

		public static bool ForceSerialUpdate;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly object m_lock;

		private DataGuard EntitySyncGuard;

		private readonly Dictionary<string, int> m_typesStats;

		public static bool ParallelUpdateInProgress => MyEntities.IsAsyncUpdateInProgress;

		public MyParallelEntityUpdateOrchestrator()
		{
			m_entitiesForUpdateOnce = new List<MyEntity>();
			m_entitiesForUpdate = new HashSet<MyEntity>();
			m_entitiesForUpdateParallel = new HashSet<IMyParallelUpdateable>();
			m_entitiesForUpdate10 = new MyDistributedUpdater<List<MyEntity>, MyEntity>(10);
			m_entitiesForUpdate100 = new MyDistributedUpdater<List<MyEntity>, MyEntity>(100);
			m_entitiesForSimulate = new List<MyEntity>();
			m_callbacksPendingExecution = new List<(Action, string)>();
			m_callbacksPendingExecutionSwap = new List<(Action, string)>();
			m_entitiesToRemove = new HashSet<MyEntity>();
			m_entitiesToAdd = new HashSet<MyEntity>();
			m_lastUpdateRecord = new Dictionary<MyEntity, MyParallelUpdateFlags>();
			m_lock = m_lastUpdateRecord;
			m_typesStats = new Dictionary<string, int>();
			m_parallelUpdateHandlerBeforeSimulation = ParallelUpdateHandlerBeforeSimulation;
			m_parallelUpdateHandlerAfterSimulation = ParallelUpdateHandlerAfterSimulation;
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Unload()
		{
			m_entitiesForUpdateOnce.Clear();
			m_entitiesForUpdate.Clear();
			m_entitiesForUpdateParallel.Clear();
			m_entitiesForUpdate10.List.Clear();
			m_entitiesForUpdate100.List.Clear();
			m_entitiesForSimulate.Clear();
			m_entitiesToAdd.Clear();
			m_entitiesToRemove.Clear();
			m_lastUpdateRecord.Clear();
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void AddEntity(MyEntity entity)
		{
			lock (m_lock)
			{
				using (EntitySyncGuard.Exclusive("Can't add entities from parallel while syncing"))
				{
					m_entitiesToRemove.Remove(entity);
					m_entitiesToAdd.Add(entity);
				}
			}
		}

		private void AddEntityInternal(MyEntity entity)
		{
			IMyParallelUpdateable myParallelUpdateable;
			MyParallelUpdateFlags myParallelUpdateFlags;
			if ((myParallelUpdateable = entity as IMyParallelUpdateable) != null)
			{
				myParallelUpdateFlags = myParallelUpdateable.UpdateFlags;
			}
			else
			{
				myParallelUpdateFlags = entity.NeedsUpdate.GetParallel();
				myParallelUpdateable = null;
			}
			MyParallelUpdateFlags myParallelUpdateFlags2 = myParallelUpdateFlags;
			if (m_lastUpdateRecord.TryGetValue(entity, out var value))
			{
				myParallelUpdateFlags2 = myParallelUpdateFlags & ~value;
				MyParallelUpdateFlags flags = value & ~myParallelUpdateFlags;
				RemoveWithFlags(entity, flags);
			}
			if (myParallelUpdateFlags2 != 0)
			{
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.BEFORE_NEXT_FRAME) != 0)
				{
					m_entitiesForUpdateOnce.Add(entity);
				}
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != 0)
				{
					AddOnce(m_entitiesForUpdateParallel, myParallelUpdateable);
				}
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.EACH_FRAME) != 0)
				{
					AddOnce(m_entitiesForUpdate, entity);
				}
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.EACH_10TH_FRAME) != 0)
				{
					m_entitiesForUpdate10.List.Add(entity);
				}
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.EACH_100TH_FRAME) != 0)
				{
					m_entitiesForUpdate100.List.Add(entity);
				}
				if ((myParallelUpdateFlags2 & MyParallelUpdateFlags.SIMULATE) != 0)
				{
					m_entitiesForSimulate.Add(entity);
				}
			}
			m_lastUpdateRecord[entity] = myParallelUpdateFlags;
		}

		private void AddOnce<T>(HashSet<T> set, T value)
		{
			set.Add(value);
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void RemoveEntity(MyEntity entity, bool immediate)
		{
			lock (m_lock)
			{
				using (EntitySyncGuard.Exclusive("Can't remove entities from parallel while syncing"))
				{
					if (!m_entitiesToAdd.Remove(entity) && m_lastUpdateRecord.ContainsKey(entity))
					{
						if (immediate)
						{
							RemoveEntityInternal(entity);
						}
						else
						{
							m_entitiesToRemove.Add(entity);
						}
					}
				}
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void EntityFlagsChanged(MyEntity entity)
		{
			lock (m_lock)
			{
				m_entitiesToAdd.Add(entity);
			}
		}

		private void RemoveEntityInternal(MyEntity entity)
		{
			if (m_lastUpdateRecord.TryGetValue(entity, out var value))
			{
				RemoveWithFlags(entity, value);
				m_lastUpdateRecord.Remove(entity);
			}
		}

		private void RemoveWithFlags(MyEntity entity, MyParallelUpdateFlags flags)
		{
			if ((flags & MyParallelUpdateFlags.BEFORE_NEXT_FRAME) != 0)
			{
				m_entitiesForUpdateOnce.Remove(entity);
			}
			if ((flags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != 0)
			{
				m_entitiesForUpdateParallel.Remove((IMyParallelUpdateable)entity);
			}
			if ((flags & MyParallelUpdateFlags.EACH_FRAME) != 0)
			{
				m_entitiesForUpdate.Remove(entity);
			}
			if ((flags & MyParallelUpdateFlags.EACH_10TH_FRAME) != 0)
			{
				m_entitiesForUpdate10.List.Remove(entity);
			}
			if ((flags & MyParallelUpdateFlags.EACH_100TH_FRAME) != 0)
			{
				m_entitiesForUpdate100.List.Remove(entity);
			}
			if ((flags & MyParallelUpdateFlags.SIMULATE) != 0)
			{
				m_entitiesForSimulate.Remove(entity);
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void InvokeLater(Action action, string debugName = null)
		{
			lock (m_lock)
			{
				m_callbacksPendingExecution.Add((action, debugName));
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchOnceBeforeFrame()
		{
			ApplyChanges();
			for (int i = 0; i < m_entitiesForUpdateOnce.Count; i++)
			{
				MyEntity myEntity = m_entitiesForUpdateOnce[i];
				m_lastUpdateRecord[myEntity] &= ~MyParallelUpdateFlags.BEFORE_NEXT_FRAME;
				EntityFlags flags = myEntity.Flags;
				if ((flags & EntityFlags.NeedsUpdateBeforeNextFrame) != 0)
				{
					myEntity.Flags = flags & ~EntityFlags.NeedsUpdateBeforeNextFrame;
					if (!myEntity.MarkedForClose && myEntity.InScene)
					{
						myEntity.UpdateOnceBeforeFrame();
					}
				}
			}
			m_entitiesForUpdateOnce.Clear();
			ProcessInvokeLater();
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchBeforeSimulation()
		{
			ApplyChanges();
			MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "DispatchBeforeSimulation");
			PerformParallelUpdate(m_parallelUpdateHandlerBeforeSimulation);
			ProcessInvokeLater();
			ApplyChanges();
			UpdateBeforeSimulation();
			UpdateBeforeSimulation10();
			UpdateBeforeSimulation100();
			MySimpleProfiler.End("DispatchBeforeSimulation");
		}

		private void ParallelUpdateHandlerBeforeSimulation(IMyParallelUpdateable entity)
		{
			if (!entity.MarkedForClose && (entity.UpdateFlags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != 0 && entity.InScene)
			{
				entity.UpdateBeforeSimulationParallel();
			}
		}

		private void UpdateBeforeSimulation()
		{
<<<<<<< HEAD
			foreach (MyEntity item in m_entitiesForUpdate)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate) != 0 && item.InScene)
				{
					item.UpdateBeforeSimulation();
				}
			}
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyEntity> enumerator = m_entitiesForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyEntity current = enumerator.get_Current();
					if (!current.MarkedForClose && (current.Flags & EntityFlags.NeedsUpdate) != 0 && current.InScene)
					{
						current.UpdateBeforeSimulation();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void UpdateBeforeSimulation10()
		{
			m_entitiesForUpdate10.Update();
			foreach (MyEntity item in m_entitiesForUpdate10)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate10) != 0 && item.InScene)
				{
					item.UpdateBeforeSimulation10();
				}
			}
		}

		private void UpdateBeforeSimulation100()
		{
			m_entitiesForUpdate100.Update();
			foreach (MyEntity item in m_entitiesForUpdate100)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate100) != 0 && item.InScene)
				{
					item.UpdateBeforeSimulation100();
				}
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchSimulate()
		{
			ApplyChanges();
			for (int num = m_entitiesForSimulate.Count - 1; num >= 0; num--)
			{
				MyEntity myEntity = m_entitiesForSimulate[num];
				if (!myEntity.MarkedForClose)
				{
					myEntity.Simulate();
				}
			}
			ProcessInvokeLater();
		}

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void DispatchAfterSimulation()
		{
			ApplyChanges();
			MySimpleProfiler.Begin("Blocks", MySimpleProfiler.ProfilingBlockType.BLOCK, "DispatchAfterSimulation");
			PerformParallelUpdate(m_parallelUpdateHandlerAfterSimulation);
			ProcessInvokeLater();
			ApplyChanges();
			UpdateAfterSimulation();
			UpdateAfterSimulation10();
			UpdateAfterSimulation100();
			MySimpleProfiler.End("DispatchAfterSimulation");
		}

		private void ParallelUpdateHandlerAfterSimulation(IMyParallelUpdateable entity)
		{
			if (!entity.MarkedForClose && (entity.UpdateFlags & MyParallelUpdateFlags.EACH_FRAME_PARALLEL) != 0 && entity.InScene)
			{
				entity.UpdateAfterSimulationParallel();
			}
		}

		private void UpdateAfterSimulation()
		{
<<<<<<< HEAD
			foreach (MyEntity item in m_entitiesForUpdate)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate) != 0 && item.InScene)
				{
					item.UpdateAfterSimulation();
				}
			}
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyEntity> enumerator = m_entitiesForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyEntity current = enumerator.get_Current();
					if (!current.MarkedForClose && (current.Flags & EntityFlags.NeedsUpdate) != 0 && current.InScene)
					{
						current.UpdateAfterSimulation();
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void UpdateAfterSimulation10()
		{
			foreach (MyEntity item in m_entitiesForUpdate10)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate10) != 0 && item.InScene)
				{
					item.UpdateAfterSimulation10();
				}
			}
		}

		private void UpdateAfterSimulation100()
		{
			foreach (MyEntity item in m_entitiesForUpdate100)
			{
				if (!item.MarkedForClose && (item.Flags & EntityFlags.NeedsUpdate100) != 0 && item.InScene)
				{
					item.UpdateAfterSimulation100();
				}
			}
		}

		private void PerformParallelUpdate(Action<IMyParallelUpdateable> updateFunction)
		{
<<<<<<< HEAD
=======
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			using (HkAccessControl.PushState(HkAccessControl.AccessState.SharedRead))
			{
				if (!ForceSerialUpdate)
				{
					using (MyEntities.StartAsyncUpdateBlock())
					{
<<<<<<< HEAD
						Parallel.ForEach(m_entitiesForUpdateParallel, updateFunction, WorkerPriority, null, blocking: true);
					}
					return;
				}
				foreach (IMyParallelUpdateable item in m_entitiesForUpdateParallel)
				{
					updateFunction(item);
=======
						Parallel.ForEach((IEnumerable<IMyParallelUpdateable>)m_entitiesForUpdateParallel, updateFunction, WorkerPriority, null, blocking: true);
					}
					return;
				}
				Enumerator<IMyParallelUpdateable> enumerator = m_entitiesForUpdateParallel.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IMyParallelUpdateable current = enumerator.get_Current();
						updateFunction(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Process all queued invoke later callbacks.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void ProcessInvokeLater()
		{
			if (m_callbacksPendingExecution.Count != 0)
			{
				lock (m_lock)
				{
					MyUtils.Swap(ref m_callbacksPendingExecution, ref m_callbacksPendingExecutionSwap);
				}
				int count = m_callbacksPendingExecutionSwap.Count;
				for (int i = 0; i < count; i++)
				{
					m_callbacksPendingExecutionSwap[i].Callback();
				}
				m_callbacksPendingExecutionSwap.Clear();
			}
		}

		private void ApplyChanges()
		{
<<<<<<< HEAD
=======
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			lock (m_lock)
			{
				using (EntitySyncGuard.Exclusive("Can't sync entities"))
				{
<<<<<<< HEAD
					foreach (MyEntity item in m_entitiesToRemove)
					{
						RemoveEntityInternal(item);
					}
					foreach (MyEntity item2 in m_entitiesToAdd)
					{
						AddEntityInternal(item2);
=======
					Enumerator<MyEntity> enumerator = m_entitiesToRemove.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyEntity current = enumerator.get_Current();
							RemoveEntityInternal(current);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					enumerator = m_entitiesToAdd.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyEntity current2 = enumerator.get_Current();
							AddEntityInternal(current2);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				m_entitiesToAdd.Clear();
				m_entitiesToRemove.Clear();
			}
		}

<<<<<<< HEAD
		/// <inheritdoc />
		public void DispatchUpdatingStopped()
		{
			foreach (MyEntity item in m_entitiesForUpdate)
			{
				item.UpdatingStopped();
=======
		public void DispatchUpdatingStopped()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyEntity> enumerator = m_entitiesForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().UpdatingStopped();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void DebugDraw()
		{
<<<<<<< HEAD
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
			foreach (MyEntity item in m_entitiesForUpdate)
			{
				string debugName = MyDebugUtils.GetDebugName(item);
				if (!m_typesStats.ContainsKey(debugName))
				{
					m_typesStats.Add(debugName, 0);
				}
				m_typesStats[debugName]++;
=======
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			Vector2 screenCoord = new Vector2(100f, 0f);
			MyRenderProxy.DebugDrawText2D(screenCoord, "Detailed entity statistics", Color.Yellow, 1f);
			Enumerator<MyEntity> enumerator = m_entitiesForUpdate.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string debugName = MyDebugUtils.GetDebugName(enumerator.get_Current());
					if (!m_typesStats.ContainsKey(debugName))
					{
						m_typesStats.Add(debugName, 0);
					}
					m_typesStats[debugName]++;
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			float scale = 0.7f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item2 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item2.Key + ": " + item2.Value + "x", Color.Yellow, scale);
=======
			foreach (KeyValuePair<string, int> item in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item.Key + ": " + item.Value + "x", Color.Yellow, scale);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
<<<<<<< HEAD
			foreach (MyEntity item3 in m_entitiesForUpdate10.List)
			{
				string debugName2 = MyDebugUtils.GetDebugName(item3);
=======
			foreach (MyEntity item2 in m_entitiesForUpdate10.List)
			{
				string debugName2 = MyDebugUtils.GetDebugName(item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_typesStats.ContainsKey(debugName2))
				{
					m_typesStats.Add(debugName2, 0);
				}
				m_typesStats[debugName2]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update10:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item4 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item4.Key + ": " + item4.Value + "x", Color.Yellow, scale);
=======
			foreach (KeyValuePair<string, int> item3 in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item3.Key + ": " + item3.Value + "x", Color.Yellow, scale);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
<<<<<<< HEAD
			foreach (MyEntity item5 in m_entitiesForUpdate100.List)
			{
				string debugName3 = MyDebugUtils.GetDebugName(item5);
=======
			foreach (MyEntity item4 in m_entitiesForUpdate100.List)
			{
				string debugName3 = MyDebugUtils.GetDebugName(item4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_typesStats.ContainsKey(debugName3))
				{
					m_typesStats.Add(debugName3, 0);
				}
				m_typesStats[debugName3]++;
			}
			screenCoord.X += 300f;
			screenCoord.Y += 50f;
			MyRenderProxy.DebugDrawText2D(screenCoord, "Entities for update100:", Color.Yellow, scale);
			screenCoord.Y += 30f;
<<<<<<< HEAD
			foreach (KeyValuePair<string, int> item6 in m_typesStats.OrderByDescending((KeyValuePair<string, int> x) => x.Value))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item6.Key + ": " + item6.Value + "x", Color.Yellow, scale);
=======
			foreach (KeyValuePair<string, int> item5 in (IEnumerable<KeyValuePair<string, int>>)Enumerable.OrderByDescending<KeyValuePair<string, int>, int>((IEnumerable<KeyValuePair<string, int>>)m_typesStats, (Func<KeyValuePair<string, int>, int>)((KeyValuePair<string, int> x) => x.Value)))
			{
				MyRenderProxy.DebugDrawText2D(screenCoord, item5.Key + ": " + item5.Value + "x", Color.Yellow, scale);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				screenCoord.Y += 20f;
			}
			m_typesStats.Clear();
			screenCoord.Y = 0f;
		}

		[Conditional("DEBUG")]
		private void CheckConsistent(MyEntity entity)
		{
			m_lastUpdateRecord.TryGetValue(entity, out var _);
<<<<<<< HEAD
			CheckCollection<MyEntity>(m_entitiesForUpdate, MyParallelUpdateFlags.EACH_FRAME);
			CheckCollection<IMyParallelUpdateable>(m_entitiesForUpdateParallel, MyParallelUpdateFlags.EACH_FRAME_PARALLEL);
=======
			CheckCollection<MyEntity>((ICollection<MyEntity>)m_entitiesForUpdate, MyParallelUpdateFlags.EACH_FRAME);
			CheckCollection<IMyParallelUpdateable>((ICollection<IMyParallelUpdateable>)m_entitiesForUpdateParallel, MyParallelUpdateFlags.EACH_FRAME_PARALLEL);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			CheckCollection<MyEntity>(m_entitiesForUpdate10.List, MyParallelUpdateFlags.EACH_10TH_FRAME);
			CheckCollection<MyEntity>(m_entitiesForUpdate100.List, MyParallelUpdateFlags.EACH_100TH_FRAME);
			void CheckCollection<T>(ICollection<T> collection, MyParallelUpdateFlags flag) where T : class, IMyEntity
			{
			}
		}
	}
}
