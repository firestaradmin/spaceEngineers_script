using System;

namespace SharpDX.Toolkit.Collections
{
	/// <summary>
	/// An event providing the item changed in a collection (inserted or removed).
	/// </summary>
	/// <typeparam name="T">Type of a collection item</typeparam>
	public class ObservableCollectionEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets the item from the collection that was inserted or removed.
		/// </summary>
		/// <value>The collection item.</value>
		public T Item { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SharpDX.Toolkit.Collections.ObservableCollectionEventArgs`1" /> class.
		/// </summary>
		/// <param name="item">The item from the collection.</param>
		public ObservableCollectionEventArgs(T item)
		{
			Item = item;
		}
	}
}
