using System.Threading;

namespace VRage.Library.Parallelization
{
	public struct AtomicFlag
	{
		private int m_state;

		public bool IsSet
		{
			get
			{
				return Volatile.Read(ref m_state) != 0;
			}
			set
			{
				if (value)
				{
					Set();
				}
				else
				{
					Clear();
				}
			}
		}

		public bool Set()
		{
			return Interlocked.Exchange(ref m_state, 1) == 0;
		}

		public bool Clear()
		{
			return Interlocked.Exchange(ref m_state, 0) != 0;
		}
	}
}
