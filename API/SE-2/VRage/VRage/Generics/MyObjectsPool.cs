using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRage.Collections;
using VRage.Library.Threading;

namespace VRage.Generics
{
	public class MyObjectsPool<T> where T : class, new()
	{
		private MyConcurrentQueue<T> m_unused;

		private HashSet<T> m_active;

		private HashSet<T> m_marked;

		private SpinLockRef m_activeLock = new SpinLockRef();

		private Func<T> m_activator;

		private Action<T> m_clearFunction;

		private int m_baseCapacity;

		public SpinLockRef ActiveLock => m_activeLock;

		public HashSetReader<T> ActiveWithoutLock => new HashSetReader<T>(m_active);

		public HashSetReader<T> Active
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return new HashSetReader<T>(m_active);
				}
			}
		}

		public int ActiveCount
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return m_active.get_Count();
				}
			}
		}

		public int BaseCapacity => m_baseCapacity;

		public int Capacity
		{
			get
			{
				using (m_activeLock.Acquire())
				{
					return m_unused.Count + m_active.get_Count();
				}
			}
		}

		public MyObjectsPool(int baseCapacity, Func<T> activator = null, Action<T> clearFunction = null)
		{
			m_clearFunction = clearFunction;
			m_activator = activator ?? ExpressionExtension.CreateActivator<T>();
			m_baseCapacity = baseCapacity;
			m_unused = new MyConcurrentQueue<T>(m_baseCapacity);
			m_active = new HashSet<T>();
			m_marked = new HashSet<T>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue(m_activator());
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out T item)
		{
			bool flag = false;
			using (m_activeLock.Acquire())
			{
				flag = m_unused.Count == 0;
				if (flag)
				{
					item = m_activator();
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
			T result = null;
			using (m_activeLock.Acquire())
			{
				if (m_unused.Count > 0)
				{
					result = m_unused.Dequeue();
					m_active.Add(result);
					return result;
				}
				return result;
			}
		}

		public void Deallocate(T item)
		{
			using (m_activeLock.Acquire())
			{
				m_active.Remove(item);
				m_clearFunction?.Invoke(item);
				m_unused.Enqueue(item);
			}
		}

		public void MarkForDeallocate(T item)
		{
			using (m_activeLock.Acquire())
			{
				m_marked.Add(item);
			}
		}

		public void MarkAllActiveForDeallocate()
		{
			using (m_activeLock.Acquire())
			{
				m_marked.UnionWith((IEnumerable<T>)m_active);
			}
		}

		public void DeallocateAllMarked()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			using (m_activeLock.Acquire())
			{
				Enumerator<T> enumerator = m_marked.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.get_Current();
						m_active.Remove(current);
						m_clearFunction?.Invoke(current);
						m_unused.Enqueue(current);
					}
				}
				finally
				{
<<<<<<< HEAD
					m_active.Remove(item);
					m_clearFunction?.Invoke(item);
					m_unused.Enqueue(item);
=======
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_marked.Clear();
			}
		}

		public void DeallocateAll()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			using (m_activeLock.Acquire())
			{
				Enumerator<T> enumerator = m_active.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.get_Current();
						m_clearFunction?.Invoke(current);
						m_unused.Enqueue(current);
					}
				}
				finally
				{
<<<<<<< HEAD
					m_clearFunction?.Invoke(item);
					m_unused.Enqueue(item);
=======
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_active.Clear();
				m_marked.Clear();
			}
		}

		public void TrimInternalCollections()
		{
			using (m_activeLock.Acquire())
			{
				m_active.TrimExcess();
				m_marked.TrimExcess();
			}
		}
	}
}
