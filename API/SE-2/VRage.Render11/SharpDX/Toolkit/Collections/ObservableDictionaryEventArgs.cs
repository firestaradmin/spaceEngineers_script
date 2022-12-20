using System;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Collections
{
	/// <summary>
	/// Event arguments for the <see cref="E:SharpDX.Toolkit.Collections.ObservableDictionary`2.ItemAdded" /> and <see cref="E:SharpDX.Toolkit.Collections.ObservableDictionary`2.ItemRemoved" /> events.
	/// </summary>
	/// <typeparam name="TKey">The dictionary key type.</typeparam>
	/// <typeparam name="TValue">The dictionary value type.</typeparam>
	public class ObservableDictionaryEventArgs<TKey, TValue> : EventArgs
	{
		/// <summary>
		/// Gets the event's key argument.
		/// </summary>
		public TKey Key { get; private set; }

		/// <summary>
		/// Gets the event's value argument.
		/// </summary>
		public TValue Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Collections.ObservableDictionaryEventArgs`2" /> class from the provided <see cref="T:System.Collections.Generic.KeyValuePair`2" />.
		/// </summary>
		/// <param name="pair">The <see cref="T:System.Collections.Generic.KeyValuePair`2" /> that contains the event arguments.</param>
		public ObservableDictionaryEventArgs(KeyValuePair<TKey, TValue> pair)
			: this(pair.Key, pair.Value)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Collections.ObservableDictionaryEventArgs`2" /> class from the provided key and value.
		/// </summary>
		/// <param name="key">The event's key argument.</param>
		/// <param name="value">The event's value argument.</param>
		public ObservableDictionaryEventArgs(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}
	}
}
