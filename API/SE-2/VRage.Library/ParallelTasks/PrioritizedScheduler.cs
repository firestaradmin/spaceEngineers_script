using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace ParallelTasks
{
	/// <summary>
	/// Sheduler that supports interruption of normal 
	/// </summary>
	public class PrioritizedScheduler : IWorkScheduler
	{
		/// <summary>
		/// Worker array groups workers of the same thread priority.
		/// It also contains the queues of tasks belonging to this thread priority.
		/// </summary>
		private class WorkerArray
		{
			private bool m_suspended;

			/// <summary>
			/// Reference to the scheduler.
			/// </summary>
			private PrioritizedScheduler m_prioritizedScheduler;

			/// <summary>
			/// Index of this worker array.
			/// </summary>
			private readonly int m_workerArrayIndex;

			/// <summary>
			/// Task queues. Even in one worker group, tasks will be sorted according to their priority.
			/// </summary>
			private readonly Queue<Task> m_taskQueue = new Queue<Task>(64);

			/// <summary>
			/// Array of worker threads.
			/// </summary>
			private readonly Worker[] m_workers;

			private const int DEFAULT_QUEUE_CAPACITY = 64;

			/// <summary>
			/// Array of worker threads.
			/// </summary>
			public Worker[] Workers => m_workers;

			/// <summary>
			/// Constructor of the worker array.
			/// </summary>
			/// <param name="prioritizedScheduler">Scheduler, owner of this group.</param>
			/// <param name="workerArrayIndex"></param>
			/// <param name="threadCount"></param>
			/// <param name="systemThreadPriority"></param>
			public WorkerArray(PrioritizedScheduler prioritizedScheduler, int workerArrayIndex, int threadCount, ThreadPriority systemThreadPriority)
			{
				m_workerArrayIndex = workerArrayIndex;
				m_prioritizedScheduler = prioritizedScheduler;
				m_workers = new Worker[threadCount];
				for (int i = 0; i < threadCount; i++)
				{
					m_workers[i] = new Worker(this, string.Concat("Parallel ", systemThreadPriority, "_", i), systemThreadPriority, i);
				}
			}

			/// <summary>
			/// Try getting the task from the internal queue.
			/// </summary>
			/// <param name="task"></param>
			/// <returns></returns>
			public bool TryGetTask(out Task task)
			{
				if (m_suspended)
				{
					task = default(Task);
					return false;
				}
				lock (m_taskQueue)
				{
					return m_taskQueue.TryDequeue<Task>(out task);
				}
			}

			/// <summary>
			/// Schedule the task in this worker array.
			/// </summary>
			/// <param name="task"></param>            
			public void Schedule(Task task)
			{
				int num = task.Item.Work.Options.MaximumThreads;
				if (num < 1)
				{
					num = 1;
				}
				num = Math.Min(num, m_workers.Length);
				lock (m_taskQueue)
				{
					for (int i = 0; i < num; i++)
					{
						m_taskQueue.Enqueue(task);
					}
				}
				Worker[] workers = m_workers;
				foreach (Worker worker in workers)
				{
					worker.Gate.Set();
				}
			}

			public Task ScheduleOnEachWorker(Action action)
			{
<<<<<<< HEAD
=======
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Barrier barrier = new Barrier(Workers.Length);
				ActionWork work = new ActionWork(delegate
				{
					barrier.SignalAndWait();
					action();
				}, Parallel.DefaultOptions.WithMaxThreads(Workers.Length));
				WorkItem workItem = WorkItem.Get();
				workItem.Callback = null;
				workItem.WorkData = null;
				workItem.CompletionCallbacks = null;
				Task task = workItem.PrepareStart(work);
				Schedule(task);
				return task;
			}

			public int ReadAndClearWorklog()
			{
				int num = 0;
				Worker[] workers = m_workers;
				foreach (Worker worker in workers)
				{
					num += worker.ReadAndClearWorklog();
				}
				return num;
			}

			public void Suspend()
			{
				m_suspended = true;
			}

			public void Resume()
			{
				if (m_suspended)
				{
					m_suspended = false;
					Worker[] workers = m_workers;
					foreach (Worker worker in workers)
					{
						worker.Gate.Set();
					}
				}
			}
		}

		/// <summary>
		/// One worker thread of the prioritized scheduler.
		/// </summary>
		private class Worker
		{
			private readonly WorkerArray m_workerArray;

			private readonly int m_workerIndex;

			private readonly Thread m_thread;

			public readonly ManualResetEvent HasNoWork;

			public readonly AutoResetEvent Gate;

			private long Worklog;

			private int ExecutedWork;

			/// <summary>
			/// Get the underlying system thread.
			/// </summary>
			public Thread Thread => m_thread;

			public Worker(WorkerArray workerArray, string name, ThreadPriority priority, int workerIndex)
			{
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Expected O, but got Unknown
				m_workerArray = workerArray;
				m_workerIndex = workerIndex;
				m_thread = new Thread((ThreadStart)WorkerLoop);
				HasNoWork = new ManualResetEvent(initialState: false);
				Gate = new AutoResetEvent(initialState: false);
				m_thread.set_Name(name);
				m_thread.set_IsBackground(true);
				m_thread.set_Priority(priority);
				m_thread.set_CurrentCulture(CultureInfo.InvariantCulture);
				m_thread.set_CurrentUICulture(CultureInfo.InvariantCulture);
				m_thread.Start();
			}

			private void OpenWork()
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num = Interlocked.Exchange(ref Worklog, timestamp);
			}

			private void CloseWork()
			{
				long timestamp = Stopwatch.GetTimestamp();
				long num = Interlocked.Exchange(ref Worklog, 0L);
				int value = (int)(timestamp - num);
				Interlocked.Add(ref ExecutedWork, value);
			}

			public int ReadAndClearWorklog()
			{
				long worklog = Worklog;
				if (worklog != 0L)
				{
					long timestamp = Stopwatch.GetTimestamp();
					worklog = Interlocked.CompareExchange(ref Worklog, timestamp, worklog);
				}
				return Interlocked.Exchange(ref ExecutedWork, 0);
			}

			private void WorkerLoop()
			{
				while (true)
				{
					if (m_workerArray.TryGetTask(out var task))
					{
						OpenWork();
						task.DoWork();
						CloseWork();
					}
					else
					{
						HasNoWork.Set();
						Gate.WaitOne();
						HasNoWork.Reset();
					}
				}
			}
		}

		private readonly int[] m_mappingPriorityToWorker = new int[5] { 0, 1, 1, 1, 2 };

		private readonly ThreadPriority[] m_mappingWorkerToThreadPriority = new ThreadPriority[3]
		{
			ThreadPriority.Highest,
			ThreadPriority.Normal,
			ThreadPriority.Lowest
		};

		private readonly float[] m_mappingWorkerToThreadFactor = new float[3] { 1f, 1f, 2f };

		private readonly float[] m_mappingWorkerToThreadFactorAMD = new float[3] { 1f, 1f, 1f };

		private WorkerArray[] m_workerArrays;

		private WaitHandle[] m_hasNoWork;

		/// <summary>
		/// Reveals only the count of one thread array. This value is intended to be used as a "number of threads that can run in parallel."
		/// </summary>
		public int ThreadCount => m_workerArrays[0].Workers.Length;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="threadCount">Number of threads in each worker group.</param>
		/// <param name="amd"></param>
		public PrioritizedScheduler(int threadCount, bool amd)
		{
			InitializeWorkerArrays(threadCount, amd);
		}

		/// <summary>
		/// Initialize all worker arrays.
		/// </summary>
		/// <param name="threadCount">Each array of workers will contain number of threads = threadCount.</param>
		/// <param name="amd"></param>
		private void InitializeWorkerArrays(int threadCount, bool amd)
		{
			int num = 0;
			int[] mappingPriorityToWorker = m_mappingPriorityToWorker;
			foreach (int num2 in mappingPriorityToWorker)
			{
				num = ((num2 > num) ? num2 : num);
			}
			float[] array = (amd ? m_mappingWorkerToThreadFactorAMD : m_mappingWorkerToThreadFactor);
			int num3 = 0;
			int[] array2 = new int[array.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = (int)Math.Floor(array[j] * (float)threadCount);
				num3 += array2[j];
			}
			m_workerArrays = new WorkerArray[num + 1];
			m_hasNoWork = new WaitHandle[num3];
			int num4 = 0;
			for (int k = 0; k <= num; k++)
			{
				int num5 = array2[k];
				m_workerArrays[k] = new WorkerArray(this, k, num5, m_mappingWorkerToThreadPriority[k]);
				for (int l = 0; l < num5; l++)
				{
					m_hasNoWork[num4++] = m_workerArrays[k].Workers[l].HasNoWork;
				}
			}
		}

		private WorkerArray GetWorkerArray(WorkPriority priority)
		{
			return m_workerArrays[m_mappingPriorityToWorker[(int)priority]];
		}

		public void Schedule(Task task)
		{
			if (task.Item.Work != null)
			{
				WorkPriority priority = ((task.Item.WorkData != null) ? task.Item.WorkData.Priority : WorkPriority.Normal);
				IPrioritizedWork prioritizedWork = task.Item.Work as IPrioritizedWork;
				if (prioritizedWork != null)
				{
					priority = prioritizedWork.Priority;
				}
				GetWorkerArray(priority).Schedule(task);
			}
		}

		public bool WaitForTasksToFinish(TimeSpan waitTimeout)
		{
			return Parallel.WaitForAll(m_hasNoWork, waitTimeout);
		}

		public void ScheduleOnEachWorker(Action action)
		{
			List<Task> list = new List<Task>();
			WorkerArray[] workerArrays = m_workerArrays;
			foreach (WorkerArray workerArray in workerArrays)
			{
				list.Add(workerArray.ScheduleOnEachWorker(action));
			}
			foreach (Task item in list)
			{
				item.Wait();
			}
		}

		public int ReadAndClearExecutionTime()
		{
			int num = 0;
			WorkerArray[] workerArrays = m_workerArrays;
			foreach (WorkerArray workerArray in workerArrays)
			{
				num += workerArray.ReadAndClearWorklog();
			}
			return num;
		}

		public void SuspendThreads(TimeSpan waitTimeout)
		{
			WorkerArray[] workerArrays = m_workerArrays;
			foreach (WorkerArray workerArray in workerArrays)
			{
				workerArray.Suspend();
			}
			WaitForTasksToFinish(waitTimeout);
		}

		public void ResumeThreads()
		{
			WorkerArray[] workerArrays = m_workerArrays;
			foreach (WorkerArray workerArray in workerArrays)
			{
				workerArray.Resume();
			}
		}

		public void SuspendThreads(WorkPriority priority)
		{
			GetWorkerArray(priority).Suspend();
		}

		public void ResumeThreads(WorkPriority priority)
		{
			GetWorkerArray(priority).Resume();
		}
	}
}
