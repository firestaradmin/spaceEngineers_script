using System;
using System.Threading;

namespace ParallelTasks
{
	/// <summary>
	/// Provides support for spin-based waiting.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <see cref="T:ParallelTasks.MySpinWait" /> encapsulates common spinning logic. On single-processor machines, yields are
	/// always used instead of busy waits, and on computers with Intel™ processors employing Hyper-Threading™
	/// technology, it helps to prevent hardware thread starvation. MySpinWait encapsulates a good mixture of
	/// spinning and true yielding.
	/// </para>
	/// <para>
	/// <see cref="T:ParallelTasks.MySpinWait" /> is a value type, which means that low-level code can utilize MySpinWait without
	/// fear of unnecessary allocation overheads. MySpinWait is not generally useful for ordinary applications.
	/// In most cases, you should use the synchronization classes provided by the .NET Framework, such as
	/// <see cref="T:System.Threading.Monitor" />. For most purposes where spin waiting is required, however,
	/// the <see cref="T:ParallelTasks.MySpinWait" /> type should be preferred over the <see cref="M:System.Threading.Thread.SpinWait(System.Int32)" /> method.
	/// </para>
	/// <para>
	/// While MySpinWait is designed to be used in concurrent applications, it is not designed to be
	/// used from multiple threads concurrently.  MySpinWait's members are not thread-safe.  If multiple
	/// threads must spin, each should use its own instance of MySpinWait.
	/// </para>
	/// </remarks>
	public struct MySpinWait
	{
		internal const int YIELD_THRESHOLD = 10;

		internal const int SLEEP_0_EVERY_HOW_MANY_TIMES = 5;

		internal const int SLEEP_1_EVERY_HOW_MANY_MS = 10;

		internal const int SLEEP_LONG_EVERY_HOW_MANY_MS = 100;

		private int m_count;

		private long m_startTime;

		private long m_startTimeLong;

		/// <summary>
		/// Gets the number of times <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> has been called on this instance.
		/// </summary>
		public int Count => m_count;

		/// <summary>
		/// Gets whether the next call to <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> will yield the processor, triggering a
		/// forced context switch.
		/// </summary>
		/// <value>Whether the next call to <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> will yield the processor, triggering a
		/// forced context switch.</value>
		/// <remarks>
		/// On a single-CPU machine, <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> always yields the processor. On machines with
		/// multiple CPUs, <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> may yield after an unspecified number of calls.
		/// </remarks>
		public bool NextSpinWillYield
		{
			get
			{
				if (m_count <= 10)
				{
					return PlatformHelper.IsSingleProcessor;
				}
				return true;
			}
		}

		/// <summary>
		/// Performs a single spin.
		/// </summary>
		/// <remarks>
		/// This is typically called in a loop, and may change in behavior based on the number of times a
		/// <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> has been called thus far on this instance.
		/// </remarks>
		public void SpinOnce()
		{
			if (NextSpinWillYield)
			{
				if (m_startTime == 0L)
				{
					m_startTime = TimeoutHelper.GetTime();
					m_startTimeLong = TimeoutHelper.GetTime();
				}
				int num = ((m_count >= 10) ? (m_count - 10) : m_count);
				if (TimeoutHelper.UpdateTimeOut(m_startTimeLong, 100) <= 0)
				{
					Thread.Sleep(13);
					m_startTime = (m_startTimeLong = TimeoutHelper.GetTime());
				}
				else if (TimeoutHelper.UpdateTimeOut(m_startTime, 10) <= 0)
				{
					Thread.Sleep(1);
					m_startTime = TimeoutHelper.GetTime();
				}
				else if (num % 5 == 4)
				{
					Thread.Sleep(0);
				}
				else
				{
					Thread.Yield();
				}
			}
			else
			{
				Thread.SpinWait(4 << m_count);
			}
			m_count = ((m_count == int.MaxValue) ? 10 : (m_count + 1));
		}

		/// <summary>
		/// Resets the spin counter.
		/// </summary>
		/// <remarks>
		/// This makes <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> and <see cref="P:ParallelTasks.MySpinWait.NextSpinWillYield" /> behave as though no calls
		/// to <see cref="M:ParallelTasks.MySpinWait.SpinOnce" /> had been issued on this instance. If a <see cref="T:ParallelTasks.MySpinWait" /> instance
		/// is reused many times, it may be useful to reset it to avoid yielding too soon.
		/// </remarks>
		public void Reset()
		{
			m_count = 0;
		}

		/// <summary>
		/// Spins until the specified condition is satisfied.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		public static void SpinUntil(Func<bool> condition)
		{
			SpinUntil(condition, -1);
		}

		/// <summary>
		/// Spins until the specified condition is satisfied or until the specified timeout is expired.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <param name="timeout">
		/// A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, 
		/// or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
		/// <returns>True if the condition is satisfied within the timeout; otherwise, false</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative number
		/// other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than
		/// <see cref="F:System.Int32.MaxValue" />.</exception>
		public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
		{
			long num = (long)timeout.TotalMilliseconds;
			if (num < -1 || num > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("timeout", timeout, "SpinWait_SpinUntil_TimeoutWrong");
			}
			return SpinUntil(condition, (int)timeout.TotalMilliseconds);
		}

		/// <summary>
		/// Spins until the specified condition is satisfied or until the specified timeout is expired.
		/// </summary>
		/// <param name="condition">A delegate to be executed over and over until it returns true.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>True if the condition is satisfied within the timeout; otherwise, false</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a
		/// negative number other than -1, which represents an infinite time-out.</exception>
		public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
		{
			if (millisecondsTimeout < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, "SpinWait_SpinUntil_TimeoutWrong");
			}
			if (condition == null)
			{
				throw new ArgumentNullException("condition", "SpinWait_SpinUntil_ArgumentNull");
			}
			long startTime = 0L;
			if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
			{
				startTime = TimeoutHelper.GetTime();
			}
			MySpinWait mySpinWait = default(MySpinWait);
			while (!condition())
			{
				if (millisecondsTimeout == 0)
				{
					return false;
				}
				mySpinWait.SpinOnce();
				if (millisecondsTimeout != -1 && mySpinWait.NextSpinWillYield && TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout) <= 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
