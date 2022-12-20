using System.Diagnostics;
using VRage.Utils;

namespace Sandbox.Game.AI.Pathfinding
{
	internal static class MyPathfindingStopwatch
	{
		private static readonly Stopwatch m_stopWatch;

		private static readonly Stopwatch m_globalStopwatch;

		private static readonly MyLog m_log;

		private const int STOP_TIME_MS = 10000;

		private static int m_levelOfStarting;

		static MyPathfindingStopwatch()
		{
<<<<<<< HEAD
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_stopWatch = new Stopwatch();
			m_globalStopwatch = new Stopwatch();
			m_log = new MyLog();
		}

		[Conditional("DEBUG")]
		public static void StartMeasuring()
		{
			m_stopWatch.Reset();
			m_globalStopwatch.Reset();
			m_globalStopwatch.Start();
		}

		[Conditional("DEBUG")]
		public static void CheckStopMeasuring()
		{
<<<<<<< HEAD
			if (m_globalStopwatch.IsRunning)
			{
				_ = m_globalStopwatch.ElapsedMilliseconds;
=======
			if (m_globalStopwatch.get_IsRunning())
			{
				m_globalStopwatch.get_ElapsedMilliseconds();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				_ = 10000;
			}
		}

		[Conditional("DEBUG")]
		public static void StopMeasuring()
		{
			m_globalStopwatch.Stop();
<<<<<<< HEAD
			string msg = $"pathfinding elapsed time: {m_stopWatch.ElapsedMilliseconds} ms / in {10000} ms";
=======
			string msg = $"pathfinding elapsed time: {m_stopWatch.get_ElapsedMilliseconds()} ms / in {10000} ms";
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_log.WriteLineAndConsole(msg);
		}

		[Conditional("DEBUG")]
		public static void Start()
		{
<<<<<<< HEAD
			if (!m_stopWatch.IsRunning)
=======
			if (!m_stopWatch.get_IsRunning())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_stopWatch.Start();
				m_levelOfStarting = 1;
			}
			else
			{
				m_levelOfStarting++;
			}
		}

		[Conditional("DEBUG")]
		public static void Stop()
		{
<<<<<<< HEAD
			if (m_stopWatch.IsRunning)
=======
			if (m_stopWatch.get_IsRunning())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_levelOfStarting--;
				if (m_levelOfStarting == 0)
				{
					m_stopWatch.Stop();
				}
			}
		}

		[Conditional("DEBUG")]
		public static void Reset()
		{
			m_stopWatch.Reset();
		}
	}
}
