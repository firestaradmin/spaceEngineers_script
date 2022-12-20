using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VRageRender
{
	public static class MyDictionaryExtensions
	{
		public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue def = default(TValue))
		{
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
			}
			return def;
		}

		public static TValue Get<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue def = default(TValue))
		{
<<<<<<< HEAD
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
=======
			TValue result = default(TValue);
			if (dictionary.TryGetValue(key, ref result))
			{
				return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return def;
		}

		public static TValue SetDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue def = default(TValue))
		{
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
			}
			return dictionary[key] = (value = def);
		}

		public static TValue Get<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key, TValue def = default(TValue))
		{
<<<<<<< HEAD
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
=======
			TValue result = default(TValue);
			if (dictionary.TryGetValue(key, ref result))
			{
				return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return def;
		}

		public static TValue SetDefault<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key, TValue def = default(TValue))
		{
<<<<<<< HEAD
			if (dictionary.TryGetValue(key, out var value))
			{
				return value;
			}
			return dictionary[key] = (value = def);
=======
			TValue result = default(TValue);
			if (dictionary.TryGetValue(key, ref result))
			{
				return result;
			}
			TValue result2;
			dictionary.set_Item(key, result2 = (result = def));
			return result2;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
