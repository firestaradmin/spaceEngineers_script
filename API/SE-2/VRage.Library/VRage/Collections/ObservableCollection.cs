using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VRage.Collections
{
<<<<<<< HEAD
	/// <summary>
	/// Observable collection that also fix support to clear all.
	/// Don't know if ObservableCollection&lt;T&gt; is allocation free.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>
=======
	public class ObservableCollection<T> : ObservableCollection<T>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		/// <summary>
		/// Enumerator which uses index access.
		/// Index access on Collection is O(1) operation
		/// </summary>
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private ObservableCollection<T> m_collection;

			private int m_index;

			public T Current => ((Collection<T>)(object)m_collection)[m_index];

			object IEnumerator.Current => Current;

			public Enumerator(ObservableCollection<T> collection)
			{
				m_index = -1;
				m_collection = collection;
			}

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				m_index++;
				return m_index < ((Collection<T>)(object)m_collection).Count;
			}

			public void Reset()
			{
				m_index = -1;
			}
		}

		public bool FireEvents = true;

		/// <summary>
		/// Clears the items.
		/// </summary>
		protected override void ClearItems()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			NotifyCollectionChangedEventArgs val = new NotifyCollectionChangedEventArgs((NotifyCollectionChangedAction)1, (IList)this);
			if (FireEvents)
			{
				((ObservableCollection<T>)this).OnCollectionChanged(val);
			}
			base.ClearItems();
		}

		/// <summary>
		/// Gets allocation free enumerator (returns struct)
		/// </summary>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public int FindIndex(Predicate<T> match)
		{
			int result = -1;
			for (int i = 0; i < ((Collection<T>)(object)this).Items.Count; i++)
			{
				if (match(((Collection<T>)(object)this).Items[i]))
				{
					result = i;
					break;
				}
			}
			return result;
		}
	}
}
