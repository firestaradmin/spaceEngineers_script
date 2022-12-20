using System;
using System.Collections;
using System.Collections.Generic;

namespace VRageMath
{
	public struct BoxCornerEnumerator : IEnumerator<Vector3>, IEnumerator, IDisposable, IEnumerable<Vector3>, IEnumerable
	{
		private static Vector3B[] m_indicesCache = new Vector3B[8]
		{
			new Vector3B(0, 4, 5),
			new Vector3B(3, 4, 5),
			new Vector3B(3, 1, 5),
			new Vector3B(0, 1, 5),
			new Vector3B(0, 4, 2),
			new Vector3B(3, 4, 2),
			new Vector3B(3, 1, 2),
			new Vector3B(0, 1, 2)
		};

		private int m_index;

		private unsafe fixed float m_minMax[6];

		public unsafe Vector3 Current
		{
			get
			{
				Vector3B vector3B = m_indicesCache[m_index];
				return new Vector3(m_minMax[vector3B.X], m_minMax[vector3B.Y], m_minMax[vector3B.Z]);
			}
		}

		object IEnumerator.Current => Current;

		public unsafe BoxCornerEnumerator(Vector3 min, Vector3 max)
		{
			for (int i = 0; i < 3; i++)
			{
				m_minMax[i] = min.GetDim(i);
				m_minMax[i + 3] = max.GetDim(i);
			}
			m_index = -1;
		}

		public void Add(object tmp)
		{
		}

		public bool MoveNext()
		{
			return ++m_index < 8;
		}

		void IDisposable.Dispose()
		{
		}

		void IEnumerator.Reset()
		{
			m_index = -1;
		}

		public BoxCornerEnumerator GetEnumerator()
		{
			return this;
		}

		IEnumerator<Vector3> IEnumerable<Vector3>.GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}
