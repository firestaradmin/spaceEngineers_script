using System;
using System.Diagnostics;
using System.Threading;

namespace VRage
{
	public sealed class FastResourceLock : IDisposable, IResourceLock
	{
		private const int LOCK_OWNED = 1;

		private const int LOCK_EXCLUSIVE_WAKING = 2;

		private const int LOCK_SHARED_OWNERS_SHIFT = 2;

		private const int LOCK_SHARED_OWNERS_MASK = 1023;

		private const int LOCK_SHARED_OWNERS_INCREMENT = 4;

		private const int LOCK_SHARED_WAITERS_SHIFT = 12;

		private const int LOCK_SHARED_WAITERS_MASK = 1023;

		private const int LOCK_SHARED_WAITERS_INCREMENT = 4096;

		private const int LOCK_EXCLUSIVE_WAITERS_SHIFT = 22;

		private const int LOCK_EXCLUSIVE_WAITERS_MASK = 1023;

		private const int LOCK_EXCLUSIVE_WAITERS_INCREMENT = 4194304;

		private const int EXCLUSIVE_MASK = -4194302;

<<<<<<< HEAD
		private static readonly int SPIN_COUNT = ((Environment.ProcessorCount != 1) ? 4000 : 0);
=======
		private static readonly int SPIN_COUNT = ((Environment.get_ProcessorCount() != 1) ? 4000 : 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private int m_value;

		private Semaphore m_sharedWakeEvent;

		private Semaphore m_exclusiveWakeEvent;

		public int ExclusiveWaiters => (m_value >> 22) & 0x3FF;

		public bool Owned => (m_value & 1) != 0;

		public int SharedOwners => (m_value >> 2) & 0x3FF;

		public int SharedWaiters => (m_value >> 12) & 0x3FF;

		public FastResourceLock()
		{
			m_value = 0;
			m_sharedWakeEvent = new Semaphore(0, int.MaxValue);
			m_exclusiveWakeEvent = new Semaphore(0, int.MaxValue);
		}

		~FastResourceLock()
		{
			Dispose(disposing: false);
		}

		private void Dispose(bool disposing)
		{
			if (m_sharedWakeEvent != null)
			{
				m_sharedWakeEvent.Dispose();
				m_sharedWakeEvent = null;
			}
			if (m_exclusiveWakeEvent != null)
			{
				m_exclusiveWakeEvent.Dispose();
				m_exclusiveWakeEvent = null;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		[DebuggerStepThrough]
		public void AcquireExclusive()
		{
			int num = 0;
			int value;
			while (true)
			{
				value = m_value;
				if ((value & 3) == 0)
				{
					if (Interlocked.CompareExchange(ref m_value, value + 1, value) == value)
					{
						return;
					}
				}
				else if (num >= SPIN_COUNT && Interlocked.CompareExchange(ref m_value, value + 4194304, value) == value)
				{
					break;
				}
				num++;
			}
			m_exclusiveWakeEvent.WaitOne();
			SpinWait spinWait = default(SpinWait);
			value = m_value;
			while (Interlocked.CompareExchange(ref m_value, value + 1 - 2, value) != value)
			{
				spinWait.SpinOnce();
				value = m_value;
			}
		}

		[DebuggerStepThrough]
		public void AcquireShared()
		{
			int num = 0;
			while (true)
			{
				int value = m_value;
				if ((value & -4190209) == 0)
				{
					if (Interlocked.CompareExchange(ref m_value, value + 1 + 4, value) == value)
					{
						break;
					}
				}
				else if (((uint)value & (true ? 1u : 0u)) != 0 && ((uint)(value >> 2) & 0x3FFu) != 0 && (value & -4194302) == 0)
				{
					if (Interlocked.CompareExchange(ref m_value, value + 4, value) == value)
					{
						break;
					}
				}
				else if (num >= SPIN_COUNT && Interlocked.CompareExchange(ref m_value, value + 4096, value) == value)
				{
					m_sharedWakeEvent.WaitOne();
					continue;
				}
				num++;
			}
		}

		public void ConvertExclusiveToShared()
		{
			SpinWait spinWait = default(SpinWait);
			int num;
			while (true)
			{
				int value = m_value;
				num = (value >> 12) & 0x3FF;
				if (Interlocked.CompareExchange(ref m_value, (value + 4) & -4190209, value) == value)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			if (num != 0)
			{
				m_sharedWakeEvent.Release(num);
			}
		}

		public void ReleaseExclusive()
		{
			SpinWait spinWait = default(SpinWait);
			int num;
			while (true)
			{
				int value = m_value;
				if (((uint)(value >> 22) & 0x3FFu) != 0)
				{
					if (Interlocked.CompareExchange(ref m_value, value - 1 + 2 - 4194304, value) == value)
					{
						m_exclusiveWakeEvent.Release(1);
						return;
					}
				}
				else
				{
					num = (value >> 12) & 0x3FF;
					if (Interlocked.CompareExchange(ref m_value, value & -4190210, value) == value)
					{
						break;
					}
				}
				spinWait.SpinOnce();
			}
			if (num != 0)
			{
				m_sharedWakeEvent.Release(num);
			}
		}

		public void ReleaseShared()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int value = m_value;
				int num = (value >> 2) & 0x3FF;
				if (num > 1)
				{
					if (Interlocked.CompareExchange(ref m_value, value - 4, value) == value)
					{
						break;
					}
				}
				else if (((uint)(value >> 22) & 0x3FFu) != 0)
				{
					if (Interlocked.CompareExchange(ref m_value, value - 1 + 2 - 4 - 4194304, value) == value)
					{
						m_exclusiveWakeEvent.Release(1);
						break;
					}
				}
				else if (Interlocked.CompareExchange(ref m_value, value - 1 - 4, value) == value)
				{
					break;
				}
				spinWait.SpinOnce();
			}
		}

		[DebuggerStepThrough]
		public void SpinAcquireExclusive()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int value = m_value;
				if (((uint)value & 3u) != 0 || Interlocked.CompareExchange(ref m_value, value + 1, value) != value)
				{
					spinWait.SpinOnce();
					continue;
				}
				break;
			}
		}

		[DebuggerStepThrough]
		public void SpinAcquireShared()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int value = m_value;
				if ((value & -4194302) == 0)
				{
					if ((value & 1) == 0)
					{
						if (Interlocked.CompareExchange(ref m_value, value + 1 + 4, value) == value)
						{
							break;
						}
					}
					else if (((uint)(value >> 2) & 0x3FFu) != 0 && Interlocked.CompareExchange(ref m_value, value + 4, value) == value)
					{
						break;
					}
				}
				spinWait.SpinOnce();
			}
		}

		[DebuggerStepThrough]
		public void SpinConvertSharedToExclusive()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int value = m_value;
				if (((value >> 2) & 0x3FF) != 1 || Interlocked.CompareExchange(ref m_value, value - 4, value) != value)
				{
					spinWait.SpinOnce();
					continue;
				}
				break;
			}
		}

		public bool TryAcquireExclusive()
		{
			int value = m_value;
			if (((uint)value & 3u) != 0)
			{
				return false;
			}
			return Interlocked.CompareExchange(ref m_value, value + 1, value) == value;
		}

		public bool TryAcquireShared()
		{
			int value = m_value;
			if (((uint)value & 0xFFC00002u) != 0)
			{
				return false;
			}
			if ((value & 1) == 0)
			{
				return Interlocked.CompareExchange(ref m_value, value + 1 + 4, value) == value;
			}
			if (((uint)(value >> 2) & 0x3FFu) != 0)
			{
				return Interlocked.CompareExchange(ref m_value, value + 4, value) == value;
			}
			return false;
		}

		public bool TryConvertSharedToExclusive()
		{
			SpinWait spinWait = default(SpinWait);
			while (true)
			{
				int value = m_value;
				if (((value >> 2) & 0x3FF) != 1)
				{
					return false;
				}
				if (Interlocked.CompareExchange(ref m_value, value - 4, value) == value)
				{
					break;
				}
				spinWait.SpinOnce();
			}
			return true;
		}
	}
}
