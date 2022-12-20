using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct SortedDictionaryValuesReader<K, V> : IEnumerable<V>, IEnumerable
	{
		private readonly SortedDictionary<K, V> m_collection;

		public int Count => m_collection.get_Count();

		public V this[K key] => m_collection.get_Item(key);

		public SortedDictionaryValuesReader(SortedDictionary<K, V> collection)
		{
			m_collection = collection;
		}

		public bool TryGetValue(K key, out V result)
		{
			return m_collection.TryGetValue(key, ref result);
		}

		public Enumerator<K, V> GetEnumerator()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return m_collection.get_Values().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator)(object)GetEnumerator();
		}

		IEnumerator<V> IEnumerable<V>.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator<V>)(object)GetEnumerator();
		}

		public static implicit operator SortedDictionaryValuesReader<K, V>(SortedDictionary<K, V> v)
		{
			return new SortedDictionaryValuesReader<K, V>(v);
		}
	}
}
