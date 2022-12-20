using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	public static class DictionaryExtensions
	{
		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key)
		{
			dictionary.TryGetValue(key, out var value);
			return value;
		}

		public static V GetValueOrDefault<K, V>(this Dictionary<K, V> dictionary, K key, V defaultValue)
		{
			if (!dictionary.TryGetValue(key, out var value))
			{
				return defaultValue;
			}
			return value;
		}

		public static KeyValuePair<K, V> FirstPair<K, V>(this Dictionary<K, V> dictionary)
		{
			Dictionary<K, V>.Enumerator enumerator = dictionary.GetEnumerator();
			enumerator.MoveNext();
			return enumerator.Current;
		}

		public static V GetValueOrDefault<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V defaultValue)
		{
<<<<<<< HEAD
			if (!dictionary.TryGetValue(key, out var value))
=======
			V result = default(V);
			if (!dictionary.TryGetValue(key, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return defaultValue;
			}
			return result;
		}

		public static void Remove<K, V>(this ConcurrentDictionary<K, V> dictionary, K key)
		{
<<<<<<< HEAD
			dictionary.TryRemove(key, out var _);
=======
			V val = default(V);
			dictionary.TryRemove(key, ref val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static TValue GetOrAdd<TKey, TValue, TContext>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TContext context, Func<TContext, TKey, TValue> activator)
		{
<<<<<<< HEAD
			if (!dictionary.TryGetValue(key, out var value))
=======
			TValue result = default(TValue);
			if (!dictionary.TryGetValue(key, ref result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return dictionary.GetOrAdd(key, activator(context, key));
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[DebuggerStepThrough]
		public static void AssertEmpty<K, V>(this Dictionary<K, V> collection)
		{
			if (collection.Count != 0)
			{
				collection.Clear();
			}
		}
	}
}
