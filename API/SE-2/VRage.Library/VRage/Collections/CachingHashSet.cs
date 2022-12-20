using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public class CachingHashSet<T> : IEnumerable<T>, IEnumerable
	{
		private HashSet<T> m_hashSet = new HashSet<T>();

		private HashSet<T> m_toAdd = new HashSet<T>();

		private HashSet<T> m_toRemove = new HashSet<T>();

		public int Count => m_hashSet.get_Count();

		public void Clear()
		{
			m_hashSet.Clear();
			m_toAdd.Clear();
			m_toRemove.Clear();
		}

		public bool Contains(T item)
		{
			return m_hashSet.Contains(item);
		}

		public bool Add(T item)
		{
			if (!m_toRemove.Remove(item) && !m_hashSet.Contains(item))
			{
				return m_toAdd.Add(item);
			}
			return false;
		}

		public void Remove(T item, bool immediate = false)
		{
			if (immediate)
			{
				m_toAdd.Remove(item);
				m_hashSet.Remove(item);
				m_toRemove.Remove(item);
			}
			else if (!m_toAdd.Remove(item) && m_hashSet.Contains(item))
			{
				m_toRemove.Add(item);
			}
		}

		public void ApplyChanges()
		{
			ApplyAdditions();
			ApplyRemovals();
		}

		public void ApplyAdditions()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
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

		public void ApplyRemovals()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
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

		public Enumerator<T> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_hashSet.GetEnumerator();
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
