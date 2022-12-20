using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRage.Generics
{
	public class MyConcurrentObjectsPool<T> where T : class, new()
	{
		private readonly FastResourceLock m_lock = new FastResourceLock();

		private readonly MyQueue<T> m_unused;

		private readonly HashSet<T> m_active;

		private readonly HashSet<T> m_marked;

		private readonly int m_baseCapacity;

		public int ActiveCount
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_active.get_Count();
				}
			}
		}

		public int BaseCapacity
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					m_lock.AcquireShared();
					return m_baseCapacity;
				}
			}
		}

		public int Capacity
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					m_lock.AcquireShared();
					return m_unused.Count + m_active.get_Count();
				}
			}
		}

		public void ApplyActionOnAllActives(Action<T> action)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			using (m_lock.AcquireSharedUsing())
			{
				Enumerator<T> enumerator = m_active.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.get_Current();
						action(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		private MyConcurrentObjectsPool()
		{
		}

		public MyConcurrentObjectsPool(int baseCapacity)
		{
			m_baseCapacity = baseCapacity;
			m_unused = new MyQueue<T>(m_baseCapacity);
			m_active = new HashSet<T>();
			m_marked = new HashSet<T>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue(new T());
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				bool flag = m_unused.Count == 0;
				if (flag)
				{
					item = new T();
				}
				else
				{
					item = m_unused.Dequeue();
				}
				m_active.Add(item);
				return flag;
			}
		}

		public T Allocate(bool nullAllowed = false)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				T val = null;
				if (m_unused.Count > 0)
				{
					val = m_unused.Dequeue();
					m_active.Add(val);
				}
				return val;
			}
		}

		public void Deallocate(T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_active.Remove(item);
				m_unused.Enqueue(item);
			}
		}

		public void MarkForDeallocate(T item)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_marked.Add(item);
			}
		}

		public void DeallocateAllMarked()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			using (m_lock.AcquireExclusiveUsing())
			{
				Enumerator<T> enumerator = m_marked.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.get_Current();
						m_active.Remove(current);
						m_unused.Enqueue(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_marked.Clear();
			}
		}

		public void DeallocateAll()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			using (m_lock.AcquireExclusiveUsing())
			{
				Enumerator<T> enumerator = m_active.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.get_Current();
						m_unused.Enqueue(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				m_active.Clear();
				m_marked.Clear();
			}
		}
	}
}
