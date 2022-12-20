using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct QueueReader<T> : IEnumerable<T>, IEnumerable
	{
		private readonly Queue<T> m_collection;

		public int Count => m_collection.get_Count();

		public QueueReader(Queue<T> collection)
		{
			m_collection = collection;
		}

		public Enumerator<T> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator)(object)GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return (IEnumerator<T>)(object)GetEnumerator();
		}
	}
}
