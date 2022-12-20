using System.Diagnostics;

namespace ParallelTasks
{
	/// <summary>
	/// A helper class to capture a start time using Environment.TickCout as a time in milliseconds, also updates a given timeout bu subtracting the current time from
	/// the start time
	/// </summary>
	internal static class TimeoutHelper
	{
		/// <summary>
		/// Returns start time
		/// </summary>
		/// <returns></returns>
		public static long GetTime()
		{
			return Stopwatch.GetTimestamp();
		}

		/// <summary>
		/// Helper function to measure and update the elapsed time
		/// </summary>
		/// <param name="startTime"> The first time (in milliseconds) observed when the wait started</param>
		/// <param name="originalWaitMillisecondsTimeout">The orginal wait timeoutout in milliseconds</param>
		/// <returns>The new wait time in milliseconds, -1 if the time expired</returns>
		public static int UpdateTimeOut(long startTime, int originalWaitMillisecondsTimeout)
		{
			long num = Stopwatch.Frequency / 1000;
			long num2 = (GetTime() - startTime) / num;
			return (int)(originalWaitMillisecondsTimeout - num2);
		}
	}
}
