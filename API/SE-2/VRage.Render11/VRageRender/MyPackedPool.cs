using System;

namespace VRageRender
{
	internal class MyPackedPool<T> where T : struct
	{
		internal T[] m_entities;

		private MyPackedIndexerDynamic m_indexer;

		public int Size => m_indexer.Size;

		public T[] Data => m_entities;

		public MyPackedPool(int startingSize)
		{
			m_indexer = new MyPackedIndexerDynamic(startingSize);
			m_entities = new T[startingSize];
		}

		public T GetByHandle(MyPackedPoolHandle handle)
		{
			return m_entities[m_indexer.GetIndex(handle)];
		}

		public int AsIndex(MyPackedPoolHandle handle)
		{
			return m_indexer.GetIndex(handle);
		}

		public void Free(MyPackedPoolHandle handle)
		{
			m_entities[m_indexer.GetIndex(handle)] = m_entities[m_indexer.Size - 1];
			m_entities[m_indexer.Size - 1] = new T();
			m_indexer.Free(handle);
		}

		public MyPackedPoolHandle Allocate()
		{
			MyPackedPoolHandle result = m_indexer.Allocate();
			if (m_indexer.Capacity != m_entities.Length)
			{
				Array.Resize(ref m_entities, m_indexer.Capacity);
			}
			m_entities[m_indexer.Size - 1] = new T();
			return result;
		}

		public void Clear()
		{
			for (int i = 0; i < m_indexer.Size; i++)
			{
				m_entities[i] = default(T);
			}
			m_indexer.Clear();
		}
	}
}
