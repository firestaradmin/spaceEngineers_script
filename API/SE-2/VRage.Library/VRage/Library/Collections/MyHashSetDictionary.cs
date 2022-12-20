using System.Collections.Generic;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Collection which stores multiple elements under same key by using list.
	/// Collection does not allow removing single value, only all items with same key.
	/// </summary>
	public class MyHashSetDictionary<TKey, TValue> : MyCollectionDictionary<TKey, HashSet<TValue>, TValue>
	{
		private readonly IEqualityComparer<TValue> m_valueComparer;

		public MyHashSetDictionary()
		{
		}

		public MyHashSetDictionary(IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
			: base(keyComparer)
		{
			m_valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
		}

		protected override HashSet<TValue> CreateCollection()
		{
<<<<<<< HEAD
			return new HashSet<TValue>(m_valueComparer);
=======
			return (HashSet<TValue>)(object)new HashSet<_003F>((IEqualityComparer<_003F>)m_valueComparer);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
