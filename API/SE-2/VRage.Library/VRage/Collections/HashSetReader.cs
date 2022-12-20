using System;
using System.Collections;
using System.Collections.Generic;

namespace VRage.Collections
{
	public struct HashSetReader<T> : IEnumerable<T>, IEnumerable
	{
		private readonly HashSet<T> m_hashset;

		public bool IsValid => m_hashset != null;

		public int Count => m_hashset.get_Count();

		public HashSetReader(HashSet<T> set)
		{
			m_hashset = set;
		}

		public static implicit operator HashSetReader<T>(HashSet<T> v)
		{
			return new HashSetReader<T>(v);
		}

		public bool Contains(T item)
		{
			return m_hashset.Contains(item);
		}

		public T First()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<T> enumerator = GetEnumerator();
			try
			{
				if (!enumerator.MoveNext())
				{
					throw new InvalidOperationException("No elements in collection!");
				}
				return enumerator.get_Current();
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public T[] ToArray()
		{
			T[] array = new T[m_hashset.get_Count()];
			m_hashset.CopyTo(array);
			return array;
		}

		public Enumerator<T> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_hashset.GetEnumerator();
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
