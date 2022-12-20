using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace VRage.Collections
{
	public abstract class MyConcurrentBucketPool
	{
		public static bool EnablePooling = true;

		private static List<MyConcurrentBucketPool> m_poolsForDispose = new List<MyConcurrentBucketPool>();

		public static void OnExit()
		{
			lock (m_poolsForDispose)
			{
				foreach (MyConcurrentBucketPool item in m_poolsForDispose)
				{
					item.DisposeInternal();
				}
				m_poolsForDispose.Clear();
			}
		}

		protected MyConcurrentBucketPool(bool requiresDispose)
		{
			if (requiresDispose)
			{
				lock (m_poolsForDispose)
				{
					m_poolsForDispose.Add(this);
				}
			}
		}

		protected abstract void DisposeInternal();
	}
	/// <summary>
	/// Simple thread-safe pool.
	/// Can store external objects by calling return.
	/// Creates new instances when empty.
	/// </summary>
	public class MyConcurrentBucketPool<T> : MyConcurrentBucketPool where T : class
	{
		private readonly IMyElementAllocator<T> m_allocator;

		private readonly ConcurrentDictionary<int, ConcurrentStack<T>> m_instances;

		private MyBufferStatistics m_statistics;

		public MyConcurrentBucketPool(string debugName, IMyElementAllocator<T> allocator)
			: base(allocator.ExplicitlyDisposeAllElements)
		{
			m_allocator = allocator;
			m_instances = (ConcurrentDictionary<int, ConcurrentStack<T>>)(object)new ConcurrentDictionary<int, ConcurrentStack<int>>();
			m_statistics.Name = debugName;
		}

		public unsafe T Get(int bucketId)
		{
<<<<<<< HEAD
			T result = null;
			if (m_instances.TryGetValue(bucketId, out var value))
=======
			T val = null;
			ConcurrentStack<T> val2 = default(ConcurrentStack<T>);
			if (((ConcurrentDictionary<int, ConcurrentStack<int>>)(object)m_instances).TryGetValue(bucketId, ref *(ConcurrentStack<int>*)(&val2)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				val2.TryPop(ref val);
			}
			if (val == null)
			{
				val = m_allocator.Allocate(bucketId);
				Interlocked.Increment(ref m_statistics.TotalBuffersAllocated);
				Interlocked.Add(ref m_statistics.TotalBytesAllocated, m_allocator.GetBytes(val));
			}
			int bytes = m_allocator.GetBytes(val);
			Interlocked.Add(ref m_statistics.ActiveBytes, bytes);
			Interlocked.Increment(ref m_statistics.ActiveBuffers);
			m_allocator.Init(val);
			return val;
		}

		public unsafe void Return(T instance)
		{
			int bytes = m_allocator.GetBytes(instance);
			int bucketId = m_allocator.GetBucketId(instance);
			Interlocked.Add(ref m_statistics.ActiveBytes, -bytes);
			Interlocked.Decrement(ref m_statistics.ActiveBuffers);
			if (MyConcurrentBucketPool.EnablePooling)
			{
<<<<<<< HEAD
				if (!m_instances.TryGetValue(bucketId, out var value))
=======
				ConcurrentStack<T> orAdd = default(ConcurrentStack<T>);
				if (!((ConcurrentDictionary<int, ConcurrentStack<int>>)(object)m_instances).TryGetValue(bucketId, ref *(ConcurrentStack<int>*)(&orAdd)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					orAdd = new ConcurrentStack<T>();
					orAdd = ((ConcurrentDictionary<int, ConcurrentStack<int>>)(object)m_instances).GetOrAdd(bucketId, (ConcurrentStack<int>)(object)orAdd);
				}
				orAdd.Push(instance);
			}
			else
			{
				m_allocator.Dispose(instance);
			}
		}

		public MyBufferStatistics GetReport()
		{
			return m_statistics;
		}

		public void Clear()
		{
			T instance = default(T);
			foreach (KeyValuePair<int, ConcurrentStack<T>> item in (ConcurrentDictionary<int, ConcurrentStack<int>>)(object)m_instances)
			{
				while (item.Value.TryPop(ref instance))
				{
					m_allocator.Dispose(instance);
				}
			}
			((ConcurrentDictionary<int, ConcurrentStack<int>>)(object)m_instances).Clear();
			m_statistics = new MyBufferStatistics
			{
				Name = m_statistics.Name
			};
		}

		protected override void DisposeInternal()
		{
			Clear();
		}
	}
	public class MyConcurrentBucketPool<TElement, TAllocator> : MyConcurrentBucketPool<TElement> where TElement : class where TAllocator : IMyElementAllocator<TElement>, new()
	{
		public MyConcurrentBucketPool(string debugName)
			: base(debugName, (IMyElementAllocator<TElement>)new TAllocator())
		{
		}
	}
}
