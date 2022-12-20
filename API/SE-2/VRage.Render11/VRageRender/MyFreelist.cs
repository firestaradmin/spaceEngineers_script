using System;
using VRage.Utils;

namespace VRageRender
{
	internal class MyFreelist<T> where T : struct
	{
		private T[] m_entities;

		private int[] m_next;

		private int m_sizeLimit;

		private int m_nextFree;

		private int m_holeCount;

		private T m_defaultValue;

		private string m_creationStack;

		private int m_creationSizeLimit;

		public int Size { get; private set; }

		public int Capacity => m_sizeLimit;

		public int FilledSize => Size + m_holeCount;

		public T[] Data => m_entities;

		public ref T this[int index] => ref m_entities[index];

		public void Clear()
		{
			for (int i = 0; i < m_sizeLimit; i++)
			{
				m_entities[i] = m_defaultValue;
				m_next[i] = i + 1;
			}
			m_nextFree = 0;
			m_holeCount = 0;
			Size = 0;
		}

		public MyFreelist(int initialSize, T defaultValue = default(T))
		{
<<<<<<< HEAD
=======
			m_creationStack = Environment.get_StackTrace();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_creationSizeLimit = initialSize;
			m_defaultValue = defaultValue;
			m_sizeLimit = initialSize;
			m_next = new int[initialSize];
			m_entities = new T[initialSize];
			Clear();
		}

		public int Allocate()
		{
			try
			{
				int nextFree = m_nextFree;
				bool flag = false;
				if (nextFree == m_sizeLimit)
				{
					int num = (int)((float)m_sizeLimit * ((m_sizeLimit < 1024) ? 2f : 1.5f));
					Array.Resize(ref m_next, num);
					Array.Resize(ref m_entities, num);
					for (int i = m_sizeLimit; i < num; i++)
					{
						m_next[i] = i + 1;
					}
					m_sizeLimit = num;
					flag = true;
				}
				m_nextFree = m_next[nextFree];
				if (m_nextFree == -1)
				{
					throw new IndexOutOfRangeException($"invalid m_nextFree value @ {nextFree}, resized: {flag}");
				}
				m_next[nextFree] = -1;
				if (nextFree < FilledSize - 1)
				{
					m_holeCount--;
				}
				Size++;
				return nextFree;
			}
			catch (Exception ex)
			{
				Log();
				throw ex;
			}
		}

		public void Free(int index)
		{
			if (m_next[index] != -1)
			{
				throw new ArgumentException("The element is already free.", "index");
			}
			m_next[index] = m_nextFree;
			m_nextFree = index;
			m_entities[index] = m_defaultValue;
			if (Size == 1)
			{
				m_holeCount = 0;
			}
			else if (index < FilledSize - 1)
			{
				m_holeCount++;
			}
			Size--;
		}

		public void Log()
		{
<<<<<<< HEAD
			MyLog.Default.WriteLine($"MyFreelist log: {Size} {m_sizeLimit} {m_nextFree} {m_next.Length} {m_holeCount} {m_creationSizeLimit} {typeof(T)}");
=======
			MyLog.Default.WriteLine($"MyFreelist log: {Size} {m_sizeLimit} {m_nextFree} {m_next.Length} {m_holeCount} {m_creationSizeLimit}");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyLog.Default.WriteLine(string.Join(",", m_next) ?? "");
			MyLog.Default.WriteLine("Creation stack: " + m_creationStack);
		}
	}
}
