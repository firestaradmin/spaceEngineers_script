using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Threading;

namespace VRage.Collections
{
	public class ConcurrentCachingHashSet<T> : IEnumerable<T>, IEnumerable
	{
		private readonly HashSet<T> m_hashSet = new HashSet<T>();

		private readonly HashSet<T> m_toAdd = new HashSet<T>();

		private readonly HashSet<T> m_toRemove = new HashSet<T>();

		private readonly SpinLockRef m_setLock = new SpinLockRef();

		private readonly SpinLockRef m_changelistLock = new SpinLockRef();

		public int Count
		{
			get
			{
				using (m_setLock.Acquire())
				{
					return m_hashSet.get_Count();
				}
			}
		}

		public void Clear(bool clearCache = true)
		{
			using (m_setLock.Acquire())
			{
				using (m_changelistLock.Acquire())
				{
					m_hashSet.Clear();
					if (clearCache)
					{
						m_toAdd.Clear();
						m_toRemove.Clear();
					}
				}
			}
		}

		public bool Contains(T item)
		{
			using (m_setLock.Acquire())
			{
				return m_hashSet.Contains(item);
			}
		}

		public void Add(T item)
		{
			using (m_changelistLock.Acquire())
			{
				m_toRemove.Remove(item);
				m_toAdd.Add(item);
			}
		}

		public void Remove(T item, bool immediate = false)
		{
			if (immediate)
			{
				using (m_setLock.Acquire())
				{
					using (m_changelistLock.Acquire())
					{
						m_toAdd.Remove(item);
						m_toRemove.Remove(item);
						m_hashSet.Remove(item);
					}
				}
			}
			else
			{
				using (m_changelistLock.Acquire())
				{
					m_toAdd.Remove(item);
					m_toRemove.Add(item);
				}
			}
		}

		public void ApplyChanges()
		{
			ApplyAdditions();
			ApplyRemovals();
		}

		public void ApplyAdditions()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			using (m_setLock.Acquire())
			{
				using (m_changelistLock.Acquire())
				{
					Enumerator<T> enumerator = m_toAdd.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T current = enumerator.get_Current();
							m_hashSet.Add(current);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					m_toAdd.Clear();
				}
			}
		}

		public void ApplyRemovals()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			using (m_setLock.Acquire())
			{
				using (m_changelistLock.Acquire())
				{
					Enumerator<T> enumerator = m_toRemove.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							T current = enumerator.get_Current();
							m_hashSet.Remove(current);
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					m_toRemove.Clear();
				}
			}
		}

		public Enumerator<T> GetEnumerator()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			using (m_setLock.Acquire())
			{
				return m_hashSet.GetEnumerator();
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator<T>)(object)GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator)(object)GetEnumerator();
		}

		public override string ToString()
		{
			return $"Count = {m_hashSet.get_Count()}; ToAdd = {m_toAdd.get_Count()}; ToRemove = {m_toRemove.get_Count()}";
		}
	}
}
