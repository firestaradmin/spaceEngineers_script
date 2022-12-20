using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Library.Collections;

namespace VRage.Collections
{
	/// <summary>
	/// Simple thread-safe queue.
	/// Uses spin-lock
	/// </summary>
	public class MyConcurrentSortedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		private SortedDictionary<TKey, TValue> m_dictionary;

		private FastResourceLock m_lock = new FastResourceLock();

		public int Count
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_dictionary.get_Count();
				}
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				using (m_lock.AcquireSharedUsing())
				{
					return m_dictionary.get_Item(key);
				}
			}
			set
			{
				using (m_lock.AcquireExclusiveUsing())
				{
					m_dictionary.set_Item(key, value);
				}
			}
		}

		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TKey, KeyCollection<TKey, TValue>> Keys
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TKey, KeyCollection<TKey, TValue>>(@lock, (IEnumerable<TKey>)m_dictionary.get_Keys());
			}
		}

		public ConcurrentEnumerable<FastResourceLockExtensions.MySharedLock, TValue, ValueCollection<TKey, TValue>> Values
		{
			get
			{
				FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
				return ConcurrentEnumerable.Create<FastResourceLockExtensions.MySharedLock, TValue, ValueCollection<TKey, TValue>>(@lock, (IEnumerable<TValue>)m_dictionary.get_Values());
			}
		}

		public MyConcurrentSortedDictionary(IComparer<TKey> comparer = null)
		{
			m_dictionary = new SortedDictionary<TKey, TValue>(comparer);
		}

		public TValue ChangeKey(TKey oldKey, TKey newKey)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				TValue val = m_dictionary.get_Item(oldKey);
				m_dictionary.Remove(oldKey);
				m_dictionary.set_Item(newKey, val);
				return val;
			}
		}

		public void Clear()
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_dictionary.Clear();
			}
		}

		public void Add(TKey key, TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				m_dictionary.Add(key, value);
			}
		}

		public bool TryAdd(TKey key, TValue value)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				if (!m_dictionary.ContainsKey(key))
				{
					m_dictionary.Add(key, value);
					return true;
				}
				return false;
			}
		}

		public bool ContainsKey(TKey key)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.ContainsKey(key);
			}
		}

		public bool ContainsValue(TValue value)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.ContainsValue(value);
			}
		}

		public bool Remove(TKey key)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				return m_dictionary.Remove(key);
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			using (m_lock.AcquireSharedUsing())
			{
				return m_dictionary.TryGetValue(key, ref value);
			}
		}

		public void GetValues(List<TValue> result)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			using (m_lock.AcquireSharedUsing())
			{
				Enumerator<TKey, TValue> enumerator = m_dictionary.get_Values().GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						TValue current = enumerator.get_Current();
						result.Add(current);
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
		}

		public ConcurrentEnumerator<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Enumerator<TKey, TValue>> GetEnumerator()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			FastResourceLockExtensions.MySharedLock @lock = m_lock.AcquireSharedUsing();
			return ConcurrentEnumerator.Create<FastResourceLockExtensions.MySharedLock, KeyValuePair<TKey, TValue>, Enumerator<TKey, TValue>>(@lock, m_dictionary.GetEnumerator());
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
