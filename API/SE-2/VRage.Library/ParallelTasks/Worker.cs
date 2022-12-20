using System;
using System.Globalization;
using System.Threading;

namespace ParallelTasks
{
	internal class Worker
	{
		private Thread thread;

		private Deque<Task> tasks;

		private WorkStealingScheduler scheduler;

		private static Hashtable<Thread, Worker> workers = new Hashtable<Thread, Worker>(Environment.ProcessorCount);

		public AutoResetEvent Gate { get; private set; }

		public ManualResetEvent HasNoWork { get; private set; }

		public static Worker CurrentWorker
		{
			get
			{
<<<<<<< HEAD
				Thread currentThread = Thread.CurrentThread;
=======
				Thread currentThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (workers.TryGet(currentThread, out var data))
				{
					return data;
				}
				return null;
			}
		}

		public Worker(WorkStealingScheduler scheduler, int index, ThreadPriority priority)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			thread = new Thread((ThreadStart)Work);
			thread.set_Name("Parallel " + index);
			thread.set_IsBackground(true);
			thread.set_Priority(priority);
			thread.set_CurrentCulture(CultureInfo.InvariantCulture);
			thread.set_CurrentUICulture(CultureInfo.InvariantCulture);
			tasks = new Deque<Task>();
			this.scheduler = scheduler;
			Gate = new AutoResetEvent(initialState: false);
			HasNoWork = new ManualResetEvent(initialState: false);
			workers.Add(thread, this);
		}

		public void Start()
		{
			thread.Start();
		}

		public void AddWork(Task task)
		{
			tasks.LocalPush(task);
		}

		private void Work()
		{
			while (true)
			{
				FindWork(out var task);
				task.DoWork();
			}
		}

		private void FindWork(out Task task)
		{
			bool flag = false;
			task = default(Task);
			while (!tasks.LocalPop(ref task) && !scheduler.TryGetTask(out task))
			{
				for (int i = 0; i < scheduler.Workers.Count; i++)
				{
					Worker worker = scheduler.Workers[i];
					if (worker != this && worker.tasks.TrySteal(ref task))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					HasNoWork.Set();
					Gate.WaitOne();
					HasNoWork.Reset();
				}
				if (flag)
				{
					break;
				}
			}
		}
	}
}
