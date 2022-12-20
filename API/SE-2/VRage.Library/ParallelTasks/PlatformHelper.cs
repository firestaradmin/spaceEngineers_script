using System;

namespace ParallelTasks
{
	/// <summary>
	/// A helper class to get the number of processors, it updates the numbers of processors every sampling interval.
	/// </summary>
	internal static class PlatformHelper
	{
		private static volatile int s_processorCount;

		/// <summary>
		/// Gets the number of available processors
		/// </summary>
		internal static int ProcessorCount
		{
			get
			{
<<<<<<< HEAD
				int tickCount = Environment.TickCount;
				int num = s_processorCount;
				if (num == 0)
				{
					num = (s_processorCount = Environment.ProcessorCount);
=======
				int tickCount = Environment.get_TickCount();
				int num = s_processorCount;
				if (num == 0)
				{
					num = (s_processorCount = Environment.get_ProcessorCount());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				return num;
			}
		}

		/// <summary>
		/// Gets whether the current machine has only a single processor.
		/// </summary>
		internal static bool IsSingleProcessor => ProcessorCount == 1;
	}
}
