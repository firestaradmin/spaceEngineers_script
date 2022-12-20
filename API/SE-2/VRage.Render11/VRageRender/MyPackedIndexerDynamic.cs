using System;

namespace VRageRender
{
	internal class MyPackedIndexerDynamic
	{
		internal int m_free;

		internal int[] m_nextFree;

		internal int[] m_backref;

		internal int[] m_indirection;

		private int m_sizeLimit;

		internal int Size { get; private set; }

		internal int Capacity => m_sizeLimit;

		public MyPackedIndexerDynamic(int startingSize)
		{
			m_sizeLimit = startingSize;
			m_indirection = new int[m_sizeLimit];
			m_nextFree = new int[m_sizeLimit];
			m_backref = new int[m_sizeLimit];
			for (int i = 0; i < m_sizeLimit; i++)
			{
				m_nextFree[i] = i + 1;
				m_backref[i] = -1;
			}
		}

		public void Extend(int newSize)
		{
			Array.Resize(ref m_nextFree, newSize);
			Array.Resize(ref m_backref, newSize);
			Array.Resize(ref m_indirection, newSize);
			for (int i = m_sizeLimit; i < newSize; i++)
			{
				m_nextFree[i] = i + 1;
				m_backref[i] = -1;
			}
			m_sizeLimit = newSize;
		}

		public int GetIndex(MyPackedPoolHandle handle)
		{
			return m_indirection[handle.INDEX];
		}

		public void Free(MyPackedPoolHandle handle)
		{
			int iNDEX = handle.INDEX;
			m_nextFree[iNDEX] = m_free;
			m_free = iNDEX;
			m_indirection[m_backref[Size - 1]] = m_indirection[iNDEX];
			m_backref[m_indirection[iNDEX]] = m_backref[Size - 1];
			m_indirection[iNDEX] = m_sizeLimit;
			Size--;
		}

		public MyPackedPoolHandle Allocate()
		{
			if (Size == m_sizeLimit)
			{
				float num = (float)m_sizeLimit * ((m_sizeLimit > 1024) ? 2f : 1.5f);
				Extend((int)Math.Ceiling(num));
			}
			MyPackedPoolHandle result = new MyPackedPoolHandle(m_free);
			m_backref[Size] = m_free;
			m_indirection[m_free] = Size;
			m_free = m_nextFree[m_free];
			Size++;
			return result;
		}

		public void Clear()
		{
			Size = 0;
			for (int i = 0; i < m_sizeLimit; i++)
			{
				m_nextFree[i] = i + 1;
				m_backref[i] = -1;
			}
		}
	}
}
