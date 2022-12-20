using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Havok;
using VRage;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public class MyEntityCreationThread : IDisposable
	{
		private struct Item
		{
			public MyObjectBuilder_EntityBase ObjectBuilder;

			public bool AddToScene;

			public bool InScene;

			public MyEntity Result;

			public Action<MyEntity> DoneHandler;

			public List<IMyEntity> EntityIds;

			public MyTimeSpan SerializationTimestamp;

			public byte WaitGroup;

			public Dictionary<long, MatrixD> ReleaseMatrices;

			public bool FadeIn;
		}

		private MyConcurrentQueue<Item> m_jobQueue = new MyConcurrentQueue<Item>(16);

		private MyConcurrentQueue<Item> m_resultQueue = new MyConcurrentQueue<Item>(16);

		private ConcurrentCachingHashSet<Item> m_waitingItems = new ConcurrentCachingHashSet<Item>();

		private AutoResetEvent m_event = new AutoResetEvent(initialState: false);

		private Thread m_thread;

		private bool m_exitting;

		public bool AnyResult => m_resultQueue.Count > 0;

		public MyEntityCreationThread()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			RuntimeHelpers.RunClassConstructor(typeof(MyEntityIdentifier).TypeHandle);
			m_thread = new Thread((ThreadStart)ThreadProc);
			m_thread.set_CurrentCulture(CultureInfo.InvariantCulture);
			m_thread.set_CurrentUICulture(CultureInfo.InvariantCulture);
			m_thread.Start();
		}

		public void Dispose()
		{
			m_exitting = true;
			m_event.Set();
			m_thread.Join();
		}

		private void ThreadProc()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Thread.get_CurrentThread().set_Name("Entity creation thread");
			HkBaseSystem.InitThread("Entity creation thread");
			MyEntityIdentifier.InEntityCreationBlock = true;
			MyEntityIdentifier.InitPerThreadStorage(2048);
			while (!m_exitting)
			{
				if (!ConsumeWork(out var item))
				{
					continue;
				}
				if (item.ReleaseMatrices != null)
				{
					Enumerator<Item> enumerator = m_waitingItems.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
<<<<<<< HEAD
							if (item.ReleaseMatrices.TryGetValue(waitingItem.Result.EntityId, out var value))
							{
								waitingItem.Result.PositionComp.SetWorldMatrix(ref value);
=======
							Item current = enumerator.get_Current();
							if (current.WaitGroup == item.WaitGroup)
							{
								if (item.ReleaseMatrices.TryGetValue(current.Result.EntityId, out var value))
								{
									current.Result.PositionComp.SetWorldMatrix(ref value);
								}
								m_waitingItems.Remove(current);
								m_resultQueue.Enqueue(current);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					m_waitingItems.ApplyRemovals();
				}
				else if (item.ObjectBuilder != null)
				{
					if (item.Result == null)
					{
						item.Result = MyEntities.CreateFromObjectBuilderNoinit(item.ObjectBuilder);
					}
					item.InScene = (item.ObjectBuilder.PersistentFlags & MyPersistentEntityFlags2.InScene) == MyPersistentEntityFlags2.InScene;
					item.ObjectBuilder.PersistentFlags &= ~MyPersistentEntityFlags2.InScene;
					item.Result.DebugAsyncLoading = true;
					MyEntities.InitEntity(item.ObjectBuilder, ref item.Result);
					if (item.Result != null)
					{
						item.Result.Render.FadeIn = item.FadeIn;
						item.EntityIds = new List<IMyEntity>();
						MyEntityIdentifier.GetPerThreadEntities(item.EntityIds);
						MyEntityIdentifier.ClearPerThreadEntities();
						if (item.WaitGroup == 0)
						{
							m_resultQueue.Enqueue(item);
							continue;
						}
						m_waitingItems.Add(item);
						m_waitingItems.ApplyAdditions();
					}
				}
				else
				{
					if (item.Result != null)
					{
						item.Result.DebugAsyncLoading = true;
					}
					if (item.WaitGroup == 0)
					{
						m_resultQueue.Enqueue(item);
						continue;
					}
					m_waitingItems.Add(item);
					m_waitingItems.ApplyAdditions();
				}
			}
			MyEntityIdentifier.DestroyPerThreadStorage();
			HkBaseSystem.QuitThread();
		}

		private void SubmitWork(Item item)
		{
			m_jobQueue.Enqueue(item);
			m_event.Set();
		}

		private bool ConsumeWork(out Item item)
		{
			if (m_jobQueue.Count == 0)
			{
				m_event.WaitOne();
			}
			return m_jobQueue.TryDequeue(out item);
		}

		public void SubmitWork(MyObjectBuilder_EntityBase objectBuilder, bool addToScene, Action<MyEntity> doneHandler, MyEntity entity = null, byte waitGroup = 0, double serializationTimestamp = 0.0, bool fadeIn = false)
		{
			SubmitWork(new Item
			{
				ObjectBuilder = objectBuilder,
				AddToScene = addToScene,
				DoneHandler = doneHandler,
				Result = entity,
				WaitGroup = waitGroup,
				SerializationTimestamp = MyTimeSpan.FromMilliseconds(serializationTimestamp),
				FadeIn = fadeIn
			});
		}

		public bool ConsumeResult(MyTimeSpan timestamp)
		{
			if (m_resultQueue.TryDequeue(out var instance))
			{
				if (instance.Result != null)
				{
					instance.Result.DebugAsyncLoading = false;
				}
				bool flag = false;
				if (instance.EntityIds != null)
				{
					while (MyEntities.HasEntitiesToDelete())
					{
						MyEntities.DeleteRememberedEntities();
					}
					foreach (IMyEntity entityId in instance.EntityIds)
					{
						if (MyEntityIdentifier.TryGetEntity(entityId.EntityId, out var _))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						foreach (IMyEntity entityId2 in instance.EntityIds)
						{
							MyEntityIdentifier.AddEntityWithId(entityId2);
						}
					}
					instance.EntityIds.Clear();
				}
				if (!flag)
				{
					if (instance.AddToScene)
					{
						MyEntities.Add(instance.Result, instance.InScene);
					}
					if (instance.DoneHandler != null)
					{
						instance.DoneHandler(instance.Result);
					}
				}
				else if (instance.DoneHandler != null)
				{
					instance.DoneHandler(null);
				}
				return true;
			}
			return false;
		}

		public void ReleaseWaiting(byte index, Dictionary<long, MatrixD> matrices)
		{
			SubmitWork(new Item
			{
				ReleaseMatrices = matrices,
				WaitGroup = index
			});
		}
	}
}
