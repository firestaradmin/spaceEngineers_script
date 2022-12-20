using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace VRage.Voxels.Clipmap
{
	public class MyClipmapTiming
	{
		[ThreadStatic]
		private static Stopwatch m_threadStopwatch;

		private static Dictionary<Thread, Stopwatch> m_stopwatches = new Dictionary<Thread, Stopwatch>();

		private static TimeSpan m_total;

		private static Stopwatch Stopwatch
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Expected O, but got Unknown
				if (m_threadStopwatch == null)
				{
					lock (m_stopwatches)
					{
						m_threadStopwatch = new Stopwatch();
						m_stopwatches[Thread.get_CurrentThread()] = m_threadStopwatch;
					}
				}
				return m_threadStopwatch;
			}
		}

		/// <summary>
		/// Total time spent calculating ticks.
		/// </summary>
		public static TimeSpan Total => m_total;

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StartTiming()
		{
			Stopwatch.Start();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void StopTiming()
		{
			Stopwatch.Stop();
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		public static void Reset()
		{
			lock (m_stopwatches)
			{
				foreach (Stopwatch value in m_stopwatches.Values)
				{
					value.Reset();
				}
			}
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		private static void ReadTotal()
		{
			lock (m_stopwatches)
			{
				long num = 0L;
				foreach (Stopwatch value in m_stopwatches.Values)
				{
					num += value.get_ElapsedTicks();
				}
				m_total = new TimeSpan(num);
			}
		}
	}
}
