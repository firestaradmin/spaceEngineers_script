using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Collections
{
	/// <summary>
	/// An observable dictionary.
	/// </summary>
	/// <typeparam name="TKey">The dictionary's key type.</typeparam>
	/// <typeparam name="TValue">The dictionary's value type.</typeparam>
	public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		private readonly Dictionary<TKey, TValue> dictionary;

		/// <summary>
		/// Returns the collection of the keys present in dictionary.
		/// </summary>
		public ICollection<TKey> Keys => dictionary.Keys;

		/// <summary>
		/// Gets the collection of the values present in dictionary.
		/// </summary>
		public ICollection<TValue> Values => dictionary.Values;

		/// <summary>
		/// Gets the cound of items present in dictionary.
		/// </summary>
		public int Count => dictionary.Count;

		/// <summary>
		/// Gets or sets a value associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The associated value.</returns>
		public TValue this[TKey key]
		{
			get
			{
				return dictionary[key];
			}
			set
			{
				TValue value2;
				bool num = dictionary.TryGetValue(key, out value2);
				dictionary[key] = value;
				if (num)
				{
					OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(key, value2));
				}
				OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));
			}
		}

		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).IsReadOnly;

		/// <summary>
		/// Is raised when a new item is added to the dictionary.
		/// </summary>
		public event EventHandler<ObservableDictionaryEventArgs<TKey, TValue>> ItemAdded;

		/// <summary>
		/// Is raised when an item is removed from the dictionary.
		/// </summary>
		public event EventHandler<ObservableDictionaryEventArgs<TKey, TValue>> ItemRemoved;

		/// <inheritdoc />
		public ObservableDictionary()
			: this(0, (IEqualityComparer<TKey>)null)
		{
		}

		/// <inheritdoc />
		public ObservableDictionary(int capacity)
			: this(capacity, (IEqualityComparer<TKey>)null)
		{
		}

		/// <inheritdoc />
		public ObservableDictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}

		/// <inheritdoc />
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
			: this(dictionary, (IEqualityComparer<TKey>)null)
		{
		}

		/// <inheritdoc />
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: this(dictionary?.Count ?? 0, comparer)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				Add(item.Key, item.Value);
			}
		}

		/// <inheritdoc />
		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		/// <summary>
		/// Gets the enumerator of this dictionary.
		/// </summary>
		/// <returns>The enumerator instance of the dictionary.</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		/// <summary>
		/// Removes all items from the dictionary.
		/// </summary>
		public void Clear()
		{
			List<ObservableDictionaryEventArgs<TKey, TValue>> list = new List<ObservableDictionaryEventArgs<TKey, TValue>>();
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				list.Add(new ObservableDictionaryEventArgs<TKey, TValue>(item));
			}
			dictionary.Clear();
			foreach (ObservableDictionaryEventArgs<TKey, TValue> item2 in list)
			{
				OnItemRemoved(item2);
			}
		}

		/// <summary>
		/// Adds a new value with the specified key to dictionary.
		/// </summary>
		/// <param name="key">The added key.</param>
		/// <param name="value">The added value.</param>
		public void Add(TKey key, TValue value)
		{
			dictionary.Add(key, value);
			OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));
		}

		/// <summary>
		/// Checks whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check for presence.</param>
		/// <returns>true if the dictionary contains the provided key, false - otherwise.</returns>
		public bool ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Removes the value corresponding to the specified key from dictionary.
		/// </summary>
		/// <param name="key">The key to remove.</param>
		/// <returns>true if the item was removed, false - otherwise.</returns>
		public bool Remove(TKey key)
		{
			if (!dictionary.TryGetValue(key, out var value))
			{
				return false;
			}
			dictionary.Remove(key);
			OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(key, value));
			return true;
		}

		/// <summary>
		/// Tries to get the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">Contains the returned value on success.</param>
		/// <returns>true if the value was returned successfuly, false - otherwise.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
			OnItemAdded(new ObservableDictionaryEventArgs<TKey, TValue>(item));
		}

		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			bool num = ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
			if (num)
			{
				OnItemRemoved(new ObservableDictionaryEventArgs<TKey, TValue>(item));
			}
			return num;
		}

		/// <inheritdoc />
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Contains(item);
		}

		/// <inheritdoc />
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		protected virtual void OnItemAdded(ObservableDictionaryEventArgs<TKey, TValue> args)
		{
			this.ItemAdded?.Invoke(this, args);
		}

		/// <inheritdoc />
		protected virtual void OnItemRemoved(ObservableDictionaryEventArgs<TKey, TValue> args)
		{
			this.ItemRemoved?.Invoke(this, args);
		}
	}
}
