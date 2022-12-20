using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	/// <summary>
	/// Class distributing updates on large amount of objects in configurable intervals. 
	/// </summary>
	public class MyDistributedUpdater<TList, TElement> : IEnumerable<TElement>, IEnumerable where TList : IReadOnlyList<TElement>, new()
	{
		public struct Enumerator : IEnumerator<TElement>, IEnumerator, IDisposable
		{
			private MyDistributedUpdater<TList, TElement> m_updater;

			private int m_index;

			/// <inheritdoc />
			public TElement Current => m_updater.List[m_index];

			/// <inheritdoc />
			object IEnumerator.Current => Current;

			public Enumerator(MyDistributedUpdater<TList, TElement> updater)
			{
				m_updater = updater;
				m_index = updater.m_updateIndex - updater.UpdateInterval;
			}

			/// <inheritdoc />
			public bool MoveNext()
			{
				int num = m_index + m_updater.UpdateInterval;
				if (num < m_updater.List.Count)
				{
					m_index = num;
					return true;
				}
				return false;
			}

			/// <inheritdoc />
			public void Reset()
			{
				m_index = -m_updater.UpdateInterval;
			}

			/// <inheritdoc />
			public void Dispose()
			{
				m_updater = null;
				m_index = -1;
			}
		}

		private TList m_list = new TList();

		private int m_updateIndex;

		public int UpdateInterval { get; set; }

		public TList List => m_list;

		public MyDistributedUpdater(int updateInterval)
		{
			UpdateInterval = updateInterval;
		}

		public void Iterate(Action<TElement> p)
		{
			for (int i = m_updateIndex; i < m_list.Count; i += UpdateInterval)
			{
				p(m_list[i]);
			}
		}

		public void Update()
		{
			m_updateIndex++;
			m_updateIndex %= UpdateInterval;
		}

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <inheritdoc />
		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
