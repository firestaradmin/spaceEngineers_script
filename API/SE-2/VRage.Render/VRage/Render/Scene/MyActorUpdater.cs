using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using ParallelTasks;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Render.Scene.Components;
using VRage.Utils;

namespace VRage.Render.Scene
{
	public class MyActorUpdater
	{
		private struct MyDelayedCall
		{
			public MyTimeSpan CallTime;

			public Action Call;
		}

		private class StateChangeCollector<T>
		{
			private bool m_stateChanged;

			private readonly HashSet<T> m_targetSet;

			private readonly ConcurrentDictionary<T, bool> m_changeLog = new ConcurrentDictionary<T, bool>();

			public StateChangeCollector(HashSet<T> mTargetSet)
			{
				m_targetSet = mTargetSet;
			}

			public void StateChanged(T instance, bool add)
			{
				if (m_targetSet.Contains(instance) != add)
				{
					m_stateChanged = true;
<<<<<<< HEAD
					m_changeLog[instance] = add;
=======
					m_changeLog.set_Item(instance, add);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}

			public void Commit()
			{
				if (!m_stateChanged)
				{
					return;
				}
				m_stateChanged = false;
				foreach (KeyValuePair<T, bool> item in m_changeLog)
				{
<<<<<<< HEAD
					m_changeLog.Remove(item.Key);
=======
					m_changeLog.Remove<T, bool>(item.Key);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (item.Value)
					{
						m_targetSet.Add(item.Key);
					}
					else
					{
						m_targetSet.Remove(item.Key);
					}
				}
			}
		}

		private HashSet<MyActor> m_pendingUpdate = new HashSet<MyActor>();

		private HashSet<MyActor> m_pendingUpdateProcessed = new HashSet<MyActor>();

		private readonly HashSet<MyActor> m_alwaysUpdateActors = new HashSet<MyActor>();

		private readonly HashSet<MyActorComponent> m_alwaysUpdateComponents = new HashSet<MyActorComponent>();

		private readonly HashSet<MyActorComponent> m_alwaysUpdateComponentsParallel = new HashSet<MyActorComponent>();

		private readonly StateChangeCollector<MyActorComponent> m_alwaysUpdateComponentsParallelCollector;

		private bool m_isUpdatingParallel;

		private readonly List<MyDelayedCall> m_delayedCalls = new List<MyDelayedCall>();

		public MyActorUpdater()
		{
			m_alwaysUpdateComponentsParallelCollector = new StateChangeCollector<MyActorComponent>(m_alwaysUpdateComponentsParallel);
		}

		public void CallIn(Action what, MyTimeSpan delay)
		{
			MyTimeSpan callTime = new MyTimeSpan(Stopwatch.GetTimestamp()) + delay;
			lock (m_delayedCalls)
			{
				m_delayedCalls.Add(new MyDelayedCall
				{
					Call = what,
					CallTime = callTime
				});
			}
		}

		public void DestroyNextFrame(MyActor actor)
		{
			DestroyIn(actor, MyTimeSpan.Zero);
		}

		public void DestroyIn(MyActor actor, MyTimeSpan delay)
		{
			CallIn(delegate
			{
				if (!actor.IsDestroyed)
				{
					actor.Destruct();
				}
			}, delay);
		}

		public void ForceDelayedCalls()
		{
			UpdateDelayedCalls(MyTimeSpan.MaxValue);
		}

		public void UpdateDelayedCalls(MyTimeSpan currentTime)
		{
			lock (m_delayedCalls)
			{
				int num = 0;
				while (num < m_delayedCalls.Count)
				{
					MyDelayedCall myDelayedCall = m_delayedCalls[num];
					if (currentTime >= myDelayedCall.CallTime)
					{
						myDelayedCall.Call();
						m_delayedCalls.RemoveAtFast(num);
					}
					else
					{
						num++;
					}
				}
			}
		}

		public void Update()
		{
<<<<<<< HEAD
			MyTimeSpan currentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
			UpdateDelayedCalls(currentTime);
			m_isUpdatingParallel = true;
			Parallel.ForEach(m_alwaysUpdateComponentsParallel, delegate(MyActorComponent c)
=======
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			MyTimeSpan currentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
			UpdateDelayedCalls(currentTime);
			m_isUpdatingParallel = true;
			Parallel.ForEach((IEnumerable<MyActorComponent>)m_alwaysUpdateComponentsParallel, delegate(MyActorComponent c)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				c.OnUpdateBeforeDraw();
			}, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.PreparePass, "ActorComponents.OnUpdateBeforeDraw").WithMaxThreads(int.MaxValue), blocking: true);
			m_isUpdatingParallel = false;
<<<<<<< HEAD
			foreach (MyActorComponent alwaysUpdateComponent in m_alwaysUpdateComponents)
			{
				alwaysUpdateComponent.OnUpdateBeforeDraw();
			}
			m_alwaysUpdateComponentsParallelCollector.Commit();
			foreach (MyActor alwaysUpdateActor in m_alwaysUpdateActors)
			{
				alwaysUpdateActor.UpdateBeforeDraw();
			}
			while (m_pendingUpdate.Count > 0)
			{
				MyUtils.Swap(ref m_pendingUpdateProcessed, ref m_pendingUpdate);
				foreach (MyActor item in m_pendingUpdateProcessed)
				{
					item.UpdateBeforeDraw();
=======
			Enumerator<MyActorComponent> enumerator = m_alwaysUpdateComponents.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().OnUpdateBeforeDraw();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_alwaysUpdateComponentsParallelCollector.Commit();
			Enumerator<MyActor> enumerator2 = m_alwaysUpdateActors.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.get_Current().UpdateBeforeDraw();
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			while (m_pendingUpdate.get_Count() > 0)
			{
				MyUtils.Swap(ref m_pendingUpdateProcessed, ref m_pendingUpdate);
				enumerator2 = m_pendingUpdateProcessed.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.get_Current().UpdateBeforeDraw();
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_pendingUpdateProcessed.Clear();
			}
		}

		public void AddForParallelUpdate(MyActorComponent component)
		{
			if (m_isUpdatingParallel)
			{
				m_alwaysUpdateComponentsParallelCollector.StateChanged(component, add: true);
			}
			else
			{
				m_alwaysUpdateComponentsParallel.Add(component);
			}
		}

		public void RemoveFromParallelUpdate(MyActorComponent component)
		{
			if (m_isUpdatingParallel)
			{
				m_alwaysUpdateComponentsParallelCollector.StateChanged(component, add: false);
			}
			else
			{
				m_alwaysUpdateComponentsParallel.Remove(component);
			}
		}

		public void AddToNextUpdate(MyActor actor)
		{
			if (!actor.AlwaysUpdate)
			{
				lock (m_pendingUpdate)
				{
					m_pendingUpdate.Add(actor);
				}
			}
		}

		public void AddToAlwaysUpdate(MyActorComponent component)
		{
			m_alwaysUpdateComponents.Add(component);
		}

		public void RemoveFromAlwaysUpdate(MyActorComponent component)
		{
			m_alwaysUpdateComponents.Remove(component);
		}

		public void AddToAlwaysUpdate(MyActor actor)
		{
			m_alwaysUpdateActors.Add(actor);
		}

		public void RemoveFromAlwaysUpdate(MyActor actor)
		{
			m_alwaysUpdateActors.Remove(actor);
		}

		public void RemoveFromUpdates(MyActor actor)
		{
			m_alwaysUpdateActors.Remove(actor);
			m_pendingUpdate.Remove(actor);
		}
	}
}
