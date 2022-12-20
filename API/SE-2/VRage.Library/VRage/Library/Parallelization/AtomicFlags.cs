using System.Threading;

namespace VRage.Library.Parallelization
{
	public struct AtomicFlags
	{
		private int m_state;

		public bool IsSet(int flags)
		{
			return (Volatile.Read(ref m_state) & flags) != 0;
		}

		public int Set(int flags)
		{
			int num = m_state;
			int num2;
			while (true)
			{
				num2 = num | flags;
				int num3 = Interlocked.CompareExchange(ref m_state, num2, num);
				if (num == num3)
				{
					break;
				}
				num = num3;
			}
			return num2 ^ num;
		}

		public int Clear(int flags)
		{
			int num = m_state;
			int num2;
			while (true)
			{
				num2 = num & ~flags;
				int num3 = Interlocked.CompareExchange(ref m_state, num2, num);
				if (num == num3)
				{
					break;
				}
				num = num3;
			}
			return num2 ^ num;
		}
	}
}
