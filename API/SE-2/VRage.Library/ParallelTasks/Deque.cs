using System.Threading;

namespace ParallelTasks
{
	internal class Deque<T>
	{
		private const int INITIAL_SIZE = 32;

		private T[] m_array = new T[32];

		private int m_mask = 31;

		private volatile int m_headIndex;

		private volatile int m_tailIndex;

		private object m_foreignLock = new object();

		public bool IsEmpty => m_headIndex >= m_tailIndex;

		public int Count => m_tailIndex - m_headIndex;

		public void LocalPush(T obj)
		{
			lock (m_foreignLock)
			{
				int num = m_tailIndex;
				if (num < m_headIndex + m_mask)
				{
					m_array[num & m_mask] = obj;
					m_tailIndex = num + 1;
					return;
				}
				int headIndex = m_headIndex;
				int num2 = m_tailIndex - m_headIndex;
				if (num2 >= m_mask)
				{
					T[] array = new T[m_array.Length << 1];
					for (int i = 0; i < num2; i++)
					{
						array[i] = m_array[(i + headIndex) & m_mask];
					}
					m_array = array;
					m_headIndex = 0;
					num = (m_tailIndex = num2);
					m_mask = (m_mask << 1) | 1;
				}
				m_array[num & m_mask] = obj;
				m_tailIndex = num + 1;
			}
		}

		public bool LocalPop(ref T obj)
		{
			lock (m_foreignLock)
			{
				int tailIndex = m_tailIndex;
				if (m_headIndex >= tailIndex)
				{
					return false;
				}
				tailIndex--;
				Interlocked.Exchange(ref m_tailIndex, tailIndex);
				if (m_headIndex <= tailIndex)
				{
					obj = m_array[tailIndex & m_mask];
					return true;
				}
				if (m_headIndex <= tailIndex)
				{
					obj = m_array[tailIndex & m_mask];
					return true;
				}
				m_tailIndex = tailIndex + 1;
				return false;
			}
		}

		public bool TrySteal(ref T obj)
		{
			bool flag = false;
			try
			{
				flag = Monitor.TryEnter(m_foreignLock);
				if (flag)
				{
					int headIndex = m_headIndex;
					Interlocked.Exchange(ref m_headIndex, headIndex + 1);
					if (headIndex < m_tailIndex)
					{
						obj = m_array[headIndex & m_mask];
						return true;
					}
					m_headIndex = headIndex;
					return false;
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(m_foreignLock);
				}
			}
			return false;
		}

		public void Clear()
		{
			for (int i = 0; i < m_array.Length; i++)
			{
				m_array[i] = default(T);
			}
			m_headIndex = 0;
			m_tailIndex = 0;
		}
	}
}
