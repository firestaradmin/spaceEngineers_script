using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace VRage.Library.Threading
{
	/// <summary>
	/// Collection of utilities for advanced process and thread management.
	/// </summary>
	public static class ProcessUtil
	{
		/// <summary>
		/// Sets the processor affinity of the current thread.
		/// </summary>
		/// <param name="cpus">A list of CPU numbers. The values should be
		/// between 0 and <see cref="P:System.Environment.ProcessorCount" />.</param>
		public static void SetThreadProcessorAffinity(params int[] cpus)
		{
			if (cpus == null)
			{
				throw new ArgumentNullException("cpus");
			}
			if (cpus.Length == 0)
			{
				throw new ArgumentException("You must specify at least one CPU.", "cpus");
			}
			long num = 0L;
			foreach (int num2 in cpus)
			{
<<<<<<< HEAD
				if (num2 < 0 || num2 >= Environment.ProcessorCount)
=======
				if (num2 < 0 || num2 >= Environment.get_ProcessorCount())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					throw new ArgumentException("Invalid CPU number.");
				}
				num |= 1L << num2;
			}
			Thread.BeginThreadAffinity();
			int osThreadId = AppDomain.GetCurrentThreadId();
<<<<<<< HEAD
			ProcessThread processThread = Process.GetCurrentProcess().Threads.Cast<ProcessThread>().Single((ProcessThread t) => t.Id == osThreadId);
			processThread.ProcessorAffinity = new IntPtr(num);
=======
			ProcessThread val = Enumerable.Single<ProcessThread>(Enumerable.Cast<ProcessThread>((IEnumerable)Process.GetCurrentProcess().get_Threads()), (Func<ProcessThread, bool>)((ProcessThread t) => t.get_Id() == osThreadId));
			val.set_ProcessorAffinity(new IntPtr(num));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
