using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct SortedDictionaryReader<K, V> : IEnumerable<KeyValuePair<K, V>>, IEnumerable
	{
		private readonly SortedDictionary<K, V> m_collection;

		public static readonly SortedDictionaryReader<K, V> Empty;

		public bool IsValid => m_collection != null;

		public int Count => m_collection.get_Count();

		public V this[K key] => m_collection.get_Item(key);

		public IEnumerable<K> Keys => (IEnumerable<K>)m_collection.get_Keys();

		public IEnumerable<V> Values => (IEnumerable<V>)m_collection.get_Values();

		public SortedDictionaryReader(SortedDictionary<K, V> collection)
		{
			m_collection = collection;
		}

		public bool ContainsKey(K key)
		{
			return m_collection.ContainsKey(key);
		}

		public bool TryGetValue(K key, out V value)
		{
			return m_collection.TryGetValue(key, ref value);
		}

		public Enumerator<K, V> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator)(object)GetEnumerator();
		}

		IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator<KeyValuePair<K, V>>)(object)GetEnumerator();
		}

		public static implicit operator SortedDictionaryReader<K, V>(SortedDictionary<K, V> v)
		{
			return new SortedDictionaryReader<K, V>(v);
		}
	}
}
