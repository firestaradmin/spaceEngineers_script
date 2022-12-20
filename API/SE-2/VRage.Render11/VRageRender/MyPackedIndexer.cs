namespace VRageRender
{
	internal class MyPackedIndexer
	{
		internal int m_free;

		internal int[] m_nextFree;

		internal int[] m_backref;

		internal int[] m_indirection;

		private int m_sizeLimit;

		internal int Size { get; private set; }

		public MyPackedIndexer(int sizeLimit)
		{
			m_sizeLimit = sizeLimit;
			m_indirection = new int[m_sizeLimit];
			m_nextFree = new int[m_sizeLimit];
			m_backref = new int[m_sizeLimit];
			for (int i = 0; i < m_sizeLimit; i++)
			{
				m_nextFree[i] = i + 1;
				m_backref[i] = -1;
			}
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
