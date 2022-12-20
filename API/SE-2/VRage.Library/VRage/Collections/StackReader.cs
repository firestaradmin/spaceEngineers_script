using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct StackReader<T> : IEnumerable<T>, IEnumerable
	{
		private readonly Stack<T> m_collection;

		public StackReader(Stack<T> collection)
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
