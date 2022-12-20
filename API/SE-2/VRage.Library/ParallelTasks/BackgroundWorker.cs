using System.Collections.Generic;
using System.Threading;

namespace ParallelTasks
{
	internal class BackgroundWorker
	{
		private static Stack<BackgroundWorker> idleWorkers = new Stack<BackgroundWorker>();

		private Thread thread;

		private AutoResetEvent resetEvent;

		private Task work;

		public BackgroundWorker()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			resetEvent = new AutoResetEvent(initialState: false);
			thread = new Thread((ThreadStart)WorkLoop);
			thread.set_IsBackground(true);
			thread.Start();
		}

		private void WorkLoop()
		{
			while (true)
			{
				resetEvent.WaitOne();
				work.DoWork();
				lock (idleWorkers)
				{
					idleWorkers.Push(this);
				}
			}
		}

		private void Start(Task work)
		{
			this.work = work;
			resetEvent.Set();
		}

		public static void StartWork(Task work)
		{
			BackgroundWorker backgroundWorker = null;
			lock (idleWorkers)
			{
				if (idleWorkers.get_Count() > 0)
				{
					backgroundWorker = idleWorkers.Pop();
				}
			}
			if (backgroundWorker == null)
			{
				backgroundWorker = new BackgroundWorker();
			}
			backgroundWorker.Start(work);
		}
	}
}
